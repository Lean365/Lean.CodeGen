using Lean.CodeGen.Application.Dtos.Workflow;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Services.Workflow;

/// <summary>
/// 工作流定义服务接口
/// </summary>
public interface ILeanWorkflowDefinitionService
{
  /// <summary>
  /// 获取工作流定义列表
  /// </summary>
  /// <returns>工作流定义列表</returns>
  Task<List<LeanWorkflowDefinitionDto>> GetListAsync();

  /// <summary>
  /// 获取工作流定义
  /// </summary>
  /// <param name="id">工作流定义ID</param>
  /// <returns>工作流定义</returns>
  Task<LeanWorkflowDefinitionDto?> GetAsync(long id);

  /// <summary>
  /// 根据编码获取工作流定义
  /// </summary>
  /// <param name="code">工作流编码</param>
  /// <returns>工作流定义</returns>
  Task<LeanWorkflowDefinitionDto?> GetByCodeAsync(string code);

  /// <summary>
  /// 创建工作流定义
  /// </summary>
  /// <param name="dto">工作流定义</param>
  /// <returns>工作流定义ID</returns>
  Task<long> CreateAsync(LeanWorkflowDefinitionDto dto);

  /// <summary>
  /// 更新工作流定义
  /// </summary>
  /// <param name="dto">工作流定义</param>
  /// <returns>是否成功</returns>
  Task<LeanApiResult> UpdateAsync(LeanWorkflowDefinitionDto dto);

  /// <summary>
  /// 删除工作流定义
  /// </summary>
  /// <param name="id">工作流定义ID</param>
  /// <returns>是否成功</returns>
  Task<LeanApiResult> DeleteAsync(long id);

  /// <summary>
  /// 发布工作流定义
  /// </summary>
  /// <param name="id">工作流定义ID</param>
  /// <returns>是否成功</returns>
  Task<LeanApiResult> PublishAsync(long id);

  /// <summary>
  /// 停用工作流定义
  /// </summary>
  /// <param name="id">工作流定义ID</param>
  /// <returns>是否成功</returns>
  Task<bool> DisableAsync(long id);

  /// <summary>
  /// 分页查询工作流定义
  /// </summary>
  /// <param name="pageIndex">页码</param>
  /// <param name="pageSize">每页大小</param>
  /// <param name="workflowName">工作流名称</param>
  /// <param name="workflowCode">工作流编码</param>
  /// <param name="status">状态</param>
  /// <returns>分页结果</returns>
  Task<LeanPageResult<LeanWorkflowDefinitionDto>> GetPagedListAsync(int pageIndex, int pageSize, string? workflowName = null, string? workflowCode = null, int? status = null);
}