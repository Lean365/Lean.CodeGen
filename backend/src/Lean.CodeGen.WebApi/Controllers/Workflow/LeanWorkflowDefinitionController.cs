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
/// 工作流定义控制器
/// </summary>
[ApiController]
[Route("api/workflow/definitions")]
[ApiExplorerSettings(GroupName = "workflow")]
public class LeanWorkflowDefinitionController : LeanBaseController
{
  private readonly ILeanWorkflowDefinitionService _service;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="service">工作流定义服务</param>
  /// <param name="localizationService">本地化服务</param>
  /// <param name="configuration">配置</param>
  public LeanWorkflowDefinitionController(
      ILeanWorkflowDefinitionService service,
      ILeanLocalizationService localizationService,
      IConfiguration configuration)
      : base(localizationService, configuration)
  {
    _service = service;
  }

  /// <summary>
  /// 获取工作流定义列表
  /// </summary>
  /// <returns>工作流定义列表</returns>
  [HttpGet("list")]
  public async Task<IActionResult> GetListAsync()
  {
    var result = await _service.GetListAsync();
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 获取工作流定义
  /// </summary>
  /// <param name="id">工作流定义ID</param>
  /// <returns>工作流定义</returns>
  [HttpGet("{id}")]
  public async Task<IActionResult> GetAsync(long id)
  {
    var result = await _service.GetAsync(id);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 根据编码获取工作流定义
  /// </summary>
  /// <param name="code">工作流编码</param>
  /// <returns>工作流定义</returns>
  [HttpGet("code/{code}")]
  public async Task<IActionResult> GetByCodeAsync(string code)
  {
    var result = await _service.GetByCodeAsync(code);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 创建工作流定义
  /// </summary>
  /// <param name="dto">工作流定义</param>
  /// <returns>工作流定义ID</returns>
  [HttpPost]
  public async Task<IActionResult> CreateAsync(LeanWorkflowDefinitionDto dto)
  {
    var result = await _service.CreateAsync(dto);
    return Success(result, LeanBusinessType.Create);
  }

  /// <summary>
  /// 更新工作流定义
  /// </summary>
  /// <param name="id">工作流定义ID</param>
  /// <param name="dto">工作流定义</param>
  /// <returns>是否成功</returns>
  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateAsync(long id, LeanWorkflowDefinitionDto dto)
  {
    if (id != dto.Id)
    {
      return await ErrorAsync("workflow.error.id_mismatch");
    }
    var result = await _service.UpdateAsync(dto);
    return Success(result, LeanBusinessType.Update);
  }

  /// <summary>
  /// 删除工作流定义
  /// </summary>
  /// <param name="id">工作流定义ID</param>
  /// <returns>是否成功</returns>
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteAsync(long id)
  {
    var result = await _service.DeleteAsync(id);
    return Success(result, LeanBusinessType.Delete);
  }

  /// <summary>
  /// 发布工作流定义
  /// </summary>
  /// <param name="id">工作流定义ID</param>
  /// <returns>是否成功</returns>
  [HttpPost("{id}/publish")]
  public async Task<IActionResult> PublishAsync(long id)
  {
    var result = await _service.PublishAsync(id);
    return Success(result, LeanBusinessType.Update);
  }

  /// <summary>
  /// 停用工作流定义
  /// </summary>
  /// <param name="id">工作流定义ID</param>
  /// <returns>是否成功</returns>
  [HttpPost("{id}/disable")]
  public async Task<IActionResult> DisableAsync(long id)
  {
    var result = await _service.DisableAsync(id);
    return Success(result, LeanBusinessType.Update);
  }

  /// <summary>
  /// 分页查询工作流定义
  /// </summary>
  /// <param name="pageIndex">页码</param>
  /// <param name="pageSize">每页大小</param>
  /// <param name="workflowName">工作流名称</param>
  /// <param name="workflowCode">工作流编码</param>
  /// <param name="status">状态</param>
  /// <returns>分页结果</returns>
  [HttpGet]
  public async Task<IActionResult> GetPagedListAsync(
      [FromQuery] int pageIndex = 1,
      [FromQuery] int pageSize = 10,
      [FromQuery] string? workflowName = null,
      [FromQuery] string? workflowCode = null,
      [FromQuery] int? status = null)
  {
    var result = await _service.GetPagedListAsync(pageIndex, pageSize, workflowName, workflowCode, status);
    return Success(result, LeanBusinessType.Query);
  }
}