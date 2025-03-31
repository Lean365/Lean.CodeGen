using Lean.CodeGen.Application.Dtos.Workflow;
using Lean.CodeGen.Application.Services.Workflow;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Common.Enums;
using Microsoft.Extensions.Configuration;
using Lean.CodeGen.Application.Services.Admin;

namespace Lean.CodeGen.WebApi.Controllers.Workflow;

/// <summary>
/// 工作流结果控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "workflow")]
public class LeanWorkflowOutcomeController : LeanBaseController
{
  private readonly ILeanWorkflowOutcomeService _service;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="service">工作流结果服务</param>
  /// <param name="localizationService">本地化服务</param>
  /// <param name="configuration">配置</param>
  /// <param name="userContext">用户上下文</param>
  public LeanWorkflowOutcomeController(
      ILeanWorkflowOutcomeService service,
      ILeanLocalizationService localizationService,
      IConfiguration configuration,
      ILeanUserContext userContext)
      : base(localizationService, configuration, userContext)
  {
    _service = service;
  }

  /// <summary>
  /// 获取结果记录
  /// </summary>
  /// <param name="id">结果ID</param>
  /// <returns>结果记录</returns>
  [HttpGet("{id}")]
  public async Task<IActionResult> GetAsync(long id)
  {
    var result = await _service.GetAsync(id);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 创建结果记录
  /// </summary>
  /// <param name="dto">结果记录</param>
  /// <returns>结果ID</returns>
  [HttpPost]
  public async Task<IActionResult> CreateAsync(LeanWorkflowOutcomeDto dto)
  {
    var result = await _service.CreateAsync(dto);
    return Success(result, LeanBusinessType.Create);
  }

  /// <summary>
  /// 更新结果记录
  /// </summary>
  /// <param name="id">结果ID</param>
  /// <param name="dto">结果记录</param>
  /// <returns>是否成功</returns>
  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateAsync(long id, LeanWorkflowOutcomeDto dto)
  {
    if (id != dto.Id)
    {
      return await ErrorAsync("workflow.error.id_mismatch");
    }
    var result = await _service.UpdateAsync(dto);
    return Success(result, LeanBusinessType.Update);
  }

  /// <summary>
  /// 删除结果记录
  /// </summary>
  /// <param name="id">结果ID</param>
  /// <returns>是否成功</returns>
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteAsync(long id)
  {
    var result = await _service.DeleteAsync(id);
    return result ? Success(LeanBusinessType.Delete) : await ErrorAsync("workflow.error.delete_failed");
  }

  /// <summary>
  /// 分页查询结果记录
  /// </summary>
  /// <param name="pageIndex">页码</param>
  /// <param name="pageSize">每页大小</param>
  /// <param name="activityInstanceId">活动实例ID</param>
  /// <param name="outcomeName">结果名称</param>
  /// <param name="outcomeType">结果类型</param>
  /// <returns>分页结果</returns>
  [HttpGet]
  public async Task<IActionResult> GetPagedListAsync(
      [FromQuery] int pageIndex = 1,
      [FromQuery] int pageSize = 10,
      [FromQuery] long? activityInstanceId = null,
      [FromQuery] string? outcomeName = null,
      [FromQuery] string? outcomeType = null)
  {
    var result = await _service.GetPagedListAsync(pageIndex, pageSize, activityInstanceId, outcomeName, outcomeType);
    return Success(result, LeanBusinessType.Query);
  }
}