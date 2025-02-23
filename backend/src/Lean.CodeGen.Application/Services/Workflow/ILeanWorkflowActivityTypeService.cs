using Lean.CodeGen.Application.Dtos.Workflow;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Services.Workflow;

/// <summary>
/// 工作流活动类型服务接口
/// </summary>
public interface ILeanWorkflowActivityTypeService
{
  /// <summary>
  /// 获取活动类型列表
  /// </summary>
  /// <returns>活动类型列表</returns>
  Task<List<LeanWorkflowActivityTypeDto>> GetListAsync();

  /// <summary>
  /// 获取活动类型
  /// </summary>
  /// <param name="typeName">活动类型名称</param>
  /// <returns>活动类型</returns>
  Task<LeanWorkflowActivityTypeDto?> GetAsync(string typeName);

  /// <summary>
  /// 创建活动类型
  /// </summary>
  /// <param name="dto">活动类型</param>
  /// <returns>是否成功</returns>
  Task<bool> CreateAsync(LeanWorkflowActivityTypeDto dto);

  /// <summary>
  /// 更新活动类型
  /// </summary>
  /// <param name="dto">活动类型</param>
  /// <returns>是否成功</returns>
  Task<bool> UpdateAsync(LeanWorkflowActivityTypeDto dto);

  /// <summary>
  /// 删除活动类型
  /// </summary>
  /// <param name="typeName">活动类型名称</param>
  /// <returns>是否成功</returns>
  Task<bool> DeleteAsync(string typeName);

  /// <summary>
  /// 分页查询活动类型
  /// </summary>
  /// <param name="pageIndex">页码</param>
  /// <param name="pageSize">每页大小</param>
  /// <param name="typeName">活动类型名称</param>
  /// <param name="category">分类</param>
  /// <param name="isBlocking">是否为阻塞活动</param>
  /// <param name="isTrigger">是否为触发器活动</param>
  /// <param name="isContainer">是否为容器活动</param>
  /// <param name="isSystem">是否为系统活动</param>
  /// <param name="status">状态</param>
  /// <returns>分页结果</returns>
  Task<LeanPageResult<LeanWorkflowActivityTypeDto>> GetPagedListAsync(
      int pageIndex,
      int pageSize,
      string? typeName = null,
      string? category = null,
      bool? isBlocking = null,
      bool? isTrigger = null,
      bool? isContainer = null,
      bool? isSystem = null,
      bool? status = null);
}