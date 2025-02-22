using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Application.Dtos.Audit;
using Lean.CodeGen.Application.Services.Audit;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.WebApi.Controllers.Audit
{
  /// <summary>
  /// SQL差异日志控制器
  /// </summary>
  [Route("api/sql-diff/logs")]
  [ApiController]
  public class LeanSqlDiffLogController : LeanBaseController
  {
    private readonly ILeanSqlDiffLogService _sqlDiffLogService;

    /// <summary>
    /// 构造函数
    /// </summary>
    public LeanSqlDiffLogController(ILeanSqlDiffLogService sqlDiffLogService)
    {
      _sqlDiffLogService = sqlDiffLogService;
    }

    /// <summary>
    /// 获取SQL差异日志列表（分页）
    /// </summary>
    [HttpGet]
    public Task<LeanPageResult<LeanSqlDiffLogDto>> GetPageListAsync([FromQuery] LeanSqlDiffLogQueryDto queryDto)
    {
      return _sqlDiffLogService.GetPageListAsync(queryDto);
    }

    /// <summary>
    /// 获取SQL差异日志详情
    /// </summary>
    [HttpGet("{id}")]
    public Task<LeanSqlDiffLogDto> GetAsync(long id)
    {
      return _sqlDiffLogService.GetAsync(id);
    }

    /// <summary>
    /// 导出SQL差异日志
    /// </summary>
    [HttpGet("export")]
    public Task<LeanFileResult> ExportAsync([FromQuery] LeanSqlDiffLogQueryDto queryDto)
    {
      return _sqlDiffLogService.ExportAsync(queryDto);
    }

    /// <summary>
    /// 清空SQL差异日志
    /// </summary>
    [HttpDelete("clear")]
    public Task<bool> ClearAsync()
    {
      return _sqlDiffLogService.ClearAsync();
    }
  }
}