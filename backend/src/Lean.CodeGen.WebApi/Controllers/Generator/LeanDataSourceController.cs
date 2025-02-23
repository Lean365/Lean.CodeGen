using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Application.Dtos.Generator;
using Lean.CodeGen.Application.Services.Generator;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Excel;

namespace Lean.CodeGen.WebApi.Controllers.Generator
{
  /// <summary>
  /// 数据源管理控制器
  /// </summary>
  [Route("api/generator/data-sources")]
  [ApiController]
  public class LeanDataSourceController : LeanBaseController
  {
    private readonly ILeanDataSourceService _dataSourceService;

    /// <summary>
    /// 构造函数
    /// </summary>
    public LeanDataSourceController(ILeanDataSourceService dataSourceService)
    {
      _dataSourceService = dataSourceService;
    }

    /// <summary>
    /// 获取数据源列表（分页）
    /// </summary>
    [HttpGet]
    public async Task<LeanApiResult<LeanPageResult<LeanDataSourceDto>>> GetPageListAsync([FromQuery] LeanDataSourceQueryDto queryDto)
    {
      var result = await _dataSourceService.GetPageListAsync(queryDto);
      return LeanApiResult<LeanPageResult<LeanDataSourceDto>>.Ok(result);
    }

    /// <summary>
    /// 获取数据源详情
    /// </summary>
    [HttpGet("{id}")]
    public async Task<LeanApiResult<LeanDataSourceDto>> GetAsync(long id)
    {
      var result = await _dataSourceService.GetAsync(id);
      return LeanApiResult<LeanDataSourceDto>.Ok(result);
    }

    /// <summary>
    /// 创建数据源
    /// </summary>
    [HttpPost]
    public async Task<LeanApiResult<LeanDataSourceDto>> CreateAsync([FromBody] LeanCreateDataSourceDto createDto)
    {
      var result = await _dataSourceService.CreateAsync(createDto);
      return LeanApiResult<LeanDataSourceDto>.Ok(result);
    }

    /// <summary>
    /// 更新数据源
    /// </summary>
    [HttpPut("{id}")]
    public async Task<LeanApiResult<LeanDataSourceDto>> UpdateAsync(long id, [FromBody] LeanUpdateDataSourceDto updateDto)
    {
      var result = await _dataSourceService.UpdateAsync(id, updateDto);
      return LeanApiResult<LeanDataSourceDto>.Ok(result);
    }

    /// <summary>
    /// 删除数据源
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<LeanApiResult<bool>> DeleteAsync(long id)
    {
      var result = await _dataSourceService.DeleteAsync(id);
      return LeanApiResult<bool>.Ok(result);
    }

    /// <summary>
    /// 导出数据源
    /// </summary>
    [HttpGet("export")]
    public async Task<LeanApiResult<LeanFileResult>> ExportAsync([FromQuery] LeanDataSourceQueryDto queryDto)
    {
      var result = await _dataSourceService.ExportAsync(queryDto);
      return LeanApiResult<LeanFileResult>.Ok(result);
    }

    /// <summary>
    /// 导入数据源
    /// </summary>
    [HttpPost("import")]
    public async Task<LeanApiResult<LeanExcelImportResult<LeanDataSourceImportDto>>> ImportAsync([FromForm] LeanFileInfo file)
    {
      var result = await _dataSourceService.ImportAsync(file);
      return LeanApiResult<LeanExcelImportResult<LeanDataSourceImportDto>>.Ok(result);
    }

    /// <summary>
    /// 下载导入模板
    /// </summary>
    [HttpGet("template")]
    public async Task<LeanApiResult<LeanFileResult>> DownloadTemplateAsync()
    {
      var result = await _dataSourceService.DownloadTemplateAsync();
      return LeanApiResult<LeanFileResult>.Ok(result);
    }

    /// <summary>
    /// 测试连接
    /// </summary>
    [HttpPost("{id}/test-connection")]
    public async Task<LeanApiResult<bool>> TestConnectionAsync(long id)
    {
      var result = await _dataSourceService.TestConnectionAsync(id);
      return LeanApiResult<bool>.Ok(result);
    }
  }
}