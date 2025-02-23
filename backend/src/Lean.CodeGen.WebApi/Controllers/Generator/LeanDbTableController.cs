using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Application.Dtos.Generator;
using Lean.CodeGen.Application.Services.Generator;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Excel;
using Lean.CodeGen.WebApi.Controllers;

namespace Lean.CodeGen.WebApi.Controllers.Generator
{
  /// <summary>
  /// 数据库表控制器
  /// </summary>
  [Route("api/generator/tables")]
  [ApiController]
  [ApiExplorerSettings(GroupName = "generator")]
  public class LeanDbTableController : LeanBaseController
  {
    private readonly ILeanDbTableService _dbTableService;

    /// <summary>
    /// 构造函数
    /// </summary>
    public LeanDbTableController(ILeanDbTableService dbTableService)
    {
      _dbTableService = dbTableService;
    }

    /// <summary>
    /// 获取数据库表列表（分页）
    /// </summary>
    [HttpGet]
    public async Task<LeanApiResult<LeanPageResult<LeanDbTableDto>>> GetPageListAsync([FromQuery] LeanDbTableQueryDto queryDto)
    {
      var result = await _dbTableService.GetPageListAsync(queryDto);
      return LeanApiResult<LeanPageResult<LeanDbTableDto>>.Ok(result);
    }

    /// <summary>
    /// 获取数据库表详情
    /// </summary>
    [HttpGet("{id}")]
    public async Task<LeanApiResult<LeanDbTableDto>> GetAsync(long id)
    {
      var result = await _dbTableService.GetAsync(id);
      return LeanApiResult<LeanDbTableDto>.Ok(result);
    }

    /// <summary>
    /// 创建数据库表
    /// </summary>
    [HttpPost]
    public async Task<LeanApiResult<LeanDbTableDto>> CreateAsync([FromBody] LeanCreateDbTableDto createDto)
    {
      var result = await _dbTableService.CreateAsync(createDto);
      return LeanApiResult<LeanDbTableDto>.Ok(result);
    }

    /// <summary>
    /// 更新数据库表
    /// </summary>
    [HttpPut("{id}")]
    public async Task<LeanApiResult<LeanDbTableDto>> UpdateAsync(long id, [FromBody] LeanUpdateDbTableDto updateDto)
    {
      var result = await _dbTableService.UpdateAsync(id, updateDto);
      return LeanApiResult<LeanDbTableDto>.Ok(result);
    }

    /// <summary>
    /// 删除数据库表
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<LeanApiResult> DeleteAsync(long id)
    {
      var result = await _dbTableService.DeleteAsync(id);
      return result ? LeanApiResult.Ok() : LeanApiResult.Error("删除失败");
    }

    /// <summary>
    /// 导出数据库表
    /// </summary>
    [HttpGet("export")]
    public async Task<LeanApiResult<LeanFileResult>> ExportAsync([FromQuery] LeanDbTableQueryDto queryDto)
    {
      var result = await _dbTableService.ExportAsync(queryDto);
      return LeanApiResult<LeanFileResult>.Ok(result);
    }

    /// <summary>
    /// 导入数据库表
    /// </summary>
    [HttpPost("import")]
    public async Task<LeanApiResult<LeanExcelImportResult<LeanDbTableImportDto>>> ImportAsync([FromForm] LeanFileInfo file)
    {
      var result = await _dbTableService.ImportAsync(file);
      return LeanApiResult<LeanExcelImportResult<LeanDbTableImportDto>>.Ok(result);
    }

    /// <summary>
    /// 下载导入模板
    /// </summary>
    [HttpGet("template")]
    public async Task<LeanApiResult<LeanFileResult>> DownloadTemplateAsync()
    {
      var result = await _dbTableService.DownloadTemplateAsync();
      return LeanApiResult<LeanFileResult>.Ok(result);
    }

    /// <summary>
    /// 从数据源导入表
    /// </summary>
    [HttpPost("import-from-datasource/{dataSourceId}")]
    public async Task<LeanApiResult<List<LeanDbTableDto>>> ImportFromDataSourceAsync(long dataSourceId)
    {
      var result = await _dbTableService.ImportFromDataSourceAsync(dataSourceId);
      return LeanApiResult<List<LeanDbTableDto>>.Ok(result);
    }

    /// <summary>
    /// 同步表结构
    /// </summary>
    [HttpPost("{id}/sync")]
    public async Task<LeanApiResult> SyncStructureAsync(long id)
    {
      var result = await _dbTableService.SyncStructureAsync(id);
      return result ? LeanApiResult.Ok() : LeanApiResult.Error("同步失败");
    }
  }
}