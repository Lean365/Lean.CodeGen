using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mapster;
using Microsoft.Extensions.Options;
using Lean.CodeGen.Application.Dtos.Generator;
using Lean.CodeGen.Common.Excel;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Domain.Entities.Generator;
using Lean.CodeGen.Domain.Interfaces.Repositories;
using Lean.CodeGen.Application.Services.Base;
using Lean.CodeGen.Common.Options;
using Lean.CodeGen.Application.Services.Security;
using Lean.CodeGen.Common.Extensions;

namespace Lean.CodeGen.Application.Services.Generator
{
  /// <summary>
  /// 数据源管理服务实现
  /// </summary>
  public class LeanDataSourceService : LeanBaseService, ILeanDataSourceService
  {
    private readonly ILeanRepository<LeanDataSource> _dataSourceRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    public LeanDataSourceService(
        ILeanRepository<LeanDataSource> dataSourceRepository,
        ILeanSqlSafeService sqlSafeService,
        IOptions<LeanSecurityOptions> securityOptions)
        : base(sqlSafeService, securityOptions)
    {
      _dataSourceRepository = dataSourceRepository;
    }

    /// <summary>
    /// 获取数据源列表（分页）
    /// </summary>
    public async Task<LeanPageResult<LeanDataSourceDto>> GetPageListAsync(LeanDataSourceQueryDto queryDto)
    {
      var predicate = BuildQueryPredicate(queryDto);
      var (total, items) = await _dataSourceRepository.GetPageListAsync(predicate, queryDto.PageSize, queryDto.PageIndex);
      var list = items.Select(t => t.Adapt<LeanDataSourceDto>()).ToList();

      return new LeanPageResult<LeanDataSourceDto>
      {
        Total = total,
        Items = list
      };
    }

    /// <summary>
    /// 获取数据源详情
    /// </summary>
    public async Task<LeanDataSourceDto> GetAsync(long id)
    {
      var dataSource = await _dataSourceRepository.FirstOrDefaultAsync(t => t.Id == id);
      if (dataSource == null)
      {
        throw new Exception($"数据源 {id} 不存在");
      }

      return dataSource.Adapt<LeanDataSourceDto>();
    }

    /// <summary>
    /// 创建数据源
    /// </summary>
    public async Task<LeanDataSourceDto> CreateAsync(LeanCreateDataSourceDto createDto)
    {
      var entity = createDto.Adapt<LeanDataSource>();
      entity.CreateTime = DateTime.Now;

      var id = await _dataSourceRepository.CreateAsync(entity);
      return await GetAsync(id);
    }

    /// <summary>
    /// 更新数据源
    /// </summary>
    public async Task<LeanDataSourceDto> UpdateAsync(long id, LeanUpdateDataSourceDto updateDto)
    {
      var entity = await _dataSourceRepository.FirstOrDefaultAsync(t => t.Id == id);
      if (entity == null)
      {
        throw new Exception($"数据源 {id} 不存在");
      }

      updateDto.Adapt(entity);
      entity.UpdateTime = DateTime.Now;

      await _dataSourceRepository.UpdateAsync(entity);
      return await GetAsync(id);
    }

    /// <summary>
    /// 删除数据源
    /// </summary>
    public async Task<bool> DeleteAsync(long id)
    {
      var dataSource = await _dataSourceRepository.FirstOrDefaultAsync(t => t.Id == id);
      if (dataSource == null)
      {
        throw new Exception($"数据源 {id} 不存在");
      }

      return await _dataSourceRepository.DeleteAsync(t => t.Id == id);
    }

    /// <summary>
    /// 导出数据源
    /// </summary>
    public async Task<LeanFileResult> ExportAsync(LeanDataSourceQueryDto queryDto)
    {
      var predicate = BuildQueryPredicate(queryDto);
      var items = await _dataSourceRepository.GetListAsync(predicate);
      var list = items.Select(t => t.Adapt<LeanDataSourceExportDto>()).ToList();

      var excelBytes = LeanExcelHelper.Export(list);
      return new LeanFileResult
      {
        FileName = $"数据源_{DateTime.Now:yyyyMMddHHmmss}.xlsx",
        ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        Stream = new MemoryStream(excelBytes)
      };
    }

    /// <summary>
    /// 导入数据源
    /// </summary>
    public async Task<LeanExcelImportResult<LeanDataSourceImportDto>> ImportAsync(LeanFileInfo file)
    {
      var result = new LeanExcelImportResult<LeanDataSourceImportDto>();

      try
      {
        var importResult = LeanExcelHelper.Import<LeanDataSourceImportDto>(File.ReadAllBytes(file.FilePath));
        foreach (var item in importResult.Data)
        {
          try
          {
            var entity = item.Adapt<LeanDataSource>();
            entity.CreateTime = DateTime.Now;

            await _dataSourceRepository.CreateAsync(entity);
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
      var template = new List<LeanDataSourceImportTemplateDto>
      {
        new LeanDataSourceImportTemplateDto()
      };

      var excelBytes = LeanExcelHelper.Export(template);
      var result = new LeanFileResult
      {
        FileName = "数据源导入模板.xlsx",
        ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        Stream = new MemoryStream(excelBytes)
      };

      return Task.FromResult(result);
    }

    /// <summary>
    /// 测试连接
    /// </summary>
    public async Task<bool> TestConnectionAsync(long id)
    {
      var dataSource = await _dataSourceRepository.FirstOrDefaultAsync(t => t.Id == id);
      if (dataSource == null)
      {
        throw new Exception($"数据源 {id} 不存在");
      }

      // TODO: 实现数据库连接测试逻辑
      return true;
    }

    /// <summary>
    /// 构建查询条件
    /// </summary>
    private Expression<Func<LeanDataSource, bool>> BuildQueryPredicate(LeanDataSourceQueryDto queryDto)
    {
      Expression<Func<LeanDataSource, bool>> predicate = t => true;

      if (!string.IsNullOrEmpty(queryDto.Name))
      {
        var name = CleanInput(queryDto.Name);
        predicate = LeanExpressionExtensions.And(predicate, t => t.Name.Contains(name));
      }

      if (!string.IsNullOrEmpty(queryDto.DbType))
      {
        var dbType = CleanInput(queryDto.DbType);
        predicate = LeanExpressionExtensions.And(predicate, t => t.DbType == dbType);
      }

      if (queryDto.IsEnabled.HasValue)
      {
        predicate = LeanExpressionExtensions.And(predicate, t => t.IsEnabled == queryDto.IsEnabled);
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
