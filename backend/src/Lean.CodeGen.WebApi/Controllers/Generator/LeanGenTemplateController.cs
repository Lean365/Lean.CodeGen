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
    public Task<LeanPageResult<LeanGenTemplateDto>> GetPageListAsync([FromQuery] LeanGenTemplateQueryDto queryDto)
    {
      return _genTemplateService.GetPageListAsync(queryDto);
    }

    /// <summary>
    /// 获取代码生成模板详情
    /// </summary>
    [HttpGet("{id}")]
    public Task<LeanGenTemplateDto> GetAsync(long id)
    {
      return _genTemplateService.GetAsync(id);
    }

    /// <summary>
    /// 创建代码生成模板
    /// </summary>
    [HttpPost]
    public Task<LeanGenTemplateDto> CreateAsync([FromBody] LeanCreateGenTemplateDto createDto)
    {
      return _genTemplateService.CreateAsync(createDto);
    }

    /// <summary>
    /// 更新代码生成模板
    /// </summary>
    [HttpPut("{id}")]
    public Task<LeanGenTemplateDto> UpdateAsync(long id, [FromBody] LeanUpdateGenTemplateDto updateDto)
    {
      return _genTemplateService.UpdateAsync(id, updateDto);
    }

    /// <summary>
    /// 删除代码生成模板
    /// </summary>
    [HttpDelete("{id}")]
    public Task<bool> DeleteAsync(long id)
    {
      return _genTemplateService.DeleteAsync(id);
    }

    /// <summary>
    /// 导出代码生成模板
    /// </summary>
    [HttpGet("export")]
    public Task<LeanFileResult> ExportAsync([FromQuery] LeanGenTemplateQueryDto queryDto)
    {
      return _genTemplateService.ExportAsync(queryDto);
    }

    /// <summary>
    /// 导入代码生成模板
    /// </summary>
    [HttpPost("import")]
    public Task<LeanExcelImportResult<LeanGenTemplateImportDto>> ImportAsync([FromForm] LeanFileInfo file)
    {
      return _genTemplateService.ImportAsync(file);
    }

    /// <summary>
    /// 下载导入模板
    /// </summary>
    [HttpGet("template")]
    public Task<LeanFileResult> DownloadTemplateAsync()
    {
      return _genTemplateService.DownloadTemplateAsync();
    }

    /// <summary>
    /// 复制模板
    /// </summary>
    [HttpPost("{id}/copy")]
    public Task<LeanGenTemplateDto> CopyAsync(long id)
    {
      return _genTemplateService.CopyAsync(id);
    }
  }
}