using Lean.CodeGen.Application.Dtos.Workflow;
using Lean.CodeGen.Application.Services.Workflow;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Lean.CodeGen.WebApi.Controllers.Workflow;

/// <summary>
/// 工作流活动类型控制器
/// </summary>
[ApiController]
[Route("api/workflow/activity-types")]
public class LeanWorkflowActivityTypeController : LeanBaseController
{
  private readonly ILeanWorkflowActivityTypeService _service;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="service">工作流活动类型服务</param>
  public LeanWorkflowActivityTypeController(ILeanWorkflowActivityTypeService service)
  {
    _service = service;
  }

  /// <summary>
  /// 获取活动类型列表
  /// </summary>
  /// <returns>活动类型列表</returns>
  [HttpGet("list")]
  public async Task<LeanApiResult<List<LeanWorkflowActivityTypeDto>>> GetListAsync()
  {
    var result = await _service.GetListAsync();
    return LeanApiResult<List<LeanWorkflowActivityTypeDto>>.Ok(result);
  }

  /// <summary>
  /// 获取活动类型
  /// </summary>
  /// <param name="typeName">活动类型名称</param>
  /// <returns>活动类型</returns>
  [HttpGet("{typeName}")]
  public async Task<LeanApiResult<LeanWorkflowActivityTypeDto?>> GetAsync(string typeName)
  {
    var result = await _service.GetAsync(typeName);
    return LeanApiResult<LeanWorkflowActivityTypeDto?>.Ok(result);
  }

  /// <summary>
  /// 创建活动类型
  /// </summary>
  /// <param name="dto">活动类型</param>
  /// <returns>是否成功</returns>
  [HttpPost]
  public async Task<LeanApiResult<bool>> CreateAsync(LeanWorkflowActivityTypeDto dto)
  {
    var result = await _service.CreateAsync(dto);
    return LeanApiResult<bool>.Ok(result);
  }

  /// <summary>
  /// 更新活动类型
  /// </summary>
  /// <param name="typeName">活动类型名称</param>
  /// <param name="dto">活动类型</param>
  /// <returns>是否成功</returns>
  [HttpPut("{typeName}")]
  public async Task<LeanApiResult<bool>> UpdateAsync(string typeName, LeanWorkflowActivityTypeDto dto)
  {
    if (typeName != dto.TypeName)
    {
      return LeanApiResult<bool>.Error("类型名称不匹配");
    }
    var result = await _service.UpdateAsync(dto);
    return LeanApiResult<bool>.Ok(result);
  }

  /// <summary>
  /// 删除活动类型
  /// </summary>
  /// <param name="typeName">活动类型名称</param>
  /// <returns>是否成功</returns>
  [HttpDelete("{typeName}")]
  public async Task<LeanApiResult<bool>> DeleteAsync(string typeName)
  {
    var result = await _service.DeleteAsync(typeName);
    return LeanApiResult<bool>.Ok(result);
  }
}