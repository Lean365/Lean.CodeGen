using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Application.Dtos.Audit;
using Lean.CodeGen.Application.Services.Audit;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Enums;
using Microsoft.Extensions.Configuration;
using Lean.CodeGen.Application.Services.Admin;

namespace Lean.CodeGen.WebApi.Controllers.Audit
{
  /// <summary>
  /// 定时任务日志控制器
  /// </summary>
  [ApiController]
  [Route("api/[controller]")]
  [ApiExplorerSettings(GroupName = "audit")]
  public class LeanQuartzLogController : LeanBaseController
  {
    private readonly ILeanQuartzLogService _quartzLogService;

    /// <summary>
    /// 构造函数
    /// </summary>
    public LeanQuartzLogController(
        ILeanQuartzLogService quartzLogService,
        ILeanLocalizationService localizationService,
        IConfiguration configuration)
        : base(localizationService, configuration)
    {
      _quartzLogService = quartzLogService;
    }

    /// <summary>
    /// 获取定时任务日志列表（分页）
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetPageListAsync([FromQuery] LeanQuartzLogQueryDto queryDto)
    {
      var result = await _quartzLogService.GetPageListAsync(queryDto);
      return Success(result, LeanBusinessType.Query);
    }

    /// <summary>
    /// 获取定时任务日志详情
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(long id)
    {
      var result = await _quartzLogService.GetAsync(id);
      return Success(result, LeanBusinessType.Query);
    }

    /// <summary>
    /// 导出定时任务日志
    /// </summary>
    [HttpGet("export")]
    public async Task<IActionResult> ExportAsync([FromQuery] LeanQuartzLogQueryDto queryDto)
    {
      var result = await _quartzLogService.ExportAsync(queryDto);
      return File(result.Stream, result.ContentType, result.FileName);
    }

    /// <summary>
    /// 清空定时任务日志
    /// </summary>
    [HttpDelete("clear")]
    public async Task<IActionResult> ClearAsync()
    {
      var result = await _quartzLogService.ClearAsync();
      return Success(result, LeanBusinessType.Delete);
    }
  }
}