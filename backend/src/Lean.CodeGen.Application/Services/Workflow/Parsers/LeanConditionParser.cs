using System.Linq.Dynamic.Core;

namespace Lean.CodeGen.Application.Services.Workflow.Parsers;

/// <summary>
/// 条件表达式解析器
/// </summary>
public class LeanConditionParser
{
  /// <summary>
  /// 评估条件表达式
  /// </summary>
  public bool Evaluate(string condition, Dictionary<string, object> context)
  {
    if (string.IsNullOrEmpty(condition))
    {
      return true;
    }

    try
    {
      // 将条件表达式中的变量替换为实际值
      var expression = ReplaceVariables(condition, context);

      // 使用Dynamic Linq解析和执行表达式
      var result = DynamicExpressionParser.ParseLambda(typeof(object), typeof(bool), expression).Compile().DynamicInvoke();
      return (bool)result!;
    }
    catch (Exception ex)
    {
      throw new Exception($"Failed to evaluate condition: {condition}", ex);
    }
  }

  private string ReplaceVariables(string condition, Dictionary<string, object> context)
  {
    foreach (var variable in context)
    {
      var placeholder = $"${{{variable.Key}}}";
      var value = variable.Value switch
      {
        string s => $"\"{s}\"",
        DateTime dt => $"\"{dt:yyyy-MM-dd HH:mm:ss}\"",
        _ => variable.Value.ToString()
      };

      condition = condition.Replace(placeholder, value);
    }

    return condition;
  }
}