namespace Lean.CodeGen.Common.Attributes;

/// <summary>
/// 权限特性
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class LeanPermissionAttribute : Attribute
{
  /// <summary>
  /// 权限编码
  /// </summary>
  public string Code { get; }

  /// <summary>
  /// 权限名称
  /// </summary>
  public string Name { get; }

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="code">权限编码</param>
  /// <param name="name">权限名称</param>
  public LeanPermissionAttribute(string code, string name)
  {
    Code = code;
    Name = name;
  }
}