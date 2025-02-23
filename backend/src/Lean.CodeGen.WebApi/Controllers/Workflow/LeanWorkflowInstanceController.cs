using Lean.CodeGen.Application.Dtos.Workflow;
using Lean.CodeGen.Application.Services.Workflow;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Lean.CodeGen.WebApi.Controllers.Workflow;

/// <summary>
/// 工作流实例控制器
/// </summary>
[ApiController]
[Route("api/workflow/instances")]
public class LeanWorkflowInstanceController : LeanBaseController
{
  private readonly ILeanWorkflowInstanceService _service;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="service">工作流实例服务</param>
  public LeanWorkflowInstanceController(ILeanWorkflowInstanceService service)
  {
    _service = service;
  }

  /// <summary>
  /// 获取工作流实例
  /// </summary>
  /// <param name="id">工作流实例ID</param>
  /// <returns>工作流实例</returns>
  [HttpGet("{id}")]
  public async Task<LeanApiResult<LeanWorkflowInstanceDto>> GetAsync(long id)
  {
    var result = await _service.GetAsync(id);
    return LeanApiResult<LeanWorkflowInstanceDto>.Ok(result);
  }

  /// <summary>
  /// 根据业务主键获取工作流实例
  /// </summary>
  /// <param name="businessKey">业务主键</param>
  /// <returns>工作流实例</returns>
  [HttpGet("business-key/{businessKey}")]
  public async Task<LeanApiResult<LeanWorkflowInstanceDto>> GetByBusinessKeyAsync(string businessKey)
  {
    var result = await _service.GetByBusinessKeyAsync(businessKey);
    return LeanApiResult<LeanWorkflowInstanceDto>.Ok(result);
  }

  /// <summary>
  /// 创建工作流实例
  /// </summary>
  /// <param name="dto">工作流实例</param>
  /// <returns>工作流实例ID</returns>
  [HttpPost]
  public async Task<LeanApiResult<long>> CreateAsync(LeanWorkflowInstanceDto dto)
  {
    var result = await _service.CreateAsync(dto);
    return LeanApiResult<long>.Ok(result);
  }

  /// <summary>
  /// 更新工作流实例
  /// </summary>
  /// <param name="id">工作流实例ID</param>
  /// <param name="dto">工作流实例</param>
  /// <returns>是否成功</returns>
  [HttpPut("{id}")]
  public async Task<LeanApiResult> UpdateAsync(long id, LeanWorkflowInstanceDto dto)
  {
    if (id != dto.Id)
    {
      return LeanApiResult.Error("ID不匹配");
    }
    var result = await _service.UpdateAsync(dto);
    return result ? LeanApiResult.Ok() : LeanApiResult.Error("更新失败");
  }

  /// <summary>
  /// 删除工作流实例
  /// </summary>
  /// <param name="id">工作流实例ID</param>
  /// <returns>是否成功</returns>
  [HttpDelete("{id}")]
  public async Task<LeanApiResult> DeleteAsync(long id)
  {
    var result = await _service.DeleteAsync(id);
    return result ? LeanApiResult.Ok() : LeanApiResult.Error("删除失败");
  }

  /// <summary>
  /// 启动工作流实例
  /// </summary>
  /// <param name="id">工作流实例ID</param>
  /// <returns>是否成功</returns>
  [HttpPost("{id}/start")]
  public async Task<LeanApiResult> StartAsync(long id)
  {
    var result = await _service.StartAsync(id);
    return result ? LeanApiResult.Ok() : LeanApiResult.Error("启动失败");
  }

  /// <summary>
  /// 暂停工作流实例
  /// </summary>
  /// <param name="id">工作流实例ID</param>
  /// <returns>是否成功</returns>
  [HttpPost("{id}/suspend")]
  public async Task<LeanApiResult> SuspendAsync(long id)
  {
    var result = await _service.SuspendAsync(id);
    return result ? LeanApiResult.Ok() : LeanApiResult.Error("暂停失败");
  }

  /// <summary>
  /// 恢复工作流实例
  /// </summary>
  /// <param name="id">工作流实例ID</param>
  /// <returns>是否成功</returns>
  [HttpPost("{id}/resume")]
  public async Task<LeanApiResult> ResumeAsync(long id)
  {
    var result = await _service.ResumeAsync(id);
    return result ? LeanApiResult.Ok() : LeanApiResult.Error("恢复失败");
  }

  /// <summary>
  /// 终止工作流实例
  /// </summary>
  /// <param name="id">工作流实例ID</param>
  /// <returns>是否成功</returns>
  [HttpPost("{id}/terminate")]
  public async Task<LeanApiResult> TerminateAsync(long id)
  {
    var result = await _service.TerminateAsync(id);
    return result ? LeanApiResult.Ok() : LeanApiResult.Error("终止失败");
  }

  /// <summary>
  /// 归档工作流实例
  /// </summary>
  /// <param name="id">工作流实例ID</param>
  /// <returns>是否成功</returns>
  [HttpPost("{id}/archive")]
  public async Task<LeanApiResult> ArchiveAsync(long id)
  {
    var result = await _service.ArchiveAsync(id);
    return result ? LeanApiResult.Ok() : LeanApiResult.Error("归档失败");
  }

  /// <summary>
  /// 分页查询工作流实例
  /// </summary>
  /// <param name="pageIndex">页码</param>
  /// <param name="pageSize">每页大小</param>
  /// <param name="definitionId">工作流定义ID</param>
  /// <param name="businessKey">业务主键</param>
  /// <param name="businessType">业务类型</param>
  /// <param name="title">实例标题</param>
  /// <param name="initiatorId">发起人ID</param>
  /// <param name="workflowStatus">工作流状态</param>
  /// <returns>分页结果</returns>
  [HttpGet]
  public async Task<LeanApiResult<LeanPageResult<LeanWorkflowInstanceDto>>> GetPagedListAsync(
      [FromQuery] int pageIndex = 1,
      [FromQuery] int pageSize = 10,
      [FromQuery] long? definitionId = null,
      [FromQuery] string? businessKey = null,
      [FromQuery] string? businessType = null,
      [FromQuery] string? title = null,
      [FromQuery] long? initiatorId = null,
      [FromQuery] LeanWorkflowInstanceStatus? workflowStatus = null)
  {
    var result = await _service.GetPagedListAsync(pageIndex, pageSize, definitionId, businessKey, businessType, title, initiatorId, workflowStatus);
    return LeanApiResult<LeanPageResult<LeanWorkflowInstanceDto>>.Ok(result);
  }
}