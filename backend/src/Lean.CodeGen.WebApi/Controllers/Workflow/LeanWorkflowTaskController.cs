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
/// 工作流任务控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "workflow")]
public class LeanWorkflowTaskController : LeanBaseController
{
  private readonly ILeanWorkflowTaskService _service;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="service">工作流任务服务</param>
  /// <param name="localizationService">本地化服务</param>
  /// <param name="configuration">配置</param>
  /// <param name="userContext">用户上下文</param>
  public LeanWorkflowTaskController(
      ILeanWorkflowTaskService service,
      ILeanLocalizationService localizationService,
      IConfiguration configuration,
      ILeanUserContext userContext)
      : base(localizationService, configuration, userContext)
  {
    _service = service;
  }

  /// <summary>
  /// 获取工作流任务
  /// </summary>
  /// <param name="id">任务ID</param>
  /// <returns>工作流任务</returns>
  [HttpGet("{id}")]
  public async Task<IActionResult> GetAsync(long id)
  {
    var result = await _service.GetAsync(id);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 创建工作流任务
  /// </summary>
  /// <param name="dto">创建参数</param>
  /// <returns>任务ID</returns>
  [HttpPost]
  public async Task<IActionResult> CreateAsync(LeanWorkflowTaskDto dto)
  {
    var result = await _service.CreateAsync(dto);
    return Success(result, LeanBusinessType.Create);
  }

  /// <summary>
  /// 更新工作流任务
  /// </summary>
  /// <param name="id">任务ID</param>
  /// <param name="dto">更新参数</param>
  /// <returns>是否成功</returns>
  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateAsync(long id, LeanWorkflowTaskDto dto)
  {
    if (id != dto.Id)
    {
      return await ErrorAsync("workflow.error.id_mismatch");
    }
    var result = await _service.UpdateAsync(dto);
    return Success(result, LeanBusinessType.Update);
  }

  /// <summary>
  /// 删除工作流任务
  /// </summary>
  /// <param name="id">任务ID</param>
  /// <returns>是否成功</returns>
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteAsync(long id)
  {
    var result = await _service.DeleteAsync(id);
    return Success(result, LeanBusinessType.Delete);
  }

  /// <summary>
  /// 完成工作流任务
  /// </summary>
  /// <param name="id">任务ID</param>
  /// <param name="comment">审批意见</param>
  /// <returns>是否成功</returns>
  [HttpPost("{id}/complete")]
  public async Task<IActionResult> CompleteAsync(long id, [FromQuery] string? comment = null)
  {
    var result = await _service.CompleteAsync(id, comment);
    return Success(result, LeanBusinessType.Update);
  }

  /// <summary>
  /// 驳回工作流任务
  /// </summary>
  /// <param name="id">任务ID</param>
  /// <param name="comment">审批意见</param>
  /// <returns>是否成功</returns>
  [HttpPost("{id}/reject")]
  public async Task<IActionResult> RejectAsync(long id, [FromQuery] string? comment = null)
  {
    var result = await _service.RejectAsync(id, comment);
    return Success(result, LeanBusinessType.Update);
  }

  /// <summary>
  /// 转办工作流任务
  /// </summary>
  /// <param name="id">任务ID</param>
  /// <param name="assigneeId">转办人ID</param>
  /// <param name="comment">审批意见</param>
  /// <returns>是否成功</returns>
  [HttpPost("{id}/transfer/{assigneeId}")]
  public async Task<IActionResult> TransferAsync(long id, long assigneeId, [FromQuery] string? comment = null)
  {
    var result = await _service.TransferAsync(id, assigneeId, comment);
    return Success(result, LeanBusinessType.Update);
  }

  /// <summary>
  /// 委派工作流任务
  /// </summary>
  /// <param name="id">任务ID</param>
  /// <param name="assigneeId">委派人ID</param>
  /// <param name="comment">审批意见</param>
  /// <returns>是否成功</returns>
  [HttpPost("{id}/delegate/{assigneeId}")]
  public async Task<IActionResult> DelegateAsync(long id, long assigneeId, [FromQuery] string? comment = null)
  {
    var result = await _service.DelegateAsync(id, assigneeId, comment);
    return Success(result, LeanBusinessType.Update);
  }

  /// <summary>
  /// 分页查询工作流任务
  /// </summary>
  /// <param name="pageIndex">页码</param>
  /// <param name="pageSize">每页大小</param>
  /// <param name="instanceId">实例ID</param>
  /// <param name="taskType">任务类型</param>
  /// <param name="taskNode">任务节点</param>
  /// <param name="priority">优先级</param>
  /// <param name="assigneeId">办理人ID</param>
  /// <param name="taskStatus">任务状态</param>
  /// <param name="startTime">开始时间</param>
  /// <param name="endTime">结束时间</param>
  /// <returns>分页结果</returns>
  [HttpGet]
  public async Task<IActionResult> GetPagedListAsync(
      [FromQuery] int pageIndex = 1,
      [FromQuery] int pageSize = 10,
      [FromQuery] long? instanceId = null,
      [FromQuery] string? taskType = null,
      [FromQuery] string? taskNode = null,
      [FromQuery] int? priority = null,
      [FromQuery] long? assigneeId = null,
      [FromQuery] int? taskStatus = null,
      [FromQuery] DateTime? startTime = null,
      [FromQuery] DateTime? endTime = null)
  {
    var result = await _service.GetPagedListAsync(pageIndex, pageSize, instanceId, taskType, taskNode, priority, assigneeId, taskStatus, startTime, endTime);
    return Success(result, LeanBusinessType.Query);
  }
}