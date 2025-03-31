// -----------------------------------------------------------------------
// <copyright file="ILeanMailService.cs" company="Lean">
// Copyright (c) Lean. All rights reserved.
// </copyright>
// <author>Lean</author>
// <created>2024-03-09</created>
// <summary>邮件服务接口</summary>
// -----------------------------------------------------------------------

using Lean.CodeGen.Application.Dtos.Routine;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Excel;
using System.Threading.Tasks;

namespace Lean.CodeGen.Application.Services.Routine;

/// <summary>
/// 邮件服务接口
/// </summary>
public interface ILeanMailService
{
  /// <summary>
  /// 获取邮件列表
  /// </summary>
  /// <param name="query">查询条件</param>
  /// <returns>邮件列表</returns>
  Task<LeanApiResult<LeanPageResult<LeanMailDto>>> GetListAsync(LeanMailQueryDto query);

  /// <summary>
  /// 获取邮件详情
  /// </summary>
  /// <param name="id">主键</param>
  /// <returns>邮件详情</returns>
  Task<LeanApiResult<LeanMailDto>> GetAsync(long id);

  /// <summary>
  /// 创建邮件
  /// </summary>
  /// <param name="input">邮件信息</param>
  /// <returns>邮件详情</returns>
  Task<LeanApiResult<LeanMailDto>> CreateAsync(LeanMailCreateDto input);

  /// <summary>
  /// 更新邮件
  /// </summary>
  /// <param name="input">邮件信息</param>
  /// <returns>邮件详情</returns>
  Task<LeanApiResult<LeanMailDto>> UpdateAsync(LeanMailUpdateDto input);

  /// <summary>
  /// 删除邮件
  /// </summary>
  /// <param name="id">主键</param>
  /// <returns>是否成功</returns>
  Task<LeanApiResult> DeleteAsync(long id);

  /// <summary>
  /// 批量删除邮件
  /// </summary>
  /// <param name="ids">主键列表</param>
  /// <returns>是否成功</returns>
  Task<LeanApiResult> BatchDeleteAsync(List<long> ids);

  /// <summary>
  /// 导出邮件
  /// </summary>
  /// <param name="input">查询条件</param>
  /// <returns>导出文件</returns>
  Task<byte[]> ExportAsync(LeanMailQueryDto input);

  /// <summary>
  /// 导入邮件
  /// </summary>
  /// <param name="file">导入文件</param>
  /// <returns>导入结果</returns>
  Task<LeanMailImportResultDto> ImportAsync(LeanFileInfo file);

  /// <summary>
  /// 获取导入模板
  /// </summary>
  /// <returns>导入模板</returns>
  Task<byte[]> GetTemplateAsync();

  /// <summary>
  /// 发送邮件
  /// </summary>
  /// <param name="to">收件人</param>
  /// <param name="subject">主题</param>
  /// <param name="body">内容</param>
  /// <param name="isHtml">是否HTML格式</param>
  Task<LeanApiResult> SendAsync(string to, string subject, string body, bool isHtml = true);

  /// <summary>
  /// 发送邮件
  /// </summary>
  /// <param name="to">收件人列表</param>
  /// <param name="subject">主题</param>
  /// <param name="body">内容</param>
  /// <param name="isHtml">是否HTML格式</param>
  Task<LeanApiResult> SendAsync(string[] to, string subject, string body, bool isHtml = true);

  /// <summary>
  /// 发送邮件
  /// </summary>
  /// <param name="id">邮件ID</param>
  /// <returns>是否成功</returns>
  Task<LeanApiResult> SendAsync(long id);

  /// <summary>
  /// 批量发送邮件
  /// </summary>
  /// <param name="ids">主键列表</param>
  /// <returns>是否成功</returns>
  Task<LeanApiResult> BatchSendAsync(List<long> ids);

  /// <summary>
  /// 撤回邮件
  /// </summary>
  /// <param name="id">主键</param>
  /// <returns>是否成功</returns>
  Task<LeanApiResult> WithdrawAsync(long id);

  /// <summary>
  /// 批量撤回邮件
  /// </summary>
  /// <param name="ids">主键列表</param>
  /// <returns>是否成功</returns>
  Task<LeanApiResult> BatchWithdrawAsync(List<long> ids);
}