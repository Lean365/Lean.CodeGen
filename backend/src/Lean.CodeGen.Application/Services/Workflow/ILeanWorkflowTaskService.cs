using Lean.CodeGen.Application.Dtos.Workflow;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Services.Workflow;

/// <summary>
/// 工作流任务服务接口
/// </summary>
public interface ILeanWorkflowTaskService
{
  /// <summary>
  /// 获取工作流任务
  /// </summary>
  /// <param name="id">任务ID</param>
  /// <returns>工作流任务</returns>
  Task<LeanWorkflowTaskDto> GetAsync(long id);

  /// <summary>
  /// 创建工作流任务
  /// </summary>
  /// <param name="dto">创建参数</param>
  /// <returns>任务ID</returns>
  Task<long> CreateAsync(LeanWorkflowTaskDto dto);

  /// <summary>
  /// 更新工作流任务
  /// </summary>
  /// <param name="dto">更新参数</param>
  /// <returns>是否成功</returns>
  Task<bool> UpdateAsync(LeanWorkflowTaskDto dto);

  /// <summary>
  /// 删除工作流任务
  /// </summary>
  /// <param name="id">任务ID</param>
  /// <returns>是否成功</returns>
  Task<bool> DeleteAsync(long id);

  /// <summary>
  /// 完成工作流任务
  /// </summary>
  /// <param name="id">任务ID</param>
  /// <param name="comment">审批意见</param>
  /// <returns>是否成功</returns>
  Task<bool> CompleteAsync(long id, string? comment = null);

  /// <summary>
  /// 驳回工作流任务
  /// </summary>
  /// <param name="id">任务ID</param>
  /// <param name="comment">审批意见</param>
  /// <returns>是否成功</returns>
  Task<bool> RejectAsync(long id, string? comment = null);

  /// <summary>
  /// 转办工作流任务
  /// </summary>
  /// <param name="id">任务ID</param>
  /// <param name="assigneeId">转办人ID</param>
  /// <param name="comment">审批意见</param>
  /// <returns>是否成功</returns>
  Task<bool> TransferAsync(long id, long assigneeId, string? comment = null);

  /// <summary>
  /// 委派工作流任务
  /// </summary>
  /// <param name="id">任务ID</param>
  /// <param name="assigneeId">委派人ID</param>
  /// <param name="comment">审批意见</param>
  /// <returns>是否成功</returns>
  Task<bool> DelegateAsync(long id, long assigneeId, string? comment = null);

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
  Task<LeanPageResult<LeanWorkflowTaskDto>> GetPagedListAsync(
      int pageIndex,
      int pageSize,
      long? instanceId = null,
      string? taskType = null,
      string? taskNode = null,
      int? priority = null,
      long? assigneeId = null,
      int? taskStatus = null,
      DateTime? startTime = null,
      DateTime? endTime = null);
}