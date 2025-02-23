using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Application.Services.Identity;
using Lean.CodeGen.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.WebApi.Controllers;

namespace Lean.CodeGen.WebApi.Controllers.Identity;

/// <summary>
/// 菜单管理
/// </summary>
[Route("api/identity/[controller]")]
[Tags("身份认证")]
[ApiController]
public class LeanMenuController : LeanBaseController
{
    private readonly ILeanMenuService _menuService;

    public LeanMenuController(ILeanMenuService menuService)
    {
        _menuService = menuService;
    }

    /// <summary>
    /// 创建菜单
    /// </summary>
    [HttpPost]
    public Task<LeanApiResult<long>> CreateAsync([FromBody] LeanCreateMenuDto input)
    {
        return _menuService.CreateAsync(input);
    }

    /// <summary>
    /// 更新菜单
    /// </summary>
    [HttpPut]
    public Task<LeanApiResult> UpdateAsync([FromBody] LeanUpdateMenuDto input)
    {
        return _menuService.UpdateAsync(input);
    }

    /// <summary>
    /// 删除菜单
    /// </summary>
    [HttpDelete("{id}")]
    public Task<LeanApiResult> DeleteAsync([FromRoute] long id)
    {
        return _menuService.DeleteAsync(id);
    }

    /// <summary>
    /// 批量删除菜单
    /// </summary>
    [HttpDelete("batch")]
    public Task<LeanApiResult> BatchDeleteAsync([FromBody] List<long> ids)
    {
        return _menuService.BatchDeleteAsync(ids);
    }

    /// <summary>
    /// 获取菜单信息
    /// </summary>
    [HttpGet("{id}")]
    public Task<LeanApiResult<LeanMenuDto>> GetAsync([FromRoute] long id)
    {
        return _menuService.GetAsync(id);
    }

    /// <summary>
    /// 分页查询菜单
    /// </summary>
    [HttpGet("page")]
    public Task<LeanApiResult<LeanPageResult<LeanMenuDto>>> GetPageAsync([FromQuery] LeanQueryMenuDto input)
    {
        return _menuService.GetPageAsync(input);
    }

    /// <summary>
    /// 修改菜单状态
    /// </summary>
    [HttpPut("status")]
    public Task<LeanApiResult> SetStatusAsync([FromBody] LeanChangeMenuStatusDto input)
    {
        return _menuService.SetStatusAsync(input);
    }

    /// <summary>
    /// 获取菜单树形结构
    /// </summary>
    [HttpGet("tree")]
    public Task<LeanApiResult<List<LeanMenuTreeDto>>> GetTreeAsync()
    {
        return _menuService.GetTreeAsync();
    }

    /// <summary>
    /// 获取用户菜单树
    /// </summary>
    [HttpGet("user/{userId}/tree")]
    public Task<LeanApiResult<List<LeanMenuTreeDto>>> GetUserMenuTreeAsync([FromRoute] long userId)
    {
        return _menuService.GetUserMenuTreeAsync(userId);
    }

    /// <summary>
    /// 获取用户权限列表
    /// </summary>
    [HttpGet("user/{userId}/permissions")]
    public Task<LeanApiResult<List<string>>> GetUserPermissionsAsync([FromRoute] long userId)
    {
        return _menuService.GetUserPermissionsAsync(userId);
    }

    /// <summary>
    /// 获取角色菜单树
    /// </summary>
    [HttpGet("role/{roleId}/tree")]
    public Task<LeanApiResult<List<LeanMenuTreeDto>>> GetRoleMenuTreeAsync([FromRoute] long roleId)
    {
        return _menuService.GetRoleMenuTreeAsync(roleId);
    }

    /// <summary>
    /// 获取菜单的操作权限列表
    /// </summary>
    [HttpGet("{menuId}/operations")]
    public Task<LeanApiResult<List<LeanMenuOperationDto>>> GetMenuOperationsAsync([FromRoute] long menuId)
    {
        return _menuService.GetMenuOperationsAsync(menuId);
    }

    /// <summary>
    /// 设置菜单的操作权限
    /// </summary>
    [HttpPut("operations")]
    public Task<LeanApiResult> SetMenuOperationsAsync([FromBody] LeanSetMenuOperationDto input)
    {
        return _menuService.SetMenuOperationsAsync(input);
    }

    /// <summary>
    /// 获取用户在指定菜单的操作权限
    /// </summary>
    [HttpGet("user/{userId}/menu/{menuId}/operations")]
    public Task<LeanApiResult<List<string>>> GetUserMenuOperationsAsync([FromRoute] long userId, [FromRoute] long menuId)
    {
        return _menuService.GetUserMenuOperationsAsync(userId, menuId);
    }

    /// <summary>
    /// 验证用户是否有指定菜单的操作权限
    /// </summary>
    [HttpGet("user/{userId}/menu/{menuId}/validate-operation")]
    public Task<LeanApiResult<bool>> ValidateUserMenuOperationAsync([FromRoute] long userId, [FromRoute] long menuId, [FromQuery] string operation)
    {
        return _menuService.ValidateUserMenuOperationAsync(userId, menuId, operation);
    }
}