using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Application.Dtos.Generator;
using Lean.CodeGen.Application.Services.Generator;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Excel;
using Lean.CodeGen.Common.Enums;
using System.IO;

namespace Lean.CodeGen.WebApi.Controllers.Generator
{
  /// <summary>
  /// 代码生成配置控制器
  /// </summary>
  [Route("api/generator/configs")]
  [ApiController]
  public class LeanGenConfigController : LeanBaseController
  {
    private readonly ILeanGenConfigService _genConfigService;

    /// <summary>
    /// 构造函数
    /// </summary>
    public LeanGenConfigController(ILeanGenConfigService genConfigService)
    {
      _genConfigService = genConfigService;
    }

    /// <summary>
    /// 获取代码生成配置列表（分页）
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetPageListAsync([FromQuery] LeanGenConfigQueryDto queryDto)
    {
      var result = await _genConfigService.GetPageListAsync(queryDto);
      return Success(result, LeanBusinessType.Query);
    }

    /// <summary>
    /// 获取代码生成配置详情
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(long id)
    {
      var result = await _genConfigService.GetAsync(id);
      return Success(result, LeanBusinessType.Query);
    }

    /// <summary>
    /// 创建代码生成配置
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] LeanCreateGenConfigDto createDto)
    {
      var result = await _genConfigService.CreateAsync(createDto);
      return Success(result, LeanBusinessType.Create);
    }

    /// <summary>
    /// 更新代码生成配置
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(long id, [FromBody] LeanUpdateGenConfigDto updateDto)
    {
      var result = await _genConfigService.UpdateAsync(id, updateDto);
      return Success(result, LeanBusinessType.Update);
    }

    /// <summary>
    /// 删除代码生成配置
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(long id)
    {
      var result = await _genConfigService.DeleteAsync(id);
      return Success(result, LeanBusinessType.Delete);
    }

    /// <summary>
    /// 导出代码生成配置
    /// </summary>
    [HttpGet("export")]
    public async Task<IActionResult> ExportAsync([FromQuery] LeanGenConfigQueryDto queryDto)
    {
      var result = await _genConfigService.ExportAsync(queryDto);
      var stream = new MemoryStream();
      result.Stream.CopyTo(stream);
      return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "gen-configs.xlsx");
    }

    /// <summary>
    /// 导入代码生成配置
    /// </summary>
    [HttpPost("import")]
    public async Task<IActionResult> ImportAsync([FromForm] LeanFileInfo file)
    {
      var result = await _genConfigService.ImportAsync(file);
      return Success(result, LeanBusinessType.Import);
    }

    /// <summary>
    /// 下载导入模板
    /// </summary>
    [HttpGet("template")]
    public async Task<IActionResult> DownloadTemplateAsync()
    {
      var result = await _genConfigService.DownloadTemplateAsync();
      var stream = new MemoryStream();
      result.Stream.CopyTo(stream);
      return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "gen-config-template.xlsx");
    }

    /// <summary>
    /// 复制配置
    /// </summary>
    [HttpPost("{id}/copy")]
    public async Task<IActionResult> CopyAsync(long id)
    {
      var result = await _genConfigService.CopyAsync(id);
      return Success(result, LeanBusinessType.Create);
    }
  }
}