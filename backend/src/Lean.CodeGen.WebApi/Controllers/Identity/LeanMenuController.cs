using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Application.Services.Identity;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Enums;
using Microsoft.Extensions.Configuration;
using Lean.CodeGen.Application.Services.Admin;

namespace Lean.CodeGen.WebApi.Controllers.Identity;

/// <summary>
/// 菜单控制器
/// </summary>
/// <remarks>
/// 提供菜单管理相关的API接口，包括：
/// 1. 菜单的增删改查
/// 2. 菜单状态管理
/// 3. 菜单树形结构管理
/// 4. 菜单权限管理
/// </remarks>
[Route("api/admin/menus")]
[ApiController]
public class LeanMenuController : LeanBaseController
{
  private readonly ILeanMenuService _menuService;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="menuService">菜单服务</param>
  /// <param name="localizationService">本地化服务</param>
  /// <param name="configuration">配置</param>
  public LeanMenuController(
      ILeanMenuService menuService,
      ILeanLocalizationService localizationService,
      IConfiguration configuration)
      : base(localizationService, configuration)
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
  /// <param name="id">菜单ID</param>
  /// <returns>删除结果</returns>
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteAsync(long id)
  {
    var result = await _menuService.DeleteAsync(id);
    return Success(result, LeanBusinessType.Delete);
  }

  /// <summary>
  /// 批量删除菜单
  /// </summary>
  /// <param name="ids">菜单ID列表</param>
  /// <returns>删除结果</returns>
  [HttpDelete]
  public async Task<IActionResult> BatchDeleteAsync([FromBody] List<long> ids)
  {
    var result = await _menuService.BatchDeleteAsync(ids);
    return Success(result, LeanBusinessType.Delete);
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
  public async Task<IActionResult> GetPageAsync([FromQuery] LeanQueryMenuDto input)
  {
    var result = await _menuService.GetPageAsync(input);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 设置菜单状态
  /// </summary>
  /// <param name="input">状态修改参数</param>
  /// <returns>修改后的菜单状态</returns>
  [HttpPut("status")]
  public async Task<IActionResult> SetStatusAsync([FromBody] LeanChangeMenuStatusDto input)
  {
    var result = await _menuService.SetStatusAsync(input);
    return Success(result, LeanBusinessType.Update);
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
  /// 导出菜单数据
  /// </summary>
  /// <param name="input">查询参数</param>
  /// <returns>菜单数据导出结果</returns>
  [HttpGet("export")]
  public async Task<IActionResult> ExportAsync([FromQuery] LeanQueryMenuDto input)
  {
    var bytes = await _menuService.ExportAsync(input);
    return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "menus.xlsx");
  }

  /// <summary>
  /// 导入菜单数据
  /// </summary>
  /// <param name="file">菜单数据文件</param>
  /// <returns>导入结果</returns>
  [HttpPost("import")]
  public async Task<IActionResult> ImportAsync([FromForm] IFormFile file)
  {
    using var ms = new MemoryStream();
    await file.CopyToAsync(ms);
    ms.Position = 0;
    var fileInfo = new LeanFileInfo
    {
      Stream = ms,
      FileName = file.FileName,
      ContentType = file.ContentType
    };
    var result = await _menuService.ImportAsync(fileInfo);
    return Success(result, LeanBusinessType.Import);
  }

  /// <summary>
  /// 获取导入模板
  /// </summary>
  /// <returns>菜单导入模板文件</returns>
  [HttpGet("template")]
  public async Task<IActionResult> GetImportTemplateAsync()
  {
    var bytes = await _menuService.GetImportTemplateAsync();
    return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "menu-template.xlsx");
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