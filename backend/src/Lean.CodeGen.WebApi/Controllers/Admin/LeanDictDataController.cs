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
/// 字典数据控制器
/// </summary>
[Route("api/admin/[controller]")]
[ApiController]
public class LeanDictDataController : LeanBaseController
{
  private readonly ILeanDictDataService _service;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanDictDataController(ILeanDictDataService service)
  {
    _service = service;
  }

  /// <summary>
  /// 创建字典数据
  /// </summary>
  [HttpPost]
  public async Task<IActionResult> CreateAsync([FromBody] LeanCreateDictDataDto input)
  {
    var result = await _service.CreateAsync(input);
    return ApiResult(result);
  }

  /// <summary>
  /// 更新字典数据
  /// </summary>
  [HttpPut]
  public async Task<IActionResult> UpdateAsync([FromBody] LeanUpdateDictDataDto input)
  {
    var result = await _service.UpdateAsync(input);
    return ApiResult(result);
  }

  /// <summary>
  /// 删除字典数据
  /// </summary>
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteAsync(long id)
  {
    var result = await _service.DeleteAsync(id);
    return ApiResult(result);
  }

  /// <summary>
  /// 批量删除字典数据
  /// </summary>
  [HttpDelete]
  public async Task<IActionResult> BatchDeleteAsync([FromBody] List<long> ids)
  {
    var result = await _service.BatchDeleteAsync(ids);
    return ApiResult(result);
  }

  /// <summary>
  /// 获取字典数据详情
  /// </summary>
  [HttpGet("{id}")]
  public async Task<IActionResult> GetAsync(long id)
  {
    var result = await _service.GetAsync(id);
    return ApiResult(result);
  }

  /// <summary>
  /// 分页查询字典数据
  /// </summary>
  [HttpGet("list")]
  public async Task<IActionResult> GetPageAsync([FromQuery] LeanQueryDictDataDto input)
  {
    var result = await _service.GetPageAsync(input);
    return ApiResult(result);
  }

  /// <summary>
  /// 设置字典数据状态
  /// </summary>
  [HttpPut("status")]
  public async Task<IActionResult> SetStatusAsync([FromBody] LeanChangeDictDataStatusDto input)
  {
    var result = await _service.SetStatusAsync(input);
    return ApiResult(result);
  }

  /// <summary>
  /// 根据字典类型编码获取字典数据列表
  /// </summary>
  [HttpGet("type/{typeCode}")]
  public async Task<IActionResult> GetListByTypeCodeAsync(string typeCode)
  {
    var result = await _service.GetListByTypeCodeAsync(typeCode);
    return ApiResult(result);
  }
}