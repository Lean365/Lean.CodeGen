namespace Lean.CodeGen.Common.Exceptions;

/// <summary>
/// 业务异常
/// </summary>
public class LeanException : Exception
{
  /// <summary>
  /// 错误代码
  /// </summary>
  public int Code { get; }

  public LeanException(string message, int code = 500) : base(message)
  {
    Code = code;
  }
}

/// <summary>
/// 未授权异常
/// </summary>
public class LeanUnauthorizedException : LeanException
{
  public LeanUnauthorizedException(string message = "未授权") : base(message, 401)
  {
  }
}

/// <summary>
/// 禁止访问异常
/// </summary>
public class LeanForbiddenException : LeanException
{
  public LeanForbiddenException(string message = "禁止访问") : base(message, 403)
  {
  }
}

/// <summary>
/// 数据验证异常
/// </summary>
public class LeanValidationException : LeanException
{
  public LeanValidationException(string message) : base(message, 400)
  {
  }
}