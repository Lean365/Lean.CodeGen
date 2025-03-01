using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Application.Services.Identity;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Enums;

namespace Lean.CodeGen.WebApi.Controllers.Identity;

/// <summary>
/// 角色管理控制器
/// </summary>
/// <remarks>
/// 提供角色管理相关的API接口，包括：
/// 1. 角色的增删改查
/// 2. 角色状态管理
/// 3. 角色菜单管理
/// 4. 角色数据权限管理
/// </remarks>
[Route("api/[controller]")]
public class LeanRoleController : LeanBaseController
{
  private readonly ILeanRoleService _roleService;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="roleService">角色服务</param>
  public LeanRoleController(ILeanRoleService roleService)
  {
    _roleService = roleService;
  }

  /// <summary>
  /// 创建角色
  /// </summary>
  /// <param name="input">角色创建参数</param>
  /// <returns>创建成功的角色信息</returns>
  [HttpPost]
  public async Task<IActionResult> CreateAsync([FromBody] LeanCreateRoleDto input)
  {
    var result = await _roleService.CreateAsync(input);
    return Success(result, LeanBusinessType.Create);
  }

  /// <summary>
  /// 更新角色
  /// </summary>
  /// <param name="input">角色更新参数</param>
  /// <returns>更新后的角色信息</returns>
  [HttpPut]
  public async Task<IActionResult> UpdateAsync([FromBody] LeanUpdateRoleDto input)
  {
    var result = await _roleService.UpdateAsync(input);
    return Success(result, LeanBusinessType.Update);
  }

  /// <summary>
  /// 删除角色
  /// </summary>
  /// <param name="input">角色删除参数</param>
  [HttpDelete]
  public async Task<IActionResult> DeleteAsync([FromBody] LeanDeleteRoleDto input)
  {
    await _roleService.DeleteAsync(input);
    return Success(LeanBusinessType.Delete);
  }

  /// <summary>
  /// 获取角色信息
  /// </summary>
  /// <param name="id">角色ID</param>
  /// <returns>角色详细信息</returns>
  [HttpGet("{id}")]
  public async Task<IActionResult> GetAsync(long id)
  {
    var result = await _roleService.GetAsync(id);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 分页查询角色
  /// </summary>
  /// <param name="input">查询参数</param>
  /// <returns>分页查询结果</returns>
  [HttpGet]
  public async Task<IActionResult> QueryAsync([FromQuery] LeanQueryRoleDto input)
  {
    var result = await _roleService.QueryAsync(input);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 修改角色状态
  /// </summary>
  /// <param name="input">状态修改参数</param>
  [HttpPut("status")]
  public async Task<IActionResult> ChangeStatusAsync([FromBody] LeanChangeRoleStatusDto input)
  {
    await _roleService.ChangeStatusAsync(input);
    return Success(LeanBusinessType.Update);
  }

  /// <summary>
  /// 分配角色菜单
  /// </summary>
  /// <param name="input">菜单分配参数</param>
  [HttpPut("menus")]
  public async Task<IActionResult> AssignMenusAsync([FromBody] LeanAssignRoleMenuDto input)
  {
    await _roleService.AssignMenusAsync(input);
    return Success(LeanBusinessType.Update);
  }

  /// <summary>
  /// 分配角色数据权限
  /// </summary>
  /// <param name="input">数据权限分配参数</param>
  [HttpPut("data-scope")]
  public async Task<IActionResult> AssignDataScopeAsync([FromBody] LeanAssignRoleDataScopeDto input)
  {
    await _roleService.AssignDataScopeAsync(input);
    return Success(LeanBusinessType.Update);
  }

  /// <summary>
  /// 获取角色菜单
  /// </summary>
  /// <param name="id">角色ID</param>
  /// <returns>角色菜单ID列表</returns>
  [HttpGet("{id}/menus")]
  public async Task<IActionResult> GetMenusAsync(long id)
  {
    var result = await _roleService.GetMenusAsync(id);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 获取角色数据权限
  /// </summary>
  /// <param name="id">角色ID</param>
  /// <returns>角色数据权限信息</returns>
  [HttpGet("{id}/data-scope")]
  public async Task<IActionResult> GetDataScopeAsync(long id)
  {
    var result = await _roleService.GetDataScopeAsync(id);
    return Success(result, LeanBusinessType.Query);
  }
}