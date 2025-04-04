using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Application.Services.Identity;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Attributes;
using Microsoft.Extensions.Configuration;
using Lean.CodeGen.Application.Services.Admin;

namespace Lean.CodeGen.WebApi.Controllers.Identity;

/// <summary>
/// 角色控制器
/// </summary>
/// <remarks>
/// 提供角色管理相关的API接口，包括：
/// 1. 角色的增删改查
/// 2. 角色状态管理
/// 3. 角色菜单管理
/// 4. 角色数据权限管理
/// </remarks>
[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "identity")]
[LeanPermission("identity:role", "角色管理")]
public class LeanRoleController : LeanBaseController
{
  private readonly ILeanRoleService _roleService;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="roleService">角色服务</param>
  /// <param name="localizationService">本地化服务</param>
  /// <param name="configuration">配置</param>
  /// <param name="userContext">用户上下文</param>
  public LeanRoleController(
      ILeanRoleService roleService,
      ILeanLocalizationService localizationService,
      IConfiguration configuration,
      ILeanUserContext userContext)
      : base(localizationService, configuration, userContext)
  {
    _roleService = roleService;
  }

  /// <summary>
  /// 分页查询角色
  /// </summary>
  /// <param name="input">查询参数</param>
  /// <returns>分页查询结果</returns>
  [LeanPermission("identity:role:list", "查询角色")]
  [HttpGet("list")]
  public async Task<IActionResult> GetPageAsync([FromQuery] LeanRoleQueryDto input)
  {
    var result = await _roleService.GetPageAsync(input);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 获取角色信息
  /// </summary>
  /// <param name="id">角色ID</param>
  /// <returns>角色详细信息</returns>
  [LeanPermission("identity:role:query", "查询角色")]
  [HttpGet("{id}")]
  public async Task<IActionResult> GetAsync(long id)
  {
    var result = await _roleService.GetAsync(id);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 创建角色
  /// </summary>
  /// <param name="input">角色创建参数</param>
  /// <returns>创建成功的角色信息</returns>
  [LeanPermission("identity:role:create", "新增角色")]
  [HttpPost]
  public async Task<IActionResult> CreateAsync([FromBody] LeanRoleCreateDto input)
  {
    var result = await _roleService.CreateAsync(input);
    return Success(result, LeanBusinessType.Create);
  }

  /// <summary>
  /// 更新角色
  /// </summary>
  /// <param name="input">角色更新参数</param>
  /// <returns>更新后的角色信息</returns>
  [LeanPermission("identity:role:update", "修改角色")]
  [HttpPut]
  public async Task<IActionResult> UpdateAsync([FromBody] LeanRoleUpdateDto input)
  {
    var result = await _roleService.UpdateAsync(input);
    return Success(result, LeanBusinessType.Update);
  }

  /// <summary>
  /// 删除角色
  /// </summary>
  /// <param name="id">角色ID</param>
  /// <returns>删除结果</returns>
  [LeanPermission("identity:role:delete", "删除角色")]
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteAsync(long id)
  {
    var result = await _roleService.DeleteAsync(id);
    return Success(result, LeanBusinessType.Delete);
  }

  /// <summary>
  /// 批量删除角色
  /// </summary>
  /// <param name="ids">角色ID列表</param>
  /// <returns>删除结果</returns>
  [LeanPermission("identity:role:delete", "删除角色")]
  [HttpDelete("batch")]
  public async Task<IActionResult> BatchDeleteAsync([FromBody] List<long> ids)
  {
    var result = await _roleService.BatchDeleteAsync(ids);
    return Success(result, LeanBusinessType.Delete);
  }

  /// <summary>
  /// 导出角色数据
  /// </summary>
  /// <param name="input">查询参数</param>
  /// <returns>角色数据导出结果</returns>
  [LeanPermission("identity:role:export", "导出角色")]
  [HttpGet("export")]
  public async Task<IActionResult> ExportAsync([FromQuery] LeanRoleQueryDto input)
  {
    var bytes = await _roleService.ExportAsync(input);
    return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "roles.xlsx");
  }

  /// <summary>
  /// 获取导入模板
  /// </summary>
  /// <returns>角色数据导入模板</returns>
  [LeanPermission("identity:role:import", "导入角色")]
  [HttpGet("template")]
  public async Task<IActionResult> GetTemplateAsync()
  {
    var bytes = await _roleService.GetTemplateAsync();
    return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "role-template.xlsx");
  }

  /// <summary>
  /// 导入角色数据
  /// </summary>
  /// <param name="file">Excel文件</param>
  /// <returns>角色数据导入结果</returns>
  [LeanPermission("identity:role:import", "导入角色")]
  [HttpPost("import")]
  public async Task<IActionResult> ImportAsync([FromForm] LeanFileInfo file)
  {
    var result = await _roleService.ImportAsync(file);
    return Success(result, LeanBusinessType.Import);
  }

  /// <summary>
  /// 获取角色的菜单权限
  /// </summary>
  /// <param name="roleId">角色ID</param>
  /// <returns>角色菜单ID列表</returns>
  [HttpGet("{roleId}/menus")]
  public async Task<IActionResult> GetRoleMenusAsync(long roleId)
  {
    var result = await _roleService.GetRoleMenusAsync(roleId);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 设置角色的菜单权限
  /// </summary>
  /// <param name="input">菜单分配参数</param>
  /// <returns>设置后的角色信息</returns>
  [LeanPermission("identity:role:update", "修改角色")]
  [HttpPut("menus")]
  public async Task<IActionResult> SetRoleMenusAsync([FromBody] LeanRoleSetMenusDto input)
  {
    var result = await _roleService.SetRoleMenusAsync(input);
    return Success(result, LeanBusinessType.Update);
  }
}