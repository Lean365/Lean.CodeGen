using Lean.CodeGen.Application.Dtos.Admin;
using Lean.CodeGen.Application.Services.Admin;
using Lean.CodeGen.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lean.CodeGen.WebApi.Controllers.Admin;

/// <summary>
/// 系统配置控制器
/// </summary>
[Route("api/admin/[controller]")]
[ApiController]
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
    return ApiResult(result);
  }

  /// <summary>
  /// 更新系统配置
  /// </summary>
  [HttpPut]
  public async Task<IActionResult> UpdateAsync([FromBody] LeanUpdateConfigDto input)
  {
    var result = await _service.UpdateAsync(input);
    return ApiResult(result);
  }

  /// <summary>
  /// 删除系统配置
  /// </summary>
  [HttpDelete]
  public async Task<IActionResult> DeleteAsync([FromBody] List<long> ids)
  {
    var result = await _service.DeleteAsync(ids);
    return ApiResult(result);
  }

  /// <summary>
  /// 获取系统配置详情
  /// </summary>
  [HttpGet("{id}")]
  public async Task<IActionResult> GetAsync(long id)
  {
    var result = await _service.GetAsync(id);
    return ApiResult(result);
  }

  /// <summary>
  /// 分页查询系统配置
  /// </summary>
  [HttpGet("list")]
  public async Task<IActionResult> GetPagedListAsync([FromQuery] LeanQueryConfigDto input)
  {
    var result = await _service.GetPagedListAsync(input);
    return ApiResult(result);
  }

  /// <summary>
  /// 导出系统配置
  /// </summary>
  [HttpGet("export")]
  public async Task<IActionResult> ExportAsync([FromQuery] LeanQueryConfigDto input)
  {
    var result = await _service.ExportAsync(input);
    return FileResult(result, $"系统配置_{DateTime.Now:yyyyMMddHHmmss}.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
  }

  /// <summary>
  /// 导入系统配置
  /// </summary>
  [HttpPost("import")]
  public async Task<IActionResult> ImportAsync([FromForm] IFormFile file)
  {
    if (file == null || file.Length == 0)
    {
      return ApiResult(LeanApiResult.Error("请选择要导入的文件"));
    }

    var fileInfo = new LeanFileInfo
    {
      FileName = file.FileName,
      FilePath = Path.GetTempFileName()
    };

    try
    {
      using (var stream = new FileStream(fileInfo.FilePath, FileMode.Create))
      {
        await file.CopyToAsync(stream);
      }

      var result = await _service.ImportAsync(fileInfo);
      return ApiResult(result);
    }
    finally
    {
      if (System.IO.File.Exists(fileInfo.FilePath))
      {
        System.IO.File.Delete(fileInfo.FilePath);
      }
    }
  }

  /// <summary>
  /// 下载导入模板
  /// </summary>
  [HttpGet("import/template")]
  public async Task<IActionResult> GetImportTemplateAsync()
  {
    var result = await _service.GetImportTemplateAsync();
    return FileResult(result, "系统配置导入模板.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
  }
}