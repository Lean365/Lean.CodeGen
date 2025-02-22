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
    public Task<LeanPageResult<LeanDbTableDto>> GetPageListAsync([FromQuery] LeanDbTableQueryDto queryDto)
    {
      return _dbTableService.GetPageListAsync(queryDto);
    }

    /// <summary>
    /// 获取数据库表详情
    /// </summary>
    [HttpGet("{id}")]
    public Task<LeanDbTableDto> GetAsync(long id)
    {
      return _dbTableService.GetAsync(id);
    }

    /// <summary>
    /// 创建数据库表
    /// </summary>
    [HttpPost]
    public Task<LeanDbTableDto> CreateAsync([FromBody] LeanCreateDbTableDto createDto)
    {
      return _dbTableService.CreateAsync(createDto);
    }

    /// <summary>
    /// 更新数据库表
    /// </summary>
    [HttpPut("{id}")]
    public Task<LeanDbTableDto> UpdateAsync(long id, [FromBody] LeanUpdateDbTableDto updateDto)
    {
      return _dbTableService.UpdateAsync(id, updateDto);
    }

    /// <summary>
    /// 删除数据库表
    /// </summary>
    [HttpDelete("{id}")]
    public Task<bool> DeleteAsync(long id)
    {
      return _dbTableService.DeleteAsync(id);
    }

    /// <summary>
    /// 导出数据库表
    /// </summary>
    [HttpGet("export")]
    public Task<LeanFileResult> ExportAsync([FromQuery] LeanDbTableQueryDto queryDto)
    {
      return _dbTableService.ExportAsync(queryDto);
    }

    /// <summary>
    /// 导入数据库表
    /// </summary>
    [HttpPost("import")]
    public Task<LeanExcelImportResult<LeanDbTableImportDto>> ImportAsync([FromForm] LeanFileInfo file)
    {
      return _dbTableService.ImportAsync(file);
    }

    /// <summary>
    /// 下载导入模板
    /// </summary>
    [HttpGet("template")]
    public Task<LeanFileResult> DownloadTemplateAsync()
    {
      return _dbTableService.DownloadTemplateAsync();
    }

    /// <summary>
    /// 从数据源导入表
    /// </summary>
    [HttpPost("import-from-datasource/{dataSourceId}")]
    public Task<List<LeanDbTableDto>> ImportFromDataSourceAsync(long dataSourceId)
    {
      return _dbTableService.ImportFromDataSourceAsync(dataSourceId);
    }

    /// <summary>
    /// 同步表结构
    /// </summary>
    [HttpPost("{id}/sync")]
    public Task<bool> SyncStructureAsync(long id)
    {
      return _dbTableService.SyncStructureAsync(id);
    }
  }
}