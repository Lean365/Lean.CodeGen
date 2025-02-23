using Lean.CodeGen.Application.Dtos.Workflow;
using Lean.CodeGen.Application.Services.Workflow;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Lean.CodeGen.WebApi.Controllers.Workflow;

/// <summary>
/// 工作流活动属性控制器
/// </summary>
[ApiController]
[Route("api/workflow/activity-properties")]
public class LeanWorkflowActivityPropertyController : LeanBaseController
{
  private readonly ILeanWorkflowActivityPropertyService _service;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="service">工作流活动属性服务</param>
  public LeanWorkflowActivityPropertyController(ILeanWorkflowActivityPropertyService service)
  {
    _service = service;
  }

  /// <summary>
  /// 获取活动属性
  /// </summary>
  /// <param name="id">属性ID</param>
  /// <returns>活动属性</returns>
  [HttpGet("{id}")]
  public Task<LeanWorkflowActivityPropertyDto?> GetAsync(long id)
  {
    return _service.GetAsync(id);
  }

  /// <summary>
  /// 根据活动ID和属性名称获取活动属性
  /// </summary>
  /// <param name="activityId">活动ID</param>
  /// <param name="propertyName">属性名称</param>
  /// <returns>活动属性</returns>
  [HttpGet("activity/{activityId}/property/{propertyName}")]
  public Task<LeanWorkflowActivityPropertyDto?> GetByNameAsync(long activityId, string propertyName)
  {
    return _service.GetByNameAsync(activityId, propertyName);
  }

  /// <summary>
  /// 创建活动属性
  /// </summary>
  /// <param name="dto">活动属性</param>
  /// <returns>属性ID</returns>
  [HttpPost]
  public Task<long> CreateAsync(LeanWorkflowActivityPropertyDto dto)
  {
    return _service.CreateAsync(dto);
  }

  /// <summary>
  /// 更新活动属性
  /// </summary>
  /// <param name="id">属性ID</param>
  /// <param name="dto">活动属性</param>
  /// <returns>是否成功</returns>
  [HttpPut("{id}")]
  public async Task<bool> UpdateAsync(long id, LeanWorkflowActivityPropertyDto dto)
  {
    if (id != dto.Id)
    {
      return false;
    }
    return await _service.UpdateAsync(dto);
  }

  /// <summary>
  /// 删除活动属性
  /// </summary>
  /// <param name="id">属性ID</param>
  /// <returns>是否成功</returns>
  [HttpDelete("{id}")]
  public Task<bool> DeleteAsync(long id)
  {
    return _service.DeleteAsync(id);
  }
}