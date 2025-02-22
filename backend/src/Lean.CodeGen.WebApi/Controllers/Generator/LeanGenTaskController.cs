using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Application.Dtos.Generator;
using Lean.CodeGen.Application.Services.Generator;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Excel;
using Lean.CodeGen.WebApi.Controllers;

namespace Lean.CodeGen.WebApi.Controllers.Generator
{
  /// <summary>
  /// 代码生成任务控制器
  /// </summary>
  [Route("api/generator/tasks")]
  [ApiController]
  public class LeanGenTaskController : LeanBaseController
  {
    private readonly ILeanGenTaskService _genTaskService;

    /// <summary>
    /// 构造函数
    /// </summary>
    public LeanGenTaskController(ILeanGenTaskService genTaskService)
    {
      _genTaskService = genTaskService;
    }

    /// <summary>
    /// 获取代码生成任务列表（分页）
    /// </summary>
    [HttpGet]
    public Task<LeanPageResult<LeanGenTaskDto>> GetPageListAsync([FromQuery] LeanGenTaskQueryDto queryDto)
    {
      return _genTaskService.GetPageListAsync(queryDto);
    }

    /// <summary>
    /// 获取代码生成任务详情
    /// </summary>
    [HttpGet("{id}")]
    public Task<LeanGenTaskDto> GetAsync(long id)
    {
      return _genTaskService.GetAsync(id);
    }

    /// <summary>
    /// 创建代码生成任务
    /// </summary>
    [HttpPost]
    public Task<LeanGenTaskDto> CreateAsync([FromBody] LeanCreateGenTaskDto createDto)
    {
      return _genTaskService.CreateAsync(createDto);
    }

    /// <summary>
    /// 更新代码生成任务
    /// </summary>
    [HttpPut("{id}")]
    public Task<LeanGenTaskDto> UpdateAsync(long id, [FromBody] LeanUpdateGenTaskDto updateDto)
    {
      return _genTaskService.UpdateAsync(id, updateDto);
    }

    /// <summary>
    /// 删除代码生成任务
    /// </summary>
    [HttpDelete("{id}")]
    public Task<bool> DeleteAsync(long id)
    {
      return _genTaskService.DeleteAsync(id);
    }

    /// <summary>
    /// 导出代码生成任务
    /// </summary>
    [HttpGet("export")]
    public Task<LeanFileResult> ExportAsync([FromQuery] LeanGenTaskQueryDto queryDto)
    {
      return _genTaskService.ExportAsync(queryDto);
    }

    /// <summary>
    /// 导入代码生成任务
    /// </summary>
    [HttpPost("import")]
    public Task<LeanExcelImportResult<LeanGenTaskImportDto>> ImportAsync([FromForm] LeanFileInfo file)
    {
      return _genTaskService.ImportAsync(file);
    }

    /// <summary>
    /// 下载导入模板
    /// </summary>
    [HttpGet("template")]
    public Task<LeanFileResult> DownloadTemplateAsync()
    {
      return _genTaskService.DownloadTemplateAsync();
    }

    /// <summary>
    /// 启动任务
    /// </summary>
    [HttpPost("{id}/start")]
    public Task<bool> StartAsync(long id)
    {
      return _genTaskService.StartAsync(id);
    }

    /// <summary>
    /// 停止任务
    /// </summary>
    [HttpPost("{id}/stop")]
    public Task<bool> StopAsync(long id)
    {
      return _genTaskService.StopAsync(id);
    }

    /// <summary>
    /// 重试任务
    /// </summary>
    [HttpPost("{id}/retry")]
    public Task<bool> RetryAsync(long id)
    {
      return _genTaskService.RetryAsync(id);
    }

    /// <summary>
    /// 预览代码
    /// </summary>
    [HttpPost("preview")]
    public Task<LeanGenPreviewResultDto> PreviewAsync([FromBody] LeanGenPreviewRequestDto requestDto)
    {
      return _genTaskService.PreviewAsync(requestDto);
    }

    /// <summary>
    /// 下载代码
    /// </summary>
    [HttpPost("download")]
    public Task<LeanGenDownloadResultDto> DownloadAsync([FromBody] LeanGenDownloadRequestDto requestDto)
    {
      return _genTaskService.DownloadAsync(requestDto);
    }
  }
}