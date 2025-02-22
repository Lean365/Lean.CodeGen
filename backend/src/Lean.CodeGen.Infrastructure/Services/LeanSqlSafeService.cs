using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using Lean.CodeGen.Common.Options;

namespace Lean.CodeGen.Infrastructure.Services;

/// <summary>
/// SQL 注入防护服务
/// </summary>
public interface ILeanSqlSafeService
{
  /// <summary>
  /// 检查是否包含 SQL 注入风险
  /// </summary>
  bool HasSqlInjectionRisk(string input);

  /// <summary>
  /// 清理 SQL 注入风险字符
  /// </summary>
  string CleanSqlInjection(string input);

  /// <summary>
  /// 转义 SQL 字符串
  /// </summary>
  string EscapeSqlString(string input);

  /// <summary>
  /// 验证并清理输入
  /// </summary>
  /// <returns>如果输入安全，返回清理后的输入；如果不安全，返回 null</returns>
  string? ValidateAndClean(string input);
}

/// <summary>
/// SQL 注入防护服务实现
/// </summary>
public class LeanSqlSafeService : ILeanSqlSafeService
{
  private readonly Regex _sqlInjectionPattern;
  private readonly bool _isEnabled;

  public LeanSqlSafeService(IOptions<LeanSecurityOptions> options)
  {
    _isEnabled = options.Value.EnableSqlInjection;
    var keywords = options.Value.SqlInjection.BlockedKeywords;
    var pattern = $@"(\b({string.Join("|", keywords)})\b)|([;'])";
    _sqlInjectionPattern = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
  }

  public bool HasSqlInjectionRisk(string input)
  {
    if (!_isEnabled) return false;
    return !string.IsNullOrEmpty(input) && _sqlInjectionPattern.IsMatch(input);
  }

  public string CleanSqlInjection(string input)
  {
    if (!_isEnabled || string.IsNullOrEmpty(input)) return input;
    return _sqlInjectionPattern.Replace(input, "");
  }

  public string EscapeSqlString(string input)
  {
    if (!_isEnabled || string.IsNullOrEmpty(input)) return input;
    return input.Replace("'", "''");
  }

  public string? ValidateAndClean(string input)
  {
    if (!_isEnabled) return input;
    if (string.IsNullOrEmpty(input)) return input;

    // 如果包含高风险字符，直接返回 null
    if (input.Contains(';') || input.Contains('\''))
    {
      return null;
    }

    // 清理 SQL 关键字
    var cleaned = CleanSqlInjection(input);

    // 如果清理后的字符串与原字符串长度相差太大，说明可能存在攻击
    if (Math.Abs(cleaned.Length - input.Length) > input.Length * 0.3)
    {
      return null;
    }

    return cleaned;
  }
}