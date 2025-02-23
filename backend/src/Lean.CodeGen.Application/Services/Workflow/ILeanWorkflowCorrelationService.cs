using Lean.CodeGen.Application.Dtos.Workflow;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Services.Workflow;

/// <summary>
/// 工作流关联服务接口
/// </summary>
public interface ILeanWorkflowCorrelationService
{
  /// <summary>
  /// 获取工作流关联
  /// </summary>
  /// <param name="id">关联ID</param>
  /// <returns>工作流关联DTO</returns>
  Task<LeanWorkflowCorrelationDto?> GetAsync(long id);

  /// <summary>
  /// 根据关联键获取工作流关联
  /// </summary>
  /// <param name="correlationId">关联键</param>
  /// <returns>工作流关联DTO</returns>
  Task<LeanWorkflowCorrelationDto?> GetByCorrelationIdAsync(string correlationId);

  /// <summary>
  /// 创建工作流关联
  /// </summary>
  /// <param name="dto">工作流关联DTO</param>
  /// <returns>关联ID</returns>
  Task<long> CreateAsync(LeanWorkflowCorrelationDto dto);

  /// <summary>
  /// 更新工作流关联
  /// </summary>
  /// <param name="dto">工作流关联DTO</param>
  /// <returns>是否成功</returns>
  Task<bool> UpdateAsync(LeanWorkflowCorrelationDto dto);

  /// <summary>
  /// 删除工作流关联
  /// </summary>
  /// <param name="id">关联ID</param>
  /// <returns>是否成功</returns>
  Task<bool> DeleteAsync(long id);

  /// <summary>
  /// 获取工作流关联分页列表
  /// </summary>
  /// <param name="pageIndex">页码</param>
  /// <param name="pageSize">每页大小</param>
  /// <param name="instanceId">工作流实例ID</param>
  /// <param name="correlationId">关联键</param>
  /// <param name="correlationType">关联类型</param>
  /// <param name="status">关联状态</param>
  /// <param name="startTime">开始时间</param>
  /// <param name="endTime">结束时间</param>
  /// <returns>工作流关联分页列表</returns>
  Task<LeanPageResult<LeanWorkflowCorrelationDto>> GetPagedListAsync(
      int pageIndex,
      int pageSize,
      long? instanceId = null,
      string? correlationId = null,
      string? correlationType = null,
      bool? status = null,
      DateTime? startTime = null,
      DateTime? endTime = null);
}