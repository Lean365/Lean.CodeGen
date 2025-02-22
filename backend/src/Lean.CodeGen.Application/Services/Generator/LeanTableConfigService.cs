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
  /// 表配置关联服务实现
  /// </summary>
  public class LeanTableConfigService : LeanBaseService, ILeanTableConfigService
  {
    private readonly ILogger<LeanTableConfigService> _logger;
    private readonly ILeanRepository<LeanTableConfig> _repository;
    private readonly ILeanRepository<LeanDbTable> _dbTableRepository;
    private readonly ILeanRepository<LeanDataSource> _dataSourceRepository;
    private readonly LeanUniqueValidator<LeanTableConfig> _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    public LeanTableConfigService(
        ILeanRepository<LeanTableConfig> repository,
        ILeanRepository<LeanDbTable> dbTableRepository,
        ILeanRepository<LeanDataSource> dataSourceRepository,
        ILeanSqlSafeService sqlSafeService,
        IOptions<LeanSecurityOptions> securityOptions,
        ILogger<LeanTableConfigService> logger)
        : base(sqlSafeService, securityOptions, logger)
    {
      _logger = logger;
      _repository = repository;
      _dbTableRepository = dbTableRepository;
      _dataSourceRepository = dataSourceRepository;
      _uniqueValidator = new LeanUniqueValidator<LeanTableConfig>(_repository);
    }

    /// <summary>
    /// 获取表配置关联列表（分页）
    /// </summary>
    public async Task<LeanPageResult<LeanTableConfigDto>> GetPageListAsync(LeanTableConfigQueryDto queryDto)
    {
      var predicate = BuildQueryPredicate(queryDto);
      var (total, items) = await _repository.GetPageListAsync(predicate, queryDto.PageSize, queryDto.PageIndex);
      var list = items.Select(t => t.Adapt<LeanTableConfigDto>()).ToList();

      await FillTableConfigRelationsAsync(list);

      return new LeanPageResult<LeanTableConfigDto>
      {
        Total = total,
        Items = list
      };
    }

    /// <summary>
    /// 获取表配置关联详情
    /// </summary>
    public async Task<LeanTableConfigDto> GetAsync(long id)
    {
      var tableConfig = await _repository.FirstOrDefaultAsync(t => t.Id == id);
      if (tableConfig == null)
      {
        throw new Exception($"表配置关联 {id} 不存在");
      }

      var result = tableConfig.Adapt<LeanTableConfigDto>();
      await FillTableConfigRelationsAsync(new List<LeanTableConfigDto> { result });

      return result;
    }

    /// <summary>
    /// 创建表配置关联
    /// </summary>
    public async Task<LeanTableConfigDto> CreateAsync(LeanCreateTableConfigDto createDto)
    {
      // 查找表
      var table = await _dbTableRepository.FirstOrDefaultAsync(t => t.Id == createDto.TableId);
      if (table == null)
      {
        throw new Exception($"数据库表 {createDto.TableId} 不存在");
      }

      // 查找配置
      var config = await _dataSourceRepository.FirstOrDefaultAsync(t => t.Id == createDto.ConfigId);
      if (config == null)
      {
        throw new Exception($"代码生成配置 {createDto.ConfigId} 不存在");
      }

      // 检查是否已存在关联
      var exists = await _repository.AnyAsync(t => t.TableId == createDto.TableId && t.ConfigId == createDto.ConfigId);
      if (exists)
      {
        throw new Exception($"表 {createDto.TableId} 与配置 {createDto.ConfigId} 的关联已存在");
      }

      var entity = createDto.Adapt<LeanTableConfig>();
      entity.CreateTime = DateTime.Now;

      await _repository.CreateAsync(entity);

      var result = entity.Adapt<LeanTableConfigDto>();
      await FillTableConfigRelationsAsync(new List<LeanTableConfigDto> { result });

      return result;
    }

    /// <summary>
    /// 更新表配置关联
    /// </summary>
    public async Task<LeanTableConfigDto> UpdateAsync(long id, LeanUpdateTableConfigDto updateDto)
    {
      var entity = await _repository.FirstOrDefaultAsync(t => t.Id == id);
      if (entity == null)
      {
        throw new Exception($"表配置关联 {id} 不存在");
      }

      // 查找表
      var table = await _dbTableRepository.FirstOrDefaultAsync(t => t.Id == updateDto.TableId);
      if (table == null)
      {
        throw new Exception($"数据库表 {updateDto.TableId} 不存在");
      }

      // 查找配置
      var config = await _dataSourceRepository.FirstOrDefaultAsync(t => t.Id == updateDto.ConfigId);
      if (config == null)
      {
        throw new Exception($"代码生成配置 {updateDto.ConfigId} 不存在");
      }

      // 检查是否已存在关联
      var exists = await _repository.AnyAsync(t => t.Id != id && t.TableId == updateDto.TableId && t.ConfigId == updateDto.ConfigId);
      if (exists)
      {
        throw new Exception($"表 {updateDto.TableId} 与配置 {updateDto.ConfigId} 的关联已存在");
      }

      updateDto.Adapt(entity);
      await _repository.UpdateAsync(entity);

      var result = entity.Adapt<LeanTableConfigDto>();
      await FillTableConfigRelationsAsync(new List<LeanTableConfigDto> { result });

      return result;
    }

    /// <summary>
    /// 删除表配置关联
    /// </summary>
    public async Task<bool> DeleteAsync(long id)
    {
      var tableConfig = await _repository.FirstOrDefaultAsync(t => t.Id == id);
      if (tableConfig == null)
      {
        throw new Exception($"表配置关联 {id} 不存在");
      }

      return await _repository.DeleteAsync(tableConfig);
    }

    /// <summary>
    /// 导出表配置关联
    /// </summary>
    public async Task<LeanFileResult> ExportAsync(LeanTableConfigQueryDto queryDto)
    {
      var predicate = BuildQueryPredicate(queryDto);
      var items = await _repository.GetListAsync(predicate);
      var list = items.Select(t => t.Adapt<LeanTableConfigExportDto>()).ToList();

      var excelBytes = LeanExcelHelper.Export(list);
      return new LeanFileResult
      {
        FileName = $"表配置关联_{DateTime.Now:yyyyMMddHHmmss}.xlsx",
        ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        Stream = new MemoryStream(excelBytes)
      };
    }

    /// <summary>
    /// 导入表配置关联
    /// </summary>
    public async Task<LeanExcelImportResult<LeanTableConfigImportDto>> ImportAsync(LeanFileInfo file)
    {
      var result = new LeanExcelImportResult<LeanTableConfigImportDto>();

      try
      {
        var importResult = LeanExcelHelper.Import<LeanTableConfigImportDto>(File.ReadAllBytes(file.FilePath));
        result.Data.AddRange(importResult.Data);
        result.Errors.AddRange(importResult.Errors);

        if (result.IsSuccess)
        {
          foreach (var item in result.Data)
          {
            try
            {
              var createDto = item.Adapt<LeanCreateTableConfigDto>();
              await CreateAsync(createDto);
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
      var template = new List<LeanTableConfigImportTemplateDto>
      {
        new LeanTableConfigImportTemplateDto()
      };

      var excelBytes = LeanExcelHelper.Export(template);
      var result = new LeanFileResult
      {
        FileName = "表配置关联导入模板.xlsx",
        ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        Stream = new MemoryStream(excelBytes)
      };

      return Task.FromResult(result);
    }

    /// <summary>
    /// 构建查询条件
    /// </summary>
    private Expression<Func<LeanTableConfig, bool>> BuildQueryPredicate(LeanTableConfigQueryDto queryDto)
    {
      Expression<Func<LeanTableConfig, bool>> predicate = t => true;

      if (queryDto.TableId.HasValue)
      {
        predicate = LeanExpressionExtensions.And(predicate, t => t.TableId == queryDto.TableId.Value);
      }

      if (queryDto.ConfigId.HasValue)
      {
        predicate = LeanExpressionExtensions.And(predicate, t => t.ConfigId == queryDto.ConfigId.Value);
      }

      if (!string.IsNullOrEmpty(queryDto.EntityName))
      {
        predicate = LeanExpressionExtensions.And(predicate, t => t.EntityName.Contains(queryDto.EntityName));
      }

      if (!string.IsNullOrEmpty(queryDto.BusinessName))
      {
        predicate = LeanExpressionExtensions.And(predicate, t => t.BusinessName.Contains(queryDto.BusinessName));
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
    /// 填充表配置关联信息
    /// </summary>
    private async Task FillTableConfigRelationsAsync(List<LeanTableConfigDto> tableConfigs)
    {
      if (tableConfigs.Count == 0)
      {
        return;
      }

      var tableIds = tableConfigs.Select(t => t.TableId).Distinct().ToList();
      var tables = await _dbTableRepository.GetListAsync(t => tableIds.Contains(t.Id));

      var configIds = tableConfigs.Select(t => t.ConfigId).Distinct().ToList();
      var configs = await _dataSourceRepository.GetListAsync(t => configIds.Contains(t.Id));

      foreach (var tableConfig in tableConfigs)
      {
        var table = tables.FirstOrDefault(t => t.Id == tableConfig.TableId);
        if (table != null)
        {
          tableConfig.TableName = table.TableName;
        }

        var config = configs.FirstOrDefault(t => t.Id == tableConfig.ConfigId);
        if (config != null)
        {
          tableConfig.ConfigName = config.Name;
        }
      }
    }
  }
}