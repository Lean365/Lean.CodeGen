using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Application.Services.Identity;
using Lean.CodeGen.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.WebApi.Controllers;

namespace Lean.CodeGen.WebApi.Controllers.Identity;

/// <summary>
/// 角色管理
/// </summary>
[Route("api/identity/[controller]")]
[Tags("身份认证")]
[ApiController]
public class LeanRoleController : LeanBaseController
{
    private readonly ILeanRoleService _roleService;

    public LeanRoleController(ILeanRoleService roleService)
    {
        _roleService = roleService;
    }

    /// <summary>
    /// 创建角色
    /// </summary>
    [HttpPost]
    public Task<LeanApiResult<long>> CreateAsync([FromBody] LeanCreateRoleDto input)
    {
        return _roleService.CreateAsync(input);
    }

    /// <summary>
    /// 更新角色
    /// </summary>
    [HttpPut]
    public Task<LeanApiResult> UpdateAsync([FromBody] LeanUpdateRoleDto input)
    {
        return _roleService.UpdateAsync(input);
    }

    /// <summary>
    /// 删除角色
    /// </summary>
    [HttpDelete("{id}")]
    public Task<LeanApiResult> DeleteAsync([FromRoute] long id)
    {
        return _roleService.DeleteAsync(id);
    }

    /// <summary>
    /// 批量删除角色
    /// </summary>
    [HttpDelete("batch")]
    public Task<LeanApiResult> BatchDeleteAsync([FromBody] List<long> ids)
    {
        return _roleService.BatchDeleteAsync(ids);
    }

    /// <summary>
    /// 获取角色信息
    /// </summary>
    [HttpGet("{id}")]
    public Task<LeanApiResult<LeanRoleDto>> GetAsync([FromRoute] long id)
    {
        return _roleService.GetAsync(id);
    }

    /// <summary>
    /// 分页查询角色
    /// </summary>
    [HttpGet("page")]
    public Task<LeanApiResult<LeanPageResult<LeanRoleDto>>> GetPageAsync([FromQuery] LeanQueryRoleDto input)
    {
        return _roleService.GetPageAsync(input);
    }

    /// <summary>
    /// 修改角色状态
    /// </summary>
    [HttpPut("status")]
    public Task<LeanApiResult> SetStatusAsync([FromBody] LeanChangeRoleStatusDto input)
    {
        return _roleService.SetStatusAsync(input);
    }

    /// <summary>
    /// 获取角色菜单列表
    /// </summary>
    [HttpGet("{roleId}/menus")]
    public Task<LeanApiResult<List<long>>> GetRoleMenusAsync([FromRoute] long roleId)
    {
        return _roleService.GetRoleMenusAsync(roleId);
    }

    /// <summary>
    /// 获取角色API列表
    /// </summary>
    [HttpGet("{roleId}/apis")]
    public Task<LeanApiResult<List<long>>> GetRoleApisAsync([FromRoute] long roleId)
    {
        return _roleService.GetRoleApisAsync(roleId);
    }

    /// <summary>
    /// 分配角色菜单
    /// </summary>
    [HttpPut("assign-menus")]
    public Task<LeanApiResult> AssignMenusAsync([FromBody] LeanAssignRoleMenuDto input)
    {
        return _roleService.AssignMenusAsync(input);
    }

    /// <summary>
    /// 分配角色API
    /// </summary>
    [HttpPut("assign-apis")]
    public Task<LeanApiResult> AssignApisAsync([FromBody] LeanAssignRoleApiDto input)
    {
        return _roleService.AssignApisAsync(input);
    }

    /// <summary>
    /// 获取角色数据范围
    /// </summary>
    [HttpGet("{roleId}/data-scope")]
    public Task<LeanApiResult<List<long>>> GetRoleDataScopeAsync([FromRoute] long roleId)
    {
        return _roleService.GetRoleDataScopeAsync(roleId);
    }

    /// <summary>
    /// 分配角色数据范围
    /// </summary>
    [HttpPut("assign-data-scope")]
    public Task<LeanApiResult> AssignDataScopeAsync([FromBody] LeanAssignRoleDataScopeDto input)
    {
        return _roleService.AssignDataScopeAsync(input);
    }

    /// <summary>
    /// 获取角色互斥列表
    /// </summary>
    [HttpGet("{roleId}/mutexes")]
    public Task<LeanApiResult<List<long>>> GetRoleMutexesAsync([FromRoute] long roleId)
    {
        return _roleService.GetRoleMutexesAsync(roleId);
    }

    /// <summary>
    /// 设置角色互斥关系
    /// </summary>
    [HttpPut("mutexes")]
    public Task<LeanApiResult> SetRoleMutexesAsync([FromBody] LeanSetRoleMutexDto input)
    {
        return _roleService.SetRoleMutexesAsync(input);
    }

    /// <summary>
    /// 获取角色前置条件列表
    /// </summary>
    [HttpGet("{roleId}/prerequisites")]
    public Task<LeanApiResult<List<long>>> GetRolePrerequisitesAsync([FromRoute] long roleId)
    {
        return _roleService.GetRolePrerequisitesAsync(roleId);
    }

    /// <summary>
    /// 设置角色前置条件
    /// </summary>
    [HttpPut("prerequisites")]
    public Task<LeanApiResult> SetRolePrerequisitesAsync([FromBody] LeanSetRolePrerequisiteDto input)
    {
        return _roleService.SetRolePrerequisitesAsync(input);
    }

    /// <summary>
    /// 验证用户是否可以分配指定角色
    /// </summary>
    [HttpGet("user/{userId}/validate-assignment/{roleId}")]
    public Task<LeanApiResult<bool>> ValidateUserRoleAssignmentAsync([FromRoute] long userId, [FromRoute] long roleId)
    {
        return _roleService.ValidateUserRoleAssignmentAsync(userId, roleId);
    }
}