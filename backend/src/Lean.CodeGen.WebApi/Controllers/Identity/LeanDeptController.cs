using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Application.Services.Identity;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Enums;
using Microsoft.Extensions.Configuration;
using Lean.CodeGen.Application.Services.Admin;
using Lean.CodeGen.Common.Attributes;

namespace Lean.CodeGen.WebApi.Controllers.Identity;

/// <summary>
/// 部门管理控制器
/// </summary>
/// <remarks>
/// 提供部门管理相关的API接口，包括：
/// 1. 部门的增删改查
/// 2. 部门状态管理
/// 3. 部门导入导出
/// </remarks>
[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "identity")]
[LeanPermission("system:dept", "部门管理")]
public class LeanDeptController : LeanBaseController
{
  private readonly ILeanDeptService _deptService;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="deptService">部门服务</param>
  /// <param name="localizationService">本地化服务</param>
  /// <param name="configuration">配置</param>
  public LeanDeptController(
      ILeanDeptService deptService,
      ILeanLocalizationService localizationService,
      IConfiguration configuration)
      : base(localizationService, configuration)
  {
    _deptService = deptService;
  }

  /// <summary>
  /// 创建部门
  /// </summary>
  /// <param name="input">部门创建参数</param>
  /// <returns>创建成功的部门信息</returns>
  [HttpPost]
  [LeanPermission("system:dept:create", "创建部门")]
  public async Task<IActionResult> CreateAsync([FromBody] LeanDeptCreateDto input)
  {
    var result = await _deptService.CreateAsync(input);
    return Success(result, LeanBusinessType.Create);
  }

  /// <summary>
  /// 更新部门
  /// </summary>
  /// <param name="input">部门更新参数</param>
  /// <returns>更新后的部门信息</returns>
  [HttpPut]
  [LeanPermission("system:dept:update", "更新部门")]
  public async Task<IActionResult> UpdateAsync([FromBody] LeanDeptUpdateDto input)
  {
    var result = await _deptService.UpdateAsync(input);
    return Success(result, LeanBusinessType.Update);
  }

  /// <summary>
  /// 删除部门
  /// </summary>
  [HttpDelete("{id}")]
  [LeanPermission("system:dept:delete", "删除部门")]
  public async Task<IActionResult> DeleteAsync(long id)
  {
    var result = await _deptService.DeleteAsync(id);
    return Success(result, LeanBusinessType.Delete);
  }

  /// <summary>
  /// 批量删除部门
  /// </summary>
  [HttpDelete]
  [LeanPermission("system:dept:delete", "删除部门")]
  public async Task<IActionResult> BatchDeleteAsync([FromBody] List<long> ids)
  {
    var result = await _deptService.BatchDeleteAsync(ids);
    return Success(result, LeanBusinessType.Delete);
  }

  /// <summary>
  /// 获取部门信息
  /// </summary>
  /// <param name="id">部门ID</param>
  /// <returns>部门详细信息</returns>
  [HttpGet("{id}")]
  [LeanPermission("system:dept:query", "查询部门")]
  public async Task<IActionResult> GetAsync(long id)
  {
    var result = await _deptService.GetAsync(id);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 查询部门列表
  /// </summary>
  /// <param name="input">查询参数</param>
  /// <returns>部门列表</returns>
  [HttpGet]
  [LeanPermission("system:dept:query", "查询部门")]
  public async Task<IActionResult> QueryAsync([FromQuery] LeanDeptQueryDto input)
  {
    var result = await _deptService.QueryAsync(input);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 获取部门树形结构
  /// </summary>
  /// <param name="input">查询参数</param>
  /// <returns>部门树形结构</returns>
  [HttpGet("tree")]
  [LeanPermission("system:dept:query", "查询部门")]
  public async Task<IActionResult> GetTreeAsync([FromQuery] LeanDeptQueryDto input)
  {
    var result = await _deptService.GetTreeAsync(input);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 设置部门状态
  /// </summary>
  [HttpPut("status")]
  [LeanPermission("system:dept:update", "更新部门")]
  public async Task<IActionResult> SetStatusAsync([FromBody] LeanDeptChangeStatusDto input)
  {
    var result = await _deptService.SetStatusAsync(input);
    return Success(result, LeanBusinessType.Update);
  }

  /// <summary>
  /// 获取角色部门树
  /// </summary>
  /// <param name="roleId">角色ID</param>
  /// <returns>角色部门树</returns>
  [HttpGet("role/{roleId}")]
  [LeanPermission("system:dept:query", "查询部门")]
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
  [HttpGet("user/{userId}")]
  [LeanPermission("system:dept:query", "查询部门")]
  public async Task<IActionResult> GetUserDeptTreeAsync(long userId)
  {
    var result = await _deptService.GetUserDeptTreeAsync(userId);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 导出部门数据
  /// </summary>
  /// <param name="input">导出参数</param>
  /// <returns>导出的Excel文件</returns>
  [HttpGet("export")]
  [LeanPermission("system:dept:export", "导出部门")]
  public async Task<IActionResult> ExportAsync([FromQuery] LeanDeptQueryDto input)
  {
    var bytes = await _deptService.ExportAsync(input);
    return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "departments.xlsx");
  }

  /// <summary>
  /// 导入部门数据
  /// </summary>
  /// <param name="file">Excel文件</param>
  /// <returns>导入结果</returns>
  [HttpPost("import")]
  [LeanPermission("system:dept:import", "导入部门")]
  public async Task<IActionResult> ImportAsync([FromForm] LeanFileInfo file)
  {
    var result = await _deptService.ImportAsync(file);
    return Success(result, LeanBusinessType.Import);
  }

  /// <summary>
  /// 获取导入模板
  /// </summary>
  /// <returns>导入模板Excel文件</returns>
  [HttpGet("template")]
  [LeanPermission("system:dept:import", "导入部门")]
  public async Task<IActionResult> GetImportTemplateAsync()
  {
    var bytes = await _deptService.GetImportTemplateAsync();
    return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "department-template.xlsx");
  }
}