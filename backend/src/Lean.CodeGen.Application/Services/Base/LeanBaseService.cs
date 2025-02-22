using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Lean.CodeGen.Common.Options;
using Lean.CodeGen.Application.Services.Security;
using Microsoft.Extensions.Options;
using Lean.CodeGen.Common.Exceptions;

namespace Lean.CodeGen.Application.Services.Base;

/// <summary>
/// 应用层基础服务
/// </summary>
public abstract class LeanBaseService
{
  protected readonly ILeanSqlSafeService SqlSafeService;
  protected readonly ILogger Logger;
  protected readonly LeanSecurityOptions SecurityOptions;

  protected LeanBaseService(
      ILeanSqlSafeService sqlSafeService,
      IOptions<LeanSecurityOptions> securityOptions,
      ILogger logger)
  {
    SqlSafeService = sqlSafeService;
    SecurityOptions = securityOptions.Value;
    Logger = logger;
  }

  /// <summary>
  /// 验证输入参数
  /// </summary>
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
  protected virtual string CleanInput(string input)
  {
    if (string.IsNullOrEmpty(input)) return input;
    return SqlSafeService.CleanSqlInjection(input);
  }

  /// <summary>
  /// 转义 SQL 字符串
  /// </summary>
  protected virtual string EscapeSql(string input)
  {
    if (string.IsNullOrEmpty(input)) return input;
    return SqlSafeService.EscapeSqlString(input);
  }

  /// <summary>
  /// 执行带事务的操作
  /// </summary>
  protected async Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action, string operationName)
  {
    try
    {
      using var scope = Logger.BeginScope(new
      {
        Operation = operationName,
        StartTime = DateTime.Now
      });

      Logger.LogInformation($"开始执行操作: {operationName}");

      var result = await action();

      Logger.LogInformation($"操作执行成功: {operationName}");
      return result;
    }
    catch (LeanException ex)
    {
      Logger.LogWarning(ex, $"业务异常: {operationName}");
      throw;
    }
    catch (Exception ex)
    {
      Logger.LogError(ex, $"操作执行失败: {operationName}");
      throw new LeanException($"执行 {operationName} 时发生错误: {ex.Message}");
    }
  }

  /// <summary>
  /// 执行带事务的操作（无返回值）
  /// </summary>
  protected async Task ExecuteInTransactionAsync(Func<Task> action, string operationName)
  {
    try
    {
      using var scope = Logger.BeginScope(new
      {
        Operation = operationName,
        StartTime = DateTime.Now
      });

      Logger.LogInformation($"开始执行操作: {operationName}");

      await action();

      Logger.LogInformation($"操作执行成功: {operationName}");
    }
    catch (LeanException ex)
    {
      Logger.LogWarning(ex, $"业务异常: {operationName}");
      throw;
    }
    catch (Exception ex)
    {
      Logger.LogError(ex, $"操作执行失败: {operationName}");
      throw new LeanException($"执行 {operationName} 时发生错误: {ex.Message}");
    }
  }

  /// <summary>
  /// 记录审计日志
  /// </summary>
  protected void LogAudit(string action, string details, bool success = true)
  {
    var logLevel = success ? LogLevel.Information : LogLevel.Warning;
    Logger.Log(logLevel, $"审计日志 - 操作: {action}, 详情: {details}, 状态: {(success ? "成功" : "失败")}");
  }

  /// <summary>
  /// 记录性能日志
  /// </summary>
  protected IDisposable LogPerformance(string operation)
  {
    var startTime = DateTime.Now;
    return new DisposableAction(() =>
    {
      var duration = DateTime.Now - startTime;
      Logger.LogInformation($"性能日志 - 操作: {operation}, 耗时: {duration.TotalMilliseconds}ms");
    });
  }
}

/// <summary>
/// 用于性能日志的辅助类
/// </summary>
internal class DisposableAction : IDisposable
{
  private readonly Action _action;

  public DisposableAction(Action action)
  {
    _action = action;
  }

  public void Dispose()
  {
    _action();
  }
}
