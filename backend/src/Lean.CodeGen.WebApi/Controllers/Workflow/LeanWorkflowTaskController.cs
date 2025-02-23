using Lean.CodeGen.Application.Dtos.Workflow;
using Lean.CodeGen.Application.Services.Workflow;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Lean.CodeGen.WebApi.Controllers.Workflow;

/// <summary>
/// 工作流任务控制器
/// </summary>
[ApiController]
[Route("api/workflow/tasks")]
public class LeanWorkflowTaskController : LeanBaseController
{
  private readonly ILeanWorkflowTaskService _service;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="service">工作流任务服务</param>
  public LeanWorkflowTaskController(ILeanWorkflowTaskService service)
  {
    _service = service;
  }

  /// <summary>
  /// 获取工作流任务
  /// </summary>
  /// <param name="id">任务ID</param>
  /// <returns>工作流任务</returns>
  [HttpGet("{id}")]
  public async Task<LeanApiResult<LeanWorkflowTaskDto>> GetAsync(long id)
  {
    var result = await _service.GetAsync(id);
    return LeanApiResult<LeanWorkflowTaskDto>.Ok(result);
  }

  /// <summary>
  /// 创建工作流任务
  /// </summary>
  /// <param name="dto">创建参数</param>
  /// <returns>任务ID</returns>
  [HttpPost]
  public async Task<LeanApiResult<long>> CreateAsync(LeanWorkflowTaskDto dto)
  {
    var result = await _service.CreateAsync(dto);
    return LeanApiResult<long>.Ok(result);
  }

  /// <summary>
  /// 更新工作流任务
  /// </summary>
  /// <param name="id">任务ID</param>
  /// <param name="dto">更新参数</param>
  /// <returns>是否成功</returns>
  [HttpPut("{id}")]
  public async Task<LeanApiResult<bool>> UpdateAsync(long id, LeanWorkflowTaskDto dto)
  {
    if (id != dto.Id)
    {
      return LeanApiResult<bool>.Error("ID不匹配");
    }
    var result = await _service.UpdateAsync(dto);
    return LeanApiResult<bool>.Ok(result);
  }

  /// <summary>
  /// 删除工作流任务
  /// </summary>
  /// <param name="id">任务ID</param>
  /// <returns>是否成功</returns>
  [HttpDelete("{id}")]
  public async Task<LeanApiResult<bool>> DeleteAsync(long id)
  {
    var result = await _service.DeleteAsync(id);
    return LeanApiResult<bool>.Ok(result);
  }

  /// <summary>
  /// 完成工作流任务
  /// </summary>
  /// <param name="id">任务ID</param>
  /// <param name="comment">审批意见</param>
  /// <returns>是否成功</returns>
  [HttpPost("{id}/complete")]
  public async Task<LeanApiResult<bool>> CompleteAsync(long id, [FromQuery] string? comment = null)
  {
    var result = await _service.CompleteAsync(id, comment);
    return LeanApiResult<bool>.Ok(result);
  }

  /// <summary>
  /// 驳回工作流任务
  /// </summary>
  /// <param name="id">任务ID</param>
  /// <param name="comment">审批意见</param>
  /// <returns>是否成功</returns>
  [HttpPost("{id}/reject")]
  public async Task<LeanApiResult<bool>> RejectAsync(long id, [FromQuery] string? comment = null)
  {
    var result = await _service.RejectAsync(id, comment);
    return LeanApiResult<bool>.Ok(result);
  }

  /// <summary>
  /// 转办工作流任务
  /// </summary>
  /// <param name="id">任务ID</param>
  /// <param name="assigneeId">转办人ID</param>
  /// <param name="comment">审批意见</param>
  /// <returns>是否成功</returns>
  [HttpPost("{id}/transfer/{assigneeId}")]
  public async Task<LeanApiResult<bool>> TransferAsync(long id, long assigneeId, [FromQuery] string? comment = null)
  {
    var result = await _service.TransferAsync(id, assigneeId, comment);
    return LeanApiResult<bool>.Ok(result);
  }

  /// <summary>
  /// 委派工作流任务
  /// </summary>
  /// <param name="id">任务ID</param>
  /// <param name="assigneeId">委派人ID</param>
  /// <param name="comment">审批意见</param>
  /// <returns>是否成功</returns>
  [HttpPost("{id}/delegate/{assigneeId}")]
  public async Task<LeanApiResult<bool>> DelegateAsync(long id, long assigneeId, [FromQuery] string? comment = null)
  {
    var result = await _service.DelegateAsync(id, assigneeId, comment);
    return LeanApiResult<bool>.Ok(result);
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
  public async Task<LeanApiResult<LeanPageResult<LeanWorkflowTaskDto>>> GetPagedListAsync(
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
    return LeanApiResult<LeanPageResult<LeanWorkflowTaskDto>>.Ok(result);
  }
}