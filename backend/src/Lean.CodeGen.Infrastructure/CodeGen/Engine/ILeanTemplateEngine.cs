namespace Lean.CodeGen.Infrastructure.CodeGen.Engine;

/// <summary>
/// 模板引擎接口
/// </summary>
public interface ILeanTemplateEngine
{
  /// <summary>
  /// 渲染模板
  /// </summary>
  Task<string> RenderAsync(string templatePath, object model);

  /// <summary>
  /// 渲染字符串模板
  /// </summary>
  Task<string> RenderStringAsync(string templateContent, object model);

  /// <summary>
  /// 编译模板
  /// </summary>
  Task<bool> CompileAsync(string templatePath);
}