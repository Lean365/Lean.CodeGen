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
/// 字典类型服务实现
/// </summary>
public class LeanDictTypeService : LeanBaseService, ILeanDictTypeService
{
  private readonly ILogger<LeanDictTypeService> _logger;
  private readonly ILeanRepository<LeanDictType> _dictTypeRepository;
  private readonly ILeanRepository<LeanDictData> _dictDataRepository;
  private readonly LeanUniqueValidator<LeanDictType> _uniqueValidator;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanDictTypeService(
      ILogger<LeanDictTypeService> logger,
      ILeanRepository<LeanDictType> dictTypeRepository,
      ILeanRepository<LeanDictData> dictDataRepository,
      ILeanSqlSafeService sqlSafeService,
      IOptions<LeanSecurityOptions> securityOptions)
      : base(sqlSafeService, securityOptions, logger)
  {
    _logger = logger;
    _dictTypeRepository = dictTypeRepository;
    _dictDataRepository = dictDataRepository;
    _uniqueValidator = new LeanUniqueValidator<LeanDictType>(_dictTypeRepository);
  }

  /// <summary>
  /// 创建字典类型
  /// </summary>
  public async Task<LeanApiResult<long>> CreateAsync(LeanCreateDictTypeDto input)
  {
    try
    {
      // 检查字典类型编码是否存在
      var exists = await _dictTypeRepository.AnyAsync(x => x.DictCode == input.DictCode);
      if (exists)
      {
        return LeanApiResult<long>.Error($"字典类型编码 {input.DictCode} 已存在");
      }

      var entity = input.Adapt<LeanDictType>();
      entity.Status = LeanStatus.Normal;
      await _dictTypeRepository.CreateAsync(entity);

      return LeanApiResult<long>.Ok(entity.Id);
    }
    catch (Exception ex)
    {
      return LeanApiResult<long>.Error($"创建字典类型失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 更新字典类型
  /// </summary>
  public async Task<LeanApiResult> UpdateAsync(LeanUpdateDictTypeDto input)
  {
    try
    {
      var entity = await _dictTypeRepository.GetByIdAsync(input.Id);
      if (entity == null)
      {
        return LeanApiResult.Error($"字典类型 {input.Id} 不存在");
      }

      if (entity.IsBuiltin == LeanBuiltinStatus.Yes)
      {
        return LeanApiResult.Error("内置字典类型不允许修改");
      }

      // 检查字典类型编码是否存在
      var exists = await _dictTypeRepository.AnyAsync(x => x.DictCode == input.DictCode && x.Id != input.Id);
      if (exists)
      {
        return LeanApiResult.Error($"字典类型编码 {input.DictCode} 已存在");
      }

      input.Adapt(entity);
      await _dictTypeRepository.UpdateAsync(entity);

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"更新字典类型失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 删除字典类型
  /// </summary>
  public async Task<LeanApiResult> DeleteAsync(long id)
  {
    try
    {
      var entity = await _dictTypeRepository.GetByIdAsync(id);
      if (entity == null)
      {
        return LeanApiResult.Error($"字典类型 {id} 不存在");
      }

      if (entity.IsBuiltin == LeanBuiltinStatus.Yes)
      {
        return LeanApiResult.Error("内置字典类型不允许删除");
      }

      // 删除字典类型下的所有字典数据
      await _dictDataRepository.DeleteAsync(x => x.TypeId == id);
      await _dictTypeRepository.DeleteAsync(entity);

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"删除字典类型失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 批量删除字典类型
  /// </summary>
  public async Task<LeanApiResult> BatchDeleteAsync(List<long> ids)
  {
    try
    {
      var entities = await _dictTypeRepository.GetListAsync(x => ids.Contains(x.Id));
      if (entities.Any(x => x.IsBuiltin == LeanBuiltinStatus.Yes))
      {
        return LeanApiResult.Error("选中的字典类型中包含内置字典类型，不允许删除");
      }

      // 删除字典类型下的所有字典数据
      await _dictDataRepository.DeleteAsync(x => ids.Contains(x.TypeId));
      await _dictTypeRepository.DeleteAsync(x => ids.Contains(x.Id));

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"批量删除字典类型失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取字典类型信息
  /// </summary>
  public async Task<LeanApiResult<LeanDictTypeDto>> GetAsync(long id)
  {
    try
    {
      var entity = await _dictTypeRepository.GetByIdAsync(id);
      if (entity == null)
      {
        return LeanApiResult<LeanDictTypeDto>.Error($"字典类型 {id} 不存在");
      }

      var dto = entity.Adapt<LeanDictTypeDto>();
      return LeanApiResult<LeanDictTypeDto>.Ok(dto);
    }
    catch (Exception ex)
    {
      return LeanApiResult<LeanDictTypeDto>.Error($"获取字典类型信息失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 分页查询字典类型
  /// </summary>
  public async Task<LeanApiResult<LeanPageResult<LeanDictTypeDto>>> GetPageAsync(LeanQueryDictTypeDto input)
  {
    try
    {
      Expression<Func<LeanDictType, bool>> predicate = x => true;

      if (!string.IsNullOrEmpty(input.DictName))
      {
        var name = input.DictName.Trim();
        predicate = predicate.And(x => x.DictName.Contains(name));
      }

      if (!string.IsNullOrEmpty(input.DictCode))
      {
        var code = input.DictCode.Trim();
        predicate = predicate.And(x => x.DictCode.Contains(code));
      }

      if (input.Status.HasValue)
      {
        predicate = LeanExpressionExtensions.And(predicate, x => x.Status == input.Status.Value);
      }

      if (input.StartTime.HasValue)
      {
        predicate = LeanExpressionExtensions.And(predicate, x => x.CreateTime >= input.StartTime.Value);
      }

      if (input.EndTime.HasValue)
      {
        predicate = LeanExpressionExtensions.And(predicate, x => x.CreateTime <= input.EndTime.Value);
      }

      var (total, items) = await _dictTypeRepository.GetPageListAsync(predicate, input.PageSize, input.PageIndex);
      var dtos = items.Select(t => t.Adapt<LeanDictTypeDto>()).ToList();

      var result = new LeanPageResult<LeanDictTypeDto>
      {
        Total = total,
        Items = dtos,
        PageIndex = input.PageIndex,
        PageSize = input.PageSize
      };

      return LeanApiResult<LeanPageResult<LeanDictTypeDto>>.Ok(result);
    }
    catch (Exception ex)
    {
      return LeanApiResult<LeanPageResult<LeanDictTypeDto>>.Error($"分页查询字典类型失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 设置字典类型状态
  /// </summary>
  public async Task<LeanApiResult> SetStatusAsync(LeanChangeDictTypeStatusDto input)
  {
    try
    {
      var entity = await _dictTypeRepository.GetByIdAsync(input.Id);
      if (entity == null)
      {
        return LeanApiResult.Error($"字典类型 {input.Id} 不存在");
      }

      if (entity.IsBuiltin == LeanBuiltinStatus.Yes)
      {
        return LeanApiResult.Error("内置字典类型不允许修改状态");
      }

      entity.Status = input.Status;
      await _dictTypeRepository.UpdateAsync(entity);

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"设置字典类型状态失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 导出字典类型
  /// </summary>
  public async Task<LeanApiResult<byte[]>> ExportAsync(LeanQueryDictTypeDto input)
  {
    try
    {
      Expression<Func<LeanDictType, bool>> predicate = x => true;

      if (!string.IsNullOrEmpty(input.DictName))
      {
        var name = input.DictName.Trim();
        predicate = predicate.And(x => x.DictName.Contains(name));
      }

      if (!string.IsNullOrEmpty(input.DictCode))
      {
        var code = input.DictCode.Trim();
        predicate = predicate.And(x => x.DictCode.Contains(code));
      }

      if (input.Status.HasValue)
      {
        predicate = LeanExpressionExtensions.And(predicate, x => x.Status == input.Status.Value);
      }

      if (input.StartTime.HasValue)
      {
        predicate = LeanExpressionExtensions.And(predicate, x => x.CreateTime >= input.StartTime.Value);
      }

      if (input.EndTime.HasValue)
      {
        predicate = LeanExpressionExtensions.And(predicate, x => x.CreateTime <= input.EndTime.Value);
      }

      var items = await _dictTypeRepository.GetListAsync(predicate);
      var dtos = items.Select(t => t.Adapt<LeanDictTypeExportDto>()).ToList();

      var bytes = LeanExcelHelper.Export(dtos);
      return LeanApiResult<byte[]>.Ok(bytes);
    }
    catch (Exception ex)
    {
      return LeanApiResult<byte[]>.Error($"导出字典类型失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 导入字典类型
  /// </summary>
  public async Task<LeanApiResult<LeanExcelImportResult<LeanDictTypeImportDto>>> ImportAsync(LeanFileInfo file)
  {
    try
    {
      var result = new LeanExcelImportResult<LeanDictTypeImportDto>();

      // 导入Excel
      var importResult = LeanExcelHelper.Import<LeanDictTypeImportDto>(File.ReadAllBytes(file.FilePath));
      result.Data.AddRange(importResult.Data);
      result.Errors.AddRange(importResult.Errors);

      if (result.IsSuccess)
      {
        foreach (var item in result.Data)
        {
          try
          {
            // 检查字典类型编码是否存在
            var exists = await _dictTypeRepository.AnyAsync(x => x.DictCode == item.DictCode);
            if (exists)
            {
              result.Errors.Add(new LeanExcelImportError
              {
                RowIndex = importResult.Data.IndexOf(item) + 2,
                ErrorMessage = $"字典类型编码 {item.DictCode} 已存在"
              });
              continue;
            }

            // 创建实体
            var entity = item.Adapt<LeanDictType>();
            entity.Status = LeanStatus.Normal;
            await _dictTypeRepository.CreateAsync(entity);
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

      return LeanApiResult<LeanExcelImportResult<LeanDictTypeImportDto>>.Ok(result);
    }
    catch (Exception ex)
    {
      return LeanApiResult<LeanExcelImportResult<LeanDictTypeImportDto>>.Error($"导入字典类型失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取导入模板
  /// </summary>
  public Task<LeanApiResult<byte[]>> GetImportTemplateAsync()
  {
    try
    {
      var template = new List<LeanDictTypeImportTemplateDto>
      {
        new LeanDictTypeImportTemplateDto()
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