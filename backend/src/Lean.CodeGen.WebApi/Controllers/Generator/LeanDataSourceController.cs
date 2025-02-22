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
    public Task<LeanPageResult<LeanDataSourceDto>> GetPageListAsync([FromQuery] LeanDataSourceQueryDto queryDto)
    {
      return _dataSourceService.GetPageListAsync(queryDto);
    }

    /// <summary>
    /// 获取数据源详情
    /// </summary>
    [HttpGet("{id}")]
    public Task<LeanDataSourceDto> GetAsync(long id)
    {
      return _dataSourceService.GetAsync(id);
    }

    /// <summary>
    /// 创建数据源
    /// </summary>
    [HttpPost]
    public Task<LeanDataSourceDto> CreateAsync([FromBody] LeanCreateDataSourceDto createDto)
    {
      return _dataSourceService.CreateAsync(createDto);
    }

    /// <summary>
    /// 更新数据源
    /// </summary>
    [HttpPut("{id}")]
    public Task<LeanDataSourceDto> UpdateAsync(long id, [FromBody] LeanUpdateDataSourceDto updateDto)
    {
      return _dataSourceService.UpdateAsync(id, updateDto);
    }

    /// <summary>
    /// 删除数据源
    /// </summary>
    [HttpDelete("{id}")]
    public Task<bool> DeleteAsync(long id)
    {
      return _dataSourceService.DeleteAsync(id);
    }

    /// <summary>
    /// 导出数据源
    /// </summary>
    [HttpGet("export")]
    public Task<LeanFileResult> ExportAsync([FromQuery] LeanDataSourceQueryDto queryDto)
    {
      return _dataSourceService.ExportAsync(queryDto);
    }

    /// <summary>
    /// 导入数据源
    /// </summary>
    [HttpPost("import")]
    public Task<LeanExcelImportResult<LeanDataSourceImportDto>> ImportAsync([FromForm] LeanFileInfo file)
    {
      return _dataSourceService.ImportAsync(file);
    }

    /// <summary>
    /// 下载导入模板
    /// </summary>
    [HttpGet("template")]
    public Task<LeanFileResult> DownloadTemplateAsync()
    {
      return _dataSourceService.DownloadTemplateAsync();
    }

    /// <summary>
    /// 测试连接
    /// </summary>
    [HttpPost("{id}/test-connection")]
    public Task<bool> TestConnectionAsync(long id)
    {
      return _dataSourceService.TestConnectionAsync(id);
    }
  }
}