using Lean.CodeGen.Application.Dtos.Admin;
using Lean.CodeGen.Application.Services.Admin;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Excel;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Attributes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Lean.CodeGen.WebApi.Controllers.Admin;

/// <summary>
/// 系统配置控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "admin")]
[LeanPermission("system:config", "系统配置管理")]
public class LeanConfigController : LeanBaseController
{
  private readonly ILeanConfigService _configService;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanConfigController(
      ILeanConfigService configService,
      ILeanLocalizationService localizationService,
      IConfiguration configuration,
      ILeanUserContext userContext)
      : base(localizationService, configuration, userContext)
  {
    _configService = configService;
  }

  /// <summary>
  /// 创建系统配置
  /// </summary>
  [HttpPost]
  [LeanPermission("system:config:create", "创建系统配置")]
  public async Task<IActionResult> CreateAsync([FromBody] LeanConfigCreateDto input)
  {
    var result = await _configService.CreateAsync(input);
    if (!result.Success)
    {
      return await ErrorAsync(result.Message ?? "common.error.create_failed");
    }
    return Success(result.Data, LeanBusinessType.Create);
  }

  /// <summary>
  /// 更新系统配置
  /// </summary>
  [HttpPut]
  [LeanPermission("system:config:update", "更新系统配置")]
  public async Task<IActionResult> UpdateAsync([FromBody] LeanConfigUpdateDto input)
  {
    var result = await _configService.UpdateAsync(input);
    if (!result.Success)
    {
      return await ErrorAsync(result.Message ?? "common.error.update_failed");
    }
    return Success<object?>(null, LeanBusinessType.Update);
  }

  /// <summary>
  /// 删除系统配置
  /// </summary>
  [HttpDelete]
  [LeanPermission("system:config:delete", "删除系统配置")]
  public async Task<IActionResult> DeleteAsync([FromBody] List<long> ids)
  {
    var result = await _configService.DeleteAsync(ids);
    if (!result.Success)
    {
      return await ErrorAsync(result.Message ?? "common.error.delete_failed");
    }
    return Success<object?>(null, LeanBusinessType.Delete);
  }

  /// <summary>
  /// 获取系统配置详情
  /// </summary>
  [HttpGet("{id}")]
  [LeanPermission("system:config:query", "查询系统配置")]
  public async Task<IActionResult> GetAsync(long id)
  {
    var result = await _configService.GetAsync(id);
    if (!result.Success)
    {
      return await ErrorAsync(result.Message ?? "common.error.get_failed");
    }
    return Success(result.Data, LeanBusinessType.Query);
  }

  /// <summary>
  /// 分页查询系统配置
  /// </summary>
  [HttpGet]
  [LeanPermission("system:config:query", "查询系统配置")]
  public async Task<IActionResult> GetPagedListAsync([FromQuery] LeanConfigQueryDto input)
  {
    var result = await _configService.GetPagedListAsync(input);
    if (!result.Success)
    {
      return await ErrorAsync(result.Message ?? "common.error.query_failed");
    }
    return Success(result.Data, LeanBusinessType.Query);
  }

  /// <summary>
  /// 导出系统配置
  /// </summary>
  [HttpGet("export")]
  [LeanPermission("system:config:export", "导出系统配置")]
  public async Task<IActionResult> ExportAsync([FromQuery] LeanConfigQueryDto input)
  {
    var bytes = await _configService.ExportAsync(input);
    var fileName = $"configs_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
    return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
  }

  /// <summary>
  /// 导入系统配置
  /// </summary>
  /// <param name="file">Excel文件</param>
  /// <returns>导入结果</returns>
  [HttpPost("import")]
  [LeanPermission("system:config:import", "导入系统配置")]
  public async Task<IActionResult> ImportAsync([FromForm] LeanFileInfo file)
  {
    var result = await _configService.ImportAsync(file);
    return Success(result, LeanBusinessType.Import);
  }

  /// <summary>
  /// 获取导入模板
  /// </summary>
  [HttpGet("template")]
  [LeanPermission("system:config:import", "导入系统配置")]
  public async Task<IActionResult> GetTemplateAsync()
  {
    var bytes = await _configService.GetTemplateAsync();
    var fileName = $"config_template_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
    return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
  }
}