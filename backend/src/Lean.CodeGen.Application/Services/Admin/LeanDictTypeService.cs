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
using Mapster;
using Lean.CodeGen.Domain.Validators;

namespace Lean.CodeGen.Application.Services.Admin;

/// <summary>
/// 字典类型服务实现
/// </summary>
public class LeanDictTypeService : LeanBaseService, ILeanDictTypeService
{
    private readonly ILogger _logger;
    private readonly ILeanRepository<LeanDictType> _dictTypeRepository;
    private readonly ILeanRepository<LeanDictData> _dictDataRepository;
    private readonly LeanUniqueValidator<LeanDictType> _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    public LeanDictTypeService(
        ILeanRepository<LeanDictType> dictTypeRepository,
        ILeanRepository<LeanDictData> dictDataRepository,
        LeanBaseServiceContext context)
        : base(context)
    {
        _dictTypeRepository = dictTypeRepository;
        _dictDataRepository = dictDataRepository;
        _logger = context.Logger;
        _uniqueValidator = new LeanUniqueValidator<LeanDictType>(_dictTypeRepository);
    }

    /// <summary>
    /// 创建字典类型
    /// </summary>
    public async Task<LeanApiResult<long>> CreateAsync(LeanDictTypeCreateDto input)
    {
        try
        {
            // 检查字典类型编码是否存在
            var exists = await _dictTypeRepository.AnyAsync(x => x.DictTypeCode == input.DictTypeCode);
            if (exists)
            {
                return LeanApiResult<long>.Error($"字典类型编码 {input.DictTypeCode} 已存在");
            }

            var entity = input.Adapt<LeanDictType>();
            entity.DictTypeStatus = 2; // LeanStatus.Normal = 2
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
    public async Task<LeanApiResult> UpdateAsync(LeanDictTypeUpdateDto input)
    {
        try
        {
            var entity = await _dictTypeRepository.GetByIdAsync(input.Id);
            if (entity == null)
            {
                return LeanApiResult.Error($"字典类型 {input.Id} 不存在");
            }

            if (entity.IsBuiltin == 1) // LeanBuiltinStatus.Yes = 1
            {
                return LeanApiResult.Error("内置字典类型不允许修改");
            }

            // 检查字典类型编码是否存在
            var exists = await _dictTypeRepository.AnyAsync(x => x.DictTypeCode == input.DictTypeCode && x.Id != input.Id);
            if (exists)
            {
                return LeanApiResult.Error($"字典类型编码 {input.DictTypeCode} 已存在");
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

            if (entity.IsBuiltin == 1) // LeanBuiltinStatus.Yes = 1
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
            if (entities.Any(x => x.IsBuiltin == 1)) // LeanBuiltinStatus.Yes = 1
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
    public async Task<LeanApiResult<LeanPageResult<LeanDictTypeDto>>> GetPageAsync(LeanDictTypeQueryDto input)
    {
        try
        {
            Expression<Func<LeanDictType, bool>> predicate = x => true;

            if (!string.IsNullOrEmpty(input.DictTypeName))
            {
                var name = input.DictTypeName.Trim();
                predicate = predicate.And(x => x.DictTypeName.Contains(name));
            }

            if (!string.IsNullOrEmpty(input.DictTypeCode))
            {
                var code = input.DictTypeCode.Trim();
                predicate = predicate.And(x => x.DictTypeCode.Contains(code));
            }

            if (input.DictTypeStatus.HasValue)
            {
                predicate = LeanExpressionExtensions.And(predicate, x => x.DictTypeStatus == (int)input.DictTypeStatus.Value);
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
    public async Task<LeanApiResult> SetStatusAsync(LeanDictTypeChangeStatusDto input)
    {
        try
        {
            var entity = await _dictTypeRepository.GetByIdAsync(input.Id);
            if (entity == null)
            {
                return LeanApiResult.Error($"字典类型 {input.Id} 不存在");
            }

            if (entity.IsBuiltin == 1) // LeanBuiltinStatus.Yes = 1
            {
                return LeanApiResult.Error("内置字典类型不允许修改状态");
            }

            entity.DictTypeStatus = (int)input.DictTypeStatus;
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
    public async Task<byte[]> ExportAsync(LeanDictTypeQueryDto input)
    {
        Expression<Func<LeanDictType, bool>> predicate = x => true;

        if (!string.IsNullOrEmpty(input.DictTypeName))
        {
            var name = input.DictTypeName.Trim();
            predicate = predicate.And(x => x.DictTypeName.Contains(name));
        }

        if (!string.IsNullOrEmpty(input.DictTypeCode))
        {
            var code = input.DictTypeCode.Trim();
            predicate = predicate.And(x => x.DictTypeCode.Contains(code));
        }

        if (input.DictTypeStatus.HasValue)
        {
            predicate = LeanExpressionExtensions.And(predicate, x => x.DictTypeStatus == (int)input.DictTypeStatus.Value);
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

        return LeanExcelHelper.Export(dtos);
    }

    /// <summary>
    /// 导入字典类型
    /// </summary>
    public async Task<LeanExcelImportResult<LeanDictTypeImportDto>> ImportAsync(LeanFileInfo file)
    {
        var result = new LeanExcelImportResult<LeanDictTypeImportDto>();

        try
        {
            // 导入Excel
            var bytes = new byte[file.Stream.Length];
            await file.Stream.ReadAsync(bytes, 0, (int)file.Stream.Length);
            var importResult = LeanExcelHelper.Import<LeanDictTypeImportDto>(bytes);
            result.Data.AddRange(importResult.Data);
            result.Errors.AddRange(importResult.Errors);

            if (result.IsSuccess)
            {
                foreach (var item in result.Data)
                {
                    try
                    {
                        // 检查字典类型编码是否存在
                        var exists = await _dictTypeRepository.AnyAsync(x => x.DictTypeCode == item.DictTypeCode);
                        if (exists)
                        {
                            result.Errors.Add(new LeanExcelImportError
                            {
                                RowIndex = importResult.Data.IndexOf(item) + 2,
                                ErrorMessage = $"字典类型编码 {item.DictTypeCode} 已存在"
                            });
                            continue;
                        }

                        // 创建实体
                        var entity = item.Adapt<LeanDictType>();
                        entity.DictTypeStatus = 2; // LeanStatus.Normal = 2
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

            return result;
        }
        catch (Exception ex)
        {
            result.ErrorMessage = $"导入失败：{ex.Message}";
            return result;
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    public Task<byte[]> GetImportTemplateAsync()
    {
        var template = new List<LeanDictTypeImportDto>();
        var bytes = LeanExcelHelper.Export(template);
        return Task.FromResult(bytes);
    }
}