using Lean.CodeGen.Application.Dtos.Workflow;
using Lean.CodeGen.Application.Services.Workflow;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Common.Enums;
using Microsoft.Extensions.Configuration;
using Lean.CodeGen.Application.Services.Admin;

namespace Lean.CodeGen.WebApi.Controllers.Workflow;

/// <summary>
/// 工作流表单控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "workflow")]
public class LeanWorkflowFormController : LeanBaseController
{
  private readonly ILeanWorkflowFormService _service;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="service">工作流表单服务</param>
  /// <param name="localizationService">本地化服务</param>
  /// <param name="configuration">配置</param>
  /// <param name="userContext">用户上下文</param>
  public LeanWorkflowFormController(
      ILeanWorkflowFormService service,
      ILeanLocalizationService localizationService,
      IConfiguration configuration,
      ILeanUserContext userContext)
      : base(localizationService, configuration, userContext)
  {
    _service = service;
  }

  /// <summary>
  /// 获取表单定义
  /// </summary>
  /// <param name="id">表单定义ID</param>
  /// <returns>表单定义</returns>
  [HttpGet("definitions/{id}")]
  public async Task<IActionResult> GetFormDefinitionAsync(long id)
  {
    var result = await _service.GetFormDefinitionAsync(id);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 根据编码获取表单定义
  /// </summary>
  /// <param name="formCode">表单编码</param>
  /// <returns>表单定义</returns>
  [HttpGet("definitions/code/{formCode}")]
  public async Task<IActionResult> GetFormDefinitionByCodeAsync(string formCode)
  {
    var result = await _service.GetFormDefinitionByCodeAsync(formCode);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 创建表单定义
  /// </summary>
  /// <param name="dto">表单定义</param>
  /// <returns>表单定义ID</returns>
  [HttpPost("definitions")]
  public async Task<IActionResult> CreateFormDefinitionAsync(LeanWorkflowFormDefinitionDto dto)
  {
    var result = await _service.CreateFormDefinitionAsync(dto);
    return Success(result, LeanBusinessType.Create);
  }

  /// <summary>
  /// 更新表单定义
  /// </summary>
  /// <param name="id">表单定义ID</param>
  /// <param name="dto">表单定义</param>
  /// <returns>是否成功</returns>
  [HttpPut("definitions/{id}")]
  public async Task<IActionResult> UpdateFormDefinitionAsync(long id, LeanWorkflowFormDefinitionDto dto)
  {
    if (id != dto.Id)
    {
      return await ErrorAsync("workflow.error.id_mismatch");
    }
    var result = await _service.UpdateFormDefinitionAsync(dto);
    return Success(result, LeanBusinessType.Update);
  }

  /// <summary>
  /// 删除表单定义
  /// </summary>
  /// <param name="id">表单定义ID</param>
  /// <returns>是否成功</returns>
  [HttpDelete("definitions/{id}")]
  public async Task<IActionResult> DeleteFormDefinitionAsync(long id)
  {
    var result = await _service.DeleteFormDefinitionAsync(id);
    return Success(result, LeanBusinessType.Delete);
  }

  /// <summary>
  /// 获取表单字段
  /// </summary>
  /// <param name="id">表单字段ID</param>
  /// <returns>表单字段</returns>
  [HttpGet("fields/{id}")]
  public async Task<IActionResult> GetFormFieldAsync(long id)
  {
    var result = await _service.GetFormFieldAsync(id);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 根据编码获取表单字段
  /// </summary>
  /// <param name="formId">表单定义ID</param>
  /// <param name="fieldCode">字段编码</param>
  /// <returns>表单字段</returns>
  [HttpGet("fields/form/{formId}/field/{fieldCode}")]
  public async Task<IActionResult> GetFormFieldByCodeAsync(long formId, string fieldCode)
  {
    var result = await _service.GetFormFieldByCodeAsync(formId, fieldCode);
    return Success(result, LeanBusinessType.Query);
  }
}