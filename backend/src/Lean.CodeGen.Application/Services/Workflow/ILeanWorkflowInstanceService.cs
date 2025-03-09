using Lean.CodeGen.Application.Dtos.Workflow;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Enums;

namespace Lean.CodeGen.Application.Services.Workflow;

/// <summary>
/// 工作流实例服务接口
/// </summary>
public interface ILeanWorkflowInstanceService
{
  /// <summary>
  /// 获取工作流实例
  /// </summary>
  /// <param name="id">工作流实例ID</param>
  /// <returns>工作流实例</returns>
  Task<LeanWorkflowInstanceDto?> GetAsync(long id);

  /// <summary>
  /// 根据业务主键获取工作流实例
  /// </summary>
  /// <param name="businessKey">业务主键</param>
  /// <returns>工作流实例</returns>
  Task<LeanWorkflowInstanceDto?> GetByBusinessKeyAsync(string businessKey);

  /// <summary>
  /// 创建工作流实例
  /// </summary>
  /// <param name="dto">工作流实例</param>
  /// <returns>工作流实例ID</returns>
  Task<long> CreateAsync(LeanWorkflowInstanceDto dto);

  /// <summary>
  /// 更新工作流实例
  /// </summary>
  /// <param name="dto">工作流实例</param>
  /// <returns>是否成功</returns>
  Task<LeanApiResult> UpdateAsync(LeanWorkflowInstanceDto dto);

  /// <summary>
  /// 删除工作流实例
  /// </summary>
  /// <param name="id">工作流实例ID</param>
  /// <returns>是否成功</returns>
  Task<LeanApiResult> DeleteAsync(long id);

  /// <summary>
  /// 启动工作流实例
  /// </summary>
  /// <param name="id">工作流实例ID</param>
  /// <returns>是否成功</returns>
  Task<LeanApiResult> StartAsync(long id);

  /// <summary>
  /// 暂停工作流实例
  /// </summary>
  /// <param name="id">工作流实例ID</param>
  /// <returns>是否成功</returns>
  Task<LeanApiResult> SuspendAsync(long id);

  /// <summary>
  /// 恢复工作流实例
  /// </summary>
  /// <param name="id">工作流实例ID</param>
  /// <returns>是否成功</returns>
  Task<LeanApiResult> ResumeAsync(long id);

  /// <summary>
  /// 终止工作流实例
  /// </summary>
  /// <param name="id">工作流实例ID</param>
  /// <returns>是否成功</returns>
  Task<LeanApiResult> TerminateAsync(long id);

  /// <summary>
  /// 归档工作流实例
  /// </summary>
  /// <param name="id">工作流实例ID</param>
  /// <returns>是否成功</returns>
  Task<LeanApiResult> ArchiveAsync(long id);

  /// <summary>
  /// 获取分页列表
  /// </summary>
  /// <param name="pageIndex">页码</param>
  /// <param name="pageSize">页大小</param>
  /// <param name="definitionId">流程定义ID</param>
  /// <param name="businessKey">业务主键</param>
  /// <param name="businessType">业务类型</param>
  /// <param name="title">标题</param>
  /// <param name="initiatorId">发起人ID</param>
  /// <param name="workflowStatus">流程状态</param>
  /// <returns>分页列表</returns>
  Task<LeanPageResult<LeanWorkflowInstanceDto>> GetPagedListAsync(
      int pageIndex,
      int pageSize,
      long? definitionId = null,
      string? businessKey = null,
      string? businessType = null,
      string? title = null,
      long? initiatorId = null,
      int? workflowStatus = null);
}