using Lean.CodeGen.Application.Dtos.Workflow;
using Lean.CodeGen.Application.Services.Workflow;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Lean.CodeGen.WebApi.Controllers.Workflow;

/// <summary>
/// 工作流变量控制器
/// </summary>
[ApiController]
[Route("api/workflow/variables")]
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
  public Task<LeanWorkflowVariableDto?> GetAsync(long id)
  {
    return _service.GetAsync(id);
  }

  /// <summary>
  /// 根据名称获取变量
  /// </summary>
  /// <param name="definitionId">工作流定义ID</param>
  /// <param name="variableName">变量名称</param>
  /// <returns>变量</returns>
  [HttpGet("definition/{definitionId}/variable/{variableName}")]
  public Task<LeanWorkflowVariableDto?> GetByNameAsync(long definitionId, string variableName)
  {
    return _service.GetByNameAsync(definitionId, variableName);
  }

  /// <summary>
  /// 创建变量
  /// </summary>
  /// <param name="dto">变量</param>
  /// <returns>变量ID</returns>
  [HttpPost]
  public Task<long> CreateAsync(LeanWorkflowVariableDto dto)
  {
    return _service.CreateAsync(dto);
  }

  /// <summary>
  /// 更新变量
  /// </summary>
  /// <param name="id">变量ID</param>
  /// <param name="dto">变量</param>
  /// <returns>是否成功</returns>
  [HttpPut("{id}")]
  public async Task<bool> UpdateAsync(long id, LeanWorkflowVariableDto dto)
  {
    if (id != dto.Id)
    {
      return false;
    }
    return await _service.UpdateAsync(dto);
  }

  /// <summary>
  /// 删除变量
  /// </summary>
  /// <param name="id">变量ID</param>
  /// <returns>是否成功</returns>
  [HttpDelete("{id}")]
  public Task<bool> DeleteAsync(long id)
  {
    return _service.DeleteAsync(id);
  }
}