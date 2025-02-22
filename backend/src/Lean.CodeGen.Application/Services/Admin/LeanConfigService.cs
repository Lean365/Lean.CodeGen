using System.Linq.Expressions;
using Lean.CodeGen.Application.Dtos.Admin;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Excel;
using Lean.CodeGen.Common.Exceptions;
using Lean.CodeGen.Common.Extensions;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Domain.Entities.Admin;
using Lean.CodeGen.Domain.Interfaces.Repositories;
using Lean.CodeGen.Application.Services.Base;
using Lean.CodeGen.Common.Options;
using Lean.CodeGen.Common.Security;
using Lean.CodeGen.Application.Services.Security;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Mapster;
using Lean.CodeGen.Domain.Validators;

namespace Lean.CodeGen.Application.Services.Admin;

/// <summary>
/// 系统配置服务实现
/// </summary>
public class LeanConfigService : LeanBaseService, ILeanConfigService
{
  private readonly ILogger<LeanConfigService> _logger;
  private readonly ILeanRepository<LeanConfig> _repository;
  private readonly LeanUniqueValidator<LeanConfig> _uniqueValidator;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanConfigService(
    ILogger<LeanConfigService> logger,
    ILeanRepository<LeanConfig> repository,
    ILeanSqlSafeService sqlSafeService,
    IOptions<LeanSecurityOptions> securityOptions)
    : base(sqlSafeService, securityOptions)
  {
    _logger = logger;
    _repository = repository;
    _uniqueValidator = new LeanUniqueValidator<LeanConfig>(repository);
  }

  /// <summary>
  /// 创建系统配置
  /// </summary>
  public async Task<LeanApiResult<long>> CreateAsync(LeanCreateConfigDto input)
  {
    try
    {
      // 检查配置键是否已存在
      var exists = await _repository.AnyAsync(x => x.ConfigKey == input.ConfigKey);
      if (exists)
      {
        return LeanApiResult<long>.Error($"配置键 {input.ConfigKey} 已存在");
      }

      // 创建实体
      var entity = input.Adapt<LeanConfig>();
      entity.IsBuiltin = LeanBuiltinStatus.No;

      // 保存数据
      var id = await _repository.CreateAsync(entity);
      return LeanApiResult<long>.Ok(id);
    }
    catch (Exception ex)
    {
      return LeanApiResult<long>.Error($"创建系统配置失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 更新系统配置
  /// </summary>
  public async Task<LeanApiResult> UpdateAsync(LeanUpdateConfigDto input)
  {
    try
    {
      // 获取原数据
      var entity = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
      if (entity == null)
      {
        return LeanApiResult.Error($"系统配置 {input.Id} 不存在");
      }

      // 检查是否为系统内置
      if (entity.IsBuiltin == LeanBuiltinStatus.Yes)
      {
        return LeanApiResult.Error("系统内置配置不允许修改");
      }

      // 检查配置键是否已存在
      var exists = await _repository.AnyAsync(x => x.ConfigKey == input.ConfigKey && x.Id != input.Id);
      if (exists)
      {
        return LeanApiResult.Error($"配置键 {input.ConfigKey} 已存在");
      }

      // 更新数据
      input.Adapt(entity);
      await _repository.UpdateAsync(entity);

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"更新系统配置失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 删除系统配置
  /// </summary>
  public async Task<LeanApiResult> DeleteAsync(List<long> ids)
  {
    try
    {
      if (ids == null || !ids.Any())
      {
        return LeanApiResult.Error("请选择要删除的数据");
      }

      // 检查是否包含系统内置配置
      var entities = await _repository.GetListAsync(x => ids.Contains(x.Id));
      if (entities.Any(x => x.IsBuiltin == LeanBuiltinStatus.Yes))
      {
        return LeanApiResult.Error("系统内置配置不允许删除");
      }

      // 删除数据
      await _repository.DeleteAsync(x => ids.Contains(x.Id));

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"删除系统配置失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取系统配置详情
  /// </summary>
  public async Task<LeanApiResult<LeanConfigDto>> GetAsync(long id)
  {
    try
    {
      var entity = await _repository.FirstOrDefaultAsync(x => x.Id == id);
      if (entity == null)
      {
        return LeanApiResult<LeanConfigDto>.Error($"系统配置 {id} 不存在");
      }

      var dto = entity.Adapt<LeanConfigDto>();
      return LeanApiResult<LeanConfigDto>.Ok(dto);
    }
    catch (Exception ex)
    {
      return LeanApiResult<LeanConfigDto>.Error($"获取系统配置详情失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 分页查询系统配置
  /// </summary>
  public async Task<LeanApiResult<LeanPageResult<LeanConfigDto>>> GetPagedListAsync(LeanQueryConfigDto input)
  {
    try
    {
      Expression<Func<LeanConfig, bool>> predicate = x => true;

      if (!string.IsNullOrEmpty(input.ConfigName))
      {
        var configName = CleanInput(input.ConfigName);
        predicate = LeanExpressionExtensions.And(predicate, x => x.ConfigName.Contains(configName));
      }

      if (!string.IsNullOrEmpty(input.ConfigKey))
      {
        var configKey = CleanInput(input.ConfigKey);
        predicate = LeanExpressionExtensions.And(predicate, x => x.ConfigKey.Contains(configKey));
      }

      if (input.ConfigType.HasValue)
      {
        predicate = LeanExpressionExtensions.And(predicate, x => x.ConfigType == input.ConfigType.Value);
      }

      if (!string.IsNullOrEmpty(input.ConfigGroup))
      {
        var configGroup = CleanInput(input.ConfigGroup);
        predicate = LeanExpressionExtensions.And(predicate, x => x.ConfigGroup == configGroup);
      }

      if (input.StartTime.HasValue)
      {
        predicate = LeanExpressionExtensions.And(predicate, x => x.CreateTime >= input.StartTime.Value);
      }

      if (input.EndTime.HasValue)
      {
        predicate = LeanExpressionExtensions.And(predicate, x => x.CreateTime <= input.EndTime.Value);
      }

      var (total, items) = await _repository.GetPageListAsync(predicate, input.PageSize, input.PageIndex);
      var dtos = items.Select(t => t.Adapt<LeanConfigDto>()).ToList();

      var result = new LeanPageResult<LeanConfigDto>
      {
        Total = total,
        Items = dtos,
        PageIndex = input.PageIndex,
        PageSize = input.PageSize
      };

      return LeanApiResult<LeanPageResult<LeanConfigDto>>.Ok(result);
    }
    catch (Exception ex)
    {
      return LeanApiResult<LeanPageResult<LeanConfigDto>>.Error($"分页查询系统配置失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 导出系统配置
  /// </summary>
  public async Task<LeanApiResult<byte[]>> ExportAsync(LeanQueryConfigDto input)
  {
    try
    {
      Expression<Func<LeanConfig, bool>> predicate = x => true;

      if (!string.IsNullOrEmpty(input.ConfigName))
      {
        var configName = CleanInput(input.ConfigName);
        predicate = LeanExpressionExtensions.And(predicate, x => x.ConfigName.Contains(configName));
      }

      if (!string.IsNullOrEmpty(input.ConfigKey))
      {
        var configKey = CleanInput(input.ConfigKey);
        predicate = LeanExpressionExtensions.And(predicate, x => x.ConfigKey.Contains(configKey));
      }

      if (input.ConfigType.HasValue)
      {
        predicate = LeanExpressionExtensions.And(predicate, x => x.ConfigType == input.ConfigType.Value);
      }

      if (!string.IsNullOrEmpty(input.ConfigGroup))
      {
        var configGroup = CleanInput(input.ConfigGroup);
        predicate = LeanExpressionExtensions.And(predicate, x => x.ConfigGroup == configGroup);
      }

      if (input.StartTime.HasValue)
      {
        predicate = LeanExpressionExtensions.And(predicate, x => x.CreateTime >= input.StartTime.Value);
      }

      if (input.EndTime.HasValue)
      {
        predicate = LeanExpressionExtensions.And(predicate, x => x.CreateTime <= input.EndTime.Value);
      }

      var items = await _repository.GetListAsync(predicate);
      var dtos = items.Select(t => t.Adapt<LeanConfigExportDto>()).ToList();

      var bytes = LeanExcelHelper.Export(dtos);
      return LeanApiResult<byte[]>.Ok(bytes);
    }
    catch (Exception ex)
    {
      return LeanApiResult<byte[]>.Error($"导出系统配置失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 导入系统配置
  /// </summary>
  public async Task<LeanApiResult<LeanExcelImportResult<LeanConfigImportDto>>> ImportAsync(LeanFileInfo file)
  {
    try
    {
      var result = new LeanExcelImportResult<LeanConfigImportDto>();

      // 导入Excel
      var importResult = LeanExcelHelper.Import<LeanConfigImportDto>(File.ReadAllBytes(file.FilePath));
      result.Data.AddRange(importResult.Data);
      result.Errors.AddRange(importResult.Errors);

      if (result.IsSuccess)
      {
        foreach (var item in result.Data)
        {
          try
          {
            // 检查配置键是否已存在
            var exists = await _repository.AnyAsync(x => x.ConfigKey == item.ConfigKey);
            if (exists)
            {
              result.Errors.Add(new LeanExcelImportError
              {
                RowIndex = importResult.Data.IndexOf(item) + 2,
                ErrorMessage = $"配置键 {item.ConfigKey} 已存在"
              });
              continue;
            }

            // 创建实体
            var entity = item.Adapt<LeanConfig>();
            entity.IsBuiltin = LeanBuiltinStatus.No;
            await _repository.CreateAsync(entity);
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

      return LeanApiResult<LeanExcelImportResult<LeanConfigImportDto>>.Ok(result);
    }
    catch (Exception ex)
    {
      return LeanApiResult<LeanExcelImportResult<LeanConfigImportDto>>.Error($"导入系统配置失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 下载导入模板
  /// </summary>
  public Task<LeanApiResult<byte[]>> GetImportTemplateAsync()
  {
    try
    {
      var template = new List<LeanConfigImportTemplateDto>
      {
        new LeanConfigImportTemplateDto()
      };

      var bytes = LeanExcelHelper.Export(template);
      return Task.FromResult(LeanApiResult<byte[]>.Ok(bytes));
    }
    catch (Exception ex)
    {
      return Task.FromResult(LeanApiResult<byte[]>.Error($"获取导入模板失败: {ex.Message}"));
    }
  }
}