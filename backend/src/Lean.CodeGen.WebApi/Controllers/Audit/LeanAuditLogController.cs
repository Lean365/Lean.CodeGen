using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Application.Dtos.Audit;
using Lean.CodeGen.Application.Services.Audit;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Enums;

namespace Lean.CodeGen.WebApi.Controllers.Audit
{
  /// <summary>
  /// 审计日志控制器
  /// </summary>
  [Route("api/audit/logs")]
  [ApiController]
  [ApiExplorerSettings(GroupName = "audit")]
  public class LeanAuditLogController : LeanBaseController
  {
    private readonly ILeanAuditLogService _auditLogService;

    /// <summary>
    /// 构造函数
    /// </summary>
    public LeanAuditLogController(ILeanAuditLogService auditLogService)
    {
      _auditLogService = auditLogService;
    }

    /// <summary>
    /// 获取审计日志列表（分页）
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetPageListAsync([FromQuery] LeanAuditLogQueryDto queryDto)
    {
      var result = await _auditLogService.GetPageListAsync(queryDto);
      return Success(result, LeanBusinessType.Query);
    }

    /// <summary>
    /// 获取审计日志详情
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(long id)
    {
      var result = await _auditLogService.GetAsync(id);
      return Success(result, LeanBusinessType.Query);
    }

    /// <summary>
    /// 导出审计日志
    /// </summary>
    [HttpGet("export")]
    public async Task<IActionResult> ExportAsync([FromQuery] LeanAuditLogQueryDto queryDto)
    {
      var result = await _auditLogService.ExportAsync(queryDto);
      return File(result.Stream, result.ContentType, result.FileName);
    }

    /// <summary>
    /// 清空审计日志
    /// </summary>
    [HttpDelete("clear")]
    public async Task<IActionResult> ClearAsync()
    {
      var result = await _auditLogService.ClearAsync();
      return Success(result, LeanBusinessType.Delete);
    }
  }
}