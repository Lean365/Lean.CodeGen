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
  Task<bool> UpdateAsync(LeanWorkflowInstanceDto dto);

  /// <summary>
  /// 删除工作流实例
  /// </summary>
  /// <param name="id">工作流实例ID</param>
  /// <returns>是否成功</returns>
  Task<bool> DeleteAsync(long id);

  /// <summary>
  /// 启动工作流实例
  /// </summary>
  /// <param name="id">工作流实例ID</param>
  /// <returns>是否成功</returns>
  Task<bool> StartAsync(long id);

  /// <summary>
  /// 暂停工作流实例
  /// </summary>
  /// <param name="id">工作流实例ID</param>
  /// <returns>是否成功</returns>
  Task<bool> SuspendAsync(long id);

  /// <summary>
  /// 恢复工作流实例
  /// </summary>
  /// <param name="id">工作流实例ID</param>
  /// <returns>是否成功</returns>
  Task<bool> ResumeAsync(long id);

  /// <summary>
  /// 终止工作流实例
  /// </summary>
  /// <param name="id">工作流实例ID</param>
  /// <returns>是否成功</returns>
  Task<bool> TerminateAsync(long id);

  /// <summary>
  /// 归档工作流实例
  /// </summary>
  /// <param name="id">工作流实例ID</param>
  /// <returns>是否成功</returns>
  Task<bool> ArchiveAsync(long id);

  /// <summary>
  /// 分页查询工作流实例
  /// </summary>
  /// <param name="pageIndex">页码</param>
  /// <param name="pageSize">每页大小</param>
  /// <param name="definitionId">工作流定义ID</param>
  /// <param name="businessKey">业务主键</param>
  /// <param name="businessType">业务类型</param>
  /// <param name="title">实例标题</param>
  /// <param name="initiatorId">发起人ID</param>
  /// <param name="workflowStatus">工作流状态</param>
  /// <returns>分页结果</returns>
  Task<LeanPageResult<LeanWorkflowInstanceDto>> GetPagedListAsync(int pageIndex, int pageSize, long? definitionId = null, string? businessKey = null, string? businessType = null, string? title = null, long? initiatorId = null, LeanWorkflowInstanceStatus? workflowStatus = null);
}