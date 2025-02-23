using Lean.CodeGen.Application.Dtos.Workflow;
using Lean.CodeGen.Application.Services.Workflow;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Lean.CodeGen.WebApi.Controllers.Workflow;

/// <summary>
/// 工作流历史控制器
/// </summary>
[ApiController]
[Route("api/workflow/histories")]
public class LeanWorkflowHistoryController : LeanBaseController
{
  private readonly ILeanWorkflowHistoryService _service;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="service">工作流历史服务</param>
  public LeanWorkflowHistoryController(ILeanWorkflowHistoryService service)
  {
    _service = service;
  }

  /// <summary>
  /// 获取工作流历史
  /// </summary>
  /// <param name="id">历史ID</param>
  /// <returns>工作流历史</returns>
  [HttpGet("{id}")]
  public Task<LeanWorkflowHistoryDto> GetAsync(long id)
  {
    return _service.GetAsync(id);
  }

  /// <summary>
  /// 创建工作流历史
  /// </summary>
  /// <param name="dto">创建参数</param>
  /// <returns>历史ID</returns>
  [HttpPost]
  public Task<long> CreateAsync(LeanWorkflowHistoryDto dto)
  {
    return _service.CreateAsync(dto);
  }

  /// <summary>
  /// 更新工作流历史
  /// </summary>
  /// <param name="id">历史ID</param>
  /// <param name="dto">更新参数</param>
  /// <returns>是否成功</returns>
  [HttpPut("{id}")]
  public async Task<bool> UpdateAsync(long id, LeanWorkflowHistoryDto dto)
  {
    if (id != dto.Id)
    {
      return false;
    }
    return await _service.UpdateAsync(dto);
  }

  /// <summary>
  /// 删除工作流历史
  /// </summary>
  /// <param name="id">历史ID</param>
  /// <returns>是否成功</returns>
  [HttpDelete("{id}")]
  public Task<bool> DeleteAsync(long id)
  {
    return _service.DeleteAsync(id);
  }

  /// <summary>
  /// 分页查询工作流历史
  /// </summary>
  /// <param name="pageIndex">页码</param>
  /// <param name="pageSize">每页大小</param>
  /// <param name="instanceId">实例ID</param>
  /// <param name="taskId">任务ID</param>
  /// <param name="operationType">操作类型</param>
  /// <param name="operatorId">操作人ID</param>
  /// <param name="startTime">开始时间</param>
  /// <param name="endTime">结束时间</param>
  /// <returns>分页结果</returns>
  [HttpGet]
  public Task<LeanPageResult<LeanWorkflowHistoryDto>> GetPagedListAsync(
      [FromQuery] int pageIndex = 1,
      [FromQuery] int pageSize = 10,
      [FromQuery] long? instanceId = null,
      [FromQuery] long? taskId = null,
      [FromQuery] string? operationType = null,
      [FromQuery] long? operatorId = null,
      [FromQuery] DateTime? startTime = null,
      [FromQuery] DateTime? endTime = null)
  {
    return _service.GetPagedListAsync(pageIndex, pageSize, instanceId, taskId, operationType, operatorId, startTime, endTime);
  }
}