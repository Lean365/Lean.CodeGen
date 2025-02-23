using Lean.CodeGen.Application.Dtos.Workflow;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Services.Workflow;

/// <summary>
/// 工作流变量服务接口
/// </summary>
public interface ILeanWorkflowVariableService
{
  /// <summary>
  /// 获取变量
  /// </summary>
  /// <param name="id">变量ID</param>
  /// <returns>变量</returns>
  Task<LeanWorkflowVariableDto?> GetAsync(long id);

  /// <summary>
  /// 根据名称获取变量
  /// </summary>
  /// <param name="definitionId">工作流定义ID</param>
  /// <param name="variableName">变量名称</param>
  /// <returns>变量</returns>
  Task<LeanWorkflowVariableDto?> GetByNameAsync(long definitionId, string variableName);

  /// <summary>
  /// 创建变量
  /// </summary>
  /// <param name="dto">变量</param>
  /// <returns>变量ID</returns>
  Task<long> CreateAsync(LeanWorkflowVariableDto dto);

  /// <summary>
  /// 更新变量
  /// </summary>
  /// <param name="dto">变量</param>
  /// <returns>是否成功</returns>
  Task<bool> UpdateAsync(LeanWorkflowVariableDto dto);

  /// <summary>
  /// 删除变量
  /// </summary>
  /// <param name="id">变量ID</param>
  /// <returns>是否成功</returns>
  Task<bool> DeleteAsync(long id);

  /// <summary>
  /// 分页查询变量
  /// </summary>
  /// <param name="pageIndex">页码</param>
  /// <param name="pageSize">每页大小</param>
  /// <param name="definitionId">工作流定义ID</param>
  /// <param name="variableName">变量名称</param>
  /// <param name="variableType">变量类型</param>
  /// <param name="isRequired">是否必填</param>
  /// <param name="isReadonly">是否只读</param>
  /// <param name="status">状态</param>
  /// <returns>分页结果</returns>
  Task<LeanPageResult<LeanWorkflowVariableDto>> GetPagedListAsync(
      int pageIndex,
      int pageSize,
      long? definitionId = null,
      string? variableName = null,
      string? variableType = null,
      bool? isRequired = null,
      bool? isReadonly = null,
      bool? status = null);
}