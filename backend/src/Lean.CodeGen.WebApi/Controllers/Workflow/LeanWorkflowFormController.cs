using Lean.CodeGen.Application.Dtos.Workflow;
using Lean.CodeGen.Application.Services.Workflow;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Lean.CodeGen.WebApi.Controllers.Workflow;

/// <summary>
/// 工作流表单控制器
/// </summary>
[ApiController]
[Route("api/workflow/forms")]
public class LeanWorkflowFormController : LeanBaseController
{
  private readonly ILeanWorkflowFormService _service;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="service">工作流表单服务</param>
  public LeanWorkflowFormController(ILeanWorkflowFormService service)
  {
    _service = service;
  }

  /// <summary>
  /// 获取表单定义
  /// </summary>
  /// <param name="id">表单定义ID</param>
  /// <returns>表单定义</returns>
  [HttpGet("definitions/{id}")]
  public Task<LeanWorkflowFormDefinitionDto?> GetFormDefinitionAsync(long id)
  {
    return _service.GetFormDefinitionAsync(id);
  }

  /// <summary>
  /// 根据编码获取表单定义
  /// </summary>
  /// <param name="formCode">表单编码</param>
  /// <returns>表单定义</returns>
  [HttpGet("definitions/code/{formCode}")]
  public Task<LeanWorkflowFormDefinitionDto?> GetFormDefinitionByCodeAsync(string formCode)
  {
    return _service.GetFormDefinitionByCodeAsync(formCode);
  }

  /// <summary>
  /// 创建表单定义
  /// </summary>
  /// <param name="dto">表单定义</param>
  /// <returns>表单定义ID</returns>
  [HttpPost("definitions")]
  public Task<long> CreateFormDefinitionAsync(LeanWorkflowFormDefinitionDto dto)
  {
    return _service.CreateFormDefinitionAsync(dto);
  }

  /// <summary>
  /// 更新表单定义
  /// </summary>
  /// <param name="id">表单定义ID</param>
  /// <param name="dto">表单定义</param>
  /// <returns>是否成功</returns>
  [HttpPut("definitions/{id}")]
  public async Task<bool> UpdateFormDefinitionAsync(long id, LeanWorkflowFormDefinitionDto dto)
  {
    if (id != dto.Id)
    {
      return false;
    }
    return await _service.UpdateFormDefinitionAsync(dto);
  }

  /// <summary>
  /// 删除表单定义
  /// </summary>
  /// <param name="id">表单定义ID</param>
  /// <returns>是否成功</returns>
  [HttpDelete("definitions/{id}")]
  public Task<bool> DeleteFormDefinitionAsync(long id)
  {
    return _service.DeleteFormDefinitionAsync(id);
  }

  /// <summary>
  /// 获取表单字段
  /// </summary>
  /// <param name="id">表单字段ID</param>
  /// <returns>表单字段</returns>
  [HttpGet("fields/{id}")]
  public Task<LeanWorkflowFormFieldDto?> GetFormFieldAsync(long id)
  {
    return _service.GetFormFieldAsync(id);
  }

  /// <summary>
  /// 根据编码获取表单字段
  /// </summary>
  /// <param name="formId">表单定义ID</param>
  /// <param name="fieldCode">字段编码</param>
  /// <returns>表单字段</returns>
  [HttpGet("fields/form/{formId}/field/{fieldCode}")]
  public Task<LeanWorkflowFormFieldDto?> GetFormFieldByCodeAsync(long formId, string fieldCode)
  {
    return _service.GetFormFieldByCodeAsync(formId, fieldCode);
  }
}