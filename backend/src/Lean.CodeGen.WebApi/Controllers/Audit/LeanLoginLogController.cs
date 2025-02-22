using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Application.Dtos.Audit;
using Lean.CodeGen.Application.Services.Audit;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.WebApi.Controllers.Audit
{
  /// <summary>
  /// 登录日志控制器
  /// </summary>
  [Route("api/login/logs")]
  [ApiController]
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
    public Task<LeanPageResult<LeanLoginLogDto>> GetPageListAsync([FromQuery] LeanLoginLogQueryDto queryDto)
    {
      return _loginLogService.GetPageListAsync(queryDto);
    }

    /// <summary>
    /// 获取登录日志详情
    /// </summary>
    [HttpGet("{id}")]
    public Task<LeanLoginLogDto> GetAsync(long id)
    {
      return _loginLogService.GetAsync(id);
    }

    /// <summary>
    /// 导出登录日志
    /// </summary>
    [HttpGet("export")]
    public Task<LeanFileResult> ExportAsync([FromQuery] LeanLoginLogQueryDto queryDto)
    {
      return _loginLogService.ExportAsync(queryDto);
    }

    /// <summary>
    /// 清空登录日志
    /// </summary>
    [HttpDelete("clear")]
    public Task<bool> ClearAsync()
    {
      return _loginLogService.ClearAsync();
    }
  }
}