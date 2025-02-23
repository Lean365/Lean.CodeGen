using Lean.CodeGen.Application.Dtos.Admin;
using Lean.CodeGen.Application.Services.Admin;
using Lean.CodeGen.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lean.CodeGen.WebApi.Controllers.Admin;

/// <summary>
/// 语言控制器
/// </summary>
[Route("api/admin/language")]
[ApiController]
public class LeanLanguageController : LeanBaseController
{
  private readonly ILeanLanguageService _languageService;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanLanguageController(ILeanLanguageService languageService)
  {
    _languageService = languageService;
  }

  /// <summary>
  /// 创建语言
  /// </summary>
  [HttpPost]
  public async Task<IActionResult> CreateAsync([FromBody] LeanCreateLanguageDto input)
  {
    var result = await _languageService.CreateAsync(input);
    return ApiResult(result);
  }

  /// <summary>
  /// 更新语言
  /// </summary>
  [HttpPut]
  public async Task<IActionResult> UpdateAsync([FromBody] LeanUpdateLanguageDto input)
  {
    var result = await _languageService.UpdateAsync(input);
    return ApiResult(result);
  }

  /// <summary>
  /// 删除语言
  /// </summary>
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteAsync([FromRoute] long id)
  {
    var result = await _languageService.DeleteAsync(id);
    return ApiResult(result);
  }

  /// <summary>
  /// 批量删除语言
  /// </summary>
  [HttpDelete("batch")]
  public async Task<IActionResult> BatchDeleteAsync([FromBody] List<long> ids)
  {
    var result = await _languageService.BatchDeleteAsync(ids);
    return ApiResult(result);
  }

  /// <summary>
  /// 获取语言信息
  /// </summary>
  [HttpGet("{id}")]
  public async Task<IActionResult> GetAsync([FromRoute] long id)
  {
    var result = await _languageService.GetAsync(id);
    return ApiResult(result);
  }

  /// <summary>
  /// 分页查询语言
  /// </summary>
  [HttpGet("page")]
  public async Task<IActionResult> GetPageAsync([FromQuery] LeanQueryLanguageDto input)
  {
    var result = await _languageService.GetPageAsync(input);
    return ApiResult(result);
  }

  /// <summary>
  /// 修改语言状态
  /// </summary>
  [HttpPut("status")]
  public async Task<IActionResult> SetStatusAsync([FromBody] LeanChangeLanguageStatusDto input)
  {
    var result = await _languageService.SetStatusAsync(input);
    return ApiResult(result);
  }

  /// <summary>
  /// 获取所有正常状态的语言列表
  /// </summary>
  [HttpGet("list")]
  public async Task<IActionResult> GetListAsync()
  {
    var result = await _languageService.GetListAsync();
    return ApiResult(result);
  }

  /// <summary>
  /// 设置默认语言
  /// </summary>
  [HttpPut("{id}/default")]
  public async Task<IActionResult> SetDefaultAsync([FromRoute] long id)
  {
    var result = await _languageService.SetDefaultAsync(id);
    return ApiResult(result);
  }
}