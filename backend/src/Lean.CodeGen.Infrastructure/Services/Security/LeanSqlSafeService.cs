using System.Text.RegularExpressions;
using Lean.CodeGen.Common.Options;
using Lean.CodeGen.Application.Services.Security;
using Microsoft.Extensions.Options;

namespace Lean.CodeGen.Infrastructure.Services.Security;

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
    if (!_isEnabled || string.IsNullOrEmpty(input))
    {
      return false;
    }

    return _sqlInjectionPattern.IsMatch(input);
  }

  public string CleanSqlInjection(string input)
  {
    if (!_isEnabled || string.IsNullOrEmpty(input))
    {
      return input;
    }

    return _sqlInjectionPattern.Replace(input, string.Empty);
  }

  public string EscapeSqlString(string input)
  {
    if (string.IsNullOrEmpty(input))
    {
      return input;
    }

    return input.Replace("'", "''");
  }

  public string? ValidateAndClean(string input)
  {
    if (string.IsNullOrEmpty(input))
    {
      return input;
    }

    if (HasSqlInjectionRisk(input))
    {
      var cleaned = CleanSqlInjection(input);
      if (HasSqlInjectionRisk(cleaned))
      {
        return null;
      }
      return cleaned;
    }

    return input;
  }
}