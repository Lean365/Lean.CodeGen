using Lean.CodeGen.Application.Dtos.Workflow;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Services.Workflow;

/// <summary>
/// 工作流补偿服务接口
/// </summary>
public interface ILeanWorkflowCompensationService
{
  /// <summary>
  /// 获取补偿记录
  /// </summary>
  /// <param name="id">补偿ID</param>
  /// <returns>补偿记录</returns>
  Task<LeanWorkflowCompensationDto?> GetAsync(long id);

  /// <summary>
  /// 创建补偿记录
  /// </summary>
  /// <param name="dto">补偿记录</param>
  /// <returns>补偿ID</returns>
  Task<long> CreateAsync(LeanWorkflowCompensationDto dto);

  /// <summary>
  /// 更新补偿记录
  /// </summary>
  /// <param name="dto">补偿记录</param>
  /// <returns>是否成功</returns>
  Task<bool> UpdateAsync(LeanWorkflowCompensationDto dto);

  /// <summary>
  /// 删除补偿记录
  /// </summary>
  /// <param name="id">补偿ID</param>
  /// <returns>是否成功</returns>
  Task<bool> DeleteAsync(long id);

  /// <summary>
  /// 分页查询补偿记录
  /// </summary>
  /// <param name="pageIndex">页码</param>
  /// <param name="pageSize">每页大小</param>
  /// <param name="activityInstanceId">活动实例ID</param>
  /// <param name="startTime">开始时间</param>
  /// <param name="endTime">结束时间</param>
  /// <returns>分页结果</returns>
  Task<LeanPageResult<LeanWorkflowCompensationDto>> GetPagedListAsync(
      int pageIndex,
      int pageSize,
      long? activityInstanceId = null,
      DateTime? startTime = null,
      DateTime? endTime = null);
}