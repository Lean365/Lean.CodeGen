using System.ComponentModel.DataAnnotations;
using Lean.CodeGen.Infrastructure.CodeGen.Models;
using Microsoft.Extensions.Logging;

namespace Lean.CodeGen.Infrastructure.CodeGen.Engine;

/// <summary>
/// 代码生成引擎实现
/// </summary>
public class LeanCodeGenEngine : ILeanCodeGenEngine
{
  private readonly ILeanTemplateEngine _templateEngine;
  private readonly ILogger<LeanCodeGenEngine> _logger;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanCodeGenEngine(
      ILeanTemplateEngine templateEngine,
      ILogger<LeanCodeGenEngine> logger)
  {
    _templateEngine = templateEngine;
    _logger = logger;
  }

  /// <inheritdoc/>
  public async Task<LeanCodeGenResult> GenerateEntityAsync(LeanEntityGenOptions options)
  {
    try
    {
      // 验证参数
      ValidateOptions(options);

      // 设置默认模板
      if (string.IsNullOrEmpty(options.TemplatePath))
      {
        options.TemplatePath = "Templates/Entity.cshtml";
      }

      // 生成代码
      var code = await _templateEngine.RenderAsync(options.TemplatePath, options);

      // 保存文件
      var filePath = Path.Combine(options.OutputPath, $"{options.EntityName}.cs");
      await SaveCodeToFileAsync(filePath, code, options.Overwrite);

      return LeanCodeGenResult.CreateSuccess(filePath, code);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "生成实体代码失败");
      return LeanCodeGenResult.CreateError(ex.Message);
    }
  }

  /// <inheritdoc/>
  public async Task<LeanCodeGenResult> GenerateServiceAsync(LeanServiceGenOptions options)
  {
    try
    {
      // 验证参数
      ValidateOptions(options);

      // 设置默认模板
      if (string.IsNullOrEmpty(options.TemplatePath))
      {
        options.TemplatePath = options.GenerateInterface
            ? "Templates/IService.cshtml"
            : "Templates/Service.cshtml";
      }

      // 生成代码
      var code = await _templateEngine.RenderAsync(options.TemplatePath, options);

      // 保存文件
      var fileName = options.GenerateInterface
          ? $"I{options.ServiceName}.cs"
          : $"{options.ServiceName}.cs";
      var filePath = Path.Combine(options.OutputPath, fileName);
      await SaveCodeToFileAsync(filePath, code, options.Overwrite);

      return LeanCodeGenResult.CreateSuccess(filePath, code);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "生成服务代码失败");
      return LeanCodeGenResult.CreateError(ex.Message);
    }
  }

  /// <inheritdoc/>
  public async Task<LeanCodeGenResult> GenerateControllerAsync(LeanControllerGenOptions options)
  {
    try
    {
      // 验证参数
      ValidateOptions(options);

      // 设置默认模板
      if (string.IsNullOrEmpty(options.TemplatePath))
      {
        options.TemplatePath = "Templates/Controller.cshtml";
      }

      // 生成代码
      var code = await _templateEngine.RenderAsync(options.TemplatePath, options);

      // 保存文件
      var filePath = Path.Combine(options.OutputPath, $"{options.ControllerName}Controller.cs");
      await SaveCodeToFileAsync(filePath, code, options.Overwrite);

      return LeanCodeGenResult.CreateSuccess(filePath, code);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "生成控制器代码失败");
      return LeanCodeGenResult.CreateError(ex.Message);
    }
  }

  /// <inheritdoc/>
  public async Task<LeanCodeGenResult> GenerateCustomAsync(LeanCodeGenOptions options)
  {
    try
    {
      // 验证参数
      ValidateOptions(options);

      // 生成代码
      var code = await _templateEngine.RenderAsync(options.TemplatePath, options.Model ?? new { });

      // 保存文件
      var fileName = Path.GetFileName(options.TemplatePath)
          .Replace(".cshtml", ".cs")
          .Replace(".razor", ".cs");
      var filePath = Path.Combine(options.OutputPath, fileName);
      await SaveCodeToFileAsync(filePath, code, options.Overwrite);

      return LeanCodeGenResult.CreateSuccess(filePath, code);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "生成自定义代码失败");
      return LeanCodeGenResult.CreateError(ex.Message);
    }
  }

  /// <inheritdoc/>
  public async Task<bool> ValidateTemplateAsync(string templatePath)
  {
    try
    {
      return await _templateEngine.CompileAsync(templatePath);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "验证模板失败");
      return false;
    }
  }

  /// <inheritdoc/>
  public Task<IEnumerable<string>> GetAvailableTemplatesAsync()
  {
    try
    {
      var templateDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates");
      var templates = Directory.GetFiles(templateDir, "*.cshtml", SearchOption.AllDirectories)
          .Select(x => x.Replace(templateDir, "").TrimStart('\\', '/'))
          .ToList();
      return Task.FromResult<IEnumerable<string>>(templates);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "获取可用模板失败");
      return Task.FromResult<IEnumerable<string>>(Array.Empty<string>());
    }
  }

  /// <summary>
  /// 验证选项
  /// </summary>
  private void ValidateOptions(LeanCodeGenOptions options)
  {
    var context = new ValidationContext(options);
    var results = new List<ValidationResult>();
    if (!Validator.TryValidateObject(options, context, results, true))
    {
      throw new ValidationException(string.Join(Environment.NewLine, results.Select(x => x.ErrorMessage)));
    }
  }

  /// <summary>
  /// 保存代码到文件
  /// </summary>
  private async Task SaveCodeToFileAsync(string filePath, string code, bool overwrite)
  {
    if (File.Exists(filePath) && !overwrite)
    {
      throw new Exception($"文件已存在: {filePath}");
    }

    var directory = Path.GetDirectoryName(filePath);
    if (!string.IsNullOrEmpty(directory))
    {
      Directory.CreateDirectory(directory);
    }

    await File.WriteAllTextAsync(filePath, code);
  }
}