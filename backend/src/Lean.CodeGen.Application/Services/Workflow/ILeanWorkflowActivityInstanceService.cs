using Lean.CodeGen.Application.Dtos.Workflow;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Services.Workflow;

/// <summary>
/// 工作流活动实例服务接口
/// </summary>
public interface ILeanWorkflowActivityInstanceService
{
  /// <summary>
  /// 获取活动实例
  /// </summary>
  /// <param name="id">实例ID</param>
  /// <returns>活动实例</returns>
  Task<LeanWorkflowActivityInstanceDto?> GetAsync(long id);

  /// <summary>
  /// 创建活动实例
  /// </summary>
  /// <param name="dto">活动实例</param>
  /// <returns>实例ID</returns>
  Task<long> CreateAsync(LeanWorkflowActivityInstanceDto dto);

  /// <summary>
  /// 更新活动实例
  /// </summary>
  /// <param name="dto">活动实例</param>
  /// <returns>是否成功</returns>
  Task<bool> UpdateAsync(LeanWorkflowActivityInstanceDto dto);

  /// <summary>
  /// 删除活动实例
  /// </summary>
  /// <param name="id">实例ID</param>
  /// <returns>是否成功</returns>
  Task<bool> DeleteAsync(long id);

  /// <summary>
  /// 启动活动实例
  /// </summary>
  /// <param name="id">实例ID</param>
  /// <returns>是否成功</returns>
  Task<bool> StartAsync(long id);

  /// <summary>
  /// 完成活动实例
  /// </summary>
  /// <param name="id">实例ID</param>
  /// <returns>是否成功</returns>
  Task<bool> CompleteAsync(long id);

  /// <summary>
  /// 取消活动实例
  /// </summary>
  /// <param name="id">实例ID</param>
  /// <returns>是否成功</returns>
  Task<bool> CancelAsync(long id);

  /// <summary>
  /// 补偿活动实例
  /// </summary>
  /// <param name="id">实例ID</param>
  /// <returns>是否成功</returns>
  Task<bool> CompensateAsync(long id);

  /// <summary>
  /// 分页查询活动实例
  /// </summary>
  /// <param name="pageIndex">页码</param>
  /// <param name="pageSize">每页大小</param>
  /// <param name="workflowInstanceId">工作流实例ID</param>
  /// <param name="activityType">活动类型</param>
  /// <param name="activityStatus">活动状态</param>
  /// <param name="startTime">开始时间</param>
  /// <param name="endTime">结束时间</param>
  /// <returns>分页结果</returns>
  Task<LeanPageResult<LeanWorkflowActivityInstanceDto>> GetPagedListAsync(
      int pageIndex,
      int pageSize,
      long? workflowInstanceId = null,
      string? activityType = null,
      int? activityStatus = null,
      DateTime? startTime = null,
      DateTime? endTime = null);
}