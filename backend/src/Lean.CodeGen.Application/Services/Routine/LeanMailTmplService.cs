// -----------------------------------------------------------------------
// <copyright file="LeanMailTmplService.cs" company="Lean">
// Copyright (c) Lean. All rights reserved.
// </copyright>
// <author>Lean</author>
// <created>2024-03-09</created>
// <summary>邮件模板服务实现</summary>
// -----------------------------------------------------------------------

using Lean.CodeGen.Application.Dtos.Routine;
using Lean.CodeGen.Application.Services.Base;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Excel;
using Lean.CodeGen.Common.Extensions;
using Lean.CodeGen.Common.Exceptions;
using Lean.CodeGen.Domain.Entities.Routine;
using Lean.CodeGen.Domain.Interfaces.Repositories;
using Lean.CodeGen.Domain.Validators;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NLog;

namespace Lean.CodeGen.Application.Services.Routine;

/// <summary>
/// 邮件模板服务实现
/// </summary>
public class LeanMailTmplService : LeanBaseService, ILeanMailTmplService
{
  private readonly ILogger _logger;
  private readonly ILeanRepository<LeanMailTmpl> _repository;
  private readonly LeanUniqueValidator<LeanMailTmpl> _uniqueValidator;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanMailTmplService(
      ILeanRepository<LeanMailTmpl> repository,
      LeanBaseServiceContext context)
      : base(context)
  {
    _repository = repository;
    _logger = context.Logger;
    _uniqueValidator = new LeanUniqueValidator<LeanMailTmpl>(_repository);
  }

  /// <summary>
  /// 构建查询条件
  /// </summary>
  private Expression<Func<LeanMailTmpl, bool>> BuildQueryPredicate(LeanMailTmplQueryDto input)
  {
    Expression<Func<LeanMailTmpl, bool>> predicate = x => true;

    if (!string.IsNullOrEmpty(input.TmplCode))
    {
      predicate = predicate.And(x => x.TmplCode.Contains(input.TmplCode));
    }

    if (!string.IsNullOrEmpty(input.TmplName))
    {
      predicate = predicate.And(x => x.TmplName.Contains(input.TmplName));
    }

    if (input.TmplStatus.HasValue)
    {
      predicate = predicate.And(x => x.TmplStatus == input.TmplStatus.Value);
    }

    return predicate;
  }

  /// <summary>
  /// 获取邮件模板列表
  /// </summary>
  public async Task<LeanApiResult<LeanPageResult<LeanMailTmplDto>>> GetListAsync(LeanMailTmplQueryDto input)
  {
    try
    {
      var predicate = BuildQueryPredicate(input);
      var (total, items) = await _repository.GetPageListAsync(predicate, input.PageSize, input.PageIndex);
      var dtos = items.Select(t => t.Adapt<LeanMailTmplDto>()).ToList();

      var result = new LeanPageResult<LeanMailTmplDto>
      {
        Total = total,
        Items = dtos,
        PageIndex = input.PageIndex,
        PageSize = input.PageSize
      };

      return LeanApiResult<LeanPageResult<LeanMailTmplDto>>.Ok(result);
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "获取邮件模板列表失败");
      return LeanApiResult<LeanPageResult<LeanMailTmplDto>>.Error("获取邮件模板列表失败");
    }
  }

  /// <summary>
  /// 获取邮件模板详情
  /// </summary>
  public async Task<LeanApiResult<LeanMailTmplDto>> GetAsync(long id)
  {
    try
    {
      var entity = await _repository.GetByIdAsync(id);
      if (entity == null)
      {
        return LeanApiResult<LeanMailTmplDto>.Error("邮件模板不存在");
      }

      return LeanApiResult<LeanMailTmplDto>.Ok(entity.Adapt<LeanMailTmplDto>());
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "获取邮件模板详情失败");
      return LeanApiResult<LeanMailTmplDto>.Error("获取邮件模板详情失败");
    }
  }

  /// <summary>
  /// 创建邮件模板
  /// </summary>
  public async Task<LeanApiResult<LeanMailTmplDto>> CreateAsync(LeanMailTmplCreateDto input)
  {
    try
    {
      // 检查编码是否重复
      await _uniqueValidator.ValidateAsync(x => x.TmplCode, input.TmplCode, null, "模板编码已存在");

      var entity = input.Adapt<LeanMailTmpl>();
      await _repository.CreateAsync(entity);

      return LeanApiResult<LeanMailTmplDto>.Ok(entity.Adapt<LeanMailTmplDto>());
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "创建邮件模板失败");
      return LeanApiResult<LeanMailTmplDto>.Error("创建邮件模板失败");
    }
  }

  /// <summary>
  /// 更新邮件模板
  /// </summary>
  public async Task<LeanApiResult<LeanMailTmplDto>> UpdateAsync(LeanMailTmplUpdateDto input)
  {
    try
    {
      var entity = await _repository.GetByIdAsync(input.Id);
      if (entity == null)
      {
        return LeanApiResult<LeanMailTmplDto>.Error("邮件模板不存在");
      }

      // 检查编码是否重复
      await _uniqueValidator.ValidateAsync(x => x.TmplCode, input.TmplCode, input.Id, "模板编码已存在");

      input.Adapt(entity);
      await _repository.UpdateAsync(entity);

      return LeanApiResult<LeanMailTmplDto>.Ok(entity.Adapt<LeanMailTmplDto>());
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "更新邮件模板失败");
      return LeanApiResult<LeanMailTmplDto>.Error("更新邮件模板失败");
    }
  }

  /// <summary>
  /// 删除邮件模板
  /// </summary>
  public async Task<LeanApiResult> DeleteAsync(long id)
  {
    try
    {
      var entity = await _repository.GetByIdAsync(id);
      if (entity == null)
      {
        return LeanApiResult.Error("邮件模板不存在");
      }

      await _repository.DeleteAsync(entity);
      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "删除邮件模板失败");
      return LeanApiResult.Error("删除邮件模板失败");
    }
  }

  /// <summary>
  /// 批量删除邮件模板
  /// </summary>
  public async Task<LeanApiResult> BatchDeleteAsync(List<long> ids)
  {
    try
    {
      var entities = await _repository.GetListAsync(x => ids.Contains(x.Id));
      if (!entities.Any())
      {
        return LeanApiResult.Error("邮件模板不存在");
      }

      foreach (var entity in entities)
      {
        await _repository.DeleteAsync(entity);
      }

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "批量删除邮件模板失败");
      return LeanApiResult.Error("批量删除邮件模板失败");
    }
  }

  /// <summary>
  /// 导出邮件模板
  /// </summary>
  public async Task<byte[]> ExportAsync(LeanMailTmplQueryDto input)
  {
    try
    {
      var predicate = BuildQueryPredicate(input);
      var list = await _repository.GetListAsync(predicate);
      var dtos = list.Select(t => t.Adapt<LeanMailTmplExportDto>()).ToList();

      return LeanExcelHelper.Export(dtos);
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "导出邮件模板失败");
      throw new LeanException("导出邮件模板失败");
    }
  }

  /// <summary>
  /// 导入邮件模板
  /// </summary>
  public async Task<LeanMailTmplImportResultDto> ImportAsync(LeanFileInfo file)
  {
    var result = new LeanMailTmplImportResultDto();

    try
    {
      // 验证文件
      if (file == null || file.Stream == null || file.Stream.Length == 0)
      {
        result.AddError("", "请选择要导入的文件");
        return result;
      }

      // 读取文件内容
      var bytes = new byte[file.Stream.Length];
      await file.Stream.ReadAsync(bytes, 0, (int)file.Stream.Length);

      // 导入Excel数据
      var importResult = LeanExcelHelper.Import<LeanMailTmplImportDto>(bytes);

      foreach (var item in importResult.Data)
      {
        try
        {
          var entity = item.Adapt<LeanMailTmpl>();

          // 检查编码是否重复
          await _uniqueValidator.ValidateAsync(x => x.TmplCode, entity.TmplCode, null, "模板编码已存在");

          // 创建实体
          await _repository.CreateAsync(entity);
          result.SuccessCount++;
          result.TotalCount++;
        }
        catch (Exception ex)
        {
          result.AddError(item.TmplName, ex.Message);
        }
      }

      // 添加Excel导入的错误信息
      foreach (var error in importResult.Errors)
      {
        result.AddError($"第{error.RowIndex}行", error.ErrorMessage);
      }

      return result;
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "导入邮件模板失败");
      result.ErrorMessage = ex.Message;
      return result;
    }
  }

  /// <summary>
  /// 获取导入模板
  /// </summary>
  public Task<byte[]> GetTemplateAsync()
  {
    try
    {
      return Task.FromResult(LeanExcelHelper.GetImportTemplate<LeanMailTmplImportDto>());
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "获取导入模板失败");
      throw new LeanException("获取导入模板失败");
    }
  }

  /// <summary>
  /// 启用邮件模板
  /// </summary>
  public async Task<LeanApiResult> EnableAsync(long id)
  {
    try
    {
      var entity = await _repository.GetByIdAsync(id);
      if (entity == null)
      {
        return LeanApiResult.Error("邮件模板不存在");
      }

      entity.TmplStatus = 1;
      await _repository.UpdateAsync(entity);
      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "启用邮件模板失败");
      return LeanApiResult.Error("启用邮件模板失败");
    }
  }

  /// <summary>
  /// 禁用邮件模板
  /// </summary>
  public async Task<LeanApiResult> DisableAsync(long id)
  {
    try
    {
      var entity = await _repository.GetByIdAsync(id);
      if (entity == null)
      {
        return LeanApiResult.Error("邮件模板不存在");
      }

      entity.TmplStatus = 0;
      await _repository.UpdateAsync(entity);
      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "禁用邮件模板失败");
      return LeanApiResult.Error("禁用邮件模板失败");
    }
  }

  /// <summary>
  /// 批量启用邮件模板
  /// </summary>
  public async Task<LeanApiResult> BatchEnableAsync(List<long> ids)
  {
    try
    {
      var entities = await _repository.GetListAsync(x => ids.Contains(x.Id));
      if (!entities.Any())
      {
        return LeanApiResult.Error("邮件模板不存在");
      }

      foreach (var entity in entities)
      {
        entity.TmplStatus = 1;
        await _repository.UpdateAsync(entity);
      }

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "批量启用邮件模板失败");
      return LeanApiResult.Error("批量启用邮件模板失败");
    }
  }

  /// <summary>
  /// 批量禁用邮件模板
  /// </summary>
  public async Task<LeanApiResult> BatchDisableAsync(List<long> ids)
  {
    try
    {
      var entities = await _repository.GetListAsync(x => ids.Contains(x.Id));
      if (!entities.Any())
      {
        return LeanApiResult.Error("邮件模板不存在");
      }

      foreach (var entity in entities)
      {
        entity.TmplStatus = 0;
        await _repository.UpdateAsync(entity);
      }

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "批量禁用邮件模板失败");
      return LeanApiResult.Error("批量禁用邮件模板失败");
    }
  }
}