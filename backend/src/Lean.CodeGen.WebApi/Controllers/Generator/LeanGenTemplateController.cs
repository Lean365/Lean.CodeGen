using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Application.Dtos.Generator;
using Lean.CodeGen.Application.Services.Generator;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Excel;
using Lean.CodeGen.WebApi.Controllers;

namespace Lean.CodeGen.WebApi.Controllers.Generator
{
  /// <summary>
  /// 代码生成模板控制器
  /// </summary>
  [Route("api/generator/templates")]
  [ApiController]
  public class LeanGenTemplateController : LeanBaseController
  {
    private readonly ILeanGenTemplateService _genTemplateService;

    /// <summary>
    /// 构造函数
    /// </summary>
    public LeanGenTemplateController(ILeanGenTemplateService genTemplateService)
    {
      _genTemplateService = genTemplateService;
    }

    /// <summary>
    /// 获取代码生成模板列表（分页）
    /// </summary>
    [HttpGet]
    public async Task<LeanApiResult<LeanPageResult<LeanGenTemplateDto>>> GetPageListAsync([FromQuery] LeanGenTemplateQueryDto queryDto)
    {
      var result = await _genTemplateService.GetPageListAsync(queryDto);
      return LeanApiResult<LeanPageResult<LeanGenTemplateDto>>.Ok(result);
    }

    /// <summary>
    /// 获取代码生成模板详情
    /// </summary>
    [HttpGet("{id}")]
    public async Task<LeanApiResult<LeanGenTemplateDto>> GetAsync(long id)
    {
      var result = await _genTemplateService.GetAsync(id);
      return LeanApiResult<LeanGenTemplateDto>.Ok(result);
    }

    /// <summary>
    /// 创建代码生成模板
    /// </summary>
    [HttpPost]
    public async Task<LeanApiResult<LeanGenTemplateDto>> CreateAsync([FromBody] LeanCreateGenTemplateDto createDto)
    {
      var result = await _genTemplateService.CreateAsync(createDto);
      return LeanApiResult<LeanGenTemplateDto>.Ok(result);
    }

    /// <summary>
    /// 更新代码生成模板
    /// </summary>
    [HttpPut("{id}")]
    public async Task<LeanApiResult<LeanGenTemplateDto>> UpdateAsync(long id, [FromBody] LeanUpdateGenTemplateDto updateDto)
    {
      var result = await _genTemplateService.UpdateAsync(id, updateDto);
      return LeanApiResult<LeanGenTemplateDto>.Ok(result);
    }

    /// <summary>
    /// 删除代码生成模板
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<LeanApiResult<bool>> DeleteAsync(long id)
    {
      var result = await _genTemplateService.DeleteAsync(id);
      return LeanApiResult<bool>.Ok(result);
    }

    /// <summary>
    /// 导出代码生成模板
    /// </summary>
    [HttpGet("export")]
    public async Task<LeanApiResult<LeanFileResult>> ExportAsync([FromQuery] LeanGenTemplateQueryDto queryDto)
    {
      var result = await _genTemplateService.ExportAsync(queryDto);
      return LeanApiResult<LeanFileResult>.Ok(result);
    }

    /// <summary>
    /// 导入代码生成模板
    /// </summary>
    [HttpPost("import")]
    public async Task<LeanApiResult<LeanExcelImportResult<LeanGenTemplateImportDto>>> ImportAsync([FromForm] LeanFileInfo file)
    {
      var result = await _genTemplateService.ImportAsync(file);
      return LeanApiResult<LeanExcelImportResult<LeanGenTemplateImportDto>>.Ok(result);
    }

    /// <summary>
    /// 下载导入模板
    /// </summary>
    [HttpGet("template")]
    public async Task<LeanApiResult<LeanFileResult>> DownloadTemplateAsync()
    {
      var result = await _genTemplateService.DownloadTemplateAsync();
      return LeanApiResult<LeanFileResult>.Ok(result);
    }

    /// <summary>
    /// 复制模板
    /// </summary>
    [HttpPost("{id}/copy")]
    public async Task<LeanApiResult<LeanGenTemplateDto>> CopyAsync(long id)
    {
      var result = await _genTemplateService.CopyAsync(id);
      return LeanApiResult<LeanGenTemplateDto>.Ok(result);
    }
  }
}