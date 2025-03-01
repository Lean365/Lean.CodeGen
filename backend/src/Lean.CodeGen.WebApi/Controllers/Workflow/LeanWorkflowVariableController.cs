using Lean.CodeGen.Application.Dtos.Workflow;
using Lean.CodeGen.Application.Services.Workflow;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Common.Enums;

namespace Lean.CodeGen.WebApi.Controllers.Workflow;

/// <summary>
/// 工作流变量控制器
/// </summary>
[ApiController]
[Route("api/workflow/variables")]
[ApiExplorerSettings(GroupName = "workflow")]
public class LeanWorkflowVariableController : LeanBaseController
{
  private readonly ILeanWorkflowVariableService _service;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="service">工作流变量服务</param>
  public LeanWorkflowVariableController(ILeanWorkflowVariableService service)
  {
    _service = service;
  }

  /// <summary>
  /// 获取变量
  /// </summary>
  /// <param name="id">变量ID</param>
  /// <returns>变量</returns>
  [HttpGet("{id}")]
  public async Task<IActionResult> GetAsync(long id)
  {
    var result = await _service.GetAsync(id);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 根据名称获取变量
  /// </summary>
  /// <param name="definitionId">工作流定义ID</param>
  /// <param name="variableName">变量名称</param>
  /// <returns>变量</returns>
  [HttpGet("definition/{definitionId}/variable/{variableName}")]
  public async Task<IActionResult> GetByNameAsync(long definitionId, string variableName)
  {
    var result = await _service.GetByNameAsync(definitionId, variableName);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 创建变量
  /// </summary>
  /// <param name="dto">变量</param>
  /// <returns>变量ID</returns>
  [HttpPost]
  public async Task<IActionResult> CreateAsync(LeanWorkflowVariableDto dto)
  {
    var result = await _service.CreateAsync(dto);
    return Success(result, LeanBusinessType.Create);
  }

  /// <summary>
  /// 更新变量
  /// </summary>
  /// <param name="id">变量ID</param>
  /// <param name="dto">变量</param>
  /// <returns>是否成功</returns>
  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateAsync(long id, LeanWorkflowVariableDto dto)
  {
    if (id != dto.Id)
    {
      return Error("ID不匹配");
    }
    var result = await _service.UpdateAsync(dto);
    return Success(result, LeanBusinessType.Update);
  }

  /// <summary>
  /// 删除变量
  /// </summary>
  /// <param name="id">变量ID</param>
  /// <returns>是否成功</returns>
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteAsync(long id)
  {
    var result = await _service.DeleteAsync(id);
    return Success(result, LeanBusinessType.Delete);
  }
}