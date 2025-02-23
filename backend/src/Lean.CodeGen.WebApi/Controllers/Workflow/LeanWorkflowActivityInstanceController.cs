using Lean.CodeGen.Application.Dtos.Workflow;
using Lean.CodeGen.Application.Services.Workflow;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Lean.CodeGen.WebApi.Controllers.Workflow;

/// <summary>
/// 工作流活动实例控制器
/// </summary>
[ApiController]
[Route("api/workflow/activity-instances")]
[ApiExplorerSettings(GroupName = "workflow")]
public class LeanWorkflowActivityInstanceController : LeanBaseController
{
  private readonly ILeanWorkflowActivityInstanceService _service;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="service">工作流活动实例服务</param>
  public LeanWorkflowActivityInstanceController(ILeanWorkflowActivityInstanceService service)
  {
    _service = service;
  }

  /// <summary>
  /// 获取活动实例
  /// </summary>
  /// <param name="id">实例ID</param>
  /// <returns>活动实例</returns>
  [HttpGet("{id}")]
  public async Task<LeanApiResult<LeanWorkflowActivityInstanceDto>> GetAsync(long id)
  {
    var result = await _service.GetAsync(id);
    return LeanApiResult<LeanWorkflowActivityInstanceDto>.Ok(result);
  }

  /// <summary>
  /// 创建活动实例
  /// </summary>
  /// <param name="dto">活动实例</param>
  /// <returns>实例ID</returns>
  [HttpPost]
  public async Task<LeanApiResult<long>> CreateAsync(LeanWorkflowActivityInstanceDto dto)
  {
    var result = await _service.CreateAsync(dto);
    return LeanApiResult<long>.Ok(result);
  }

  /// <summary>
  /// 更新活动实例
  /// </summary>
  /// <param name="id">实例ID</param>
  /// <param name="dto">活动实例</param>
  /// <returns>是否成功</returns>
  [HttpPut("{id}")]
  public async Task<LeanApiResult> UpdateAsync(long id, LeanWorkflowActivityInstanceDto dto)
  {
    if (id != dto.Id)
    {
      return LeanApiResult.Error("ID不匹配");
    }
    var result = await _service.UpdateAsync(dto);
    return result ? LeanApiResult.Ok() : LeanApiResult.Error("更新失败");
  }

  /// <summary>
  /// 删除活动实例
  /// </summary>
  /// <param name="id">实例ID</param>
  /// <returns>是否成功</returns>
  [HttpDelete("{id}")]
  public async Task<LeanApiResult> DeleteAsync(long id)
  {
    var result = await _service.DeleteAsync(id);
    return result ? LeanApiResult.Ok() : LeanApiResult.Error("删除失败");
  }

  /// <summary>
  /// 启动活动实例
  /// </summary>
  /// <param name="id">实例ID</param>
  /// <returns>是否成功</returns>
  [HttpPost("{id}/start")]
  public async Task<LeanApiResult> StartAsync(long id)
  {
    var result = await _service.StartAsync(id);
    return result ? LeanApiResult.Ok() : LeanApiResult.Error("启动失败");
  }

  /// <summary>
  /// 完成活动实例
  /// </summary>
  /// <param name="id">实例ID</param>
  /// <returns>是否成功</returns>
  [HttpPost("{id}/complete")]
  public async Task<LeanApiResult> CompleteAsync(long id)
  {
    var result = await _service.CompleteAsync(id);
    return result ? LeanApiResult.Ok() : LeanApiResult.Error("完成失败");
  }

  /// <summary>
  /// 取消活动实例
  /// </summary>
  /// <param name="id">实例ID</param>
  /// <returns>是否成功</returns>
  [HttpPost("{id}/cancel")]
  public async Task<LeanApiResult> CancelAsync(long id)
  {
    var result = await _service.CancelAsync(id);
    return result ? LeanApiResult.Ok() : LeanApiResult.Error("取消失败");
  }

  /// <summary>
  /// 补偿活动实例
  /// </summary>
  /// <param name="id">实例ID</param>
  /// <returns>是否成功</returns>
  [HttpPost("{id}/compensate")]
  public async Task<LeanApiResult> CompensateAsync(long id)
  {
    var result = await _service.CompensateAsync(id);
    return result ? LeanApiResult.Ok() : LeanApiResult.Error("补偿失败");
  }
}