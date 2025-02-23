using Lean.CodeGen.Application.Dtos.Workflow;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Services.Workflow;

/// <summary>
/// 工作流活动属性服务接口
/// </summary>
public interface ILeanWorkflowActivityPropertyService
{
  /// <summary>
  /// 获取活动属性
  /// </summary>
  /// <param name="id">属性ID</param>
  /// <returns>活动属性</returns>
  Task<LeanWorkflowActivityPropertyDto?> GetAsync(long id);

  /// <summary>
  /// 根据活动ID和属性名称获取活动属性
  /// </summary>
  /// <param name="activityId">活动ID</param>
  /// <param name="propertyName">属性名称</param>
  /// <returns>活动属性</returns>
  Task<LeanWorkflowActivityPropertyDto?> GetByNameAsync(long activityId, string propertyName);

  /// <summary>
  /// 创建活动属性
  /// </summary>
  /// <param name="dto">活动属性</param>
  /// <returns>属性ID</returns>
  Task<long> CreateAsync(LeanWorkflowActivityPropertyDto dto);

  /// <summary>
  /// 更新活动属性
  /// </summary>
  /// <param name="dto">活动属性</param>
  /// <returns>是否成功</returns>
  Task<bool> UpdateAsync(LeanWorkflowActivityPropertyDto dto);

  /// <summary>
  /// 删除活动属性
  /// </summary>
  /// <param name="id">属性ID</param>
  /// <returns>是否成功</returns>
  Task<bool> DeleteAsync(long id);

  /// <summary>
  /// 分页查询活动属性
  /// </summary>
  /// <param name="pageIndex">页码</param>
  /// <param name="pageSize">每页大小</param>
  /// <param name="activityId">活动ID</param>
  /// <param name="propertyName">属性名称</param>
  /// <param name="propertyType">属性类型</param>
  /// <param name="category">分类</param>
  /// <param name="isRequired">是否必填</param>
  /// <param name="isDesignTime">是否为设计时属性</param>
  /// <param name="isRunTime">是否为运行时属性</param>
  /// <param name="status">状态</param>
  /// <returns>分页结果</returns>
  Task<LeanPageResult<LeanWorkflowActivityPropertyDto>> GetPagedListAsync(
      int pageIndex,
      int pageSize,
      long? activityId = null,
      string? propertyName = null,
      string? propertyType = null,
      string? category = null,
      bool? isRequired = null,
      bool? isDesignTime = null,
      bool? isRunTime = null,
      bool? status = null);
}