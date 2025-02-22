using System.Linq.Expressions;
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
      ILeanRepository<LeanTranslation> translationRepository,
      ILeanRepository<LeanLanguage> languageRepository,
      ILeanSqlSafeService sqlSafeService,
      IOptions<LeanSecurityOptions> securityOptions,
      ILogger<LeanTranslationService> logger)
      : base(sqlSafeService, securityOptions, logger)
  {
    _repository = translationRepository;
    _languageRepository = languageRepository;
    _uniqueValidator = new LeanUniqueValidator<LeanTranslation>(_repository);
    _logger = logger;
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
      Expression<Func<LeanTranslation, bool>> predicate = x => true;

      if (input.LangId.HasValue)
      {
        predicate = LeanExpressionExtensions.And(predicate, x => x.LangId == input.LangId);
      }

      if (!string.IsNullOrEmpty(input.TransKey))
      {
        var transKey = CleanInput(input.TransKey);
        predicate = LeanExpressionExtensions.And(predicate, x => x.TransKey.Contains(transKey));
      }

      if (!string.IsNullOrEmpty(input.TransValue))
      {
        var transValue = CleanInput(input.TransValue);
        predicate = LeanExpressionExtensions.And(predicate, x => x.TransValue.Contains(transValue));
      }

      if (input.Status.HasValue)
      {
        predicate = LeanExpressionExtensions.And(predicate, x => x.Status == input.Status.Value);
      }

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
  public async Task<LeanApiResult<List<LeanTranslationExportDto>>> ExportAsync(LeanQueryTranslationDto input)
  {
    try
    {
      // 构建查询条件
      Expression<Func<LeanTranslation, bool>> predicate = x => true;

      if (input.LangId.HasValue)
      {
        predicate = LeanExpressionExtensions.And(predicate, x => x.LangId == input.LangId);
      }

      if (!string.IsNullOrEmpty(input.TransKey))
      {
        var transKey = CleanInput(input.TransKey);
        predicate = LeanExpressionExtensions.And(predicate, x => x.TransKey.Contains(transKey));
      }

      if (!string.IsNullOrEmpty(input.TransValue))
      {
        var transValue = CleanInput(input.TransValue);
        predicate = LeanExpressionExtensions.And(predicate, x => x.TransValue.Contains(transValue));
      }

      if (input.Status.HasValue)
      {
        predicate = LeanExpressionExtensions.And(predicate, x => x.Status == input.Status.Value);
      }

      // 获取数据
      var items = await _repository.GetListAsync(predicate);
      var list = items.Select(t => t.Adapt<LeanTranslationExportDto>()).ToList();

      // 返回结果
      return LeanApiResult<List<LeanTranslationExportDto>>.Ok(list, LeanBusinessType.Export);
    }
    catch (Exception ex)
    {
      return LeanApiResult<List<LeanTranslationExportDto>>.Error($"导出翻译失败：{ex.Message}", LeanErrorCode.SystemError, LeanBusinessType.Export);
    }
  }

  /// <summary>
  /// 导入翻译
  /// </summary>
  public async Task<LeanApiResult> ImportAsync(long langId, Dictionary<string, string> translations)
  {
    try
    {
      // 检查语言是否存在
      var language = await _languageRepository.GetByIdAsync(langId);
      if (language == null)
      {
        return LeanApiResult.Error("语言不存在", LeanErrorCode.DataNotFoundError, LeanBusinessType.Import);
      }

      // 获取现有的翻译
      var existingTranslations = await _repository.GetListAsync(x => x.LangId == langId);
      var existingKeys = existingTranslations.ToDictionary(x => x.TransKey, x => x);

      // 更新或创建翻译
      foreach (var translation in translations)
      {
        if (existingKeys.TryGetValue(translation.Key, out var existingTranslation))
        {
          // 更新现有翻译
          existingTranslation.TransValue = translation.Value;
          await _repository.UpdateAsync(existingTranslation);
        }
        else
        {
          // 创建新翻译
          var newTranslation = new LeanTranslation
          {
            LangId = langId,
            TransKey = translation.Key,
            TransValue = translation.Value,
            Status = LeanStatus.Normal,
            IsBuiltin = LeanBuiltinStatus.No
          };
          await _repository.CreateAsync(newTranslation);
        }
      }

      return LeanApiResult.Ok(LeanBusinessType.Import);
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"导入翻译失败：{ex.Message}", LeanErrorCode.SystemError, LeanBusinessType.Import);
    }
  }

  /// <summary>
  /// 获取导入模板
  /// </summary>
  public Task<LeanApiResult<List<LeanTranslationImportTemplateDto>>> GetImportTemplateAsync()
  {
    try
    {
      var template = new List<LeanTranslationImportTemplateDto>
      {
        new LeanTranslationImportTemplateDto()
      };

      var bytes = LeanExcelHelper.Export(template);
      return Task.FromResult(LeanApiResult<List<LeanTranslationImportTemplateDto>>.Ok(template, LeanBusinessType.Export));
    }
    catch (Exception ex)
    {
      return Task.FromResult(LeanApiResult<List<LeanTranslationImportTemplateDto>>.Error($"获取导入模板失败：{ex.Message}", LeanErrorCode.SystemError, LeanBusinessType.Export));
    }
  }

  /// <summary>
  /// 导入翻译（从Excel文件）
  /// </summary>
  public async Task<LeanApiResult<LeanExcelImportResult<LeanTranslationImportDto>>> ImportFromExcelAsync(LeanFileInfo file)
  {
    try
    {
      var result = new LeanExcelImportResult<LeanTranslationImportDto>();

      // 读取Excel文件
      var importResult = LeanExcelHelper.Import<LeanTranslationImportDto>(File.ReadAllBytes(file.FilePath));
      if (!importResult.Data.Any())
      {
        return LeanApiResult<LeanExcelImportResult<LeanTranslationImportDto>>.Error("导入数据为空", LeanErrorCode.InvalidParameter, LeanBusinessType.Import);
      }

      // 验证数据
      foreach (var dto in importResult.Data)
      {
        try
        {
          // 检查语言是否存在
          var language = await _languageRepository.GetByIdAsync(dto.LangId);
          if (language == null)
          {
            importResult.Errors.Add(new LeanExcelImportError
            {
              RowIndex = importResult.Data.IndexOf(dto) + 2,
              ErrorMessage = $"语言ID {dto.LangId} 不存在"
            });
            continue;
          }

          // 检查翻译键是否已存在
          var exists = await _repository.AnyAsync(x => x.LangId == dto.LangId && x.TransKey == dto.TransKey);
          if (exists)
          {
            importResult.Errors.Add(new LeanExcelImportError
            {
              RowIndex = importResult.Data.IndexOf(dto) + 2,
              ErrorMessage = "翻译键已存在"
            });
            continue;
          }

          // 创建实体
          var entity = dto.Adapt<LeanTranslation>();
          entity.Status = LeanStatus.Normal;
          entity.IsBuiltin = LeanBuiltinStatus.No;

          // 保存到数据库
          await _repository.CreateAsync(entity);
          result.Data.Add(dto);
        }
        catch (Exception ex)
        {
          importResult.Errors.Add(new LeanExcelImportError
          {
            RowIndex = importResult.Data.IndexOf(dto) + 2,
            ErrorMessage = ex.Message
          });
        }
      }

      return LeanApiResult<LeanExcelImportResult<LeanTranslationImportDto>>.Ok(result, LeanBusinessType.Import);
    }
    catch (Exception ex)
    {
      return LeanApiResult<LeanExcelImportResult<LeanTranslationImportDto>>.Error($"导入翻译失败：{ex.Message}", LeanErrorCode.SystemError, LeanBusinessType.Import);
    }
  }

  /// <summary>
  /// 获取指定语言的所有翻译
  /// </summary>
  public async Task<LeanApiResult<Dictionary<string, string>>> GetTranslationsByLangAsync(string langCode)
  {
    try
    {
      // 获取语言
      var language = await _languageRepository.FirstOrDefaultAsync(x => x.LangCode == langCode);
      if (language == null)
      {
        return LeanApiResult<Dictionary<string, string>>.Error("语言不存在", LeanErrorCode.DataNotFoundError, LeanBusinessType.Query);
      }

      // 获取翻译
      var translations = await _repository.GetListAsync(x => x.LangId == language.Id && x.Status == LeanStatus.Normal);
      var result = translations.ToDictionary(x => x.TransKey, x => x.TransValue);

      return LeanApiResult<Dictionary<string, string>>.Ok(result, LeanBusinessType.Query);
    }
    catch (Exception ex)
    {
      return LeanApiResult<Dictionary<string, string>>.Error($"获取翻译失败：{ex.Message}", LeanErrorCode.SystemError, LeanBusinessType.Query);
    }
  }

  /// <summary>
  /// 获取所有模块列表
  /// </summary>
  public async Task<LeanApiResult<List<string>>> GetModuleListAsync()
  {
    try
    {
      var translations = await _repository.GetListAsync(x => x.Status == LeanStatus.Normal);
      var modules = translations.Where(x => !string.IsNullOrEmpty(x.ModuleName))
                              .Select(x => x.ModuleName!)
                              .Distinct()
                              .OrderBy(x => x)
                              .ToList();

      return LeanApiResult<List<string>>.Ok(modules, LeanBusinessType.Query);
    }
    catch (Exception ex)
    {
      return LeanApiResult<List<string>>.Error($"获取模块列表失败：{ex.Message}", LeanErrorCode.SystemError, LeanBusinessType.Query);
    }
  }
}