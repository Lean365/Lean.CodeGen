using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Application.Dtos.Generator;
using Lean.CodeGen.Application.Services.Generator;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Excel;
using Lean.CodeGen.WebApi.Controllers;

namespace Lean.CodeGen.WebApi.Controllers.Generator
{
  /// <summary>
  /// 表配置关联控制器
  /// </summary>
  [Route("api/generator/table-configs")]
  [ApiController]
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
    public Task<LeanPageResult<LeanTableConfigDto>> GetPageListAsync([FromQuery] LeanTableConfigQueryDto queryDto)
    {
      return _tableConfigService.GetPageListAsync(queryDto);
    }

    /// <summary>
    /// 获取表配置关联详情
    /// </summary>
    [HttpGet("{id}")]
    public Task<LeanTableConfigDto> GetAsync(long id)
    {
      return _tableConfigService.GetAsync(id);
    }

    /// <summary>
    /// 创建表配置关联
    /// </summary>
    [HttpPost]
    public Task<LeanTableConfigDto> CreateAsync([FromBody] LeanCreateTableConfigDto createDto)
    {
      return _tableConfigService.CreateAsync(createDto);
    }

    /// <summary>
    /// 更新表配置关联
    /// </summary>
    [HttpPut("{id}")]
    public Task<LeanTableConfigDto> UpdateAsync(long id, [FromBody] LeanUpdateTableConfigDto updateDto)
    {
      return _tableConfigService.UpdateAsync(id, updateDto);
    }

    /// <summary>
    /// 删除表配置关联
    /// </summary>
    [HttpDelete("{id}")]
    public Task<bool> DeleteAsync(long id)
    {
      return _tableConfigService.DeleteAsync(id);
    }

    /// <summary>
    /// 导出表配置关联
    /// </summary>
    [HttpGet("export")]
    public Task<LeanFileResult> ExportAsync([FromQuery] LeanTableConfigQueryDto queryDto)
    {
      return _tableConfigService.ExportAsync(queryDto);
    }

    /// <summary>
    /// 导入表配置关联
    /// </summary>
    [HttpPost("import")]
    public Task<LeanExcelImportResult<LeanTableConfigImportDto>> ImportAsync([FromForm] LeanFileInfo file)
    {
      return _tableConfigService.ImportAsync(file);
    }

    /// <summary>
    /// 下载导入模板
    /// </summary>
    [HttpGet("template")]
    public Task<LeanFileResult> DownloadTemplateAsync()
    {
      return _tableConfigService.DownloadTemplateAsync();
    }
  }
}