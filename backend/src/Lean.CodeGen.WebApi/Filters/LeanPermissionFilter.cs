using Lean.CodeGen.Common.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using Lean.CodeGen.Application.Services.Identity;
using Lean.CodeGen.Domain.Interfaces.Repositories;
using Lean.CodeGen.Domain.Entities.Identity;

namespace Lean.CodeGen.WebApi.Filters;

/// <summary>
/// 权限过滤器
/// </summary>
/// <remarks>
/// 实现基于菜单的权限验证逻辑：
/// 1. 验证用户是否登录
/// 2. 获取用户角色
/// 3. 获取角色菜单
/// 4. 获取菜单权限
/// 5. 验证用户是否具有访问接口所需的权限
/// 
/// 注意：除了标记 [AllowAnonymous] 特性的接口外，所有接口都必须进行权限验证
/// </remarks>
public class LeanPermissionFilter : IAsyncAuthorizationFilter
{
  private readonly ILogger<LeanPermissionFilter> _logger;
  private readonly ILeanMenuService _menuService;
  private readonly ILeanRepository<LeanUser> _userRepository;
  private readonly ILeanRepository<LeanUserRole> _userRoleRepository;
  private readonly ILeanRepository<LeanRoleMenu> _roleMenuRepository;

  public LeanPermissionFilter(
    ILogger<LeanPermissionFilter> logger,
    ILeanMenuService menuService,
    ILeanRepository<LeanUser> userRepository,
    ILeanRepository<LeanUserRole> userRoleRepository,
    ILeanRepository<LeanRoleMenu> roleMenuRepository)
  {
    _logger = logger;
    _menuService = menuService;
    _userRepository = userRepository;
    _userRoleRepository = userRoleRepository;
    _roleMenuRepository = roleMenuRepository;
  }

  /// <summary>
  /// 权限验证处理
  /// </summary>
  /// <param name="context">授权过滤器上下文</param>
  public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
  {
    // 1. 检查是否是控制器动作
    if (context.ActionDescriptor is not ControllerActionDescriptor actionDescriptor)
    {
      _logger.LogWarning("非控制器动作，跳过权限验证");
      return;
    }

    // 2. 检查是否允许匿名访问
    var allowanonymous = actionDescriptor.MethodInfo.GetCustomAttributes(typeof(AllowAnonymousAttribute), false).Any();
    if (allowanonymous)
    {
      _logger.LogInformation("接口 {Controller}.{Action} 允许匿名访问",
        actionDescriptor.ControllerName, actionDescriptor.ActionName);
      return;
    }

    // 3. 验证用户是否登录
    var user = context.HttpContext.User;
    if (user?.Identity?.IsAuthenticated != true)
    {
      _logger.LogWarning("用户未登录，访问被拒绝");
      context.Result = new UnauthorizedResult();
      return;
    }

    // 4. 获取用户名
    var userName = user.FindFirst(ClaimTypes.Name)?.Value;
    if (string.IsNullOrEmpty(userName))
    {
      _logger.LogWarning("用户令牌中未找到用户名");
      context.Result = new UnauthorizedResult();
      return;
    }

    try
    {
      _logger.LogInformation("开始验证用户 {UserName} 的权限，访问接口 {Action}",
        userName, context.ActionDescriptor.DisplayName);

      // 5. 查询用户信息
      var userEntity = await _userRepository.FirstOrDefaultAsync(u => u.UserName == userName);
      if (userEntity == null)
      {
        _logger.LogWarning("用户 {UserName} 不存在", userName);
        context.Result = new UnauthorizedResult();
        return;
      }

      // 6. 查询用户角色
      var userRoles = await _userRoleRepository.GetListAsync(ur => ur.UserId == userEntity.Id);
      if (!userRoles.Any())
      {
        _logger.LogWarning("用户 {UserName} 没有分配任何角色", userName);
        context.Result = new ForbidResult();
        return;
      }

      _logger.LogInformation("用户 {UserName} 的角色列表: {Roles}",
        userName, string.Join(", ", userRoles.Select(r => r.RoleId)));

      // 7. 查询角色菜单
      var roleIds = userRoles.Select(ur => ur.RoleId).ToList();
      var roleMenus = await _roleMenuRepository.GetListAsync(rm => roleIds.Contains(rm.RoleId));

      // 8. 获取菜单权限
      var menuIds = roleMenus.Select(rm => rm.MenuId).Distinct().ToList();
      var menuPermissions = await _menuService.GetMenuPermissionsAsync(menuIds);

      _logger.LogInformation("用户 {UserName} 的菜单权限列表: {Permissions}",
        userName, string.Join(", ", menuPermissions));

      // 9. 获取接口需要的权限特性
      var permissionAttributes = GetPermissionAttributes(context);
      if (!permissionAttributes.Any())
      {
        _logger.LogWarning("接口 {Action} 未配置权限特性，访问被拒绝",
          context.ActionDescriptor.DisplayName);
        context.Result = new ForbidResult();
        return;
      }

      // 10. 验证权限
      var requiredPermissions = permissionAttributes.Select(attr => attr.Code).ToList();
      _logger.LogInformation("接口 {Action} 需要的权限: {Permissions}",
        context.ActionDescriptor.DisplayName, string.Join(", ", requiredPermissions));

      var hasPermission = permissionAttributes.Any(attr =>
          menuPermissions.Contains(attr.Code, StringComparer.OrdinalIgnoreCase));

      if (!hasPermission)
      {
        _logger.LogWarning("用户 {UserName} 尝试访问未授权的接口 {Action}，所需权限: {Permissions}",
          userName, context.ActionDescriptor.DisplayName, string.Join(", ", requiredPermissions));
        context.Result = new ForbidResult();
      }
      else
      {
        _logger.LogInformation("用户 {UserName} 权限验证通过", userName);
      }
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "验证用户 {UserName} 权限时发生错误", userName);
      context.Result = new StatusCodeResult(500);
    }
  }

  /// <summary>
  /// 获取权限特性
  /// </summary>
  /// <param name="context">授权过滤器上下文</param>
  /// <returns>权限特性列表</returns>
  /// <remarks>
  /// 获取控制器和方法上的所有权限特性
  /// </remarks>
  private static IEnumerable<LeanPermissionAttribute> GetPermissionAttributes(AuthorizationFilterContext context)
  {
    if (context.ActionDescriptor is not ControllerActionDescriptor actionDescriptor)
      return Enumerable.Empty<LeanPermissionAttribute>();

    // 获取控制器上的权限特性
    var controllerAttributes = actionDescriptor.ControllerTypeInfo
        .GetCustomAttributes(typeof(LeanPermissionAttribute), true)
        .Cast<LeanPermissionAttribute>();

    // 获取方法上的权限特性
    var methodAttributes = actionDescriptor.MethodInfo
        .GetCustomAttributes(typeof(LeanPermissionAttribute), true)
        .Cast<LeanPermissionAttribute>();

    // 合并控制器和方法的权限特性
    return controllerAttributes.Concat(methodAttributes);
  }
}
