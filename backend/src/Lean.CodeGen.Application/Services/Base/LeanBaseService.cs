using Lean.CodeGen.Common.Options;
using Lean.CodeGen.Application.Services.Security;
using Microsoft.Extensions.Options;

namespace Lean.CodeGen.Application.Services.Base;

/// <summary>
/// 应用层基础服务
/// </summary>
public abstract class LeanBaseService
{
  protected readonly ILeanSqlSafeService SqlSafeService;
  protected readonly LeanSecurityOptions SecurityOptions;

  protected LeanBaseService(
      ILeanSqlSafeService sqlSafeService,
      IOptions<LeanSecurityOptions> securityOptions)
  {
    SqlSafeService = sqlSafeService;
    SecurityOptions = securityOptions.Value;
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
}
