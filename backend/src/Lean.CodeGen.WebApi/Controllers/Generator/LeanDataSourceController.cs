using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Application.Dtos.Generator;
using Lean.CodeGen.Application.Services.Generator;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Excel;
using Lean.CodeGen.Common.Enums;
using System.IO;
using Microsoft.Extensions.Configuration;
using Lean.CodeGen.Application.Services.Admin;
using Lean.CodeGen.Common.Attributes;

namespace Lean.CodeGen.WebApi.Controllers.Generator
{
  /// <summary>
  /// 数据源控制器
  /// </summary>
  [ApiController]
  [Route("api/[controller]")]
  [ApiExplorerSettings(GroupName = "generator")]
  [LeanPermission("generator:datasource", "数据源管理")]
  public class LeanDataSourceController : LeanBaseController
  {
    private readonly ILeanDataSourceService _dataSourceService;

    /// <summary>
    /// 构造函数
    /// </summary>
    public LeanDataSourceController(
        ILeanDataSourceService dataSourceService,
        ILeanLocalizationService localizationService,
        IConfiguration configuration,
        ILeanUserContext userContext)
        : base(localizationService, configuration, userContext)
    {
      _dataSourceService = dataSourceService;
    }

    /// <summary>
    /// 获取数据源列表（分页）
    /// </summary>
    [HttpGet]
    [LeanPermission("generator:datasource:query", "查询数据源")]
    public async Task<IActionResult> GetPageListAsync([FromQuery] LeanDataSourceQueryDto queryDto)
    {
      var result = await _dataSourceService.GetPageListAsync(queryDto);
      return Success(result, LeanBusinessType.Query);
    }

    /// <summary>
    /// 获取数据源详情
    /// </summary>
    [HttpGet("{id}")]
    [LeanPermission("generator:datasource:query", "查询数据源")]
    public async Task<IActionResult> GetAsync(long id)
    {
      var result = await _dataSourceService.GetAsync(id);
      return Success(result, LeanBusinessType.Query);
    }

    /// <summary>
    /// 创建数据源
    /// </summary>
    [HttpPost]
    [LeanPermission("generator:datasource:create", "创建数据源")]
    public async Task<IActionResult> CreateAsync([FromBody] LeanCreateDataSourceDto createDto)
    {
      var result = await _dataSourceService.CreateAsync(createDto);
      return Success(result, LeanBusinessType.Create);
    }

    /// <summary>
    /// 更新数据源
    /// </summary>
    [HttpPut("{id}")]
    [LeanPermission("generator:datasource:update", "更新数据源")]
    public async Task<IActionResult> UpdateAsync(long id, [FromBody] LeanUpdateDataSourceDto updateDto)
    {
      var result = await _dataSourceService.UpdateAsync(id, updateDto);
      return Success(result, LeanBusinessType.Update);
    }

    /// <summary>
    /// 删除数据源
    /// </summary>
    [HttpDelete("{id}")]
    [LeanPermission("generator:datasource:delete", "删除数据源")]
    public async Task<IActionResult> DeleteAsync(long id)
    {
      var result = await _dataSourceService.DeleteAsync(id);
      return result ? Success(LeanBusinessType.Delete) : await ErrorAsync("generator.error.delete_failed");
    }

    /// <summary>
    /// 导出数据源
    /// </summary>
    [HttpGet("export")]
    [LeanPermission("generator:datasource:export", "导出数据源")]
    public async Task<IActionResult> ExportAsync([FromQuery] LeanDataSourceQueryDto queryDto)
    {
      var result = await _dataSourceService.ExportAsync(queryDto);
      var stream = new MemoryStream();
      result.Stream.CopyTo(stream);
      return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "datasources.xlsx");
    }

    /// <summary>
    /// 导入数据源
    /// </summary>
    [HttpPost("import")]
    [LeanPermission("generator:datasource:import", "导入数据源")]
    public async Task<IActionResult> ImportAsync([FromForm] LeanFileInfo file)
    {
      var result = await _dataSourceService.ImportAsync(file);
      return Success(result, LeanBusinessType.Import);
    }

    /// <summary>
    /// 下载导入模板
    /// </summary>
    [HttpGet("template")]
    [LeanPermission("generator:datasource:import", "导入数据源")]
    public async Task<IActionResult> DownloadTemplateAsync()
    {
      var result = await _dataSourceService.DownloadTemplateAsync();
      var stream = new MemoryStream();
      result.Stream.CopyTo(stream);
      return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "datasource-template.xlsx");
    }

    /// <summary>
    /// 测试数据源连接
    /// </summary>
    [HttpPost("{id}/test")]
    [LeanPermission("generator:datasource:update", "更新数据源")]
    public async Task<IActionResult> TestConnectionAsync(long id)
    {
      var result = await _dataSourceService.TestConnectionAsync(id);
      return result ? Success(LeanBusinessType.Other) : await ErrorAsync("generator.error.connection_failed");
    }
  }
}