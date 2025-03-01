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
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Lean.CodeGen.WebApi.Controllers.Admin;

/// <summary>
/// 系统配置控制器
/// </summary>
[Route("api/admin/config")]
[ApiController]
public class LeanConfigController : LeanBaseController
{
  private readonly ILeanConfigService _configService;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanConfigController(
      ILeanConfigService configService,
      ILeanLocalizationService localizationService,
      IConfiguration configuration)
      : base(localizationService, configuration)
  {
    _configService = configService;
  }

  /// <summary>
  /// 创建系统配置
  /// </summary>
  [HttpPost]
  public async Task<IActionResult> CreateAsync([FromBody] LeanCreateConfigDto input)
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
  public async Task<IActionResult> UpdateAsync([FromBody] LeanUpdateConfigDto input)
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
  public async Task<IActionResult> GetPagedListAsync([FromQuery] LeanQueryConfigDto input)
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
  public async Task<IActionResult> ExportAsync([FromQuery] LeanQueryConfigDto input)
  {
    var bytes = await _configService.ExportAsync(input);
    var fileName = $"configs_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
    return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
  }

  /// <summary>
  /// 导入系统配置
  /// </summary>
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
    var result = await _configService.ImportAsync(fileInfo);
    return Success(result, LeanBusinessType.Import);
  }

  /// <summary>
  /// 获取导入模板
  /// </summary>
  [HttpGet("template")]
  public async Task<IActionResult> GetImportTemplateAsync()
  {
    var bytes = await _configService.GetImportTemplateAsync();
    var fileName = $"config_template_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
    return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
  }
}