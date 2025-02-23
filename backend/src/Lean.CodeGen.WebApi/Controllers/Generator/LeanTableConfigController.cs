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
    public async Task<LeanApiResult<LeanPageResult<LeanTableConfigDto>>> GetPageListAsync([FromQuery] LeanTableConfigQueryDto queryDto)
    {
      var result = await _tableConfigService.GetPageListAsync(queryDto);
      return LeanApiResult<LeanPageResult<LeanTableConfigDto>>.Ok(result);
    }

    /// <summary>
    /// 获取表配置关联详情
    /// </summary>
    [HttpGet("{id}")]
    public async Task<LeanApiResult<LeanTableConfigDto>> GetAsync(long id)
    {
      var result = await _tableConfigService.GetAsync(id);
      return LeanApiResult<LeanTableConfigDto>.Ok(result);
    }

    /// <summary>
    /// 创建表配置关联
    /// </summary>
    [HttpPost]
    public async Task<LeanApiResult<LeanTableConfigDto>> CreateAsync([FromBody] LeanCreateTableConfigDto createDto)
    {
      var result = await _tableConfigService.CreateAsync(createDto);
      return LeanApiResult<LeanTableConfigDto>.Ok(result);
    }

    /// <summary>
    /// 更新表配置关联
    /// </summary>
    [HttpPut("{id}")]
    public async Task<LeanApiResult<LeanTableConfigDto>> UpdateAsync(long id, [FromBody] LeanUpdateTableConfigDto updateDto)
    {
      var result = await _tableConfigService.UpdateAsync(id, updateDto);
      return LeanApiResult<LeanTableConfigDto>.Ok(result);
    }

    /// <summary>
    /// 删除表配置关联
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<LeanApiResult> DeleteAsync(long id)
    {
      var result = await _tableConfigService.DeleteAsync(id);
      return result ? LeanApiResult.Ok() : LeanApiResult.Error("删除失败");
    }

    /// <summary>
    /// 导出表配置关联
    /// </summary>
    [HttpGet("export")]
    public async Task<LeanApiResult<LeanFileResult>> ExportAsync([FromQuery] LeanTableConfigQueryDto queryDto)
    {
      var result = await _tableConfigService.ExportAsync(queryDto);
      return LeanApiResult<LeanFileResult>.Ok(result);
    }

    /// <summary>
    /// 导入表配置关联
    /// </summary>
    [HttpPost("import")]
    public async Task<LeanApiResult<LeanExcelImportResult<LeanTableConfigImportDto>>> ImportAsync([FromForm] LeanFileInfo file)
    {
      var result = await _tableConfigService.ImportAsync(file);
      return LeanApiResult<LeanExcelImportResult<LeanTableConfigImportDto>>.Ok(result);
    }

    /// <summary>
    /// 下载导入模板
    /// </summary>
    [HttpGet("template")]
    public async Task<LeanApiResult<LeanFileResult>> DownloadTemplateAsync()
    {
      var result = await _tableConfigService.DownloadTemplateAsync();
      return LeanApiResult<LeanFileResult>.Ok(result);
    }
  }
}