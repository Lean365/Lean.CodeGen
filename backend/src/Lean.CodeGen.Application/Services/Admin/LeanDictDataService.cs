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
/// 字典数据服务实现
/// </summary>
public class LeanDictDataService : LeanBaseService, ILeanDictDataService
{
  private readonly ILogger<LeanDictDataService> _logger;
  private readonly ILeanRepository<LeanDictData> _dictDataRepository;
  private readonly ILeanRepository<LeanDictType> _dictTypeRepository;
  private readonly LeanUniqueValidator<LeanDictData> _uniqueValidator;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanDictDataService(
      ILogger<LeanDictDataService> logger,
      ILeanRepository<LeanDictData> dictDataRepository,
      ILeanRepository<LeanDictType> dictTypeRepository,
      ILeanSqlSafeService sqlSafeService,
      IOptions<LeanSecurityOptions> securityOptions)
      : base(sqlSafeService, securityOptions, logger)
  {
    _logger = logger;
    _dictDataRepository = dictDataRepository;
    _dictTypeRepository = dictTypeRepository;
    _uniqueValidator = new LeanUniqueValidator<LeanDictData>(_dictDataRepository);
  }

  /// <summary>
  /// 创建字典数据
  /// </summary>
  public async Task<LeanApiResult<long>> CreateAsync(LeanCreateDictDataDto input)
  {
    try
    {
      // 检查字典类型是否存在
      var dictType = await _dictTypeRepository.FirstOrDefaultAsync(x => x.Id == input.TypeId);
      if (dictType == null)
      {
        return LeanApiResult<long>.Error($"字典类型 {input.TypeId} 不存在");
      }

      // 检查字典键是否存在
      var exists = await _dictDataRepository.AnyAsync(x => x.TypeId == input.TypeId && x.DictValue == input.DictValue);
      if (exists)
      {
        return LeanApiResult<long>.Error($"字典键值 {input.DictValue} 已存在");
      }

      var entity = input.Adapt<LeanDictData>();
      entity.Status = LeanStatus.Normal;
      await _dictDataRepository.CreateAsync(entity);

      return LeanApiResult<long>.Ok(entity.Id);
    }
    catch (Exception ex)
    {
      return LeanApiResult<long>.Error($"创建字典数据失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 更新字典数据
  /// </summary>
  public async Task<LeanApiResult> UpdateAsync(LeanUpdateDictDataDto input)
  {
    try
    {
      var entity = await _dictDataRepository.GetByIdAsync(input.Id);
      if (entity == null)
      {
        return LeanApiResult.Error($"字典数据 {input.Id} 不存在");
      }

      if (entity.IsBuiltin == LeanBuiltinStatus.Yes)
      {
        return LeanApiResult.Error("内置字典数据不允许修改");
      }

      // 检查字典类型是否存在
      var dictType = await _dictTypeRepository.FirstOrDefaultAsync(x => x.Id == input.TypeId);
      if (dictType == null)
      {
        return LeanApiResult.Error($"字典类型 {input.TypeId} 不存在");
      }

      // 检查字典键是否存在
      var exists = await _dictDataRepository.AnyAsync(x => x.TypeId == input.TypeId && x.DictValue == input.DictValue && x.Id != input.Id);
      if (exists)
      {
        return LeanApiResult.Error($"字典键值 {input.DictValue} 已存在");
      }

      input.Adapt(entity);
      await _dictDataRepository.UpdateAsync(entity);

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"更新字典数据失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 删除字典数据
  /// </summary>
  public async Task<LeanApiResult> DeleteAsync(long id)
  {
    try
    {
      var entity = await _dictDataRepository.GetByIdAsync(id);
      if (entity == null)
      {
        return LeanApiResult.Error($"字典数据 {id} 不存在");
      }

      if (entity.IsBuiltin == LeanBuiltinStatus.Yes)
      {
        return LeanApiResult.Error("内置字典数据不允许删除");
      }

      await _dictDataRepository.DeleteAsync(entity);

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"删除字典数据失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 批量删除字典数据
  /// </summary>
  public async Task<LeanApiResult> BatchDeleteAsync(List<long> ids)
  {
    try
    {
      var entities = await _dictDataRepository.GetListAsync(x => ids.Contains(x.Id));
      if (entities.Any(x => x.IsBuiltin == LeanBuiltinStatus.Yes))
      {
        return LeanApiResult.Error("选中的字典数据中包含内置字典数据，不允许删除");
      }

      await _dictDataRepository.DeleteAsync(x => ids.Contains(x.Id));

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"批量删除字典数据失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取字典数据详情
  /// </summary>
  public async Task<LeanApiResult<LeanDictDataDto>> GetAsync(long id)
  {
    try
    {
      var entity = await _dictDataRepository.GetByIdAsync(id);
      if (entity == null)
      {
        return LeanApiResult<LeanDictDataDto>.Error($"字典数据 {id} 不存在");
      }

      var dto = entity.Adapt<LeanDictDataDto>();
      return LeanApiResult<LeanDictDataDto>.Ok(dto);
    }
    catch (Exception ex)
    {
      return LeanApiResult<LeanDictDataDto>.Error($"获取字典数据详情失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 分页查询字典数据
  /// </summary>
  public async Task<LeanApiResult<LeanPageResult<LeanDictDataDto>>> GetPageAsync(LeanQueryDictDataDto input)
  {
    try
    {
      Expression<Func<LeanDictData, bool>> predicate = x => true;

      if (input.TypeId.HasValue)
      {
        predicate = LeanExpressionExtensions.And(predicate, x => x.TypeId == input.TypeId.Value);
      }

      if (!string.IsNullOrEmpty(input.DictLabel))
      {
        var label = CleanInput(input.DictLabel);
        predicate = LeanExpressionExtensions.And(predicate, x => x.DictLabel.Contains(label));
      }

      if (!string.IsNullOrEmpty(input.DictValue))
      {
        var value = CleanInput(input.DictValue);
        predicate = LeanExpressionExtensions.And(predicate, x => x.DictValue.Contains(value));
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

      var (total, items) = await _dictDataRepository.GetPageListAsync(predicate, input.PageSize, input.PageIndex);
      var dtos = items.Select(t => t.Adapt<LeanDictDataDto>()).ToList();

      var result = new LeanPageResult<LeanDictDataDto>
      {
        Total = total,
        Items = dtos,
        PageIndex = input.PageIndex,
        PageSize = input.PageSize
      };

      return LeanApiResult<LeanPageResult<LeanDictDataDto>>.Ok(result);
    }
    catch (Exception ex)
    {
      return LeanApiResult<LeanPageResult<LeanDictDataDto>>.Error($"分页查询字典数据失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 设置字典数据状态
  /// </summary>
  public async Task<LeanApiResult> SetStatusAsync(LeanChangeDictDataStatusDto input)
  {
    try
    {
      var entity = await _dictDataRepository.GetByIdAsync(input.Id);
      if (entity == null)
      {
        return LeanApiResult.Error($"字典数据 {input.Id} 不存在");
      }

      if (entity.IsBuiltin == LeanBuiltinStatus.Yes)
      {
        return LeanApiResult.Error("内置字典数据不允许修改状态");
      }

      entity.Status = input.Status;
      await _dictDataRepository.UpdateAsync(entity);

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"设置字典数据状态失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 根据字典类型编码获取字典数据列表
  /// </summary>
  public async Task<LeanApiResult<List<LeanDictDataDto>>> GetListByTypeCodeAsync(string typeCode)
  {
    try
    {
      var dictType = await _dictTypeRepository.FirstOrDefaultAsync(x => x.DictCode == typeCode);
      if (dictType == null)
      {
        return LeanApiResult<List<LeanDictDataDto>>.Error($"字典类型编码 {typeCode} 不存在");
      }

      var items = await _dictDataRepository.GetListAsync(x => x.TypeId == dictType.Id && x.Status == LeanStatus.Normal);
      var dtos = items.Select(t => t.Adapt<LeanDictDataDto>()).ToList();

      return LeanApiResult<List<LeanDictDataDto>>.Ok(dtos);
    }
    catch (Exception ex)
    {
      return LeanApiResult<List<LeanDictDataDto>>.Error($"获取字典数据列表失败: {ex.Message}");
    }
  }
}