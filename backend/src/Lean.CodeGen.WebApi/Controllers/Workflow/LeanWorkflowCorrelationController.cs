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
/// 工作流关联控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "workflow")]
public class LeanWorkflowCorrelationController : LeanBaseController
{
  private readonly ILeanWorkflowCorrelationService _service;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="service">工作流关联服务</param>
  /// <param name="localizationService">本地化服务</param>
  /// <param name="configuration">配置</param>
  /// <param name="userContext">用户上下文</param>
  public LeanWorkflowCorrelationController(
      ILeanWorkflowCorrelationService service,
      ILeanLocalizationService localizationService,
      IConfiguration configuration,
      ILeanUserContext userContext)
      : base(localizationService, configuration, userContext)
  {
    _service = service;
  }

  /// <summary>
  /// 获取工作流关联
  /// </summary>
  /// <param name="id">关联ID</param>
  /// <returns>工作流关联</returns>
  [HttpGet("{id}")]
  public async Task<IActionResult> GetAsync(long id)
  {
    var result = await _service.GetAsync(id);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 根据关联键获取工作流关联
  /// </summary>
  /// <param name="correlationId">关联键</param>
  /// <returns>工作流关联</returns>
  [HttpGet("correlation-id/{correlationId}")]
  public async Task<IActionResult> GetByCorrelationIdAsync(string correlationId)
  {
    var result = await _service.GetByCorrelationIdAsync(correlationId);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 创建工作流关联
  /// </summary>
  /// <param name="dto">工作流关联</param>
  /// <returns>关联ID</returns>
  [HttpPost]
  public async Task<IActionResult> CreateAsync(LeanWorkflowCorrelationDto dto)
  {
    var result = await _service.CreateAsync(dto);
    return Success(result, LeanBusinessType.Create);
  }

  /// <summary>
  /// 更新工作流关联
  /// </summary>
  /// <param name="id">关联ID</param>
  /// <param name="dto">工作流关联</param>
  /// <returns>是否成功</returns>
  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateAsync(long id, LeanWorkflowCorrelationDto dto)
  {
    if (id != dto.Id)
    {
      return await ErrorAsync("workflow.error.id_mismatch");
    }
    var result = await _service.UpdateAsync(dto);
    return Success(result, LeanBusinessType.Update);
  }

  /// <summary>
  /// 删除工作流关联
  /// </summary>
  /// <param name="id">关联ID</param>
  /// <returns>是否成功</returns>
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteAsync(long id)
  {
    var result = await _service.DeleteAsync(id);
    return result ? Success(LeanBusinessType.Delete) : await ErrorAsync("workflow.error.delete_failed");
  }

  /// <summary>
  /// 分页查询工作流关联
  /// </summary>
  /// <param name="pageIndex">页码</param>
  /// <param name="pageSize">每页大小</param>
  /// <param name="instanceId">工作流实例ID</param>
  /// <param name="correlationId">关联键</param>
  /// <param name="correlationType">关联类型</param>
  /// <param name="status">关联状态</param>
  /// <param name="startTime">开始时间</param>
  /// <param name="endTime">结束时间</param>
  /// <returns>分页结果</returns>
  [HttpGet]
  public async Task<IActionResult> GetPagedListAsync(
      [FromQuery] int pageIndex = 1,
      [FromQuery] int pageSize = 10,
      [FromQuery] long? instanceId = null,
      [FromQuery] string? correlationId = null,
      [FromQuery] string? correlationType = null,
      [FromQuery] bool? status = null,
      [FromQuery] DateTime? startTime = null,
      [FromQuery] DateTime? endTime = null)
  {
    var result = await _service.GetPagedListAsync(pageIndex, pageSize, instanceId, correlationId, correlationType, status, startTime, endTime);
    return Success(result, LeanBusinessType.Query);
  }
}