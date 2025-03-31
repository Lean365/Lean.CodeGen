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
using Lean.CodeGen.Common.Http;
using NLog;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Lean.CodeGen.Application.Services.Admin;

/// <summary>
/// 系统配置服务实现
/// </summary>
public class LeanConfigService : LeanBaseService, ILeanConfigService
{
    private readonly ILogger _logger;
    private readonly ILeanRepository<LeanConfig> _repository;
    private readonly LeanUniqueValidator<LeanConfig> _uniqueValidator;
    private readonly ILeanLocalizationService _localizationService;
    private readonly ILeanHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// 构造函数
    /// </summary>
    public LeanConfigService(
        ILeanRepository<LeanConfig> repository,
        LeanBaseServiceContext context,
        ILeanLocalizationService localizationService,
        ILeanHttpContextAccessor httpContextAccessor)
        : base(context)
    {
        _repository = repository;
        _logger = context.Logger;
        _uniqueValidator = new LeanUniqueValidator<LeanConfig>(_repository);
        _localizationService = localizationService;
        _httpContextAccessor = httpContextAccessor;
    }

    private async Task<string> GetCurrentLanguageAsync()
    {
        var langCode = _httpContextAccessor.GetCurrentLanguage();
        var supportedLanguages = await _localizationService.GetSupportedLanguagesAsync();
        return supportedLanguages.Contains(langCode) ? langCode : "en-US";
    }

    /// <summary>
    /// 创建系统配置
    /// </summary>
    public async Task<LeanApiResult<long>> CreateAsync(LeanConfigCreateDto input)
    {
        try
        {
            // 检查配置键是否已存在
            var exists = await _repository.AnyAsync(x => x.ConfigKey == input.ConfigKey);
            if (exists)
            {
                var langCode = await GetCurrentLanguageAsync();
                var message = await _localizationService.GetTranslationAsync(langCode, "config.error.key_exists");
                return LeanApiResult<long>.Error(message);
            }

            // 创建实体
            var entity = input.Adapt<LeanConfig>();
            entity.IsBuiltin = 0;
            entity.ConfigStatus = 2; // Normal = 2

            // 保存数据
            var id = await _repository.CreateAsync(entity);
            return LeanApiResult<long>.Ok(id);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "创建系统配置失败");
            var langCode = await GetCurrentLanguageAsync();
            var message = await _localizationService.GetTranslationAsync(langCode, "config.error.create_failed");
            return LeanApiResult<long>.Error(message);
        }
    }

    /// <summary>
    /// 更新系统配置
    /// </summary>
    public async Task<LeanApiResult> UpdateAsync(LeanConfigUpdateDto input)
    {
        try
        {
            // 获取原数据
            var entity = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (entity == null)
            {
                var langCode = await GetCurrentLanguageAsync();
                var message = await _localizationService.GetTranslationAsync(langCode, "config.error.not_found");
                return LeanApiResult.Error(message);
            }

            // 检查是否为系统内置
            if (entity.IsBuiltin == 1)
            {
                var langCode = await GetCurrentLanguageAsync();
                var message = await _localizationService.GetTranslationAsync(langCode, "config.error.builtin_modify");
                return LeanApiResult.Error(message);
            }

            // 检查配置键是否已存在
            var exists = await _repository.AnyAsync(x => x.ConfigKey == input.ConfigKey && x.Id != input.Id);
            if (exists)
            {
                var langCode = await GetCurrentLanguageAsync();
                var message = await _localizationService.GetTranslationAsync(langCode, "config.error.key_exists");
                return LeanApiResult.Error(message);
            }

            // 更新数据
            input.Adapt(entity);
            await _repository.UpdateAsync(entity);

            return LeanApiResult.Ok();
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "更新系统配置失败");
            var langCode = await GetCurrentLanguageAsync();
            var message = await _localizationService.GetTranslationAsync(langCode, "config.error.update_failed");
            return LeanApiResult.Error(message);
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
                var langCode = await GetCurrentLanguageAsync();
                var message = await _localizationService.GetTranslationAsync(langCode, "config.error.select_delete");
                return LeanApiResult.Error(message);
            }

            // 检查是否包含系统内置配置
            var entities = await _repository.GetListAsync(x => ids.Contains(x.Id));
            if (entities.Any(x => x.IsBuiltin == 1))
            {
                var langCode = await GetCurrentLanguageAsync();
                var message = await _localizationService.GetTranslationAsync(langCode, "config.error.builtin_delete");
                return LeanApiResult.Error(message);
            }

            // 删除数据
            await _repository.DeleteAsync(x => ids.Contains(x.Id));

            return LeanApiResult.Ok();
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "删除系统配置失败");
            var langCode = await GetCurrentLanguageAsync();
            var message = await _localizationService.GetTranslationAsync(langCode, "config.error.delete_failed");
            return LeanApiResult.Error(message);
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
                var langCode = await GetCurrentLanguageAsync();
                var message = await _localizationService.GetTranslationAsync(langCode, "config.error.not_found");
                return LeanApiResult<LeanConfigDto>.Error(message);
            }

            var dto = entity.Adapt<LeanConfigDto>();
            return LeanApiResult<LeanConfigDto>.Ok(dto);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "获取系统配置详情失败");
            var langCode = await GetCurrentLanguageAsync();
            var message = await _localizationService.GetTranslationAsync(langCode, "config.error.get_failed");
            return LeanApiResult<LeanConfigDto>.Error(message);
        }
    }

    /// <summary>
    /// 分页查询系统配置
    /// </summary>
    public async Task<LeanApiResult<LeanPageResult<LeanConfigDto>>> GetPagedListAsync(LeanConfigQueryDto input)
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
                predicate = LeanExpressionExtensions.And(predicate, x => x.ConfigType == (int)input.ConfigType.Value);
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
            _logger.Error(ex, "分页查询系统配置失败");
            var langCode = await GetCurrentLanguageAsync();
            var message = await _localizationService.GetTranslationAsync(langCode, "config.error.query_failed");
            return LeanApiResult<LeanPageResult<LeanConfigDto>>.Error(message);
        }
    }

    /// <summary>
    /// 导出系统配置
    /// </summary>
    public async Task<byte[]> ExportAsync(LeanConfigQueryDto input)
    {
        var predicate = BuildQueryPredicate(input);
        var items = await _repository.GetListAsync(predicate);
        var dtos = items.Select(t => t.Adapt<LeanConfigExportDto>()).ToList();
        return LeanExcelHelper.Export(dtos);
    }

    /// <summary>
    /// 导入系统配置
    /// </summary>
    public async Task<LeanExcelImportResult<LeanConfigImportDto>> ImportAsync(LeanFileInfo file)
    {
        var result = new LeanExcelImportResult<LeanConfigImportDto>();

        try
        {
            // 读取Excel文件
            var bytes = new byte[file.Stream.Length];
            await file.Stream.ReadAsync(bytes, 0, (int)file.Stream.Length);
            var importDtos = LeanExcelHelper.Import<LeanConfigImportDto>(bytes);
            result.Data.AddRange(importDtos.Data);
            result.Errors.AddRange(importDtos.Errors);

            if (result.IsSuccess)
            {
                foreach (var dto in result.Data)
                {
                    try
                    {
                        // 检查配置键是否存在
                        var exists = await _repository.AnyAsync(x => x.ConfigKey == dto.ConfigKey);
                        if (exists)
                        {
                            result.Errors.Add(new LeanExcelImportError
                            {
                                RowIndex = importDtos.Data.IndexOf(dto) + 2,
                                ErrorMessage = $"配置键 {dto.ConfigKey} 已存在"
                            });
                            continue;
                        }

                        // 创建实体
                        var entity = dto.Adapt<LeanConfig>();
                        entity.ConfigStatus = 2; // Normal = 2
                        entity.IsBuiltin = 0;
                        await _repository.CreateAsync(entity);
                    }
                    catch (Exception ex)
                    {
                        result.Errors.Add(new LeanExcelImportError
                        {
                            RowIndex = importDtos.Data.IndexOf(dto) + 2,
                            ErrorMessage = ex.Message
                        });
                    }
                }
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "导入系统配置失败");
            result.ErrorMessage = $"导入失败：{ex.Message}";
            return result;
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    public Task<byte[]> GetTemplateAsync()
    {
        var template = new List<LeanConfigImportTemplateDto>
    {
      new LeanConfigImportTemplateDto()
    };

        var bytes = LeanExcelHelper.Export(template);
        return Task.FromResult(bytes);
    }

    /// <summary>
    /// 构建查询条件
    /// </summary>
    private Expression<Func<LeanConfig, bool>> BuildQueryPredicate(LeanConfigQueryDto input)
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
            predicate = LeanExpressionExtensions.And(predicate, x => x.ConfigType == (int)input.ConfigType.Value);
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

        return predicate;
    }
}