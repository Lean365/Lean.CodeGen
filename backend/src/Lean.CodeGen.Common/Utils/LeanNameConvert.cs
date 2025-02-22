using System.Text;

namespace Lean.CodeGen.Common.Utils;

/// <summary>
/// 名称转换工具类
/// </summary>
public static class LeanNameConvert
{
  /// <summary>
  /// 驼峰命名转下划线
  /// </summary>
  /// <param name="name">驼峰命名</param>
  /// <returns>下划线命名</returns>
  public static string ToUnderline(string name)
  {
    if (string.IsNullOrEmpty(name))
    {
      return string.Empty;
    }

    var builder = new StringBuilder();
    builder.Append(char.ToLower(name[0]));

    for (var i = 1; i < name.Length; i++)
    {
      var current = name[i];
      if (char.IsUpper(current))
      {
        builder.Append('_');
        builder.Append(char.ToLower(current));
      }
      else
      {
        builder.Append(current);
      }
    }

    return builder.ToString();
  }
}