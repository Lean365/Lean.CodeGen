using System;
using Lean.CodeGen.Common.Enums;

namespace Lean.CodeGen.Common.Exceptions;

/// <summary>
/// 基础异常类
/// </summary>
public class LeanException : Exception
{
  public LeanErrorCode ErrorCode { get; }

  public LeanException(string message) : base(message)
  {
    ErrorCode = LeanErrorCode.UnknownError;
  }

  public LeanException(string message, LeanErrorCode errorCode) : base(message)
  {
    ErrorCode = errorCode;
  }

  public LeanException(string message, Exception innerException) : base(message, innerException)
  {
    ErrorCode = LeanErrorCode.UnknownError;
  }
}

/// <summary>
/// 未授权异常
/// </summary>
public class LeanUnauthorizedException : LeanException
{
  public LeanUnauthorizedException(string message = "未授权的访问")
      : base(message, LeanErrorCode.Unauthorized)
  {
  }
}

/// <summary>
/// 禁止访问异常
/// </summary>
public class LeanForbiddenException : LeanException
{
  public LeanForbiddenException(string message = "禁止访问")
      : base(message, LeanErrorCode.Forbidden)
  {
  }
}

/// <summary>
/// 验证异常
/// </summary>
public class LeanValidationException : LeanException
{
  public LeanValidationException(string message)
      : base(message, LeanErrorCode.ValidationError)
  {
  }
}

/// <summary>
/// 业务异常
/// </summary>
public class LeanBusinessException : LeanException
{
  public LeanBusinessException(string message)
      : base(message, LeanErrorCode.BusinessError)
  {
  }
}

/// <summary>
/// 数据不存在异常
/// </summary>
public class LeanNotFoundException : LeanException
{
  public LeanNotFoundException(string message)
      : base(message, LeanErrorCode.NotFound)
  {
  }
}

/// <summary>
/// 并发异常
/// </summary>
public class LeanConcurrencyException : LeanException
{
  public LeanConcurrencyException(string message)
      : base(message, LeanErrorCode.ConcurrencyError)
  {
  }
}

/// <summary>
/// 数据重复异常
/// </summary>
public class LeanDuplicateException : LeanException
{
  public LeanDuplicateException(string message)
      : base(message, LeanErrorCode.DuplicateError)
  {
  }
}