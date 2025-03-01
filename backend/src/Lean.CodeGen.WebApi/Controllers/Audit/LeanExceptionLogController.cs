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
  /// 异常日志控制器
  /// </summary>
  [Route("api/exception/logs")]
  [ApiController]
  [ApiExplorerSettings(GroupName = "audit")]
  public class LeanExceptionLogController : LeanBaseController
  {
    private readonly ILeanExceptionLogService _exceptionLogService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="exceptionLogService">异常日志服务</param>
    /// <param name="localizationService">本地化服务</param>
    /// <param name="configuration">配置</param>
    public LeanExceptionLogController(
        ILeanExceptionLogService exceptionLogService,
        ILeanLocalizationService localizationService,
        IConfiguration configuration)
        : base(localizationService, configuration)
    {
      _exceptionLogService = exceptionLogService;
    }

    /// <summary>
    /// 获取异常日志列表（分页）
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetPageListAsync([FromQuery] LeanExceptionLogQueryDto queryDto)
    {
      var result = await _exceptionLogService.GetPageListAsync(queryDto);
      return Success(result, LeanBusinessType.Query);
    }

    /// <summary>
    /// 获取异常日志详情
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(long id)
    {
      var result = await _exceptionLogService.GetAsync(id);
      return Success(result, LeanBusinessType.Query);
    }

    /// <summary>
    /// 导出异常日志
    /// </summary>
    [HttpGet("export")]
    public async Task<IActionResult> ExportAsync([FromQuery] LeanExceptionLogQueryDto queryDto)
    {
      var result = await _exceptionLogService.ExportAsync(queryDto);
      return File(result.Stream, result.ContentType, result.FileName);
    }

    /// <summary>
    /// 清空异常日志
    /// </summary>
    [HttpDelete("clear")]
    public async Task<IActionResult> ClearAsync()
    {
      var result = await _exceptionLogService.ClearAsync();
      return Success(result, LeanBusinessType.Delete);
    }

    /// <summary>
    /// 处理异常日志
    /// </summary>
    [HttpPost("{id}/handle")]
    public async Task<IActionResult> HandleAsync(LeanHandleExceptionLogDto handleDto)
    {
      var result = await _exceptionLogService.HandleAsync(handleDto);
      return Success(result, LeanBusinessType.Update);
    }
  }
}