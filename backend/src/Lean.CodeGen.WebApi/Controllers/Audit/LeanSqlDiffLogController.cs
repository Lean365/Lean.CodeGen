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
  /// SQL差异日志控制器
  /// </summary>
  [ApiController]
  [Route("api/[controller]")]
  [ApiExplorerSettings(GroupName = "audit")]
  public class LeanSqlDiffLogController : LeanBaseController
  {
    private readonly ILeanSqlDiffLogService _sqlDiffLogService;

    /// <summary>
    /// 构造函数
    /// </summary>
    public LeanSqlDiffLogController(
        ILeanSqlDiffLogService sqlDiffLogService,
        ILeanLocalizationService localizationService,
        IConfiguration configuration,
        ILeanUserContext userContext)
        : base(localizationService, configuration, userContext)
    {
      _sqlDiffLogService = sqlDiffLogService;
    }

    /// <summary>
    /// 获取SQL差异日志列表（分页）
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetPageListAsync([FromQuery] LeanSqlDiffLogQueryDto queryDto)
    {
      var result = await _sqlDiffLogService.GetPageListAsync(queryDto);
      return Success(result, LeanBusinessType.Query);
    }

    /// <summary>
    /// 获取SQL差异日志详情
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(long id)
    {
      var result = await _sqlDiffLogService.GetAsync(id);
      return Success(result, LeanBusinessType.Query);
    }

    /// <summary>
    /// 导出SQL差异日志
    /// </summary>
    [HttpGet("export")]
    public async Task<IActionResult> ExportAsync([FromQuery] LeanSqlDiffLogQueryDto queryDto)
    {
      var result = await _sqlDiffLogService.ExportAsync(queryDto);
      return File(result.Stream, result.ContentType, result.FileName);
    }

    /// <summary>
    /// 清空SQL差异日志
    /// </summary>
    [HttpDelete("clear")]
    public async Task<IActionResult> ClearAsync()
    {
      var result = await _sqlDiffLogService.ClearAsync();
      return Success(result, LeanBusinessType.Delete);
    }
  }
}