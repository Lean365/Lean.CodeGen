namespace Lean.CodeGen.Application.Services.Security;

/// <summary>
/// SQL 注入防护服务接口
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