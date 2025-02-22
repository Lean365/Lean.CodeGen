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
/// 语言服务实现
/// </summary>
public class LeanLanguageService : LeanBaseService, ILeanLanguageService
{
  private readonly ILeanRepository<LeanLanguage> _languageRepository;
  private readonly LeanUniqueValidator<LeanLanguage> _uniqueValidator;
  private readonly ILogger<LeanLanguageService> _logger;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanLanguageService(
      ILeanRepository<LeanLanguage> languageRepository,
      ILeanSqlSafeService sqlSafeService,
      IOptions<LeanSecurityOptions> securityOptions,
      ILogger<LeanLanguageService> logger)
      : base(sqlSafeService, securityOptions, logger)
  {
    _languageRepository = languageRepository;
    _uniqueValidator = new LeanUniqueValidator<LeanLanguage>(languageRepository);
    _logger = logger;
  }

  /// <summary>
  /// 创建语言
  /// </summary>
  public async Task<LeanApiResult<long>> CreateAsync(LeanCreateLanguageDto input)
  {
    try
    {
      // 检查语言代码是否存在
      var exists = await _languageRepository.AnyAsync(x => x.LangCode == input.LangCode);
      if (exists)
      {
        return LeanApiResult<long>.Error($"语言代码 {input.LangCode} 已存在");
      }

      var entity = input.Adapt<LeanLanguage>();
      entity.Status = LeanStatus.Normal;
      await _languageRepository.CreateAsync(entity);

      return LeanApiResult<long>.Ok(entity.Id);
    }
    catch (Exception ex)
    {
      return LeanApiResult<long>.Error($"创建语言失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 更新语言
  /// </summary>
  public async Task<LeanApiResult> UpdateAsync(LeanUpdateLanguageDto input)
  {
    try
    {
      var entity = await _languageRepository.GetByIdAsync(input.Id);
      if (entity == null)
      {
        return LeanApiResult.Error($"语言 {input.Id} 不存在");
      }

      if (entity.IsBuiltin == LeanBuiltinStatus.Yes)
      {
        return LeanApiResult.Error("内置语言不允许修改");
      }

      // 检查语言代码是否存在
      var exists = await _languageRepository.AnyAsync(x => x.LangCode == input.LangCode && x.Id != input.Id);
      if (exists)
      {
        return LeanApiResult.Error($"语言代码 {input.LangCode} 已存在");
      }

      input.Adapt(entity);
      await _languageRepository.UpdateAsync(entity);

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"更新语言失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 删除语言
  /// </summary>
  public async Task<LeanApiResult> DeleteAsync(long id)
  {
    try
    {
      var entity = await _languageRepository.GetByIdAsync(id);
      if (entity == null)
      {
        return LeanApiResult.Error($"语言 {id} 不存在");
      }

      if (entity.IsBuiltin == LeanBuiltinStatus.Yes)
      {
        return LeanApiResult.Error("内置语言不允许删除");
      }

      await _languageRepository.DeleteAsync(entity);

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"删除语言失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 批量删除语言
  /// </summary>
  public async Task<LeanApiResult> BatchDeleteAsync(List<long> ids)
  {
    try
    {
      var entities = await _languageRepository.GetListAsync(x => ids.Contains(x.Id));
      if (entities.Any(x => x.IsBuiltin == LeanBuiltinStatus.Yes))
      {
        return LeanApiResult.Error("选中的语言中包含内置语言，不允许删除");
      }

      await _languageRepository.DeleteAsync(x => ids.Contains(x.Id));

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"批量删除语言失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取语言详情
  /// </summary>
  public async Task<LeanApiResult<LeanLanguageDto>> GetAsync(long id)
  {
    try
    {
      var entity = await _languageRepository.GetByIdAsync(id);
      if (entity == null)
      {
        return LeanApiResult<LeanLanguageDto>.Error($"语言 {id} 不存在");
      }

      var dto = entity.Adapt<LeanLanguageDto>();
      return LeanApiResult<LeanLanguageDto>.Ok(dto);
    }
    catch (Exception ex)
    {
      return LeanApiResult<LeanLanguageDto>.Error($"获取语言详情失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 分页查询语言
  /// </summary>
  public async Task<LeanApiResult<LeanPageResult<LeanLanguageDto>>> GetPageAsync(LeanQueryLanguageDto input)
  {
    try
    {
      Expression<Func<LeanLanguage, bool>> predicate = x => true;

      if (!string.IsNullOrEmpty(input.LangName))
      {
        var langName = CleanInput(input.LangName);
        predicate = LeanExpressionExtensions.And(predicate, x => x.LangName.Contains(langName));
      }

      if (!string.IsNullOrEmpty(input.LangCode))
      {
        var langCode = CleanInput(input.LangCode);
        predicate = LeanExpressionExtensions.And(predicate, x => x.LangCode.Contains(langCode));
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

      var (total, items) = await _languageRepository.GetPageListAsync(predicate, input.PageSize, input.PageIndex);
      var dtos = items.Select(t => t.Adapt<LeanLanguageDto>()).ToList();

      var result = new LeanPageResult<LeanLanguageDto>
      {
        Total = total,
        Items = dtos,
        PageIndex = input.PageIndex,
        PageSize = input.PageSize
      };

      return LeanApiResult<LeanPageResult<LeanLanguageDto>>.Ok(result);
    }
    catch (Exception ex)
    {
      return LeanApiResult<LeanPageResult<LeanLanguageDto>>.Error($"分页查询语言失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 设置语言状态
  /// </summary>
  public async Task<LeanApiResult> SetStatusAsync(LeanChangeLanguageStatusDto input)
  {
    try
    {
      var entity = await _languageRepository.GetByIdAsync(input.Id);
      if (entity == null)
      {
        return LeanApiResult.Error($"语言 {input.Id} 不存在");
      }

      if (entity.IsBuiltin == LeanBuiltinStatus.Yes)
      {
        return LeanApiResult.Error("内置语言不允许修改状态");
      }

      entity.Status = input.Status;
      await _languageRepository.UpdateAsync(entity);

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"设置语言状态失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 导出语言
  /// </summary>
  public async Task<LeanApiResult<byte[]>> ExportAsync(LeanQueryLanguageDto input)
  {
    try
    {
      Expression<Func<LeanLanguage, bool>> predicate = x => true;

      if (!string.IsNullOrEmpty(input.LangName))
      {
        var langName = CleanInput(input.LangName);
        predicate = LeanExpressionExtensions.And(predicate, x => x.LangName.Contains(langName));
      }

      if (!string.IsNullOrEmpty(input.LangCode))
      {
        var langCode = CleanInput(input.LangCode);
        predicate = LeanExpressionExtensions.And(predicate, x => x.LangCode.Contains(langCode));
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

      var items = await _languageRepository.GetListAsync(predicate);
      var dtos = items.Select(t => t.Adapt<LeanLanguageExportDto>()).ToList();

      var bytes = LeanExcelHelper.Export(dtos);
      return LeanApiResult<byte[]>.Ok(bytes);
    }
    catch (Exception ex)
    {
      return LeanApiResult<byte[]>.Error($"导出语言失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 导入语言
  /// </summary>
  public async Task<LeanApiResult<LeanExcelImportResult<LeanLanguageImportDto>>> ImportAsync(LeanFileInfo file)
  {
    try
    {
      var result = new LeanExcelImportResult<LeanLanguageImportDto>();

      // 导入Excel
      var importResult = LeanExcelHelper.Import<LeanLanguageImportDto>(File.ReadAllBytes(file.FilePath));
      result.Data.AddRange(importResult.Data);
      result.Errors.AddRange(importResult.Errors);

      if (result.IsSuccess)
      {
        foreach (var item in result.Data)
        {
          try
          {
            // 检查语言代码是否存在
            var exists = await _languageRepository.AnyAsync(x => x.LangCode == item.LangCode);
            if (exists)
            {
              result.Errors.Add(new LeanExcelImportError
              {
                RowIndex = importResult.Data.IndexOf(item) + 2,
                ErrorMessage = $"语言代码 {item.LangCode} 已存在"
              });
              continue;
            }

            // 创建实体
            var entity = item.Adapt<LeanLanguage>();
            entity.Status = LeanStatus.Normal;
            await _languageRepository.CreateAsync(entity);
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

      return LeanApiResult<LeanExcelImportResult<LeanLanguageImportDto>>.Ok(result);
    }
    catch (Exception ex)
    {
      return LeanApiResult<LeanExcelImportResult<LeanLanguageImportDto>>.Error($"导入语言失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取导入模板
  /// </summary>
  public Task<LeanApiResult<byte[]>> GetImportTemplateAsync()
  {
    try
    {
      var template = new List<LeanLanguageImportTemplateDto>
      {
        new LeanLanguageImportTemplateDto()
      };

      var bytes = LeanExcelHelper.Export(template);
      return Task.FromResult(LeanApiResult<byte[]>.Ok(bytes));
    }
    catch (Exception ex)
    {
      return Task.FromResult(LeanApiResult<byte[]>.Error($"获取导入模板失败: {ex.Message}"));
    }
  }

  /// <summary>
  /// 获取语言列表
  /// </summary>
  public async Task<LeanApiResult<List<LeanLanguageDto>>> GetListAsync()
  {
    try
    {
      var items = await _languageRepository.GetListAsync(x => x.Status == LeanStatus.Normal);
      var dtos = items.Select(t => t.Adapt<LeanLanguageDto>()).ToList();
      return LeanApiResult<List<LeanLanguageDto>>.Ok(dtos);
    }
    catch (Exception ex)
    {
      return LeanApiResult<List<LeanLanguageDto>>.Error($"获取语言列表失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 设置默认语言
  /// </summary>
  public async Task<LeanApiResult> SetDefaultAsync(long id)
  {
    try
    {
      var entity = await _languageRepository.GetByIdAsync(id);
      if (entity == null)
      {
        return LeanApiResult.Error($"语言 {id} 不存在");
      }

      if (entity.Status != LeanStatus.Normal)
      {
        return LeanApiResult.Error("只能将正常状态的语言设为默认语言");
      }

      // 将其他语言的默认状态设为否
      var defaultLanguages = await _languageRepository.GetListAsync(x => x.IsDefault == LeanYesNo.Yes);
      foreach (var lang in defaultLanguages)
      {
        lang.IsDefault = LeanYesNo.No;
        await _languageRepository.UpdateAsync(lang);
      }

      // 设置当前语言为默认语言
      entity.IsDefault = LeanYesNo.Yes;
      await _languageRepository.UpdateAsync(entity);

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"设置默认语言失败: {ex.Message}");
    }
  }
}