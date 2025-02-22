using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Application.Dtos.Audit;
using Lean.CodeGen.Application.Services.Audit;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.WebApi.Controllers.Audit
{
  /// <summary>
  /// 审计日志控制器
  /// </summary>
  [Route("api/audit/logs")]
  [ApiController]
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
    public Task<LeanPageResult<LeanAuditLogDto>> GetPageListAsync([FromQuery] LeanAuditLogQueryDto queryDto)
    {
      return _auditLogService.GetPageListAsync(queryDto);
    }

    /// <summary>
    /// 获取审计日志详情
    /// </summary>
    [HttpGet("{id}")]
    public Task<LeanAuditLogDto> GetAsync(long id)
    {
      return _auditLogService.GetAsync(id);
    }

    /// <summary>
    /// 导出审计日志
    /// </summary>
    [HttpGet("export")]
    public Task<LeanFileResult> ExportAsync([FromQuery] LeanAuditLogQueryDto queryDto)
    {
      return _auditLogService.ExportAsync(queryDto);
    }

    /// <summary>
    /// 清空审计日志
    /// </summary>
    [HttpDelete("clear")]
    public Task<bool> ClearAsync()
    {
      return _auditLogService.ClearAsync();
    }
  }
}