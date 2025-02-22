using Lean.CodeGen.Application.Dtos.Admin;
using Lean.CodeGen.Application.Services.Admin;
using Lean.CodeGen.Common.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lean.CodeGen.WebApi.Controllers.Admin;

/// <summary>
/// 字典类型控制器
/// </summary>
[Route("api/admin/[controller]")]
[ApiController]
public class LeanDictTypeController : LeanBaseController
{
  private readonly ILeanDictTypeService _service;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanDictTypeController(ILeanDictTypeService service)
  {
    _service = service;
  }

  /// <summary>
  /// 创建字典类型
  /// </summary>
  [HttpPost]
  public async Task<IActionResult> CreateAsync([FromBody] LeanCreateDictTypeDto input)
  {
    var result = await _service.CreateAsync(input);
    return ApiResult(result);
  }

  /// <summary>
  /// 更新字典类型
  /// </summary>
  [HttpPut]
  public async Task<IActionResult> UpdateAsync([FromBody] LeanUpdateDictTypeDto input)
  {
    var result = await _service.UpdateAsync(input);
    return ApiResult(result);
  }

  /// <summary>
  /// 删除字典类型
  /// </summary>
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteAsync(long id)
  {
    var result = await _service.DeleteAsync(id);
    return ApiResult(result);
  }

  /// <summary>
  /// 批量删除字典类型
  /// </summary>
  [HttpDelete]
  public async Task<IActionResult> BatchDeleteAsync([FromBody] List<long> ids)
  {
    var result = await _service.BatchDeleteAsync(ids);
    return ApiResult(result);
  }

  /// <summary>
  /// 获取字典类型信息
  /// </summary>
  [HttpGet("{id}")]
  public async Task<IActionResult> GetAsync(long id)
  {
    var result = await _service.GetAsync(id);
    return ApiResult(result);
  }

  /// <summary>
  /// 分页查询字典类型
  /// </summary>
  [HttpGet("list")]
  public async Task<IActionResult> GetPageAsync([FromQuery] LeanQueryDictTypeDto input)
  {
    var result = await _service.GetPageAsync(input);
    return ApiResult(result);
  }

  /// <summary>
  /// 设置字典类型状态
  /// </summary>
  [HttpPut("status")]
  public async Task<IActionResult> SetStatusAsync([FromBody] LeanChangeDictTypeStatusDto input)
  {
    var result = await _service.SetStatusAsync(input);
    return ApiResult(result);
  }

  /// <summary>
  /// 导出字典类型
  /// </summary>
  [HttpGet("export")]
  public async Task<IActionResult> ExportAsync([FromQuery] LeanQueryDictTypeDto input)
  {
    var result = await _service.ExportAsync(input);
    return FileResult(result, $"字典类型_{DateTime.Now:yyyyMMddHHmmss}.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
  }

  /// <summary>
  /// 导入字典类型
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
    return FileResult(result, "字典类型导入模板.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
  }
}