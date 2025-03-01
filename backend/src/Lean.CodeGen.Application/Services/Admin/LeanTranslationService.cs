using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Lean.CodeGen.Application.Dtos.Admin;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Domain.Entities.Admin;
using Lean.CodeGen.Domain.Interfaces.Repositories;
using Lean.CodeGen.Domain.Validators;
using Mapster;
using Lean.CodeGen.Common.Options;
using Lean.CodeGen.Application.Services.Base;
using Lean.CodeGen.Application.Services.Security;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Extensions;
using Lean.CodeGen.Common.Excel;
using System.IO;

namespace Lean.CodeGen.Application.Services.Admin;

/// <summary>
/// 翻译服务实现
/// </summary>
public class LeanTranslationService : LeanBaseService, ILeanTranslationService
{
  private readonly ILogger<LeanTranslationService> _logger;
  private readonly ILeanRepository<LeanTranslation> _repository;
  private readonly ILeanRepository<LeanLanguage> _languageRepository;
  private readonly LeanUniqueValidator<LeanTranslation> _uniqueValidator;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanTranslationService(
      ILeanRepository<LeanTranslation> repository,
      ILeanRepository<LeanLanguage> languageRepository,
      LeanBaseServiceContext context)
      : base(context)
  {
    _repository = repository;
    _languageRepository = languageRepository;
    _logger = (ILogger<LeanTranslationService>)context.Logger;
    _uniqueValidator = new LeanUniqueValidator<LeanTranslation>(_repository);
  }

  /// <summary>
  /// 创建翻译
  /// </summary>
  public async Task<LeanApiResult<long>> CreateAsync(LeanCreateTranslationDto input)
  {
    try
    {
      // 检查语言是否存在
      var language = await _languageRepository.GetByIdAsync(input.LangId);
      if (language == null)
      {
        return LeanApiResult<long>.Error("语言不存在", LeanErrorCode.DataNotFoundError, LeanBusinessType.Create);
      }

      // 检查翻译键是否已存在
      var exists = await _repository.AnyAsync(x => x.LangId == input.LangId && x.TransKey == input.TransKey);
      if (exists)
      {
        return LeanApiResult<long>.Error("翻译键已存在", LeanErrorCode.DuplicateError, LeanBusinessType.Create);
      }

      // 创建实体
      var entity = input.Adapt<LeanTranslation>();
      entity.Status = LeanStatus.Normal;
      entity.IsBuiltin = LeanBuiltinStatus.No;

      // 保存到数据库
      var id = await _repository.CreateAsync(entity);

      // 返回结果
      return LeanApiResult<long>.Ok(id, LeanBusinessType.Create);
    }
    catch (Exception ex)
    {
      return LeanApiResult<long>.Error($"创建翻译失败：{ex.Message}", LeanErrorCode.SystemError, LeanBusinessType.Create);
    }
  }

  /// <summary>
  /// 更新翻译
  /// </summary>
  public async Task<LeanApiResult> UpdateAsync(LeanUpdateTranslationDto input)
  {
    try
    {
      // 获取实体
      var entity = await _repository.GetByIdAsync(input.Id);
      if (entity == null)
      {
        return LeanApiResult.Error("翻译不存在", LeanErrorCode.DataNotFoundError, LeanBusinessType.Update);
      }

      // 检查语言是否存在
      var language = await _languageRepository.GetByIdAsync(input.LangId);
      if (language == null)
      {
        return LeanApiResult.Error("语言不存在", LeanErrorCode.DataNotFoundError, LeanBusinessType.Update);
      }

      // 检查翻译键是否已存在
      var exists = await _repository.AnyAsync(x => x.LangId == input.LangId && x.TransKey == input.TransKey && x.Id != input.Id);
      if (exists)
      {
        return LeanApiResult.Error("翻译键已存在", LeanErrorCode.DuplicateError, LeanBusinessType.Update);
      }

      // 更新实体
      input.Adapt(entity);
      await _repository.UpdateAsync(entity);

      // 返回结果
      return LeanApiResult.Ok(LeanBusinessType.Update);
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"更新翻译失败：{ex.Message}", LeanErrorCode.SystemError, LeanBusinessType.Update);
    }
  }

  /// <summary>
  /// 删除翻译
  /// </summary>
  public async Task<LeanApiResult> DeleteAsync(long id)
  {
    try
    {
      // 获取实体
      var entity = await _repository.GetByIdAsync(id);
      if (entity == null)
      {
        return LeanApiResult.Error("翻译不存在", LeanErrorCode.DataNotFoundError, LeanBusinessType.Delete);
      }

      // 检查是否为内置翻译
      if (entity.IsBuiltin == LeanBuiltinStatus.Yes)
      {
        return LeanApiResult.Error("内置翻译不允许删除", LeanErrorCode.OperationForbidden, LeanBusinessType.Delete);
      }

      // 删除实体
      await _repository.DeleteAsync(entity);

      // 返回结果
      return LeanApiResult.Ok(LeanBusinessType.Delete);
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"删除翻译失败：{ex.Message}", LeanErrorCode.SystemError, LeanBusinessType.Delete);
    }
  }

  /// <summary>
  /// 批量删除翻译
  /// </summary>
  public async Task<LeanApiResult> BatchDeleteAsync(List<long> ids)
  {
    try
    {
      // 获取实体列表
      var entities = await _repository.GetListAsync(x => ids.Contains(x.Id));
      if (!entities.Any())
      {
        return LeanApiResult.Error("未找到要删除的翻译", LeanErrorCode.DataNotFoundError, LeanBusinessType.Delete);
      }

      // 检查是否包含内置翻译
      if (entities.Any(x => x.IsBuiltin == LeanBuiltinStatus.Yes))
      {
        return LeanApiResult.Error("选中的翻译中包含内置翻译，不允许删除", LeanErrorCode.OperationForbidden, LeanBusinessType.Delete);
      }

      // 删除实体
      await _repository.DeleteRangeAsync(entities);

      // 返回结果
      return LeanApiResult.Ok(LeanBusinessType.Delete);
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"批量删除翻译失败：{ex.Message}", LeanErrorCode.SystemError, LeanBusinessType.Delete);
    }
  }

  /// <summary>
  /// 获取翻译详情
  /// </summary>
  public async Task<LeanApiResult<LeanTranslationDto>> GetAsync(long id)
  {
    try
    {
      // 获取实体
      var entity = await _repository.GetByIdAsync(id);
      if (entity == null)
      {
        return LeanApiResult<LeanTranslationDto>.Error("翻译不存在", LeanErrorCode.DataNotFoundError, LeanBusinessType.Query);
      }

      // 返回结果
      var dto = entity.Adapt<LeanTranslationDto>();
      return LeanApiResult<LeanTranslationDto>.Ok(dto, LeanBusinessType.Query);
    }
    catch (Exception ex)
    {
      return LeanApiResult<LeanTranslationDto>.Error($"获取翻译失败：{ex.Message}", LeanErrorCode.SystemError, LeanBusinessType.Query);
    }
  }

  /// <summary>
  /// 获取翻译列表（分页）
  /// </summary>
  public async Task<LeanApiResult<LeanPageResult<LeanTranslationDto>>> GetPageAsync(LeanQueryTranslationDto input)
  {
    try
    {
      // 构建查询条件
      Expression<Func<LeanTranslation, bool>> predicate = BuildQueryPredicate(input);

      // 获取分页数据
      var (total, items) = await _repository.GetPageListAsync(predicate, input.PageSize, input.PageIndex);
      var list = items.Select(t => t.Adapt<LeanTranslationDto>()).ToList();

      // 返回结果
      return LeanApiResult<LeanPageResult<LeanTranslationDto>>.Ok(new LeanPageResult<LeanTranslationDto>
      {
        Total = total,
        Items = list
      }, LeanBusinessType.Query);
    }
    catch (Exception ex)
    {
      return LeanApiResult<LeanPageResult<LeanTranslationDto>>.Error($"获取翻译列表失败：{ex.Message}", LeanErrorCode.SystemError, LeanBusinessType.Query);
    }
  }

  /// <summary>
  /// 设置翻译状态
  /// </summary>
  public async Task<LeanApiResult> SetStatusAsync(LeanChangeTranslationStatusDto input)
  {
    try
    {
      // 获取实体
      var entity = await _repository.GetByIdAsync(input.Id);
      if (entity == null)
      {
        return LeanApiResult.Error("翻译不存在", LeanErrorCode.DataNotFoundError, LeanBusinessType.Update);
      }

      // 检查是否为内置翻译
      if (entity.IsBuiltin == LeanBuiltinStatus.Yes)
      {
        return LeanApiResult.Error("内置翻译不允许修改状态", LeanErrorCode.OperationForbidden, LeanBusinessType.Update);
      }

      // 更新状态
      entity.Status = input.Status;
      await _repository.UpdateAsync(entity);

      // 返回结果
      return LeanApiResult.Ok(LeanBusinessType.Update);
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"设置翻译状态失败：{ex.Message}", LeanErrorCode.SystemError, LeanBusinessType.Update);
    }
  }

  /// <summary>
  /// 导出翻译
  /// </summary>
  public async Task<byte[]> ExportAsync(LeanQueryTranslationDto input)
  {
    var list = await _repository.GetListAsync(BuildQueryPredicate(input));
    var exportDtos = list.Select(t => t.Adapt<LeanTranslationExportDto>()).ToList();
    return LeanExcelHelper.Export(exportDtos);
  }

  /// <summary>
  /// 导出转置翻译
  /// </summary>
  public async Task<byte[]> ExportTransposeAsync(LeanQueryTranslationDto input)
  {
    var translations = await _repository.GetListAsync(BuildQueryPredicate(input));
    var languages = await _languageRepository.GetListAsync(x => x.Status == LeanStatus.Normal);

    var transKeys = translations.Select(x => x.TransKey).Distinct().ToList();
    var exportDtos = transKeys.Select(key =>
    {
      var dto = new LeanTranslationExportDto { TransKey = key };
      foreach (var lang in languages)
      {
        var trans = translations.FirstOrDefault(t => t.TransKey == key && t.LangId == lang.Id);
        dto.GetType().GetProperty(lang.LangCode)?.SetValue(dto, trans?.TransValue);
      }
      return dto;
    }).ToList();

    return LeanExcelHelper.Export(exportDtos);
  }

  /// <summary>
  /// 导入翻译
  /// </summary>
  public async Task<LeanExcelImportResult<LeanTranslationImportDto>> ImportAsync(LeanFileInfo file)
  {
    var bytes = await System.IO.File.ReadAllBytesAsync(file.FilePath);
    return LeanExcelHelper.Import<LeanTranslationImportDto>(bytes);
  }

  /// <summary>
  /// 导入转置翻译
  /// </summary>
  public async Task<LeanExcelImportResult<LeanTranslationImportDto>> ImportTransposeAsync(LeanFileInfo file)
  {
    var bytes = await System.IO.File.ReadAllBytesAsync(file.FilePath);
    var importResult = await Task.FromResult(LeanExcelHelper.Import<LeanTranslationImportDto>(bytes));
    if (importResult.Data == null || !importResult.Data.Any()) return importResult;

    var languages = await _languageRepository.GetListAsync(x => x.Status == LeanStatus.Normal);
    foreach (var importDto in importResult.Data)
    {
      foreach (var lang in languages)
      {
        var transValue = importDto.GetType().GetProperty(lang.LangCode)?.GetValue(importDto)?.ToString();
        if (string.IsNullOrEmpty(transValue)) continue;

        var translation = await _repository.FirstOrDefaultAsync(x =>
          x.TransKey == importDto.TransKey && x.LangId == lang.Id);

        if (translation == null)
        {
          translation = new LeanTranslation
          {
            TransKey = importDto.TransKey,
            LangId = lang.Id,
            TransValue = transValue,
            Status = LeanStatus.Normal
          };
          await _repository.CreateAsync(translation);
        }
        else
        {
          translation.TransValue = transValue;
          await _repository.UpdateAsync(translation);
        }
      }
    }

    return importResult;
  }

  /// <summary>
  /// 获取导入模板
  /// </summary>
  public Task<byte[]> GetImportTemplateAsync()
  {
    var template = new List<LeanTranslationImportDto> { new LeanTranslationImportDto() };
    return Task.FromResult(LeanExcelHelper.Export(template));
  }

  /// <summary>
  /// 获取转置导入模板
  /// </summary>
  public Task<byte[]> GetTransposeImportTemplateAsync()
  {
    var template = new List<LeanTranslationImportDto>
    {
      new LeanTranslationImportDto { TransKey = "example.key" }
    };
    var bytes = LeanExcelHelper.Export(template);
    return Task.FromResult(bytes);
  }

  /// <summary>
  /// 从字典导入翻译
  /// </summary>
  public async Task<LeanApiResult> ImportFromDictionaryAsync(long langId, Dictionary<string, string> translations)
  {
    // 检查语言是否存在
    var language = await _languageRepository.GetByIdAsync(langId);
    if (language == null)
    {
      return LeanApiResult.Error("语言不存在");
    }

    foreach (var translation in translations)
    {
      var entity = await _repository.FirstOrDefaultAsync(x => x.LangId == langId && x.TransKey == translation.Key);
      if (entity != null)
      {
        entity.TransValue = translation.Value;
        await _repository.UpdateAsync(entity);
      }
      else
      {
        entity = new LeanTranslation
        {
          LangId = langId,
          TransKey = translation.Key,
          TransValue = translation.Value,
          Status = LeanStatus.Normal,
          IsBuiltin = LeanBuiltinStatus.No
        };
        await _repository.CreateAsync(entity);
      }
    }

    return LeanApiResult.Ok();
  }

  /// <summary>
  /// 获取指定语言的所有翻译
  /// </summary>
  public async Task<Dictionary<string, string>> GetTranslationsByLangAsync(string langCode)
  {
    var language = await _languageRepository.FirstOrDefaultAsync(x => x.LangCode == langCode);
    if (language == null)
    {
      return new Dictionary<string, string>();
    }

    var translations = await _repository.GetListAsync(x => x.LangId == language.Id && x.Status == LeanStatus.Normal);
    return translations.ToDictionary(x => x.TransKey, x => x.TransValue);
  }

  /// <summary>
  /// 获取所有模块列表
  /// </summary>
  public async Task<List<string>> GetModuleListAsync()
  {
    var translations = await _repository.GetListAsync(x => true);
    return translations.Select(x => x.ModuleName ?? string.Empty)
                      .Where(x => !string.IsNullOrEmpty(x))
                      .Distinct()
                      .ToList();
  }

  /// <summary>
  /// 获取转置的翻译列表（分页）
  /// </summary>
  public async Task<LeanApiResult<LeanPageResult<LeanTranslationExportDto>>> GetTransposePageAsync(LeanQueryTranslationDto input)
  {
    try
    {
      // 1. 获取所有翻译数据
      var translations = await _repository.GetListAsync(BuildQueryPredicate(input));
      // 2. 获取所有启用的语言
      var languages = await _languageRepository.GetListAsync(x => x.Status == LeanStatus.Normal);

      // 3. 按翻译键分组进行转置
      var transKeys = translations.Select(x => x.TransKey).Distinct().ToList();
      var allItems = transKeys.Select(key =>
      {
        var dto = new LeanTranslationExportDto { TransKey = key };
        foreach (var lang in languages)
        {
          var trans = translations.FirstOrDefault(t => t.TransKey == key && t.LangId == lang.Id);
          dto.GetType().GetProperty(lang.LangCode)?.SetValue(dto, trans?.TransValue);
        }
        return dto;
      }).ToList();

      // 4. 计算分页数据
      var total = allItems.Count;
      var items = allItems.Skip((input.PageIndex - 1) * input.PageSize).Take(input.PageSize).ToList();

      // 5. 返回分页结果
      return LeanApiResult<LeanPageResult<LeanTranslationExportDto>>.Ok(new LeanPageResult<LeanTranslationExportDto>
      {
        Total = total,
        Items = items
      }, LeanBusinessType.Query);
    }
    catch (Exception ex)
    {
      return LeanApiResult<LeanPageResult<LeanTranslationExportDto>>.Error(
        $"获取转置翻译列表失败：{ex.Message}",
        LeanErrorCode.SystemError,
        LeanBusinessType.Query);
    }
  }

  /// <summary>
  /// 创建转置的翻译
  /// </summary>
  public async Task<LeanApiResult> CreateTransposeAsync(LeanCreateTranslationTransposeDto input)
  {
    try
    {
      // 获取所有语言
      var languages = await _languageRepository.GetListAsync(x => x.Status == LeanStatus.Normal);

      // 检查翻译键是否已存在
      var exists = await _repository.AnyAsync(x => x.TransKey == input.TransKey);
      if (exists)
      {
        return LeanApiResult.Error("翻译键已存在", LeanErrorCode.DuplicateError, LeanBusinessType.Create);
      }

      // 创建每种语言的翻译
      foreach (var lang in languages)
      {
        if (!input.Translations.TryGetValue(lang.LangCode, out var transValue))
          continue;

        var translation = new LeanTranslation
        {
          TransKey = input.TransKey,
          ModuleName = input.ModuleName,
          LangId = lang.Id,
          TransValue = transValue,
          Status = LeanStatus.Normal,
          IsBuiltin = LeanBuiltinStatus.No
        };

        await _repository.CreateAsync(translation);
      }

      return LeanApiResult.Ok(LeanBusinessType.Create);
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error(
        $"创建转置翻译失败：{ex.Message}",
        LeanErrorCode.SystemError,
        LeanBusinessType.Create);
    }
  }

  /// <summary>
  /// 更新转置的翻译
  /// </summary>
  public async Task<LeanApiResult> UpdateTransposeAsync(LeanUpdateTranslationTransposeDto input)
  {
    try
    {
      // 获取所有语言
      var languages = await _languageRepository.GetListAsync(x => x.Status == LeanStatus.Normal);

      // 获取现有翻译
      var existingTranslations = await _repository.GetListAsync(x => x.TransKey == input.TransKey);
      if (!existingTranslations.Any())
      {
        return LeanApiResult.Error("翻译不存在", LeanErrorCode.DataNotFoundError, LeanBusinessType.Update);
      }

      // 检查是否为内置翻译
      if (existingTranslations.Any(x => x.IsBuiltin == LeanBuiltinStatus.Yes))
      {
        return LeanApiResult.Error("内置翻译不允许修改", LeanErrorCode.OperationForbidden, LeanBusinessType.Update);
      }

      // 更新每种语言的翻译
      foreach (var lang in languages)
      {
        if (!input.Translations.TryGetValue(lang.LangCode, out var transValue))
          continue;

        var translation = existingTranslations.FirstOrDefault(x => x.LangId == lang.Id);
        if (translation == null)
        {
          translation = new LeanTranslation
          {
            TransKey = input.TransKey,
            ModuleName = input.ModuleName,
            LangId = lang.Id,
            TransValue = transValue,
            Status = LeanStatus.Normal,
            IsBuiltin = LeanBuiltinStatus.No
          };
          await _repository.CreateAsync(translation);
        }
        else
        {
          translation.TransValue = transValue;
          translation.ModuleName = input.ModuleName;
          await _repository.UpdateAsync(translation);
        }
      }

      return LeanApiResult.Ok(LeanBusinessType.Update);
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error(
        $"更新转置翻译失败：{ex.Message}",
        LeanErrorCode.SystemError,
        LeanBusinessType.Update);
    }
  }

  private Expression<Func<LeanTranslation, bool>> BuildQueryPredicate(LeanQueryTranslationDto input)
  {
    Expression<Func<LeanTranslation, bool>> predicate = x => true;

    if (input.LangId.HasValue)
    {
      predicate = x => x.LangId == input.LangId;
    }

    if (!string.IsNullOrEmpty(input.TransKey))
    {
      var transKey = CleanInput(input.TransKey);
      predicate = predicate.And(x => x.TransKey.Contains(transKey));
    }

    if (!string.IsNullOrEmpty(input.TransValue))
    {
      var transValue = CleanInput(input.TransValue);
      predicate = predicate.And(x => x.TransValue.Contains(transValue));
    }

    if (input.Status.HasValue)
    {
      predicate = predicate.And(x => x.Status == input.Status.Value);
    }

    return predicate;
  }
}