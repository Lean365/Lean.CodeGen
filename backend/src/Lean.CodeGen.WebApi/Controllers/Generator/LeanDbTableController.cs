using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Application.Dtos.Generator;
using Lean.CodeGen.Application.Services.Generator;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Excel;
using Lean.CodeGen.Common.Enums;
using Microsoft.Extensions.Configuration;
using Lean.CodeGen.Application.Services.Admin;
using Lean.CodeGen.WebApi.Controllers;
using System.IO;

namespace Lean.CodeGen.WebApi.Controllers.Generator
{
  /// <summary>
  /// 数据库表控制器
  /// </summary>
  [ApiController]
  [Route("api/[controller]")]
  [ApiExplorerSettings(GroupName = "generator")]
  public class LeanDbTableController : LeanBaseController
  {
    private readonly ILeanDbTableService _dbTableService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="dbTableService">数据库表服务</param>
    /// <param name="localizationService">本地化服务</param>
    /// <param name="configuration">配置</param>
    /// <param name="userContext">用户上下文</param>
    public LeanDbTableController(
        ILeanDbTableService dbTableService,
        ILeanLocalizationService localizationService,
        IConfiguration configuration,
        ILeanUserContext userContext)
        : base(localizationService, configuration, userContext)
    {
      _dbTableService = dbTableService;
    }

    /// <summary>
    /// 获取数据库表列表（分页）
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetPageListAsync([FromQuery] LeanDbTableQueryDto queryDto)
    {
      var result = await _dbTableService.GetPageListAsync(queryDto);
      return Success(result, LeanBusinessType.Query);
    }

    /// <summary>
    /// 获取数据库表详情
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(long id)
    {
      var result = await _dbTableService.GetAsync(id);
      return Success(result, LeanBusinessType.Query);
    }

    /// <summary>
    /// 创建数据库表
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] LeanCreateDbTableDto createDto)
    {
      var result = await _dbTableService.CreateAsync(createDto);
      return Success(result, LeanBusinessType.Create);
    }

    /// <summary>
    /// 更新数据库表
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(long id, [FromBody] LeanUpdateDbTableDto updateDto)
    {
      var result = await _dbTableService.UpdateAsync(id, updateDto);
      return Success(result, LeanBusinessType.Update);
    }

    /// <summary>
    /// 删除数据库表
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(long id)
    {
      var result = await _dbTableService.DeleteAsync(id);
      return result ? Success(LeanBusinessType.Delete) : Error("删除失败");
    }

    /// <summary>
    /// 导出数据库表
    /// </summary>
    [HttpGet("export")]
    public async Task<IActionResult> ExportAsync([FromQuery] LeanDbTableQueryDto queryDto)
    {
      var result = await _dbTableService.ExportAsync(queryDto);
      var stream = new MemoryStream();
      result.Stream.CopyTo(stream);
      return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "db-tables.xlsx");
    }

    /// <summary>
    /// 导入数据库表
    /// </summary>
    [HttpPost("import")]
    public async Task<IActionResult> ImportAsync([FromForm] LeanFileInfo file)
    {
      var result = await _dbTableService.ImportAsync(file);
      return Success(result, LeanBusinessType.Import);
    }

    /// <summary>
    /// 下载导入模板
    /// </summary>
    [HttpGet("template")]
    public async Task<IActionResult> DownloadTemplateAsync()
    {
      var result = await _dbTableService.DownloadTemplateAsync();
      var stream = new MemoryStream();
      result.Stream.CopyTo(stream);
      return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "db-table-template.xlsx");
    }

    /// <summary>
    /// 从数据源导入表
    /// </summary>
    [HttpPost("import-from-datasource/{dataSourceId}")]
    public async Task<IActionResult> ImportFromDataSourceAsync(long dataSourceId)
    {
      var result = await _dbTableService.ImportFromDataSourceAsync(dataSourceId);
      return Success(result, LeanBusinessType.Import);
    }

    /// <summary>
    /// 同步表结构
    /// </summary>
    [HttpPost("{id}/sync")]
    public async Task<IActionResult> SyncStructureAsync(long id)
    {
      var result = await _dbTableService.SyncStructureAsync(id);
      return result ? Success(LeanBusinessType.Other) : Error("同步失败");
    }
  }
}