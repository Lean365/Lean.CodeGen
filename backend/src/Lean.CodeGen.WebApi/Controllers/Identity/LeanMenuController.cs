using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Application.Services.Identity;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Attributes;
using Microsoft.Extensions.Configuration;
using Lean.CodeGen.Application.Services.Admin;
using Lean.CodeGen.WebApi.Controllers;
using System.Security.Claims;

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
[Authorize]
[LeanPermission("identity:menu", "菜单管理")]
[ApiController]
[Route("api/[controller]")]
public class LeanMenuController : LeanBaseController
{
  private readonly ILeanMenuService _menuService;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="menuService">菜单服务</param>
  /// <param name="localizationService">本地化服务</param>
  /// <param name="configuration">配置</param>
  /// <param name="userContext">用户上下文</param>
  public LeanMenuController(
      ILeanMenuService menuService,
      ILeanLocalizationService localizationService,
      IConfiguration configuration,
      ILeanUserContext userContext)
      : base(localizationService, configuration, userContext)
  {
    _menuService = menuService;
  }

  /// <summary>
  /// 分页查询菜单
  /// </summary>
  [LeanPermission("identity:menu:list", "查询菜单")]
  [HttpGet("list")]

  public async Task<LeanApiResult<LeanPageResult<LeanMenuDto>>> GetPageAsync([FromQuery] LeanMenuQueryDto input)
  {
    return await _menuService.GetPageAsync(input);
  }

  /// <summary>
  /// 获取菜单信息
  /// </summary>
  [LeanPermission("identity:menu:query", "查询菜单")]
  [HttpGet("{id}")]
  public async Task<LeanApiResult<LeanMenuDto>> GetAsync(long id)
  {
    return await _menuService.GetAsync(id);
  }

  /// <summary>
  /// 创建菜单
  /// </summary>
  [LeanPermission("identity:menu:create", "新增菜单")]
  [HttpPost]
  public async Task<LeanApiResult<long>> CreateAsync([FromBody] LeanMenuCreateDto input)
  {
    return await _menuService.CreateAsync(input);
  }

  /// <summary>
  /// 更新菜单
  /// </summary>
  [LeanPermission("identity:menu:update", "修改菜单")]
  [HttpPut]
  public async Task<LeanApiResult> UpdateAsync([FromBody] LeanMenuUpdateDto input)
  {
    return await _menuService.UpdateAsync(input);
  }

  /// <summary>
  /// 排序菜单
  /// </summary>
  [LeanPermission("identity:menu:sort", "修改菜单")]
  [HttpPut("sort")]
  public async Task<LeanApiResult> SortAsync([FromBody] List<LeanMenuSortDto> input)
  {
    return await _menuService.SortAsync(input);
  }

  /// <summary>
  /// 删除菜单
  /// </summary>
  [LeanPermission("identity:menu:delete", "删除菜单")]
  [HttpDelete("{id}")]
  public async Task<LeanApiResult> DeleteAsync(long id)
  {
    return await _menuService.DeleteAsync(id);
  }

  /// <summary>
  /// 批量删除菜单
  /// </summary>
  [LeanPermission("identity:menu:delete", "删除菜单")]
  [HttpDelete("batch")]
  public async Task<LeanApiResult> BatchDeleteAsync([FromBody] List<long> ids)
  {
    return await _menuService.BatchDeleteAsync(ids);
  }

  /// <summary>
  /// 导出菜单数据
  /// </summary>
  [LeanPermission("identity:menu:export", "导出菜单")]
  [HttpGet("export")]
  public async Task<IActionResult> ExportAsync([FromQuery] LeanMenuQueryDto input)
  {
    var data = await _menuService.ExportAsync(input);
    return File(data, "application/vnd.ms-excel", "菜单数据.xlsx");
  }

  /// <summary>
  /// 获取导入模板
  /// </summary>
  [LeanPermission("identity:menu:import", "导入菜单")]
  [HttpGet("template")]
  public async Task<IActionResult> GetTemplateAsync()
  {
    var data = await _menuService.GetTemplateAsync();
    return File(data, "application/vnd.ms-excel", "菜单导入模板.xlsx");
  }

  /// <summary>
  /// 导入菜单数据
  /// </summary>
  [LeanPermission("identity:menu:import", "导入菜单")]
  [HttpPost("import")]
  public async Task<LeanApiResult<LeanMenuImportResultDto>> ImportAsync([FromForm] LeanFileInfo file)
  {
    var result = await _menuService.ImportAsync(file);
    return LeanApiResult<LeanMenuImportResultDto>.Ok(result);
  }

  /// <summary>
  /// 设置菜单状态
  /// </summary>
  [LeanPermission("identity:menu:update", "修改菜单")]
  [HttpPut("status")]
  public async Task<LeanApiResult> SetStatusAsync([FromBody] LeanMenuChangeStatusDto input)
  {
    return await _menuService.SetStatusAsync(input);
  }

  /// <summary>
  /// 获取菜单树形结构
  /// </summary>
  [LeanPermission("identity:menu:query", "查询菜单")]
  [HttpGet("tree")]
  public async Task<List<LeanMenuTreeDto>> GetTreeAsync([FromQuery] LeanMenuQueryDto input)
  {
    return await _menuService.GetTreeAsync(input);
  }

  /// <summary>
  /// 获取角色菜单树
  /// </summary>
  [LeanPermission("identity:menu:query", "查询菜单")]
  [HttpGet("role/{roleId}/tree")]
  public async Task<List<LeanMenuTreeDto>> GetRoleMenuTreeAsync(long roleId)
  {
    return await _menuService.GetRoleMenuTreeAsync(roleId);
  }

  /// <summary>
  /// 获取当前登录用户的菜单树
  /// </summary>
  [LeanPermission("identity:menu:list", "查询菜单")]
  [HttpGet("user/tree")]
  public async Task<LeanApiResult<List<LeanMenuTreeDto>>> GetUserMenuTreeAsync()
  {
    var userId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
      ?? throw new UnauthorizedAccessException("未登录"));
    var menuTree = await _menuService.GetUserMenuTreeAsync(userId);
    return LeanApiResult<List<LeanMenuTreeDto>>.Ok(menuTree);
  }

  /// <summary>
  /// 获取当前登录用户的权限清单
  /// </summary>
  [LeanPermission("identity:menu:list", "查询菜单")]
  [HttpGet("user/permissions")]
  public async Task<LeanApiResult<List<string>>> GetUserPermissionsAsync()
  {
    var userId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
      ?? throw new UnauthorizedAccessException("未登录"));
    var permissions = await _menuService.GetUserPermissionsAsync(userId);
    return LeanApiResult<List<string>>.Ok(permissions, LeanBusinessType.Query);
  }
}