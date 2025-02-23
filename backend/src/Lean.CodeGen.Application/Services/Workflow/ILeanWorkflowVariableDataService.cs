using Lean.CodeGen.Application.Dtos.Workflow;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Services.Workflow;

/// <summary>
/// 工作流变量数据服务接口
/// </summary>
public interface ILeanWorkflowVariableDataService
{
  /// <summary>
  /// 获取变量数据
  /// </summary>
  /// <param name="id">变量数据ID</param>
  /// <returns>变量数据</returns>
  Task<LeanWorkflowVariableDataDto?> GetAsync(long id);

  /// <summary>
  /// 根据实例ID和变量名称获取变量数据
  /// </summary>
  /// <param name="instanceId">工作流实例ID</param>
  /// <param name="variableName">变量名称</param>
  /// <returns>变量数据</returns>
  Task<LeanWorkflowVariableDataDto?> GetByNameAsync(long instanceId, string variableName);

  /// <summary>
  /// 创建变量数据
  /// </summary>
  /// <param name="dto">变量数据</param>
  /// <returns>变量数据ID</returns>
  Task<long> CreateAsync(LeanWorkflowVariableDataDto dto);

  /// <summary>
  /// 更新变量数据
  /// </summary>
  /// <param name="dto">变量数据</param>
  /// <returns>是否成功</returns>
  Task<bool> UpdateAsync(LeanWorkflowVariableDataDto dto);

  /// <summary>
  /// 删除变量数据
  /// </summary>
  /// <param name="id">变量数据ID</param>
  /// <returns>是否成功</returns>
  Task<bool> DeleteAsync(long id);

  /// <summary>
  /// 分页查询变量数据
  /// </summary>
  /// <param name="pageIndex">页码</param>
  /// <param name="pageSize">每页大小</param>
  /// <param name="instanceId">工作流实例ID</param>
  /// <param name="taskId">任务ID</param>
  /// <param name="variableId">变量定义ID</param>
  /// <param name="variableName">变量名称</param>
  /// <param name="variableType">变量类型</param>
  /// <param name="operatorId">操作人ID</param>
  /// <param name="startTime">开始时间</param>
  /// <param name="endTime">结束时间</param>
  /// <returns>分页结果</returns>
  Task<LeanPageResult<LeanWorkflowVariableDataDto>> GetPagedListAsync(
      int pageIndex,
      int pageSize,
      long? instanceId = null,
      long? taskId = null,
      long? variableId = null,
      string? variableName = null,
      string? variableType = null,
      long? operatorId = null,
      DateTime? startTime = null,
      DateTime? endTime = null);
}