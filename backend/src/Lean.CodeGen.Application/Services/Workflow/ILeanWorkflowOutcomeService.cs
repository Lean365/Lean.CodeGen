using Lean.CodeGen.Application.Dtos.Workflow;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Services.Workflow;

/// <summary>
/// 工作流结果服务接口
/// </summary>
public interface ILeanWorkflowOutcomeService
{
  /// <summary>
  /// 获取结果记录
  /// </summary>
  /// <param name="id">结果ID</param>
  /// <returns>结果记录</returns>
  Task<LeanWorkflowOutcomeDto?> GetAsync(long id);

  /// <summary>
  /// 创建结果记录
  /// </summary>
  /// <param name="dto">结果记录</param>
  /// <returns>结果ID</returns>
  Task<long> CreateAsync(LeanWorkflowOutcomeDto dto);

  /// <summary>
  /// 更新结果记录
  /// </summary>
  /// <param name="dto">结果记录</param>
  /// <returns>是否成功</returns>
  Task<bool> UpdateAsync(LeanWorkflowOutcomeDto dto);

  /// <summary>
  /// 删除结果记录
  /// </summary>
  /// <param name="id">结果ID</param>
  /// <returns>是否成功</returns>
  Task<bool> DeleteAsync(long id);

  /// <summary>
  /// 分页查询结果记录
  /// </summary>
  /// <param name="pageIndex">页码</param>
  /// <param name="pageSize">每页大小</param>
  /// <param name="activityInstanceId">活动实例ID</param>
  /// <param name="outcomeName">结果名称</param>
  /// <param name="outcomeType">结果类型</param>
  /// <returns>分页结果</returns>
  Task<LeanPageResult<LeanWorkflowOutcomeDto>> GetPagedListAsync(
      int pageIndex,
      int pageSize,
      long? activityInstanceId = null,
      string? outcomeName = null,
      string? outcomeType = null);
}