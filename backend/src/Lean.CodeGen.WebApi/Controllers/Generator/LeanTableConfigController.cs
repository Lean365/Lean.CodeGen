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
  /// 表配置关联控制器
  /// </summary>
  [Route("api/generator/table-configs")]
  [ApiController]
  [ApiExplorerSettings(GroupName = "generator")]
  public class LeanTableConfigController : LeanBaseController
  {
    private readonly ILeanTableConfigService _tableConfigService;

    /// <summary>
    /// 构造函数
    /// </summary>
    public LeanTableConfigController(ILeanTableConfigService tableConfigService)
    {
      _tableConfigService = tableConfigService;
    }

    /// <summary>
    /// 获取表配置关联列表（分页）
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetPageListAsync([FromQuery] LeanTableConfigQueryDto queryDto)
    {
      var result = await _tableConfigService.GetPageListAsync(queryDto);
      return Success(result, LeanBusinessType.Query);
    }

    /// <summary>
    /// 获取表配置关联详情
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(long id)
    {
      var result = await _tableConfigService.GetAsync(id);
      return Success(result, LeanBusinessType.Query);
    }

    /// <summary>
    /// 创建表配置关联
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] LeanCreateTableConfigDto createDto)
    {
      var result = await _tableConfigService.CreateAsync(createDto);
      return Success(result, LeanBusinessType.Create);
    }

    /// <summary>
    /// 更新表配置关联
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(long id, [FromBody] LeanUpdateTableConfigDto updateDto)
    {
      var result = await _tableConfigService.UpdateAsync(id, updateDto);
      return Success(result, LeanBusinessType.Update);
    }

    /// <summary>
    /// 删除表配置关联
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(long id)
    {
      var result = await _tableConfigService.DeleteAsync(id);
      return result ? Success(LeanBusinessType.Delete) : Error("删除失败");
    }

    /// <summary>
    /// 导出表配置关联
    /// </summary>
    [HttpGet("export")]
    public async Task<IActionResult> ExportAsync([FromQuery] LeanTableConfigQueryDto queryDto)
    {
      var result = await _tableConfigService.ExportAsync(queryDto);
      var stream = new MemoryStream();
      result.Stream.CopyTo(stream);
      return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "table-configs.xlsx");
    }

    /// <summary>
    /// 导入表配置关联
    /// </summary>
    [HttpPost("import")]
    public async Task<IActionResult> ImportAsync([FromForm] LeanFileInfo file)
    {
      var result = await _tableConfigService.ImportAsync(file);
      return Success(result, LeanBusinessType.Import);
    }

    /// <summary>
    /// 下载导入模板
    /// </summary>
    [HttpGet("template")]
    public async Task<IActionResult> DownloadTemplateAsync()
    {
      var result = await _tableConfigService.DownloadTemplateAsync();
      var stream = new MemoryStream();
      result.Stream.CopyTo(stream);
      return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "table-config-template.xlsx");
    }
  }
}