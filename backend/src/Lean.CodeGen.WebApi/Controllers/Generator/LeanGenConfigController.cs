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
    public async Task<LeanApiResult<LeanPageResult<LeanGenConfigDto>>> GetPageListAsync([FromQuery] LeanGenConfigQueryDto queryDto)
    {
      var result = await _genConfigService.GetPageListAsync(queryDto);
      return LeanApiResult<LeanPageResult<LeanGenConfigDto>>.Ok(result);
    }

    /// <summary>
    /// 获取代码生成配置详情
    /// </summary>
    [HttpGet("{id}")]
    public async Task<LeanApiResult<LeanGenConfigDto>> GetAsync(long id)
    {
      var result = await _genConfigService.GetAsync(id);
      return LeanApiResult<LeanGenConfigDto>.Ok(result);
    }

    /// <summary>
    /// 创建代码生成配置
    /// </summary>
    [HttpPost]
    public async Task<LeanApiResult<LeanGenConfigDto>> CreateAsync([FromBody] LeanCreateGenConfigDto createDto)
    {
      var result = await _genConfigService.CreateAsync(createDto);
      return LeanApiResult<LeanGenConfigDto>.Ok(result);
    }

    /// <summary>
    /// 更新代码生成配置
    /// </summary>
    [HttpPut("{id}")]
    public async Task<LeanApiResult<LeanGenConfigDto>> UpdateAsync(long id, [FromBody] LeanUpdateGenConfigDto updateDto)
    {
      var result = await _genConfigService.UpdateAsync(id, updateDto);
      return LeanApiResult<LeanGenConfigDto>.Ok(result);
    }

    /// <summary>
    /// 删除代码生成配置
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<LeanApiResult<bool>> DeleteAsync(long id)
    {
      var result = await _genConfigService.DeleteAsync(id);
      return LeanApiResult<bool>.Ok(result);
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