using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using Lean.CodeGen.Common.Options;

namespace Lean.CodeGen.Common.Extensions;

/// <summary>
/// SQL 注入防护扩展
/// </summary>
public class LeanSqlSafeService
{
  private readonly Regex _sqlInjectionPattern;

  public LeanSqlSafeService(IOptions<LeanSecurityOptions> options)
  {
    var keywords = options.Value.SqlInjection.BlockedKeywords;
    var pattern = $@"(\b({string.Join("|", keywords)})\b)|([;'])";
    _sqlInjectionPattern = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
  }

  /// <summary>
  /// 检查是否包含 SQL 注入风险
  /// </summary>
  public bool HasSqlInjectionRisk(string input)
  {
    return !string.IsNullOrEmpty(input) && _sqlInjectionPattern.IsMatch(input);
  }

  /// <summary>
  /// 清理 SQL 注入风险字符
  /// </summary>
  public string CleanSqlInjection(string input)
  {
    if (string.IsNullOrEmpty(input)) return input;
    return _sqlInjectionPattern.Replace(input, "");
  }

  /// <summary>
  /// 转义 SQL 字符串
  /// </summary>
  public string EscapeSqlString(string input)
  {
    if (string.IsNullOrEmpty(input)) return input;
    return input.Replace("'", "''");
  }
}