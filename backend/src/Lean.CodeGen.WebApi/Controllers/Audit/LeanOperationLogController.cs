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
  /// 操作日志控制器
  /// </summary>
  [ApiController]
  [Route("api/[controller]")]
  [ApiExplorerSettings(GroupName = "audit")]
  public class LeanOperationLogController : LeanBaseController
  {
    private readonly ILeanOperationLogService _operationLogService;

    /// <summary>
    /// 构造函数
    /// </summary>
    public LeanOperationLogController(
        ILeanOperationLogService operationLogService,
        ILeanLocalizationService localizationService,
        IConfiguration configuration,
        ILeanUserContext userContext)
        : base(localizationService, configuration, userContext)
    {
      _operationLogService = operationLogService;
    }

    /// <summary>
    /// 获取操作日志列表（分页）
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetPageListAsync([FromQuery] LeanOperationLogQueryDto queryDto)
    {
      var result = await _operationLogService.GetPageListAsync(queryDto);
      return Success(result, LeanBusinessType.Query);
    }

    /// <summary>
    /// 获取操作日志详情
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(long id)
    {
      var result = await _operationLogService.GetAsync(id);
      return Success(result, LeanBusinessType.Query);
    }

    /// <summary>
    /// 导出操作日志
    /// </summary>
    [HttpGet("export")]
    public async Task<IActionResult> ExportAsync([FromQuery] LeanOperationLogQueryDto queryDto)
    {
      var result = await _operationLogService.ExportAsync(queryDto);
      return File(result.Stream, result.ContentType, result.FileName);
    }

    /// <summary>
    /// 清空操作日志
    /// </summary>
    [HttpDelete("clear")]
    public async Task<IActionResult> ClearAsync()
    {
      var result = await _operationLogService.ClearAsync();
      return result ? Success(LeanBusinessType.Delete) : await ErrorAsync("audit.error.clear_failed");
    }
  }
}