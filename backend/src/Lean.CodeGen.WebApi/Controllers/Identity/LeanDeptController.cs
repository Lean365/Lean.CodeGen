using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Application.Services.Identity;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Security;
using Lean.CodeGen.Common.Attributes;
using Lean.CodeGen.WebApi.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Lean.CodeGen.Application.Services.Admin;
using Lean.CodeGen.Common.Enums;

namespace Lean.CodeGen.WebApi.Controllers.Identity;

/// <summary>
/// 部门管理控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "identity")]
[Authorize]
[LeanPermission("identity:dept", "部门管理")]
public class LeanDeptController : LeanBaseController
{
  private readonly ILeanDeptService _deptService;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="deptService">部门服务</param>
  /// <param name="localizationService">本地化服务</param>
  /// <param name="configuration">配置</param>
  /// <param name="userContext">用户上下文</param>
  public LeanDeptController(
      ILeanDeptService deptService,
      ILeanLocalizationService localizationService,
      IConfiguration configuration,
      ILeanUserContext userContext)
      : base(localizationService, configuration, userContext)
  {
    _deptService = deptService;
  }

  /// <summary>
  /// 分页查询部门
  /// </summary>
  /// <param name="input">查询参数</param>
  /// <returns>分页结果</returns>
  [HttpGet("list")]
  [LeanPermission("identity:dept:list", "分页查询部门")]
  public async Task<IActionResult> GetPageAsync([FromQuery] LeanDeptQueryDto input)
  {
    var result = await _deptService.GetPageAsync(input);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 获取部门信息
  /// </summary>
  /// <param name="id">部门ID</param>
  /// <returns>部门信息</returns>
  [HttpGet("{id}")]
  [LeanPermission("identity:dept:query", "查询部门")]
  public async Task<IActionResult> GetAsync(long id)
  {
    var result = await _deptService.GetAsync(id);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 创建部门
  /// </summary>
  /// <param name="input">部门创建参数</param>
  /// <returns>创建结果</returns>
  [HttpPost]
  [LeanPermission("identity:dept:create", "创建部门")]
  public async Task<IActionResult> CreateAsync([FromBody] LeanDeptCreateDto input)
  {
    var result = await _deptService.CreateAsync(input);
    return Success(result, LeanBusinessType.Create);
  }

  /// <summary>
  /// 更新部门
  /// </summary>
  /// <param name="input">部门更新参数</param>
  /// <returns>更新结果</returns>
  [HttpPut]
  [LeanPermission("identity:dept:update", "更新部门")]
  public async Task<IActionResult> UpdateAsync([FromBody] LeanDeptUpdateDto input)
  {
    var result = await _deptService.UpdateAsync(input);
    return Success(result, LeanBusinessType.Update);
  }

  /// <summary>
  /// 删除部门
  /// </summary>
  /// <param name="id">部门ID</param>
  /// <returns>删除结果</returns>
  [HttpDelete("{id}")]
  [LeanPermission("identity:dept:delete", "删除部门")]
  public async Task<IActionResult> DeleteAsync(long id)
  {
    var result = await _deptService.DeleteAsync(id);
    return Success(result, LeanBusinessType.Delete);
  }

  /// <summary>
  /// 批量删除部门
  /// </summary>
  /// <param name="ids">部门ID列表</param>
  /// <returns>删除结果</returns>
  [HttpPost("batch-delete")]
  [LeanPermission("identity:dept:delete", "批量删除部门")]
  public async Task<IActionResult> BatchDeleteAsync([FromBody] List<long> ids)
  {
    var result = await _deptService.BatchDeleteAsync(ids);
    return Success(result, LeanBusinessType.Delete);
  }

  /// <summary>
  /// 导出部门数据
  /// </summary>
  /// <param name="input">导出参数</param>
  /// <returns>导出文件</returns>
  [HttpGet("export")]
  [LeanPermission("identity:dept:export", "导出部门")]
  public async Task<IActionResult> ExportAsync([FromQuery] LeanDeptExportQueryDto input)
  {
    var fileBytes = await _deptService.ExportAsync(input);
    return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"部门数据_{DateTime.Now:yyyyMMddHHmmss}.xlsx");
  }

  /// <summary>
  /// 获取导入模板
  /// </summary>
  /// <returns>模板文件</returns>
  [HttpGet("template")]
  [LeanPermission("identity:dept:import", "获取导入模板")]
  public async Task<IActionResult> GetTemplateAsync()
  {
    var fileBytes = await _deptService.GetTemplateAsync();
    return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "部门导入模板.xlsx");
  }

  /// <summary>
  /// 导入部门数据
  /// </summary>
  /// <param name="file">导入文件</param>
  /// <returns>导入结果</returns>
  [HttpPost("import")]
  [LeanPermission("identity:dept:import", "导入部门")]
  public async Task<IActionResult> ImportAsync([FromForm] LeanFileInfo file)
  {
    var result = await _deptService.ImportAsync(file);
    return Success(result, LeanBusinessType.Import);
  }

  /// <summary>
  /// 设置部门状态
  /// </summary>
  /// <param name="input">状态修改参数</param>
  /// <returns>修改结果</returns>
  [HttpPut("status")]
  [LeanPermission("identity:dept:update", "修改部门状态")]
  public async Task<IActionResult> SetStatusAsync([FromBody] LeanDeptChangeStatusDto input)
  {
    var result = await _deptService.SetStatusAsync(input);
    return Success(result, LeanBusinessType.Update);
  }

  /// <summary>
  /// 获取部门树形结构
  /// </summary>
  /// <param name="input">查询参数</param>
  /// <returns>树形结构</returns>
  [HttpGet("tree")]
  [LeanPermission("identity:dept:query", "查询部门树")]
  public async Task<IActionResult> GetTreeAsync([FromQuery] LeanDeptQueryDto input)
  {
    var result = await _deptService.GetTreeAsync(input);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 获取角色部门树
  /// </summary>
  /// <param name="roleId">角色ID</param>
  /// <returns>角色部门树</returns>
  [HttpGet("role-tree/{roleId}")]
  [LeanPermission("identity:dept:query", "查询角色部门树")]
  public async Task<IActionResult> GetRoleDeptTreeAsync(long roleId)
  {
    var result = await _deptService.GetRoleDeptTreeAsync(roleId);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 获取用户部门树
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <returns>用户部门树</returns>
  [HttpGet("user-tree/{userId}")]
  [LeanPermission("identity:dept:query", "查询用户部门树")]
  public async Task<IActionResult> GetUserDeptTreeAsync(long userId)
  {
    var result = await _deptService.GetUserDeptTreeAsync(userId);
    return Success(result, LeanBusinessType.Query);
  }
}
