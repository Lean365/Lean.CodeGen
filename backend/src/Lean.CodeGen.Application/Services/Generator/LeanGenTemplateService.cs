using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mapster;
using Lean.CodeGen.Application.Dtos.Generator;
using Lean.CodeGen.Common.Excel;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Domain.Entities.Generator;
using Lean.CodeGen.Domain.Interfaces.Repositories;
using Lean.CodeGen.Application.Services.Base;
using Lean.CodeGen.Common.Options;
using Lean.CodeGen.Application.Services.Security;
using Lean.CodeGen.Common.Extensions;
using Microsoft.Extensions.Options;

namespace Lean.CodeGen.Application.Services.Generator
{
  /// <summary>
  /// 代码生成模板服务实现
  /// </summary>
  public class LeanGenTemplateService : LeanBaseService, ILeanGenTemplateService
  {
    private readonly ILeanRepository<LeanGenTemplate> _templateRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    public LeanGenTemplateService(
        ILeanRepository<LeanGenTemplate> templateRepository,
        ILeanSqlSafeService sqlSafeService,
        IOptions<LeanSecurityOptions> securityOptions)
        : base(sqlSafeService, securityOptions)
    {
      _templateRepository = templateRepository;
    }

    /// <summary>
    /// 获取代码生成模板列表（分页）
    /// </summary>
    public async Task<LeanPageResult<LeanGenTemplateDto>> GetPageListAsync(LeanGenTemplateQueryDto queryDto)
    {
      var predicate = BuildQueryPredicate(queryDto);
      var (total, items) = await _templateRepository.GetPageListAsync(predicate, queryDto.PageSize, queryDto.PageIndex);
      var list = items.Select(t => t.Adapt<LeanGenTemplateDto>()).ToList();

      return new LeanPageResult<LeanGenTemplateDto>
      {
        Total = total,
        Items = list
      };
    }

    /// <summary>
    /// 获取代码生成模板详情
    /// </summary>
    public async Task<LeanGenTemplateDto> GetAsync(long id)
    {
      var template = await _templateRepository.FirstOrDefaultAsync(t => t.Id == id);
      if (template == null)
      {
        throw new Exception($"模板 {id} 不存在");
      }

      return template.Adapt<LeanGenTemplateDto>();
    }

    /// <summary>
    /// 创建代码生成模板
    /// </summary>
    public async Task<LeanGenTemplateDto> CreateAsync(LeanCreateGenTemplateDto createDto)
    {
      var entity = createDto.Adapt<LeanGenTemplate>();
      entity.CreateTime = DateTime.Now;

      await _templateRepository.CreateAsync(entity);
      return await GetAsync(entity.Id);
    }

    /// <summary>
    /// 更新代码生成模板
    /// </summary>
    public async Task<LeanGenTemplateDto> UpdateAsync(long id, LeanUpdateGenTemplateDto updateDto)
    {
      var entity = await _templateRepository.FirstOrDefaultAsync(t => t.Id == id);
      if (entity == null)
      {
        throw new Exception($"模板 {id} 不存在");
      }

      updateDto.Adapt(entity);
      entity.UpdateTime = DateTime.Now;

      await _templateRepository.UpdateAsync(entity);
      return await GetAsync(id);
    }

    /// <summary>
    /// 删除代码生成模板
    /// </summary>
    public async Task<bool> DeleteAsync(long id)
    {
      var template = await _templateRepository.FirstOrDefaultAsync(t => t.Id == id);
      if (template == null)
      {
        throw new Exception($"模板 {id} 不存在");
      }

      return await _templateRepository.DeleteAsync(t => t.Id == id);
    }

    /// <summary>
    /// 导出代码生成模板
    /// </summary>
    public async Task<LeanFileResult> ExportAsync(LeanGenTemplateQueryDto queryDto)
    {
      var predicate = BuildQueryPredicate(queryDto);
      var items = await _templateRepository.GetListAsync(predicate);
      var list = items.Select(t => t.Adapt<LeanGenTemplateExportDto>()).ToList();

      var excelBytes = LeanExcelHelper.Export(list);
      return new LeanFileResult
      {
        FileName = $"代码生成模板_{DateTime.Now:yyyyMMddHHmmss}.xlsx",
        ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        Stream = new MemoryStream(excelBytes)
      };
    }

    /// <summary>
    /// 导入代码生成模板
    /// </summary>
    public async Task<LeanExcelImportResult<LeanGenTemplateImportDto>> ImportAsync(LeanFileInfo file)
    {
      var result = new LeanExcelImportResult<LeanGenTemplateImportDto>();

      try
      {
        var importResult = LeanExcelHelper.Import<LeanGenTemplateImportDto>(File.ReadAllBytes(file.FilePath));
        foreach (var item in importResult.Data)
        {
          try
          {
            var entity = item.Adapt<LeanGenTemplate>();
            entity.CreateTime = DateTime.Now;

            await _templateRepository.CreateAsync(entity);
            result.Data.Add(item);
          }
          catch (Exception ex)
          {
            result.Errors.Add(new LeanExcelImportError
            {
              RowIndex = importResult.Data.IndexOf(item) + 2,
              ErrorMessage = ex.Message
            });
          }
        }
      }
      catch (Exception ex)
      {
        result.ErrorMessage = ex.Message;
      }

      return result;
    }

    /// <summary>
    /// 下载导入模板
    /// </summary>
    public Task<LeanFileResult> DownloadTemplateAsync()
    {
      var template = new List<LeanGenTemplateImportTemplateDto>
      {
        new LeanGenTemplateImportTemplateDto()
      };

      var excelBytes = LeanExcelHelper.Export(template);
      var result = new LeanFileResult
      {
        FileName = "代码生成模板导入模板.xlsx",
        ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        Stream = new MemoryStream(excelBytes)
      };

      return Task.FromResult(result);
    }

    /// <summary>
    /// 复制模板
    /// </summary>
    public async Task<LeanGenTemplateDto> CopyAsync(long id)
    {
      var template = await _templateRepository.FirstOrDefaultAsync(t => t.Id == id);
      if (template == null)
      {
        throw new Exception($"模板 {id} 不存在");
      }

      var newTemplate = template.Adapt<LeanGenTemplate>();
      newTemplate.CreateTime = DateTime.Now;
      newTemplate.Name = $"{template.Name}_Copy";

      await _templateRepository.CreateAsync(newTemplate);
      return await GetAsync(newTemplate.Id);
    }

    /// <summary>
    /// 构建查询条件
    /// </summary>
    private Expression<Func<LeanGenTemplate, bool>> BuildQueryPredicate(LeanGenTemplateQueryDto queryDto)
    {
      Expression<Func<LeanGenTemplate, bool>> predicate = t => true;

      if (!string.IsNullOrEmpty(queryDto.Name))
      {
        var name = CleanInput(queryDto.Name);
        predicate = LeanExpressionExtensions.And(predicate, t => t.Name.Contains(name));
      }

      if (!string.IsNullOrEmpty(queryDto.GroupName))
      {
        var groupName = CleanInput(queryDto.GroupName);
        predicate = LeanExpressionExtensions.And(predicate, t => t.GroupName == groupName);
      }

      if (!string.IsNullOrEmpty(queryDto.Engine))
      {
        var engine = CleanInput(queryDto.Engine);
        predicate = LeanExpressionExtensions.And(predicate, t => t.Engine == engine);
      }

      if (!string.IsNullOrEmpty(queryDto.Language))
      {
        var language = CleanInput(queryDto.Language);
        predicate = LeanExpressionExtensions.And(predicate, t => t.Language == language);
      }

      if (queryDto.IsEnabled.HasValue)
      {
        predicate = LeanExpressionExtensions.And(predicate, t => t.IsEnabled == queryDto.IsEnabled);
      }

      if (queryDto.ConfigId.HasValue)
      {
        predicate = LeanExpressionExtensions.And(predicate, t => t.ConfigId == queryDto.ConfigId);
      }

      if (queryDto.CreateTimeBegin.HasValue)
      {
        predicate = LeanExpressionExtensions.And(predicate, t => t.CreateTime >= queryDto.CreateTimeBegin);
      }

      if (queryDto.CreateTimeEnd.HasValue)
      {
        predicate = LeanExpressionExtensions.And(predicate, t => t.CreateTime <= queryDto.CreateTimeEnd);
      }

      return predicate;
    }
  }
}