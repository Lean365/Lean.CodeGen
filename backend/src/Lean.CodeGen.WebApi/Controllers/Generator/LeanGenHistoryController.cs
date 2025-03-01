using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Application.Dtos.Generator;
using Lean.CodeGen.Application.Services.Generator;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Enums;
using System.IO;

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
    public async Task<IActionResult> GetPageListAsync([FromQuery] LeanGenHistoryQueryDto queryDto)
    {
      var result = await _historyService.GetPageListAsync(queryDto);
      return Success(result, LeanBusinessType.Query);
    }

    /// <summary>
    /// 获取代码生成历史详情
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(long id)
    {
      var result = await _historyService.GetAsync(id);
      return Success(result, LeanBusinessType.Query);
    }

    /// <summary>
    /// 导出代码生成历史
    /// </summary>
    [HttpGet("export")]
    public async Task<IActionResult> ExportAsync([FromQuery] LeanGenHistoryQueryDto queryDto)
    {
      var result = await _historyService.ExportAsync(queryDto);
      var stream = new MemoryStream();
      result.Stream.CopyTo(stream);
      return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "gen-histories.xlsx");
    }

    /// <summary>
    /// 清空历史记录
    /// </summary>
    [HttpDelete("clear")]
    public async Task<IActionResult> ClearAsync()
    {
      var result = await _historyService.ClearAsync();
      return result ? Success(LeanBusinessType.Delete) : Error("清空失败");
    }
  }
}