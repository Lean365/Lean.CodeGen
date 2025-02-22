using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Application.Dtos.Generator;
using Lean.CodeGen.Application.Services.Generator;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Excel;
using Lean.CodeGen.WebApi.Controllers;

namespace Lean.CodeGen.WebApi.Controllers.Generator
{
  /// <summary>
  /// 代码生成配置控制器
  /// </summary>
  [Route("api/generator/configs")]
  [ApiController]
  public class LeanGenConfigController : LeanBaseController
  {
    private readonly ILeanGenConfigService _genConfigService;

    /// <summary>
    /// 构造函数
    /// </summary>
    public LeanGenConfigController(ILeanGenConfigService genConfigService)
    {
      _genConfigService = genConfigService;
    }

    /// <summary>
    /// 获取代码生成配置列表（分页）
    /// </summary>
    [HttpGet]
    public Task<LeanPageResult<LeanGenConfigDto>> GetPageListAsync([FromQuery] LeanGenConfigQueryDto queryDto)
    {
      return _genConfigService.GetPageListAsync(queryDto);
    }

    /// <summary>
    /// 获取代码生成配置详情
    /// </summary>
    [HttpGet("{id}")]
    public Task<LeanGenConfigDto> GetAsync(long id)
    {
      return _genConfigService.GetAsync(id);
    }

    /// <summary>
    /// 创建代码生成配置
    /// </summary>
    [HttpPost]
    public Task<LeanGenConfigDto> CreateAsync([FromBody] LeanCreateGenConfigDto createDto)
    {
      return _genConfigService.CreateAsync(createDto);
    }

    /// <summary>
    /// 更新代码生成配置
    /// </summary>
    [HttpPut("{id}")]
    public Task<LeanGenConfigDto> UpdateAsync(long id, [FromBody] LeanUpdateGenConfigDto updateDto)
    {
      return _genConfigService.UpdateAsync(id, updateDto);
    }

    /// <summary>
    /// 删除代码生成配置
    /// </summary>
    [HttpDelete("{id}")]
    public Task<bool> DeleteAsync(long id)
    {
      return _genConfigService.DeleteAsync(id);
    }

    /// <summary>
    /// 导出代码生成配置
    /// </summary>
    [HttpGet("export")]
    public Task<LeanFileResult> ExportAsync([FromQuery] LeanGenConfigQueryDto queryDto)
    {
      return _genConfigService.ExportAsync(queryDto);
    }

    /// <summary>
    /// 导入代码生成配置
    /// </summary>
    [HttpPost("import")]
    public Task<LeanExcelImportResult<LeanGenConfigImportDto>> ImportAsync([FromForm] LeanFileInfo file)
    {
      return _genConfigService.ImportAsync(file);
    }

    /// <summary>
    /// 下载导入模板
    /// </summary>
    [HttpGet("template")]
    public Task<LeanFileResult> DownloadTemplateAsync()
    {
      return _genConfigService.DownloadTemplateAsync();
    }

    /// <summary>
    /// 复制配置
    /// </summary>
    [HttpPost("{id}/copy")]
    public Task<LeanGenConfigDto> CopyAsync(long id)
    {
      return _genConfigService.CopyAsync(id);
    }
  }
}