using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Application.Dtos.Admin;
using Lean.CodeGen.Application.Services.Admin;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Attributes;
using System.IO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Lean.CodeGen.WebApi.Controllers;

namespace Lean.CodeGen.WebApi.Controllers.Admin;

/// <summary>
/// 字典数据控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "admin")]
public class LeanDictDataController : LeanBaseController
{
  private readonly ILeanDictDataService _service;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanDictDataController(
      ILeanDictDataService service,
      ILeanLocalizationService localizationService,
      IConfiguration configuration)
      : base(localizationService, configuration)
  {
    _service = service;
  }

  /// <summary>
  /// 创建字典数据
  /// </summary>
  [HttpPost]
  public async Task<IActionResult> CreateAsync([FromBody] LeanDictDataCreateDto input)
  {
    var result = await _service.CreateAsync(input);
    return Success(result, LeanBusinessType.Create);
  }

  /// <summary>
  /// 更新字典数据
  /// </summary>
  [HttpPut]
  public async Task<IActionResult> UpdateAsync([FromBody] LeanDictDataUpdateDto input)
  {
    var result = await _service.UpdateAsync(input);
    return Success(result, LeanBusinessType.Update);
  }

  /// <summary>
  /// 删除字典数据
  /// </summary>
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteAsync(long id)
  {
    await _service.DeleteAsync(id);
    return Success(LeanBusinessType.Delete);
  }

  /// <summary>
  /// 批量删除字典数据
  /// </summary>
  [HttpDelete]
  public async Task<IActionResult> BatchDeleteAsync([FromBody] List<long> ids)
  {
    var result = await _service.BatchDeleteAsync(ids);
    return Success(result, LeanBusinessType.Delete);
  }

  /// <summary>
  /// 获取字典数据详情
  /// </summary>
  [HttpGet("{id}")]
  public async Task<IActionResult> GetAsync(long id)
  {
    var result = await _service.GetAsync(id);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 分页查询字典数据
  /// </summary>
  [HttpGet("page")]
  public async Task<IActionResult> GetPagedListAsync([FromQuery] LeanDictDataQueryDto input)
  {
    var result = await _service.GetPageAsync(input);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 设置字典数据状态
  /// </summary>
  [HttpPut("status")]
  public async Task<IActionResult> SetStatusAsync([FromBody] LeanDictDataChangeStatusDto input)
  {
    var result = await _service.SetStatusAsync(input);
    return Success(result, LeanBusinessType.Update);
  }

  /// <summary>
  /// 根据字典类型编码获取字典数据列表
  /// </summary>
  [HttpGet("type/{typeCode}")]
  public async Task<IActionResult> GetListByTypeAsync(string typeCode)
  {
    var result = await _service.GetListByTypeCodeAsync(typeCode);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 导出字典数据
  /// </summary>
  [HttpGet("export")]
  public async Task<IActionResult> ExportAsync([FromQuery] LeanDictDataQueryDto input)
  {
    var bytes = await _service.ExportAsync(input);
    return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "dict-data.xlsx");
  }

  /// <summary>
  /// 导入字典数据
  /// </summary>
  /// <param name="file">Excel文件</param>
  /// <returns>导入结果</returns>
  [HttpPost("import")]
  [LeanPermission("system:dict:import", "导入字典数据")]
  public async Task<IActionResult> ImportAsync([FromForm] LeanFileInfo file)
  {
    var result = await _service.ImportAsync(file);
    return Success(result, LeanBusinessType.Import);
  }

  /// <summary>
  /// 获取导入模板
  /// </summary>
  [HttpGet("template")]
  public async Task<IActionResult> GetImportTemplateAsync()
  {
    var bytes = await _service.GetImportTemplateAsync();
    return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "dict-data-template.xlsx");
  }
}