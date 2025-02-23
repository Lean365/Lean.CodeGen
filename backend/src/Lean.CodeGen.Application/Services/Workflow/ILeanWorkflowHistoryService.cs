using Lean.CodeGen.Application.Dtos.Workflow;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Services.Workflow;

/// <summary>
/// 工作流历史服务接口
/// </summary>
public interface ILeanWorkflowHistoryService
{
  /// <summary>
  /// 获取工作流历史
  /// </summary>
  /// <param name="id">历史ID</param>
  /// <returns>工作流历史</returns>
  Task<LeanWorkflowHistoryDto> GetAsync(long id);

  /// <summary>
  /// 创建工作流历史
  /// </summary>
  /// <param name="dto">创建参数</param>
  /// <returns>历史ID</returns>
  Task<long> CreateAsync(LeanWorkflowHistoryDto dto);

  /// <summary>
  /// 更新工作流历史
  /// </summary>
  /// <param name="dto">更新参数</param>
  /// <returns>是否成功</returns>
  Task<bool> UpdateAsync(LeanWorkflowHistoryDto dto);

  /// <summary>
  /// 删除工作流历史
  /// </summary>
  /// <param name="id">历史ID</param>
  /// <returns>是否成功</returns>
  Task<bool> DeleteAsync(long id);

  /// <summary>
  /// 分页查询工作流历史
  /// </summary>
  /// <param name="pageIndex">页码</param>
  /// <param name="pageSize">每页大小</param>
  /// <param name="instanceId">实例ID</param>
  /// <param name="taskId">任务ID</param>
  /// <param name="operationType">操作类型</param>
  /// <param name="operatorId">操作人ID</param>
  /// <param name="startTime">开始时间</param>
  /// <param name="endTime">结束时间</param>
  /// <returns>分页结果</returns>
  Task<LeanPageResult<LeanWorkflowHistoryDto>> GetPagedListAsync(
      int pageIndex,
      int pageSize,
      long? instanceId = null,
      long? taskId = null,
      string? operationType = null,
      long? operatorId = null,
      DateTime? startTime = null,
      DateTime? endTime = null);
}