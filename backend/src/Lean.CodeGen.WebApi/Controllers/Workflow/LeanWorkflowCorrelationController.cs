using Lean.CodeGen.Application.Dtos.Workflow;
using Lean.CodeGen.Application.Services.Workflow;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Lean.CodeGen.WebApi.Controllers.Workflow;

/// <summary>
/// 工作流关联控制器
/// </summary>
[ApiController]
[Route("api/workflow/correlations")]
public class LeanWorkflowCorrelationController : LeanBaseController
{
  private readonly ILeanWorkflowCorrelationService _service;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="service">工作流关联服务</param>
  public LeanWorkflowCorrelationController(ILeanWorkflowCorrelationService service)
  {
    _service = service;
  }

  /// <summary>
  /// 获取工作流关联
  /// </summary>
  /// <param name="id">关联ID</param>
  /// <returns>工作流关联</returns>
  [HttpGet("{id}")]
  public Task<LeanWorkflowCorrelationDto?> GetAsync(long id)
  {
    return _service.GetAsync(id);
  }

  /// <summary>
  /// 根据关联键获取工作流关联
  /// </summary>
  /// <param name="correlationId">关联键</param>
  /// <returns>工作流关联</returns>
  [HttpGet("correlation-id/{correlationId}")]
  public Task<LeanWorkflowCorrelationDto?> GetByCorrelationIdAsync(string correlationId)
  {
    return _service.GetByCorrelationIdAsync(correlationId);
  }

  /// <summary>
  /// 创建工作流关联
  /// </summary>
  /// <param name="dto">工作流关联</param>
  /// <returns>关联ID</returns>
  [HttpPost]
  public Task<long> CreateAsync(LeanWorkflowCorrelationDto dto)
  {
    return _service.CreateAsync(dto);
  }

  /// <summary>
  /// 更新工作流关联
  /// </summary>
  /// <param name="id">关联ID</param>
  /// <param name="dto">工作流关联</param>
  /// <returns>是否成功</returns>
  [HttpPut("{id}")]
  public async Task<bool> UpdateAsync(long id, LeanWorkflowCorrelationDto dto)
  {
    if (id != dto.Id)
    {
      return false;
    }
    return await _service.UpdateAsync(dto);
  }

  /// <summary>
  /// 删除工作流关联
  /// </summary>
  /// <param name="id">关联ID</param>
  /// <returns>是否成功</returns>
  [HttpDelete("{id}")]
  public Task<bool> DeleteAsync(long id)
  {
    return _service.DeleteAsync(id);
  }

  /// <summary>
  /// 分页查询工作流关联
  /// </summary>
  /// <param name="pageIndex">页码</param>
  /// <param name="pageSize">每页大小</param>
  /// <param name="instanceId">工作流实例ID</param>
  /// <param name="correlationId">关联键</param>
  /// <param name="correlationType">关联类型</param>
  /// <param name="status">关联状态</param>
  /// <param name="startTime">开始时间</param>
  /// <param name="endTime">结束时间</param>
  /// <returns>分页结果</returns>
  [HttpGet]
  public Task<LeanPageResult<LeanWorkflowCorrelationDto>> GetPagedListAsync(
      [FromQuery] int pageIndex = 1,
      [FromQuery] int pageSize = 10,
      [FromQuery] long? instanceId = null,
      [FromQuery] string? correlationId = null,
      [FromQuery] string? correlationType = null,
      [FromQuery] bool? status = null,
      [FromQuery] DateTime? startTime = null,
      [FromQuery] DateTime? endTime = null)
  {
    return _service.GetPagedListAsync(pageIndex, pageSize, instanceId, correlationId, correlationType, status, startTime, endTime);
  }
}