namespace Lean.CodeGen.Common.Http;

/// <summary>
/// HTTP 上下文访问器接口
/// </summary>
public interface ILeanHttpContextAccessor
{
  /// <summary>
  /// 获取当前语言
  /// </summary>
  /// <returns>当前语言代码</returns>
  string GetCurrentLanguage();
}