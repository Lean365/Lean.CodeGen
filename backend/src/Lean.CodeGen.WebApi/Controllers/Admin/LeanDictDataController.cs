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
  public async Task<LeanApiResult<long>> CreateAsync([FromBody] LeanCreateDictDataDto input)
  {
    return await _service.CreateAsync(input);
  }

  /// <summary>
  /// 更新字典数据
  /// </summary>
  [HttpPut]
  public async Task<LeanApiResult> UpdateAsync([FromBody] LeanUpdateDictDataDto input)
  {
    return await _service.UpdateAsync(input);
  }

  /// <summary>
  /// 删除字典数据
  /// </summary>
  [HttpDelete("{id}")]
  public async Task<LeanApiResult> DeleteAsync(long id)
  {
    return await _service.DeleteAsync(id);
  }

  /// <summary>
  /// 批量删除字典数据
  /// </summary>
  [HttpDelete]
  public async Task<LeanApiResult> BatchDeleteAsync([FromBody] List<long> ids)
  {
    return await _service.BatchDeleteAsync(ids);
  }

  /// <summary>
  /// 获取字典数据详情
  /// </summary>
  [HttpGet("{id}")]
  public async Task<LeanApiResult<LeanDictDataDto>> GetAsync(long id)
  {
    return await _service.GetAsync(id);
  }

  /// <summary>
  /// 分页查询字典数据
  /// </summary>
  [HttpGet("page")]
  public async Task<LeanApiResult<LeanPageResult<LeanDictDataDto>>> GetPagedListAsync([FromQuery] LeanQueryDictDataDto input)
  {
    return await _service.GetPageAsync(input);
  }

  /// <summary>
  /// 设置字典数据状态
  /// </summary>
  [HttpPut("status")]
  public async Task<LeanApiResult> SetStatusAsync([FromBody] LeanChangeDictDataStatusDto input)
  {
    return await _service.SetStatusAsync(input);
  }

  /// <summary>
  /// 根据字典类型编码获取字典数据列表
  /// </summary>
  [HttpGet("type/{typeCode}")]
  public async Task<LeanApiResult<List<LeanDictDataDto>>> GetListByTypeAsync(string typeCode)
  {
    return await _service.GetListByTypeCodeAsync(typeCode);
  }
}