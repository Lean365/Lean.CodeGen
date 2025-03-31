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
using NLog;
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
  private readonly ILogger _logger;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanLanguageService(
      ILeanRepository<LeanLanguage> languageRepository,
      LeanBaseServiceContext context)
      : base(context)
  {
    _languageRepository = languageRepository;
    _logger = context.Logger;
    _uniqueValidator = new LeanUniqueValidator<LeanLanguage>(_languageRepository);
  }

  /// <summary>
  /// 创建语言
  /// </summary>
  public async Task<LeanApiResult<long>> CreateAsync(LeanLanguageCreateDto input)
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
      entity.LangStatus = 2;
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
  public async Task<LeanApiResult> UpdateAsync(LeanLanguageUpdateDto input)
  {
    try
    {
      var entity = await _languageRepository.GetByIdAsync(input.Id);
      if (entity == null)
      {
        return LeanApiResult.Error($"语言 {input.Id} 不存在");
      }

      if (entity.IsBuiltin == 1)
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

      if (entity.IsBuiltin == 1)
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
      if (entities.Any(x => x.IsBuiltin == 1))
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
  public async Task<LeanApiResult<LeanPageResult<LeanLanguageDto>>> GetPageAsync(LeanLanguageQueryDto input)
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

      if (input.LangStatus.HasValue)
      {
        predicate = LeanExpressionExtensions.And(predicate, x => x.LangStatus == (int)input.LangStatus.Value);
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
  public async Task<LeanApiResult> SetStatusAsync(LeanLanguageChangeStatusDto input)
  {
    return await ExecuteInTransactionAsync(async () =>
    {
      var entity = await _languageRepository.GetByIdAsync(input.Id);
      if (entity == null)
      {
        throw new LeanException("语言不存在");
      }

      if (entity.IsBuiltin == 1)
      {
        throw new LeanException("内置语言不允许修改状态");
      }

      entity.LangStatus = input.LangStatus;
      await _languageRepository.UpdateAsync(entity);

      return LeanApiResult.Ok();
    }, "修改语言状态");
  }

  /// <summary>
  /// 导出语言
  /// </summary>
  public async Task<byte[]> ExportAsync(LeanLanguageQueryDto input)
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

    if (input.LangStatus.HasValue)
    {
      predicate = LeanExpressionExtensions.And(predicate, x => x.LangStatus == (int)input.LangStatus.Value);
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
    return LeanExcelHelper.Export(dtos);
  }

  /// <summary>
  /// 导入语言
  /// </summary>
  public async Task<LeanExcelImportResult<LeanLanguageImportDto>> ImportAsync(LeanFileInfo file)
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
          entity.LangStatus = 2;
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

    return result;
  }

  /// <summary>
  /// 获取导入模板
  /// </summary>
  public Task<byte[]> GetTemplateAsync()
  {
    var template = new List<LeanLanguageImportTemplateDto>
    {
      new LeanLanguageImportTemplateDto()
    };

    var bytes = LeanExcelHelper.Export(template);
    return Task.FromResult(bytes);
  }

  /// <summary>
  /// 获取语言列表
  /// </summary>
  public async Task<LeanApiResult<List<LeanLanguageDto>>> GetListAsync()
  {
    try
    {
      var items = await _languageRepository.GetListAsync(x => x.LangStatus == 2);
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

      if (entity.LangStatus != 2)
      {
        return LeanApiResult.Error("只能将正常状态的语言设为默认语言");
      }

      if (entity.IsBuiltin == 1)
      {
        return LeanApiResult.Error("内置语言不允许设置为默认语言");
      }

      // 将所有语言的默认状态设置为否
      var languages = await _languageRepository.GetListAsync(x => x.IsDefault == 1);
      foreach (var lang in languages)
      {
        lang.IsDefault = 0;
        await _languageRepository.UpdateAsync(lang);
      }

      // 设置当前语言为默认语言
      entity.IsDefault = 1;
      await _languageRepository.UpdateAsync(entity);

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"设置默认语言失败: {ex.Message}");
    }
  }
}