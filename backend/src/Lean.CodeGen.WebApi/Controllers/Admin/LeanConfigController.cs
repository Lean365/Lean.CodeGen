using Lean.CodeGen.Application.Dtos.Admin;
using Lean.CodeGen.Application.Services.Admin;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Excel;
using Lean.CodeGen.Common.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Lean.CodeGen.WebApi.Controllers.Admin;

/// <summary>
/// 系统配置控制器
/// </summary>
[Route("api/[controller]")]
public class LeanConfigController : LeanBaseController
{
  private readonly ILeanConfigService _service;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanConfigController(ILeanConfigService service)
  {
    _service = service;
  }

  /// <summary>
  /// 创建系统配置
  /// </summary>
  [HttpPost]
  public async Task<IActionResult> CreateAsync([FromBody] LeanCreateConfigDto input)
  {
    var result = await _service.CreateAsync(input);
    return Success(result, LeanBusinessType.Create);
  }

  /// <summary>
  /// 更新系统配置
  /// </summary>
  [HttpPut]
  public async Task<IActionResult> UpdateAsync([FromBody] LeanUpdateConfigDto input)
  {
    var result = await _service.UpdateAsync(input);
    return Success(result, LeanBusinessType.Update);
  }

  /// <summary>
  /// 删除系统配置
  /// </summary>
  [HttpDelete]
  public async Task<IActionResult> DeleteAsync([FromBody] List<long> ids)
  {
    var result = await _service.DeleteAsync(ids);
    return Success(result, LeanBusinessType.Delete);
  }

  /// <summary>
  /// 获取系统配置详情
  /// </summary>
  [HttpGet("{id}")]
  public async Task<IActionResult> GetAsync(long id)
  {
    var result = await _service.GetAsync(id);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 分页查询系统配置
  /// </summary>
  [HttpGet]
  public async Task<IActionResult> GetPagedListAsync([FromQuery] LeanQueryConfigDto input)
  {
    var result = await _service.GetPagedListAsync(input);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 导出系统配置
  /// </summary>
  [HttpGet("export")]
  public async Task<IActionResult> ExportAsync([FromQuery] LeanQueryConfigDto input)
  {
    var bytes = await _service.ExportAsync(input);
    return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "configs.xlsx", true);
  }

  /// <summary>
  /// 导入系统配置
  /// </summary>
  [HttpPost("import")]
  public async Task<IActionResult> ImportAsync([FromForm] IFormFile file)
  {
    using var ms = new MemoryStream();
    await file.CopyToAsync(ms);
    var result = await _service.ImportAsync(ms.ToArray());
    return Success(result, LeanBusinessType.Import);
  }

  /// <summary>
  /// 获取导入模板
  /// </summary>
  [HttpGet("template")]
  public async Task<IActionResult> GetImportTemplateAsync()
  {
    var bytes = await _service.GetImportTemplateAsync();
    return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "config-template.xlsx", true);
  }
}