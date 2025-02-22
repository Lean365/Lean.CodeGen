using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Application.Dtos.Audit;
using Lean.CodeGen.Application.Services.Audit;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.WebApi.Controllers.Audit
{
  /// <summary>
  /// 异常日志控制器
  /// </summary>
  [Route("api/exception/logs")]
  [ApiController]
  public class LeanExceptionLogController : LeanBaseController
  {
    private readonly ILeanExceptionLogService _exceptionLogService;

    /// <summary>
    /// 构造函数
    /// </summary>
    public LeanExceptionLogController(ILeanExceptionLogService exceptionLogService)
    {
      _exceptionLogService = exceptionLogService;
    }

    /// <summary>
    /// 获取异常日志列表（分页）
    /// </summary>
    [HttpGet]
    public Task<LeanPageResult<LeanExceptionLogDto>> GetPageListAsync([FromQuery] LeanExceptionLogQueryDto queryDto)
    {
      return _exceptionLogService.GetPageListAsync(queryDto);
    }

    /// <summary>
    /// 获取异常日志详情
    /// </summary>
    [HttpGet("{id}")]
    public Task<LeanExceptionLogDto> GetAsync(long id)
    {
      return _exceptionLogService.GetAsync(id);
    }

    /// <summary>
    /// 导出异常日志
    /// </summary>
    [HttpGet("export")]
    public Task<LeanFileResult> ExportAsync([FromQuery] LeanExceptionLogQueryDto queryDto)
    {
      return _exceptionLogService.ExportAsync(queryDto);
    }

    /// <summary>
    /// 清空异常日志
    /// </summary>
    [HttpDelete("clear")]
    public Task<bool> ClearAsync()
    {
      return _exceptionLogService.ClearAsync();
    }

    /// <summary>
    /// 处理异常日志
    /// </summary>
    [HttpPost("{id}/handle")]
    public Task<bool> HandleAsync(LeanHandleExceptionLogDto handleDto)
    {
      return _exceptionLogService.HandleAsync(handleDto);
    }
  }
}