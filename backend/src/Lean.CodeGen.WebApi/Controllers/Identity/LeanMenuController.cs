using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Application.Services.Identity;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Enums;

namespace Lean.CodeGen.WebApi.Controllers.Identity;

/// <summary>
/// 菜单管理控制器
/// </summary>
/// <remarks>
/// 提供菜单管理相关的API接口，包括：
/// 1. 菜单的增删改查
/// 2. 菜单状态管理
/// 3. 菜单树形结构管理
/// 4. 菜单权限管理
/// </remarks>
[Route("api/[controller]")]
public class LeanMenuController : LeanBaseController
{
  private readonly ILeanMenuService _menuService;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="menuService">菜单服务</param>
  public LeanMenuController(ILeanMenuService menuService)
  {
    _menuService = menuService;
  }

  /// <summary>
  /// 创建菜单
  /// </summary>
  /// <param name="input">菜单创建参数</param>
  /// <returns>创建成功的菜单信息</returns>
  [HttpPost]
  public async Task<IActionResult> CreateAsync([FromBody] LeanCreateMenuDto input)
  {
    var result = await _menuService.CreateAsync(input);
    return Success(result, LeanBusinessType.Create);
  }

  /// <summary>
  /// 更新菜单
  /// </summary>
  /// <param name="input">菜单更新参数</param>
  /// <returns>更新后的菜单信息</returns>
  [HttpPut]
  public async Task<IActionResult> UpdateAsync([FromBody] LeanUpdateMenuDto input)
  {
    var result = await _menuService.UpdateAsync(input);
    return Success(result, LeanBusinessType.Update);
  }

  /// <summary>
  /// 删除菜单
  /// </summary>
  /// <param name="input">菜单删除参数</param>
  [HttpDelete]
  public async Task<IActionResult> DeleteAsync([FromBody] LeanDeleteMenuDto input)
  {
    await _menuService.DeleteAsync(input);
    return Success(LeanBusinessType.Delete);
  }

  /// <summary>
  /// 获取菜单信息
  /// </summary>
  /// <param name="id">菜单ID</param>
  /// <returns>菜单详细信息</returns>
  [HttpGet("{id}")]
  public async Task<IActionResult> GetAsync(long id)
  {
    var result = await _menuService.GetAsync(id);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 查询菜单列表
  /// </summary>
  /// <param name="input">查询参数</param>
  /// <returns>菜单列表</returns>
  [HttpGet]
  public async Task<IActionResult> QueryAsync([FromQuery] LeanQueryMenuDto input)
  {
    var result = await _menuService.QueryAsync(input);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 获取菜单树形结构
  /// </summary>
  /// <param name="input">查询参数</param>
  /// <returns>菜单树形结构</returns>
  [HttpGet("tree")]
  public async Task<IActionResult> GetTreeAsync([FromQuery] LeanQueryMenuDto input)
  {
    var result = await _menuService.GetTreeAsync(input);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 修改菜单状态
  /// </summary>
  /// <param name="input">状态修改参数</param>
  [HttpPut("status")]
  public async Task<IActionResult> ChangeStatusAsync([FromBody] LeanChangeMenuStatusDto input)
  {
    await _menuService.ChangeStatusAsync(input);
    return Success(LeanBusinessType.Update);
  }

  /// <summary>
  /// 获取角色菜单树
  /// </summary>
  /// <param name="roleId">角色ID</param>
  /// <returns>角色菜单树</returns>
  [HttpGet("role/{roleId}")]
  public async Task<IActionResult> GetRoleMenuTreeAsync(long roleId)
  {
    var result = await _menuService.GetRoleMenuTreeAsync(roleId);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 获取用户菜单树
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <returns>用户菜单树</returns>
  [HttpGet("user/{userId}")]
  public async Task<IActionResult> GetUserMenuTreeAsync(long userId)
  {
    var result = await _menuService.GetUserMenuTreeAsync(userId);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 获取用户权限列表
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <returns>用户权限列表</returns>
  [HttpGet("user/{userId}/permissions")]
  public async Task<IActionResult> GetUserPermissionsAsync(long userId)
  {
    var result = await _menuService.GetUserPermissionsAsync(userId);
    return Success(result, LeanBusinessType.Query);
  }
}