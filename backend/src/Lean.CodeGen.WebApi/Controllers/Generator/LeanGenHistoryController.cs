using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Application.Dtos.Generator;
using Lean.CodeGen.Application.Services.Generator;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.WebApi.Controllers;

namespace Lean.CodeGen.WebApi.Controllers.Generator
{
  /// <summary>
  /// 代码生成历史控制器
  /// </summary>
  [Route("api/generator/histories")]
  [ApiController]
  public class LeanGenHistoryController : LeanBaseController
  {
    private readonly ILeanGenHistoryService _historyService;

    /// <summary>
    /// 构造函数
    /// </summary>
    public LeanGenHistoryController(ILeanGenHistoryService historyService)
    {
      _historyService = historyService;
    }

    /// <summary>
    /// 获取代码生成历史列表（分页）
    /// </summary>
    [HttpGet]
    public Task<LeanPageResult<LeanGenHistoryDto>> GetPageListAsync([FromQuery] LeanGenHistoryQueryDto queryDto)
    {
      return _historyService.GetPageListAsync(queryDto);
    }

    /// <summary>
    /// 获取代码生成历史详情
    /// </summary>
    [HttpGet("{id}")]
    public Task<LeanGenHistoryDto> GetAsync(long id)
    {
      return _historyService.GetAsync(id);
    }

    /// <summary>
    /// 导出代码生成历史
    /// </summary>
    [HttpGet("export")]
    public Task<LeanFileResult> ExportAsync([FromQuery] LeanGenHistoryQueryDto queryDto)
    {
      return _historyService.ExportAsync(queryDto);
    }

    /// <summary>
    /// 清空历史记录
    /// </summary>
    [HttpDelete("clear")]
    public Task<bool> ClearAsync()
    {
      return _historyService.ClearAsync();
    }
  }
}