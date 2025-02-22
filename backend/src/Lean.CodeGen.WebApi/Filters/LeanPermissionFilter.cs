using Lean.CodeGen.Common.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Lean.CodeGen.WebApi.Filters;

/// <summary>
/// 权限过滤器
/// </summary>
public class LeanPermissionFilter : IAsyncAuthorizationFilter
{
  private readonly ILogger<LeanPermissionFilter> _logger;

  public LeanPermissionFilter(ILogger<LeanPermissionFilter> logger)
  {
    _logger = logger;
  }

  public Task OnAuthorizationAsync(AuthorizationFilterContext context)
  {
    // 跳过匿名访问
    if (context.ActionDescriptor is ControllerActionDescriptor actionDescriptor)
    {
      var allowanonymous = actionDescriptor.MethodInfo.GetCustomAttributes(typeof(AllowAnonymousAttribute), false).Any();
      if (allowanonymous) return Task.CompletedTask;
    }

    // 获取当前用户
    var user = context.HttpContext.User;
    if (user?.Identity?.IsAuthenticated != true)
    {
      context.Result = new UnauthorizedResult();
      return Task.CompletedTask;
    }

    // 获取权限特性
    var permissionAttributes = GetPermissionAttributes(context);
    if (!permissionAttributes.Any()) return Task.CompletedTask;

    // 获取用户权限
    var userPermissions = GetUserPermissions(user);

    // 验证权限
    var hasPermission = permissionAttributes.Any(attr =>
        userPermissions.Contains(attr.Code, StringComparer.OrdinalIgnoreCase));

    if (!hasPermission)
    {
      _logger.LogWarning("用户 {UserId} 尝试访问未授权的接口", user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
      context.Result = new ForbidResult();
    }

    return Task.CompletedTask;
  }

  private static IEnumerable<LeanPermissionAttribute> GetPermissionAttributes(AuthorizationFilterContext context)
  {
    if (context.ActionDescriptor is not ControllerActionDescriptor actionDescriptor)
      return Enumerable.Empty<LeanPermissionAttribute>();

    var controllerAttributes = actionDescriptor.ControllerTypeInfo
        .GetCustomAttributes(typeof(LeanPermissionAttribute), true)
        .Cast<LeanPermissionAttribute>();

    var methodAttributes = actionDescriptor.MethodInfo
        .GetCustomAttributes(typeof(LeanPermissionAttribute), true)
        .Cast<LeanPermissionAttribute>();

    return controllerAttributes.Concat(methodAttributes);
  }

  private static IEnumerable<string> GetUserPermissions(ClaimsPrincipal user)
  {
    var permissionClaim = user.FindFirst("Permissions");
    if (string.IsNullOrEmpty(permissionClaim?.Value)) return Enumerable.Empty<string>();

    return permissionClaim.Value.Split(',', StringSplitOptions.RemoveEmptyEntries);
  }
}
