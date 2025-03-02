using Lean.CodeGen.Infrastructure.CodeGen.Models;

namespace Lean.CodeGen.Infrastructure.CodeGen.Engine;

/// <summary>
/// 代码生成引擎接口
/// </summary>
public interface ILeanCodeGenEngine
{
  /// <summary>
  /// 生成实体代码
  /// </summary>
  /// <param name="options">实体生成选项</param>
  /// <returns>生成结果</returns>
  Task<LeanCodeGenResult> GenerateEntityAsync(LeanEntityGenOptions options);

  /// <summary>
  /// 生成服务代码
  /// </summary>
  /// <param name="options">服务生成选项</param>
  /// <returns>生成结果</returns>
  Task<LeanCodeGenResult> GenerateServiceAsync(LeanServiceGenOptions options);

  /// <summary>
  /// 生成控制器代码
  /// </summary>
  /// <param name="options">控制器生成选项</param>
  /// <returns>生成结果</returns>
  Task<LeanCodeGenResult> GenerateControllerAsync(LeanControllerGenOptions options);

  /// <summary>
  /// 生成自定义代码
  /// </summary>
  /// <param name="options">代码生成选项</param>
  /// <returns>生成结果</returns>
  Task<LeanCodeGenResult> GenerateCustomAsync(LeanCodeGenOptions options);

  /// <summary>
  /// 验证模板
  /// </summary>
  /// <param name="templatePath">模板路径</param>
  /// <returns>是否有效</returns>
  Task<bool> ValidateTemplateAsync(string templatePath);

  /// <summary>
  /// 获取所有可用模板
  /// </summary>
  /// <returns>模板列表</returns>
  Task<IEnumerable<string>> GetAvailableTemplatesAsync();
}