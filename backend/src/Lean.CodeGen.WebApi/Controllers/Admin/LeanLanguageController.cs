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
  public Task<LeanApiResult<long>> CreateAsync([FromBody] LeanCreateLanguageDto input)
  {
    return _languageService.CreateAsync(input);
  }

  /// <summary>
  /// 更新语言
  /// </summary>
  [HttpPut]
  public Task<LeanApiResult> UpdateAsync([FromBody] LeanUpdateLanguageDto input)
  {
    return _languageService.UpdateAsync(input);
  }

  /// <summary>
  /// 删除语言
  /// </summary>
  [HttpDelete("{id}")]
  public Task<LeanApiResult> DeleteAsync([FromRoute] long id)
  {
    return _languageService.DeleteAsync(id);
  }

  /// <summary>
  /// 批量删除语言
  /// </summary>
  [HttpDelete("batch")]
  public Task<LeanApiResult> BatchDeleteAsync([FromBody] List<long> ids)
  {
    return _languageService.BatchDeleteAsync(ids);
  }

  /// <summary>
  /// 获取语言信息
  /// </summary>
  [HttpGet("{id}")]
  public Task<LeanApiResult<LeanLanguageDto>> GetAsync([FromRoute] long id)
  {
    return _languageService.GetAsync(id);
  }

  /// <summary>
  /// 分页查询语言
  /// </summary>
  [HttpGet("page")]
  public Task<LeanApiResult<LeanPageResult<LeanLanguageDto>>> GetPageAsync([FromQuery] LeanQueryLanguageDto input)
  {
    return _languageService.GetPageAsync(input);
  }

  /// <summary>
  /// 修改语言状态
  /// </summary>
  [HttpPut("status")]
  public Task<LeanApiResult> SetStatusAsync([FromBody] LeanChangeLanguageStatusDto input)
  {
    return _languageService.SetStatusAsync(input);
  }

  /// <summary>
  /// 获取所有正常状态的语言列表
  /// </summary>
  [HttpGet("list")]
  public Task<LeanApiResult<List<LeanLanguageDto>>> GetListAsync()
  {
    return _languageService.GetListAsync();
  }

  /// <summary>
  /// 设置默认语言
  /// </summary>
  [HttpPut("{id}/default")]
  public Task<LeanApiResult> SetDefaultAsync([FromRoute] long id)
  {
    return _languageService.SetDefaultAsync(id);
  }
}