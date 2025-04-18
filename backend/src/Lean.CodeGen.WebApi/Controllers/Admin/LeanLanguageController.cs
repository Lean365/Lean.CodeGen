using Lean.CodeGen.Application.Dtos.Admin;
using Lean.CodeGen.Application.Services.Admin;
using Lean.CodeGen.Common.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Attributes;
using Microsoft.Extensions.Configuration;

namespace Lean.CodeGen.WebApi.Controllers.Admin;

/// <summary>
/// 语言控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "admin")]
public class LeanLanguageController : LeanBaseController
{
  private readonly ILeanLanguageService _languageService;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanLanguageController(
      ILeanLanguageService languageService,
      ILeanLocalizationService localizationService,
      IConfiguration configuration,
      ILeanUserContext userContext)
      : base(localizationService, configuration, userContext)
  {
    _languageService = languageService;
  }

  /// <summary>
  /// 创建语言
  /// </summary>
  [HttpPost]
  public async Task<IActionResult> CreateAsync([FromBody] LeanLanguageCreateDto input)
  {
    var result = await _languageService.CreateAsync(input);
    return Success(result, LeanBusinessType.Create);
  }

  /// <summary>
  /// 更新语言
  /// </summary>
  [HttpPut]
  public async Task<IActionResult> UpdateAsync([FromBody] LeanLanguageUpdateDto input)
  {
    var result = await _languageService.UpdateAsync(input);
    return Success(result, LeanBusinessType.Update);
  }

  /// <summary>
  /// 删除语言
  /// </summary>
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteAsync([FromRoute] long id)
  {
    var result = await _languageService.DeleteAsync(id);
    return Success(result, LeanBusinessType.Delete);
  }

  /// <summary>
  /// 批量删除语言
  /// </summary>
  [HttpDelete("batch")]
  public async Task<IActionResult> BatchDeleteAsync([FromBody] List<long> ids)
  {
    var result = await _languageService.BatchDeleteAsync(ids);
    return Success(result, LeanBusinessType.Delete);
  }

  /// <summary>
  /// 获取语言信息
  /// </summary>
  [HttpGet("{id}")]
  public async Task<IActionResult> GetAsync([FromRoute] long id)
  {
    var result = await _languageService.GetAsync(id);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 分页查询语言
  /// </summary>
  [HttpGet("page")]
  public async Task<IActionResult> GetPageAsync([FromQuery] LeanLanguageQueryDto input)
  {
    var result = await _languageService.GetPageAsync(input);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 修改语言状态
  /// </summary>
  [HttpPut("status")]
  public async Task<IActionResult> SetStatusAsync([FromBody] LeanLanguageChangeStatusDto input)
  {
    var result = await _languageService.SetStatusAsync(input);
    return Success(result, LeanBusinessType.Update);
  }

  /// <summary>
  /// 获取所有正常状态的语言列表
  /// </summary>
  [HttpGet("list")]
  public async Task<IActionResult> GetListAsync()
  {
    var result = await _languageService.GetListAsync();
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 设置默认语言
  /// </summary>
  [HttpPut("{id}/default")]
  public async Task<IActionResult> SetDefaultAsync([FromRoute] long id)
  {
    var result = await _languageService.SetDefaultAsync(id);
    return Success(result, LeanBusinessType.Update);
  }

  /// <summary>
  /// 导出语言
  /// </summary>
  [HttpGet("export")]
  public async Task<IActionResult> ExportAsync([FromQuery] LeanLanguageQueryDto input)
  {
    var bytes = await _languageService.ExportAsync(input);
    return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "languages.xlsx");
  }

  /// <summary>
  /// 导入语言
  /// </summary>
  /// <param name="file">Excel文件</param>
  /// <returns>导入结果</returns>
  [HttpPost("import")]
  [LeanPermission("system:language:import", "导入语言")]
  public async Task<IActionResult> ImportAsync([FromForm] LeanFileInfo file)
  {
    var result = await _languageService.ImportAsync(file);
    return Success(result, LeanBusinessType.Import);
  }

  /// <summary>
  /// 获取导入模板
  /// </summary>
  [HttpGet("template")]
  public async Task<IActionResult> GetTemplateAsync()
  {
    var bytes = await _languageService.GetTemplateAsync();
    return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "language-template.xlsx");
  }
}