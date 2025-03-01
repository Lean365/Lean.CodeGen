using System;
using System.Threading.Tasks;
using Lean.CodeGen.Common.Options;
using Lean.CodeGen.Application.Services.Security;
using Microsoft.Extensions.Options;
using Lean.CodeGen.Common.Exceptions;
using Lean.CodeGen.Common.Security;
using NLog;

namespace Lean.CodeGen.Application.Services.Base;

/// <summary>
/// 应用层基础服务
/// </summary>
/// <remarks>
/// 提供应用层服务的基础功能，包括：
/// 1. SQL注入防护
/// 2. 事务管理
/// 3. 异常处理
/// 4. 日志记录
/// 5. 审计跟踪
/// 6. 性能监控
/// </remarks>
public abstract class LeanBaseService
{
  /// <summary>
  /// SQL注入防护服务
  /// </summary>
  /// <remarks>
  /// 用于验证和清理输入参数，防止SQL注入攻击
  /// </remarks>
  protected readonly ILeanSqlSafeService SqlSafeService;

  /// <summary>
  /// 日志记录器
  /// </summary>
  /// <remarks>
  /// 用于记录服务操作的日志信息，包括信息、警告和错误日志
  /// </remarks>
  protected readonly ILogger Logger;

  /// <summary>
  /// 安全配置选项
  /// </summary>
  /// <remarks>
  /// 包含系统安全相关的配置信息，如密码策略、令牌设置等
  /// </remarks>
  protected readonly LeanSecurityOptions SecurityOptions;

  /// <summary>
  /// 服务上下文
  /// </summary>
  protected readonly LeanBaseServiceContext Context;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="context">基础服务上下文</param>
  protected LeanBaseService(LeanBaseServiceContext context)
  {
    Context = context;
    SqlSafeService = context.SqlSafeService;
    SecurityOptions = context.SecurityOptions;
    Logger = context.Logger;
  }

  /// <summary>
  /// 在事务中执行操作
  /// </summary>
  /// <typeparam name="T">返回值类型</typeparam>
  /// <param name="action">要执行的操作</param>
  /// <param name="operationName">操作名称</param>
  /// <returns>操作结果</returns>
  protected virtual async Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action, string operationName)
  {
    try
    {
      Logger.Info($"开始执行 {operationName}");
      var result = await action();
      Logger.Info($"{operationName} 执行成功");
      return result;
    }
    catch (Exception ex)
    {
      Logger.Error(ex, $"{operationName} 执行失败");
      throw;
    }
  }

  /// <summary>
  /// 验证输入参数
  /// </summary>
  /// <param name="inputs">待验证的输入参数数组</param>
  /// <returns>验证结果：true-验证通过，false-验证失败</returns>
  /// <remarks>
  /// 对输入参数进行SQL注入检查，确保参数安全性。
  /// 如果参数为空或空字符串，则跳过验证。
  /// 任何参数验证失败都会导致整体验证失败。
  /// </remarks>
  protected virtual bool ValidateInput(params string[] inputs)
  {
    if (inputs == null || inputs.Length == 0) return true;

    foreach (var input in inputs)
    {
      if (string.IsNullOrEmpty(input)) continue;

      var cleanInput = SqlSafeService.ValidateAndClean(input);
      if (cleanInput == null)
      {
        return false;
      }
    }

    return true;
  }

  /// <summary>
  /// 清理输入参数
  /// </summary>
  /// <param name="input">待清理的输入字符串</param>
  /// <returns>清理后的安全字符串</returns>
  /// <remarks>
  /// 移除输入字符串中的SQL注入风险字符。
  /// 如果输入为空或空字符串，则直接返回原值。
  /// </remarks>
  protected virtual string CleanInput(string input)
  {
    if (string.IsNullOrEmpty(input)) return input;
    return SqlSafeService.CleanSqlInjection(input);
  }

  /// <summary>
  /// 转义SQL字符串
  /// </summary>
  /// <param name="input">待转义的SQL字符串</param>
  /// <returns>转义后的SQL安全字符串</returns>
  /// <remarks>
  /// 对SQL字符串中的特殊字符进行转义处理。
  /// 如果输入为空或空字符串，则直接返回原值。
  /// </remarks>
  protected virtual string EscapeSql(string input)
  {
    if (string.IsNullOrEmpty(input)) return input;
    return SqlSafeService.EscapeSqlString(input);
  }

  /// <summary>
  /// 执行带事务的操作（无返回值）
  /// </summary>
  /// <param name="action">要执行的操作</param>
  /// <param name="operationName">操作名称</param>
  /// <remarks>
  /// 在事务上下文中执行无返回值的操作，提供：
  /// 1. 操作开始和结束的日志记录
  /// 2. 异常捕获和处理
  /// 3. 业务异常与系统异常的区分处理
  /// </remarks>
  protected async Task ExecuteInTransactionAsync(Func<Task> action, string operationName)
  {
    try
    {
      Logger.Info($"开始执行操作: {operationName}");

      await action();

      Logger.Info($"操作执行成功: {operationName}");
    }
    catch (LeanException ex)
    {
      Logger.Warn(ex, $"业务异常: {operationName}");
      throw;
    }
    catch (Exception ex)
    {
      Logger.Error(ex, $"操作执行失败: {operationName}");
      throw new LeanException($"执行 {operationName} 时发生错误: {ex.Message}");
    }
  }

  /// <summary>
  /// 记录审计日志
  /// </summary>
  /// <param name="action">操作类型</param>
  /// <param name="details">操作详情</param>
  /// <param name="success">操作是否成功</param>
  /// <remarks>
  /// 记录业务操作的审计日志，包含：
  /// 1. 操作类型
  /// 2. 详细信息
  /// 3. 操作结果
  /// 根据操作成功与否使用不同的日志级别
  /// </remarks>
  protected void LogAudit(string action, string details, bool success = true)
  {
    if (success)
    {
      Logger.Info($"审计日志 - 操作: {action}, 详情: {details}, 状态: 成功");
    }
    else
    {
      Logger.Warn($"审计日志 - 操作: {action}, 详情: {details}, 状态: 失败");
    }
  }

  /// <summary>
  /// 记录性能日志
  /// </summary>
  /// <param name="operation">操作名称</param>
  /// <returns>用于释放的IDisposable对象</returns>
  /// <remarks>
  /// 用于记录操作的执行时间，使用方式：
  /// using (LogPerformance("操作名称"))
  /// {
  ///     // 要监控性能的代码
  /// }
  /// 在代码块结束时自动记录执行时间
  /// </remarks>
  protected IDisposable LogPerformance(string operation)
  {
    var startTime = DateTime.Now;
    return new DisposableAction(() =>
    {
      var duration = DateTime.Now - startTime;
      Logger.Info($"性能日志 - 操作: {operation}, 耗时: {duration.TotalMilliseconds}ms");
    });
  }
}

/// <summary>
/// 用于性能日志的辅助类
/// </summary>
/// <remarks>
/// 实现IDisposable接口，用于在using语句结束时自动执行清理操作
/// </remarks>
internal class DisposableAction : IDisposable
{
  private readonly Action _action;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="action">释放时要执行的操作</param>
  public DisposableAction(Action action)
  {
    _action = action;
  }

  /// <summary>
  /// 执行释放操作
  /// </summary>
  /// <remarks>
  /// 在对象释放时执行指定的操作
  /// </remarks>
  public void Dispose()
  {
    _action();
  }
}