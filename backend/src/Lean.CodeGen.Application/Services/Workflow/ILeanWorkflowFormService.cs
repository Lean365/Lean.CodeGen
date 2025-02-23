using Lean.CodeGen.Application.Dtos.Workflow;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Services.Workflow;

/// <summary>
/// 工作流表单服务接口
/// </summary>
public interface ILeanWorkflowFormService
{
  /// <summary>
  /// 获取表单定义
  /// </summary>
  /// <param name="id">表单定义ID</param>
  /// <returns>表单定义</returns>
  Task<LeanWorkflowFormDefinitionDto?> GetFormDefinitionAsync(long id);

  /// <summary>
  /// 根据编码获取表单定义
  /// </summary>
  /// <param name="formCode">表单编码</param>
  /// <returns>表单定义</returns>
  Task<LeanWorkflowFormDefinitionDto?> GetFormDefinitionByCodeAsync(string formCode);

  /// <summary>
  /// 创建表单定义
  /// </summary>
  /// <param name="dto">表单定义</param>
  /// <returns>表单定义ID</returns>
  Task<long> CreateFormDefinitionAsync(LeanWorkflowFormDefinitionDto dto);

  /// <summary>
  /// 更新表单定义
  /// </summary>
  /// <param name="dto">表单定义</param>
  /// <returns>是否成功</returns>
  Task<bool> UpdateFormDefinitionAsync(LeanWorkflowFormDefinitionDto dto);

  /// <summary>
  /// 删除表单定义
  /// </summary>
  /// <param name="id">表单定义ID</param>
  /// <returns>是否成功</returns>
  Task<bool> DeleteFormDefinitionAsync(long id);

  /// <summary>
  /// 获取表单字段
  /// </summary>
  /// <param name="id">表单字段ID</param>
  /// <returns>表单字段</returns>
  Task<LeanWorkflowFormFieldDto?> GetFormFieldAsync(long id);

  /// <summary>
  /// 根据编码获取表单字段
  /// </summary>
  /// <param name="formId">表单定义ID</param>
  /// <param name="fieldCode">字段编码</param>
  /// <returns>表单字段</returns>
  Task<LeanWorkflowFormFieldDto?> GetFormFieldByCodeAsync(long formId, string fieldCode);

  /// <summary>
  /// 创建表单字段
  /// </summary>
  /// <param name="dto">表单字段</param>
  /// <returns>表单字段ID</returns>
  Task<long> CreateFormFieldAsync(LeanWorkflowFormFieldDto dto);

  /// <summary>
  /// 更新表单字段
  /// </summary>
  /// <param name="dto">表单字段</param>
  /// <returns>是否成功</returns>
  Task<bool> UpdateFormFieldAsync(LeanWorkflowFormFieldDto dto);

  /// <summary>
  /// 删除表单字段
  /// </summary>
  /// <param name="id">表单字段ID</param>
  /// <returns>是否成功</returns>
  Task<bool> DeleteFormFieldAsync(long id);

  /// <summary>
  /// 获取表单数据
  /// </summary>
  /// <param name="id">表单数据ID</param>
  /// <returns>表单数据</returns>
  Task<LeanWorkflowFormDataDto?> GetFormDataAsync(long id);

  /// <summary>
  /// 创建表单数据
  /// </summary>
  /// <param name="dto">表单数据</param>
  /// <returns>表单数据ID</returns>
  Task<long> CreateFormDataAsync(LeanWorkflowFormDataDto dto);

  /// <summary>
  /// 更新表单数据
  /// </summary>
  /// <param name="dto">表单数据</param>
  /// <returns>是否成功</returns>
  Task<bool> UpdateFormDataAsync(LeanWorkflowFormDataDto dto);

  /// <summary>
  /// 删除表单数据
  /// </summary>
  /// <param name="id">表单数据ID</param>
  /// <returns>是否成功</returns>
  Task<bool> DeleteFormDataAsync(long id);

  /// <summary>
  /// 分页查询表单定义
  /// </summary>
  /// <param name="pageIndex">页码</param>
  /// <param name="pageSize">每页大小</param>
  /// <param name="formName">表单名称</param>
  /// <param name="formCode">表单编码</param>
  /// <param name="formType">表单类型</param>
  /// <param name="status">状态</param>
  /// <returns>分页结果</returns>
  Task<LeanPageResult<LeanWorkflowFormDefinitionDto>> GetFormDefinitionPagedListAsync(
      int pageIndex,
      int pageSize,
      string? formName = null,
      string? formCode = null,
      string? formType = null,
      int? status = null);

  /// <summary>
  /// 分页查询表单字段
  /// </summary>
  /// <param name="pageIndex">页码</param>
  /// <param name="pageSize">每页大小</param>
  /// <param name="formId">表单定义ID</param>
  /// <param name="fieldName">字段名称</param>
  /// <param name="fieldCode">字段编码</param>
  /// <param name="fieldType">字段类型</param>
  /// <param name="status">状态</param>
  /// <returns>分页结果</returns>
  Task<LeanPageResult<LeanWorkflowFormFieldDto>> GetFormFieldPagedListAsync(
      int pageIndex,
      int pageSize,
      long? formId = null,
      string? fieldName = null,
      string? fieldCode = null,
      string? fieldType = null,
      int? status = null);

  /// <summary>
  /// 分页查询表单数据
  /// </summary>
  /// <param name="pageIndex">页码</param>
  /// <param name="pageSize">每页大小</param>
  /// <param name="instanceId">实例ID</param>
  /// <param name="taskId">任务ID</param>
  /// <param name="formId">表单定义ID</param>
  /// <param name="fieldCode">字段编码</param>
  /// <param name="operatorId">操作人ID</param>
  /// <param name="startTime">开始时间</param>
  /// <param name="endTime">结束时间</param>
  /// <returns>分页结果</returns>
  Task<LeanPageResult<LeanWorkflowFormDataDto>> GetFormDataPagedListAsync(
      int pageIndex,
      int pageSize,
      long? instanceId = null,
      long? taskId = null,
      long? formId = null,
      string? fieldCode = null,
      long? operatorId = null,
      DateTime? startTime = null,
      DateTime? endTime = null);
}