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
using Lean.CodeGen.Common.Exceptions;
using Lean.CodeGen.Common.Security;
using Microsoft.Extensions.Logging;
using Lean.CodeGen.Domain.Validators;

namespace Lean.CodeGen.Application.Services.Generator
{
  /// <summary>
  /// 代码生成配置服务实现
  /// </summary>
  public class LeanGenConfigService : LeanBaseService, ILeanGenConfigService
  {
    private readonly ILogger<LeanGenConfigService> _logger;
    private readonly ILeanRepository<LeanGenConfig> _repository;
    private readonly ILeanRepository<LeanGenTemplate> _templateRepository;
    private readonly LeanUniqueValidator<LeanGenConfig> _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    public LeanGenConfigService(
        ILeanRepository<LeanGenConfig> repository,
        ILeanRepository<LeanGenTemplate> templateRepository,
        LeanBaseServiceContext context)
        : base(context)
    {
      _repository = repository;
      _templateRepository = templateRepository;
      _logger = (ILogger<LeanGenConfigService>)context.Logger;
      _uniqueValidator = new LeanUniqueValidator<LeanGenConfig>(_repository);
    }

    /// <summary>
    /// 获取代码生成配置列表（分页）
    /// </summary>
    public async Task<LeanPageResult<LeanGenConfigDto>> GetPageListAsync(LeanGenConfigQueryDto queryDto)
    {
      var predicate = BuildQueryPredicate(queryDto);
      var (total, items) = await _repository.GetPageListAsync(predicate, queryDto.PageSize, queryDto.PageIndex);
      var list = items.Select(t => t.Adapt<LeanGenConfigDto>()).ToList();

      await FillConfigRelationsAsync(list);

      return new LeanPageResult<LeanGenConfigDto>
      {
        Total = total,
        Items = list
      };
    }

    /// <summary>
    /// 获取代码生成配置详情
    /// </summary>
    public async Task<LeanGenConfigDto> GetAsync(long id)
    {
      var config = await _repository.FirstOrDefaultAsync(t => t.Id == id);
      if (config == null)
      {
        throw new LeanException($"代码生成配置[{id}]不存在");
      }

      var templates = await _templateRepository.GetListAsync(t => t.ConfigId == id);
      var dto = config.Adapt<LeanGenConfigDto>();
      dto.Templates = templates.Adapt<List<LeanGenTemplateDto>>();

      return dto;
    }

    /// <summary>
    /// 创建代码生成配置
    /// </summary>
    public async Task<LeanGenConfigDto> CreateAsync(LeanCreateGenConfigDto createDto)
    {
      var entity = createDto.Adapt<LeanGenConfig>();
      entity.CreateTime = DateTime.Now;

      await _repository.CreateAsync(entity);

      // 创建模板
      foreach (var templateDto in createDto.Templates)
      {
        var template = templateDto.Adapt<LeanGenTemplate>();
        template.ConfigId = entity.Id;
        template.CreateTime = DateTime.Now;

        await _templateRepository.CreateAsync(template);
      }

      return await GetAsync(entity.Id);
    }

    /// <summary>
    /// 更新代码生成配置
    /// </summary>
    public async Task<LeanGenConfigDto> UpdateAsync(long id, LeanUpdateGenConfigDto updateDto)
    {
      var entity = await _repository.FirstOrDefaultAsync(t => t.Id == id);
      if (entity == null)
      {
        throw new LeanException($"代码生成配置[{id}]不存在");
      }

      updateDto.Adapt(entity);
      await _repository.UpdateAsync(entity);

      // 删除原有模板
      var templates = await _templateRepository.GetListAsync(t => t.ConfigId == id);
      foreach (var template in templates)
      {
        await _templateRepository.DeleteAsync(template);
      }

      // 创建新模板
      foreach (var templateDto in updateDto.Templates)
      {
        var template = templateDto.Adapt<LeanGenTemplate>();
        template.ConfigId = id;
        template.CreateTime = DateTime.Now;

        await _templateRepository.CreateAsync(template);
      }

      return await GetAsync(id);
    }

    /// <summary>
    /// 删除代码生成配置
    /// </summary>
    public async Task<bool> DeleteAsync(long id)
    {
      var config = await _repository.FirstOrDefaultAsync(t => t.Id == id);
      if (config == null)
      {
        throw new Exception($"配置 {id} 不存在");
      }

      // 删除关联的模板
      await _templateRepository.DeleteAsync(t => t.ConfigId == id);

      return await _repository.DeleteAsync(t => t.Id == id);
    }

    /// <summary>
    /// 导出代码生成配置
    /// </summary>
    public async Task<LeanFileResult> ExportAsync(LeanGenConfigQueryDto queryDto)
    {
      var predicate = BuildQueryPredicate(queryDto);
      var items = await _repository.GetListAsync(predicate);
      var list = items.Select(t => t.Adapt<LeanGenConfigExportDto>()).ToList();

      var excelBytes = LeanExcelHelper.Export(list);
      return new LeanFileResult
      {
        FileName = $"代码生成配置_{DateTime.Now:yyyyMMddHHmmss}.xlsx",
        ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        Stream = new MemoryStream(excelBytes)
      };
    }

    /// <summary>
    /// 导入代码生成配置
    /// </summary>
    public async Task<LeanExcelImportResult<LeanGenConfigImportDto>> ImportAsync(LeanFileInfo file)
    {
      var result = new LeanExcelImportResult<LeanGenConfigImportDto>();

      try
      {
        var importResult = LeanExcelHelper.Import<LeanGenConfigImportDto>(File.ReadAllBytes(file.FilePath));
        foreach (var item in importResult.Data)
        {
          try
          {
            var entity = item.Adapt<LeanGenConfig>();
            entity.CreateTime = DateTime.Now;

            await _repository.CreateAsync(entity);
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
      var template = new List<LeanGenConfigImportTemplateDto>
      {
        new LeanGenConfigImportTemplateDto()
      };

      var excelBytes = LeanExcelHelper.Export(template);
      var result = new LeanFileResult
      {
        FileName = "代码生成配置导入模板.xlsx",
        ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        Stream = new MemoryStream(excelBytes)
      };

      return Task.FromResult(result);
    }

    /// <summary>
    /// 复制配置
    /// </summary>
    public async Task<LeanGenConfigDto> CopyAsync(long id)
    {
      var config = await _repository.FirstOrDefaultAsync(t => t.Id == id);
      if (config == null)
      {
        throw new LeanException($"代码生成配置[{id}]不存在");
      }

      var newConfig = config.Adapt<LeanGenConfig>();
      newConfig.CreateTime = DateTime.Now;
      newConfig.Name = $"{config.Name}_Copy";

      await _repository.CreateAsync(newConfig);

      // 复制模板
      var templates = await _templateRepository.GetListAsync(t => t.ConfigId == id);
      foreach (var t in templates)
      {
        var template = t.Adapt<LeanGenTemplate>();
        template.ConfigId = newConfig.Id;
        template.CreateTime = DateTime.Now;

        await _templateRepository.CreateAsync(template);
      }

      return await GetAsync(newConfig.Id);
    }

    /// <summary>
    /// 构建查询条件
    /// </summary>
    private Expression<Func<LeanGenConfig, bool>> BuildQueryPredicate(LeanGenConfigQueryDto queryDto)
    {
      Expression<Func<LeanGenConfig, bool>> predicate = t => true;

      if (!string.IsNullOrEmpty(queryDto.Name))
      {
        var name = CleanInput(queryDto.Name);
        predicate = LeanExpressionExtensions.And(predicate, t => t.Name.Contains(name));
      }

      if (queryDto.GenType.HasValue)
      {
        predicate = LeanExpressionExtensions.And(predicate, t => t.GenType == queryDto.GenType);
      }

      if (!string.IsNullOrEmpty(queryDto.BackendTool))
      {
        var backendTool = CleanInput(queryDto.BackendTool);
        predicate = LeanExpressionExtensions.And(predicate, t => t.BackendTool == backendTool);
      }

      if (!string.IsNullOrEmpty(queryDto.FrontendTool))
      {
        var frontendTool = CleanInput(queryDto.FrontendTool);
        predicate = LeanExpressionExtensions.And(predicate, t => t.FrontendTool == frontendTool);
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

    /// <summary>
    /// 填充配置关联信息
    /// </summary>
    private async Task FillConfigRelationsAsync(List<LeanGenConfigDto> configs)
    {
      if (configs.Count == 0)
      {
        return;
      }

      var configIds = configs.Select(t => t.Id).ToList();
      var templates = await _templateRepository.GetListAsync(t => configIds.Contains(t.ConfigId));

      foreach (var config in configs)
      {
        config.Templates = templates.Where(t => t.ConfigId == config.Id)
            .Select(t => t.Adapt<LeanGenTemplateDto>())
            .ToList();
      }
    }
  }
}