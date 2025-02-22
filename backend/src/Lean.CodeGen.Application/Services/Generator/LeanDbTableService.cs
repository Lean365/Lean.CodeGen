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
using Microsoft.Extensions.Logging;
using Lean.CodeGen.Domain.Validators;

namespace Lean.CodeGen.Application.Services.Generator
{
  /// <summary>
  /// 数据库表管理服务实现
  /// </summary>
  public class LeanDbTableService : LeanBaseService, ILeanDbTableService
  {
    private readonly ILogger<LeanDbTableService> _logger;
    private readonly ILeanRepository<LeanDbTable> _repository;
    private readonly ILeanRepository<LeanDbColumn> _columnRepository;
    private readonly ILeanRepository<LeanDataSource> _dataSourceRepository;
    private readonly LeanUniqueValidator<LeanDbTable> _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    public LeanDbTableService(
        ILeanRepository<LeanDbTable> repository,
        ILeanRepository<LeanDbColumn> columnRepository,
        ILeanRepository<LeanDataSource> dataSourceRepository,
        ILeanSqlSafeService sqlSafeService,
        IOptions<LeanSecurityOptions> securityOptions,
        ILogger<LeanDbTableService> logger)
        : base(sqlSafeService, securityOptions, logger)
    {
      _logger = logger;
      _repository = repository;
      _columnRepository = columnRepository;
      _dataSourceRepository = dataSourceRepository;
      _uniqueValidator = new LeanUniqueValidator<LeanDbTable>(_repository);
    }

    /// <summary>
    /// 获取数据库表列表（分页）
    /// </summary>
    public async Task<LeanPageResult<LeanDbTableDto>> GetPageListAsync(LeanDbTableQueryDto queryDto)
    {
      var predicate = BuildQueryPredicate(queryDto);
      var (total, items) = await _repository.GetPageListAsync(predicate, queryDto.PageSize, queryDto.PageIndex);
      var list = items.Select(t => t.Adapt<LeanDbTableDto>()).ToList();

      await FillTableRelationsAsync(list);

      return new LeanPageResult<LeanDbTableDto>
      {
        Total = total,
        Items = list
      };
    }

    /// <summary>
    /// 获取数据库表详情
    /// </summary>
    public async Task<LeanDbTableDto> GetAsync(long id)
    {
      var table = await _repository.FirstOrDefaultAsync(t => t.Id == id);
      if (table == null)
      {
        throw new Exception($"数据库表 {id} 不存在");
      }

      return table.Adapt<LeanDbTableDto>();
    }

    /// <summary>
    /// 创建数据库表
    /// </summary>
    public async Task<LeanDbTableDto> CreateAsync(LeanCreateDbTableDto createDto)
    {
      var entity = createDto.Adapt<LeanDbTable>();
      entity.CreateTime = DateTime.Now;

      await _repository.CreateAsync(entity);

      if (createDto.Columns?.Count > 0)
      {
        var columns = createDto.Columns.Select(t =>
        {
          var column = t.Adapt<LeanDbColumn>();
          column.TableId = entity.Id;
          column.CreateTime = DateTime.Now;
          return column;
        }).ToList();

        await _columnRepository.CreateRangeAsync(columns);
      }

      return await GetAsync(long.Parse(entity.Id.ToString()));
    }

    /// <summary>
    /// 更新数据库表
    /// </summary>
    public async Task<LeanDbTableDto> UpdateAsync(long id, LeanUpdateDbTableDto updateDto)
    {
      var entity = await _repository.FirstOrDefaultAsync(t => t.Id == id);
      if (entity == null)
      {
        throw new Exception($"数据库表 {id} 不存在");
      }

      updateDto.Adapt(entity);
      entity.UpdateTime = DateTime.Now;

      await _repository.UpdateAsync(entity);

      // 删除原有的列
      await _columnRepository.DeleteAsync(t => t.TableId == entity.Id);

      // 创建新的列
      if (updateDto.Columns?.Count > 0)
      {
        var columns = updateDto.Columns.Select(t =>
        {
          var column = t.Adapt<LeanDbColumn>();
          column.TableId = entity.Id;
          column.CreateTime = DateTime.Now;
          return column;
        }).ToList();

        await _columnRepository.CreateRangeAsync(columns);
      }

      return await GetAsync(id);
    }

    /// <summary>
    /// 删除数据库表
    /// </summary>
    public async Task<bool> DeleteAsync(long id)
    {
      var table = await _repository.FirstOrDefaultAsync(t => t.Id == id);
      if (table == null)
      {
        throw new Exception($"数据库表 {id} 不存在");
      }

      // 删除关联的列
      await _columnRepository.DeleteAsync(t => t.TableId == table.Id);

      return await _repository.DeleteAsync(t => t.Id == table.Id);
    }

    /// <summary>
    /// 导出数据库表
    /// </summary>
    public async Task<LeanFileResult> ExportAsync(LeanDbTableQueryDto queryDto)
    {
      var predicate = BuildQueryPredicate(queryDto);
      var items = await _repository.GetListAsync(predicate);
      var list = items.Select(t => t.Adapt<LeanDbTableExportDto>()).ToList();

      var excelBytes = LeanExcelHelper.Export(list);
      return new LeanFileResult
      {
        FileName = $"数据库表_{DateTime.Now:yyyyMMddHHmmss}.xlsx",
        ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        Stream = new MemoryStream(excelBytes)
      };
    }

    /// <summary>
    /// 导入数据库表
    /// </summary>
    public async Task<LeanExcelImportResult<LeanDbTableImportDto>> ImportAsync(LeanFileInfo file)
    {
      var result = new LeanExcelImportResult<LeanDbTableImportDto>();

      try
      {
        var importResult = LeanExcelHelper.Import<LeanDbTableImportDto>(File.ReadAllBytes(file.FilePath));
        foreach (var item in importResult.Data)
        {
          try
          {
            var entity = item.Adapt<LeanDbTable>();
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
      var template = new List<LeanDbTableImportTemplateDto>
      {
        new LeanDbTableImportTemplateDto()
      };

      var excelBytes = LeanExcelHelper.Export(template);
      var result = new LeanFileResult
      {
        FileName = "数据库表导入模板.xlsx",
        ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        Stream = new MemoryStream(excelBytes)
      };

      return Task.FromResult(result);
    }

    /// <summary>
    /// 从数据源导入表
    /// </summary>
    public async Task<List<LeanDbTableDto>> ImportFromDataSourceAsync(long dataSourceId)
    {
      var dataSource = await _dataSourceRepository.FirstOrDefaultAsync(t => t.Id == dataSourceId);
      if (dataSource == null)
      {
        throw new Exception($"数据源 {dataSourceId} 不存在");
      }

      // TODO: 实现从数据源导入表结构的逻辑
      return new List<LeanDbTableDto>();
    }

    /// <summary>
    /// 同步表结构
    /// </summary>
    public async Task<bool> SyncStructureAsync(long id)
    {
      var table = await _repository.FirstOrDefaultAsync(t => t.Id == id);
      if (table == null)
      {
        throw new Exception($"数据库表 {id} 不存在");
      }

      // TODO: 实现同步表结构的逻辑
      return true;
    }

    /// <summary>
    /// 构建查询条件
    /// </summary>
    private Expression<Func<LeanDbTable, bool>> BuildQueryPredicate(LeanDbTableQueryDto queryDto)
    {
      Expression<Func<LeanDbTable, bool>> predicate = t => true;

      if (!string.IsNullOrEmpty(queryDto.TableName))
      {
        var tableName = CleanInput(queryDto.TableName);
        predicate = LeanExpressionExtensions.And(predicate, t => t.TableName.Contains(tableName));
      }

      if (!string.IsNullOrEmpty(queryDto.TableComment))
      {
        var tableComment = CleanInput(queryDto.TableComment);
        predicate = LeanExpressionExtensions.And(predicate, t => t.TableComment.Contains(tableComment));
      }

      if (queryDto.DataSourceId.HasValue)
      {
        predicate = LeanExpressionExtensions.And(predicate, t => t.DataSourceId == queryDto.DataSourceId);
      }

      if (queryDto.SourceType.HasValue)
      {
        predicate = LeanExpressionExtensions.And(predicate, t => t.SourceType == queryDto.SourceType);
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
    /// 填充表关联信息
    /// </summary>
    private async Task FillTableRelationsAsync(List<LeanDbTableDto> tables)
    {
      if (tables.Count == 0)
      {
        return;
      }

      var tableIds = tables.Select(t => t.Id).ToList();
      var columns = await _columnRepository.GetListAsync(t => tableIds.Contains(t.TableId));

      foreach (var table in tables)
      {
        table.Columns = columns.Where(t => t.TableId == table.Id)
            .Select(t => t.Adapt<LeanDbColumnDto>())
            .ToList();
      }

      // 填充数据源名称
      var dataSourceIds = tables.Select(t => t.DataSourceId).Distinct().ToList();
      var dataSources = await _dataSourceRepository.GetListAsync(t => dataSourceIds.Contains(t.Id));

      foreach (var table in tables)
      {
        var dataSource = dataSources.FirstOrDefault(t => t.Id == table.DataSourceId);
        if (dataSource != null)
        {
          table.DataSourceName = dataSource.Name;
        }
      }
    }
  }
}