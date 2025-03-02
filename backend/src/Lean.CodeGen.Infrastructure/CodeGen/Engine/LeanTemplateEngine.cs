using Microsoft.Extensions.Logging;
using Scriban;
using Scriban.Runtime;
using Scriban.Parsing;

namespace Lean.CodeGen.Infrastructure.CodeGen.Engine;

/// <summary>
/// 模板引擎实现
/// </summary>
public class LeanTemplateEngine : ILeanTemplateEngine
{
  private readonly ILogger<LeanTemplateEngine> _logger;
  private readonly Dictionary<string, Template> _templateCache;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanTemplateEngine(ILogger<LeanTemplateEngine> logger)
  {
    _logger = logger;
    _templateCache = new Dictionary<string, Template>();
  }

  /// <inheritdoc/>
  public async Task<bool> CompileAsync(string templatePath)
  {
    try
    {
      var template = await File.ReadAllTextAsync(templatePath);
      var compiled = Template.Parse(template);
      if (compiled.HasErrors)
      {
        _logger.LogError("模板语法错误: {Errors}", string.Join(Environment.NewLine, compiled.Messages));
        return false;
      }
      _templateCache[templatePath] = compiled;
      return true;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "编译模板失败: {TemplatePath}", templatePath);
      return false;
    }
  }

  /// <inheritdoc/>
  public async Task<string> RenderAsync(string templatePath, object model)
  {
    try
    {
      Template template;
      if (!_templateCache.TryGetValue(templatePath, out template))
      {
        var content = await File.ReadAllTextAsync(templatePath);
        template = Template.Parse(content);
        if (template.HasErrors)
        {
          throw new Exception($"模板语法错误: {string.Join(Environment.NewLine, template.Messages)}");
        }
        _templateCache[templatePath] = template;
      }

      var scriptObject = new ScriptObject();
      scriptObject.Import(model);
      var context = new TemplateContext { TemplateLoader = new LeanTemplateLoader() };
      context.PushGlobal(scriptObject);

      return await template.RenderAsync(context);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "渲染模板失败: {TemplatePath}", templatePath);
      throw;
    }
  }

  /// <inheritdoc/>
  public async Task<string> RenderStringAsync(string templateContent, object model)
  {
    try
    {
      var template = Template.Parse(templateContent);
      if (template.HasErrors)
      {
        throw new Exception($"模板语法错误: {string.Join(Environment.NewLine, template.Messages)}");
      }

      var scriptObject = new ScriptObject();
      scriptObject.Import(model);
      var context = new TemplateContext();
      context.PushGlobal(scriptObject);

      return await template.RenderAsync(context);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "渲染字符串模板失败");
      throw;
    }
  }
}

/// <summary>
/// 模板加载器
/// </summary>
public class LeanTemplateLoader : ITemplateLoader
{
  public string GetPath(TemplateContext context, SourceSpan callerSpan, string templateName) => templateName;

  public string Load(TemplateContext context, SourceSpan callerSpan, string templatePath)
  {
    return File.ReadAllText(templatePath);
  }

  public ValueTask<string> LoadAsync(TemplateContext context, SourceSpan callerSpan, string templatePath)
  {
    return new ValueTask<string>(File.ReadAllTextAsync(templatePath));
  }
}