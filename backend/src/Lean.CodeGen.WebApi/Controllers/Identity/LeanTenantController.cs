using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Application.Services.Identity;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Attributes;
using Microsoft.Extensions.Configuration;
using Lean.CodeGen.Application.Services.Admin;
using Lean.CodeGen.Common.Excel;
using Microsoft.AspNetCore.Http;

namespace Lean.CodeGen.WebApi.Controllers.Identity;

/// <summary>
/// 租户管理控制器
/// </summary>
/// <remarks>
/// 提供租户管理相关的API接口，包括：
/// 1. 租户的增删改查
/// 2. 租户状态管理
/// 3. 租户导入导出
/// </remarks>
[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "identity")]
[LeanPermission("identity:tenant", "租户管理")]
public class LeanTenantController : LeanBaseController
{
  private readonly ILeanTenantService _tenantService;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="tenantService">租户服务</param>
  /// <param name="localizationService">本地化服务</param>
  /// <param name="configuration">配置</param>
  /// <param name="userContext">用户上下文</param>
  public LeanTenantController(
      ILeanTenantService tenantService,
      ILeanLocalizationService localizationService,
      IConfiguration configuration,
      ILeanUserContext userContext)
      : base(localizationService, configuration, userContext)
  {
    _tenantService = tenantService;
  }

  /// <summary>
  /// 分页查询租户
  /// </summary>
  /// <param name="input">查询参数</param>
  /// <returns>分页查询结果</returns>
  [LeanPermission("identity:tenant:list", "查询租户")]
  [HttpGet("list")]
  public async Task<IActionResult> GetPageAsync([FromQuery] LeanTenantQueryDto input)
  {
    var result = await _tenantService.GetPageAsync(input);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 获取租户信息
  /// </summary>
  /// <param name="id">租户ID</param>
  /// <returns>租户详细信息</returns>
  [LeanPermission("identity:tenant:query", "查询租户")]
  [HttpGet("{id}")]
  public async Task<IActionResult> GetAsync(long id)
  {
    var result = await _tenantService.GetAsync(id);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 创建租户
  /// </summary>
  /// <param name="input">租户创建参数</param>
  /// <returns>创建成功的租户信息</returns>
  [LeanPermission("identity:tenant:create", "新增租户")]
  [HttpPost]
  public async Task<IActionResult> CreateAsync([FromBody] LeanTenantCreateDto input)
  {
    var result = await _tenantService.CreateAsync(input);
    return Success(result, LeanBusinessType.Create);
  }

  /// <summary>
  /// 更新租户
  /// </summary>
  /// <param name="input">租户更新参数</param>
  /// <returns>更新后的租户信息</returns>
  [LeanPermission("identity:tenant:update", "修改租户")]
  [HttpPut]
  public async Task<IActionResult> UpdateAsync([FromBody] LeanTenantUpdateDto input)
  {
    var result = await _tenantService.UpdateAsync(input);
    return Success(result, LeanBusinessType.Update);
  }

  /// <summary>
  /// 删除租户
  /// </summary>
  /// <param name="input">租户删除参数</param>
  [LeanPermission("identity:tenant:delete", "删除租户")]
  [HttpDelete]
  public async Task<IActionResult> DeleteAsync([FromBody] LeanTenantDeleteDto input)
  {
    await _tenantService.DeleteAsync(input);
    return Success(LeanBusinessType.Delete);
  }

  /// <summary>
  /// 导出租户
  /// </summary>
  /// <param name="input">导出参数</param>
  /// <returns>导出的文件</returns>
  [LeanPermission("identity:tenant:export", "导出租户")]
  [HttpGet("export")]
  public async Task<IActionResult> ExportAsync([FromQuery] LeanTenantExportQueryDto input)
  {
    var fileBytes = await _tenantService.ExportAsync(input);
    return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "tenants.xlsx");
  }

  /// <summary>
  /// 导入租户
  /// </summary>
  /// <param name="file">Excel文件</param>
  /// <returns>导入结果</returns>
  [LeanPermission("identity:tenant:import", "导入租户")]
  [HttpPost("import")]
  public async Task<IActionResult> ImportAsync([FromForm] LeanFileInfo file)
  {
    var result = await _tenantService.ImportAsync(file);
    return Success(result, LeanBusinessType.Import);
  }

  /// <summary>
  /// 获取导入模板
  /// </summary>
  /// <returns>模板文件</returns>
  [LeanPermission("identity:tenant:import", "导入租户")]
  [HttpGet("import-template")]
  public async Task<IActionResult> GetTemplateAsync()
  {
    var fileBytes = await _tenantService.GetTemplateAsync();
    return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "tenant-import-template.xlsx");
  }

  /// <summary>
  /// 修改租户状态
  /// </summary>
  /// <param name="input">状态修改参数</param>
  [LeanPermission("identity:tenant:update", "修改租户")]
  [HttpPut("status")]
  public async Task<IActionResult> ChangeStatusAsync([FromBody] LeanTenantChangeStatusDto input)
  {
    await _tenantService.ChangeStatusAsync(input);
    return Success(LeanBusinessType.Update);
  }
}