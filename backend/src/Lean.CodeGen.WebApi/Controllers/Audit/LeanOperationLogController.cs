using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Application.Dtos.Audit;
using Lean.CodeGen.Application.Services.Audit;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.WebApi.Controllers.Audit
{
  /// <summary>
  /// 操作日志控制器
  /// </summary>
  [Route("api/operation/logs")]
  [ApiController]
  [ApiExplorerSettings(GroupName = "audit")]
  public class LeanOperationLogController : LeanBaseController
  {
    private readonly ILeanOperationLogService _operationLogService;

    /// <summary>
    /// 构造函数
    /// </summary>
    public LeanOperationLogController(ILeanOperationLogService operationLogService)
    {
      _operationLogService = operationLogService;
    }

    /// <summary>
    /// 获取操作日志列表（分页）
    /// </summary>
    [HttpGet]
    public Task<LeanPageResult<LeanOperationLogDto>> GetPageListAsync([FromQuery] LeanOperationLogQueryDto queryDto)
    {
      return _operationLogService.GetPageListAsync(queryDto);
    }

    /// <summary>
    /// 获取操作日志详情
    /// </summary>
    [HttpGet("{id}")]
    public Task<LeanOperationLogDto> GetAsync(long id)
    {
      return _operationLogService.GetAsync(id);
    }

    /// <summary>
    /// 导出操作日志
    /// </summary>
    [HttpGet("export")]
    public Task<LeanFileResult> ExportAsync([FromQuery] LeanOperationLogQueryDto queryDto)
    {
      return _operationLogService.ExportAsync(queryDto);
    }

    /// <summary>
    /// 清空操作日志
    /// </summary>
    [HttpDelete("clear")]
    public Task<bool> ClearAsync()
    {
      return _operationLogService.ClearAsync();
    }
  }
}