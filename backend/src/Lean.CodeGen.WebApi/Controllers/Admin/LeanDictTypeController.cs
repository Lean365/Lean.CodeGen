using Lean.CodeGen.Application.Dtos.Admin;
using Lean.CodeGen.Application.Services.Admin;
using Lean.CodeGen.Common.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lean.CodeGen.Common.Enums;

namespace Lean.CodeGen.WebApi.Controllers.Admin;

/// <summary>
/// 字典类型控制器
/// </summary>
[Route("api/admin/dict-type")]
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
    return Success(result, LeanBusinessType.Create);
  }

  /// <summary>
  /// 更新字典类型
  /// </summary>
  [HttpPut]
  public async Task<IActionResult> UpdateAsync([FromBody] LeanUpdateDictTypeDto input)
  {
    var result = await _service.UpdateAsync(input);
    return Success(result, LeanBusinessType.Update);
  }

  /// <summary>
  /// 删除字典类型
  /// </summary>
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteAsync(long id)
  {
    await _service.DeleteAsync(id);
    return Success(LeanBusinessType.Delete);
  }

  /// <summary>
  /// 获取字典类型详情
  /// </summary>
  [HttpGet("{id}")]
  public async Task<IActionResult> GetAsync(long id)
  {
    var result = await _service.GetAsync(id);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 分页查询字典类型
  /// </summary>
  [HttpGet]
  public async Task<IActionResult> GetPageAsync([FromQuery] LeanQueryDictTypeDto input)
  {
    var result = await _service.GetPageAsync(input);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 设置字典类型状态
  /// </summary>
  [HttpPut("status")]
  public async Task<IActionResult> SetStatusAsync([FromBody] LeanChangeDictTypeStatusDto input)
  {
    var result = await _service.SetStatusAsync(input);
    return Success(result, LeanBusinessType.Update);
  }

  /// <summary>
  /// 导出字典类型
  /// </summary>
  [HttpGet("export")]
  public async Task<IActionResult> ExportAsync([FromQuery] LeanQueryDictTypeDto input)
  {
    var bytes = await _service.ExportAsync(input);
    return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "dict-types.xlsx");
  }

  /// <summary>
  /// 导入字典类型
  /// </summary>
  [HttpPost("import")]
  public async Task<IActionResult> ImportAsync([FromForm] IFormFile file)
  {
    using var ms = new MemoryStream();
    await file.CopyToAsync(ms);
    var fileInfo = new LeanFileInfo { FilePath = System.IO.Path.GetTempFileName() };
    await System.IO.File.WriteAllBytesAsync(fileInfo.FilePath, ms.ToArray());
    var result = await _service.ImportAsync(fileInfo);
    System.IO.File.Delete(fileInfo.FilePath);
    return Success(result, LeanBusinessType.Import);
  }

  /// <summary>
  /// 获取导入模板
  /// </summary>
  [HttpGet("template")]
  public async Task<IActionResult> GetImportTemplateAsync()
  {
    var bytes = await _service.GetImportTemplateAsync();
    return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "dict-type-template.xlsx");
  }
}