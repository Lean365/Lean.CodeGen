// -----------------------------------------------------------------------
// <copyright file="ILeanNotificationService.cs" company="Lean">
// Copyright (c) Lean. All rights reserved.
// </copyright>
// <author>Lean</author>
// <created>2024-03-09</created>
// <summary>通知服务接口</summary>
// -----------------------------------------------------------------------

using Lean.CodeGen.Application.Dtos.Routine;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Excel;

namespace Lean.CodeGen.Application.Services.Routine;

/// <summary>
/// 通知服务接口
/// </summary>
public interface ILeanNotificationService
{
  /// <summary>
  /// 获取通知列表
  /// </summary>
  /// <param name="query">查询条件</param>
  /// <returns>通知列表</returns>
  Task<LeanApiResult<LeanPageResult<LeanNotificationDto>>> GetListAsync(LeanNotificationQueryDto query);

  /// <summary>
  /// 获取通知详情
  /// </summary>
  /// <param name="id">主键</param>
  /// <returns>通知详情</returns>
  Task<LeanApiResult<LeanNotificationDto>> GetAsync(long id);

  /// <summary>
  /// 创建通知
  /// </summary>
  /// <param name="input">通知信息</param>
  /// <returns>通知详情</returns>
  Task<LeanApiResult<LeanNotificationDto>> CreateAsync(LeanNotificationCreateDto input);

  /// <summary>
  /// 更新通知
  /// </summary>
  /// <param name="input">通知信息</param>
  /// <returns>通知详情</returns>
  Task<LeanApiResult<LeanNotificationDto>> UpdateAsync(LeanNotificationUpdateDto input);

  /// <summary>
  /// 删除通知
  /// </summary>
  /// <param name="id">主键</param>
  /// <returns>是否成功</returns>
  Task<LeanApiResult> DeleteAsync(long id);

  /// <summary>
  /// 批量删除通知
  /// </summary>
  /// <param name="ids">主键列表</param>
  /// <returns>是否成功</returns>
  Task<LeanApiResult> BatchDeleteAsync(List<long> ids);

  /// <summary>
  /// 导入通知
  /// </summary>
  /// <param name="file">导入文件</param>
  /// <returns>导入结果</returns>
  Task<LeanNotificationImportResultDto> ImportAsync(LeanFileInfo file);

  /// <summary>
  /// 导出通知
  /// </summary>
  /// <param name="query">查询条件</param>
  /// <returns>导出文件</returns>
  Task<byte[]> ExportAsync(LeanNotificationQueryDto query);

  /// <summary>
  /// 获取导入模板
  /// </summary>
  /// <returns>导入模板</returns>
  Task<byte[]> GetTemplateAsync();

  /// <summary>
  /// 发布通知
  /// </summary>
  /// <param name="id">主键</param>
  /// <returns>是否成功</returns>
  Task<LeanApiResult> PublishAsync(long id);

  /// <summary>
  /// 批量发布通知
  /// </summary>
  /// <param name="ids">主键列表</param>
  /// <returns>是否成功</returns>
  Task<LeanApiResult> BatchPublishAsync(List<long> ids);

  /// <summary>
  /// 撤回通知
  /// </summary>
  /// <param name="id">主键</param>
  /// <returns>是否成功</returns>
  Task<LeanApiResult> WithdrawAsync(long id);

  /// <summary>
  /// 批量撤回通知
  /// </summary>
  /// <param name="ids">主键列表</param>
  /// <returns>是否成功</returns>
  Task<LeanApiResult> BatchWithdrawAsync(List<long> ids);

  /// <summary>
  /// 阅读通知
  /// </summary>
  /// <param name="id">主键</param>
  /// <returns>是否成功</returns>
  Task<LeanApiResult> ReadAsync(long id);

  /// <summary>
  /// 确认通知
  /// </summary>
  /// <param name="id">主键</param>
  /// <returns>是否成功</returns>
  Task<LeanApiResult> ConfirmAsync(long id);

  /// <summary>
  /// 发送用户登录尝试通知
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <param name="message">消息</param>
  /// <param name="loginTime">登录时间</param>
  /// <param name="loginIp">登录IP</param>
  /// <param name="loginLocation">登录地点</param>
  Task NotifyUserLoginAttemptAsync(long userId, string message, DateTime loginTime, string loginIp, string loginLocation);

  /// <summary>
  /// 发送通知
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <param name="notification">通知内容</param>
  Task SendNotificationAsync(long userId, LeanNotificationDto notification);
}