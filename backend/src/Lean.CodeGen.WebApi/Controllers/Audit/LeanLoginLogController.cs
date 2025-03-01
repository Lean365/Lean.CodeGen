using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Application.Dtos.Audit;
using Lean.CodeGen.Application.Services.Audit;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Enums;

namespace Lean.CodeGen.WebApi.Controllers.Audit
{
  /// <summary>
  /// 登录日志控制器
  /// </summary>
  [Route("api/login/logs")]
  [ApiController]
  [ApiExplorerSettings(GroupName = "audit")]
  public class LeanLoginLogController : LeanBaseController
  {
    private readonly ILeanLoginLogService _loginLogService;

    /// <summary>
    /// 构造函数
    /// </summary>
    public LeanLoginLogController(ILeanLoginLogService loginLogService)
    {
      _loginLogService = loginLogService;
    }

    /// <summary>
    /// 获取登录日志列表（分页）
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetPageListAsync([FromQuery] LeanLoginLogQueryDto queryDto)
    {
      var result = await _loginLogService.GetPageListAsync(queryDto);
      return Success(result, LeanBusinessType.Query);
    }

    /// <summary>
    /// 获取登录日志详情
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(long id)
    {
      var result = await _loginLogService.GetAsync(id);
      return Success(result, LeanBusinessType.Query);
    }

    /// <summary>
    /// 导出登录日志
    /// </summary>
    [HttpGet("export")]
    public async Task<IActionResult> ExportAsync([FromQuery] LeanLoginLogQueryDto queryDto)
    {
      var result = await _loginLogService.ExportAsync(queryDto);
      return File(result.Stream, result.ContentType, result.FileName);
    }

    /// <summary>
    /// 清空登录日志
    /// </summary>
    [HttpDelete("clear")]
    public async Task<IActionResult> ClearAsync()
    {
      var result = await _loginLogService.ClearAsync();
      return Success(result, LeanBusinessType.Delete);
    }
  }
}