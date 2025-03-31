using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Application.Dtos.Generator;
using Lean.CodeGen.Application.Services.Generator;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Excel;
using Lean.CodeGen.Common.Enums;
using System.IO;
using Microsoft.Extensions.Configuration;
using Lean.CodeGen.Application.Services.Admin;

namespace Lean.CodeGen.WebApi.Controllers.Generator
{
  /// <summary>
  /// 代码生成任务控制器
  /// </summary>
  [ApiController]
  [Route("api/[controller]")]
  [ApiExplorerSettings(GroupName = "generator")]
  public class LeanGenTaskController : LeanBaseController
  {
    private readonly ILeanGenTaskService _genTaskService;

    /// <summary>
    /// 构造函数
    /// </summary>
    public LeanGenTaskController(
        ILeanGenTaskService genTaskService,
    ILeanLocalizationService localizationService,
    IConfiguration configuration,
    ILeanUserContext userContext)
    : base(localizationService, configuration, userContext)
    {
      _genTaskService = genTaskService;
    }

    /// <summary>
    /// 获取代码生成任务列表（分页）
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetPageListAsync([FromQuery] LeanGenTaskQueryDto queryDto)
    {
      var result = await _genTaskService.GetPageListAsync(queryDto);
      return Success(result, LeanBusinessType.Query);
    }

    /// <summary>
    /// 获取代码生成任务详情
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(long id)
    {
      var result = await _genTaskService.GetAsync(id);
      return Success(result, LeanBusinessType.Query);
    }

    /// <summary>
    /// 创建代码生成任务
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] LeanCreateGenTaskDto createDto)
    {
      var result = await _genTaskService.CreateAsync(createDto);
      return Success(result, LeanBusinessType.Create);
    }

    /// <summary>
    /// 更新代码生成任务
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(long id, [FromBody] LeanUpdateGenTaskDto updateDto)
    {
      var result = await _genTaskService.UpdateAsync(id, updateDto);
      return Success(result, LeanBusinessType.Update);
    }

    /// <summary>
    /// 删除代码生成任务
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(long id)
    {
      var result = await _genTaskService.DeleteAsync(id);
      return result ? Success(LeanBusinessType.Delete) : await ErrorAsync("generator.error.delete_failed");
    }

    /// <summary>
    /// 导出代码生成任务
    /// </summary>
    [HttpGet("export")]
    public async Task<IActionResult> ExportAsync([FromQuery] LeanGenTaskQueryDto queryDto)
    {
      var result = await _genTaskService.ExportAsync(queryDto);
      var stream = new MemoryStream();
      result.Stream.CopyTo(stream);
      return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "gen-tasks.xlsx");
    }

    /// <summary>
    /// 导入代码生成任务
    /// </summary>
    [HttpPost("import")]
    public async Task<IActionResult> ImportAsync([FromForm] LeanFileInfo file)
    {
      var result = await _genTaskService.ImportAsync(file);
      return Success(result, LeanBusinessType.Import);
    }

    /// <summary>
    /// 下载导入模板
    /// </summary>
    [HttpGet("template")]
    public async Task<IActionResult> DownloadTemplateAsync()
    {
      var result = await _genTaskService.DownloadTemplateAsync();
      var stream = new MemoryStream();
      result.Stream.CopyTo(stream);
      return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "gen-task-template.xlsx");
    }

    /// <summary>
    /// 启动任务
    /// </summary>
    [HttpPost("{id}/start")]
    public async Task<IActionResult> StartAsync(long id)
    {
      var result = await _genTaskService.StartAsync(id);
      return result ? Success(LeanBusinessType.Other) : await ErrorAsync("generator.error.start_failed");
    }

    /// <summary>
    /// 停止任务
    /// </summary>
    [HttpPost("{id}/stop")]
    public async Task<IActionResult> StopAsync(long id)
    {
      var result = await _genTaskService.StopAsync(id);
      return result ? Success(LeanBusinessType.Other) : await ErrorAsync("generator.error.stop_failed");
    }

    /// <summary>
    /// 重试任务
    /// </summary>
    [HttpPost("{id}/retry")]
    public async Task<IActionResult> RetryAsync(long id)
    {
      var result = await _genTaskService.RetryAsync(id);
      return result ? Success(LeanBusinessType.Other) : await ErrorAsync("generator.error.retry_failed");
    }

    /// <summary>
    /// 预览代码
    /// </summary>
    [HttpPost("preview")]
    public async Task<IActionResult> PreviewAsync([FromBody] LeanGenPreviewRequestDto requestDto)
    {
      var result = await _genTaskService.PreviewAsync(requestDto);
      return Success(result, LeanBusinessType.Query);
    }

    /// <summary>
    /// 下载代码
    /// </summary>
    [HttpPost("download")]
    public async Task<IActionResult> DownloadAsync([FromBody] LeanGenDownloadRequestDto requestDto)
    {
      var result = await _genTaskService.DownloadAsync(requestDto);
      return Success(result, LeanBusinessType.Other);
    }
  }
}