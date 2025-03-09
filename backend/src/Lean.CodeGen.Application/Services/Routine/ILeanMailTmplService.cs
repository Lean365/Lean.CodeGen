// -----------------------------------------------------------------------
// <copyright file="ILeanMailTmplService.cs" company="Lean">
// Copyright (c) Lean. All rights reserved.
// </copyright>
// <author>Lean</author>
// <created>2024-03-09</created>
// <summary>邮件模板服务接口</summary>
// -----------------------------------------------------------------------

using Lean.CodeGen.Application.Dtos.Routine;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Excel;

namespace Lean.CodeGen.Application.Services.Routine;

/// <summary>
/// 邮件模板服务接口
/// </summary>
public interface ILeanMailTmplService
{
  /// <summary>
  /// 获取邮件模板列表
  /// </summary>
  /// <param name="query">查询条件</param>
  /// <returns>邮件模板列表</returns>
  Task<LeanApiResult<LeanPageResult<LeanMailTmplDto>>> GetListAsync(LeanMailTmplQueryDto query);

  /// <summary>
  /// 获取邮件模板详情
  /// </summary>
  /// <param name="id">主键</param>
  /// <returns>邮件模板详情</returns>
  Task<LeanApiResult<LeanMailTmplDto>> GetAsync(long id);

  /// <summary>
  /// 创建邮件模板
  /// </summary>
  /// <param name="input">邮件模板信息</param>
  /// <returns>邮件模板详情</returns>
  Task<LeanApiResult<LeanMailTmplDto>> CreateAsync(LeanMailTmplCreateDto input);

  /// <summary>
  /// 更新邮件模板
  /// </summary>
  /// <param name="input">邮件模板信息</param>
  /// <returns>邮件模板详情</returns>
  Task<LeanApiResult<LeanMailTmplDto>> UpdateAsync(LeanMailTmplUpdateDto input);

  /// <summary>
  /// 删除邮件模板
  /// </summary>
  /// <param name="id">主键</param>
  /// <returns>是否成功</returns>
  Task<LeanApiResult> DeleteAsync(long id);

  /// <summary>
  /// 批量删除邮件模板
  /// </summary>
  /// <param name="ids">主键列表</param>
  /// <returns>是否成功</returns>
  Task<LeanApiResult> BatchDeleteAsync(List<long> ids);

  /// <summary>
  /// 导出邮件模板
  /// </summary>
  /// <param name="input">查询条件</param>
  /// <returns>导出文件</returns>
  Task<byte[]> ExportAsync(LeanMailTmplQueryDto input);

  /// <summary>
  /// 导入邮件模板
  /// </summary>
  /// <param name="file">导入文件</param>
  /// <returns>导入结果</returns>
  Task<LeanMailTmplImportResultDto> ImportAsync(LeanFileInfo file);

  /// <summary>
  /// 获取导入模板
  /// </summary>
  /// <returns>导入模板</returns>
  Task<byte[]> GetImportTemplateAsync();

  /// <summary>
  /// 启用邮件模板
  /// </summary>
  /// <param name="id">主键</param>
  /// <returns>是否成功</returns>
  Task<LeanApiResult> EnableAsync(long id);

  /// <summary>
  /// 禁用邮件模板
  /// </summary>
  /// <param name="id">主键</param>
  /// <returns>是否成功</returns>
  Task<LeanApiResult> DisableAsync(long id);

  /// <summary>
  /// 批量启用邮件模板
  /// </summary>
  /// <param name="ids">主键列表</param>
  /// <returns>是否成功</returns>
  Task<LeanApiResult> BatchEnableAsync(List<long> ids);

  /// <summary>
  /// 批量禁用邮件模板
  /// </summary>
  /// <param name="ids">主键列表</param>
  /// <returns>是否成功</returns>
  Task<LeanApiResult> BatchDisableAsync(List<long> ids);
}