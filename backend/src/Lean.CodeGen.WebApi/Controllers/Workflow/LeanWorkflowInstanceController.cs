using Lean.CodeGen.Application.Dtos.Workflow;
using Lean.CodeGen.Application.Services.Workflow;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Lean.CodeGen.Application.Services.Admin;

namespace Lean.CodeGen.WebApi.Controllers.Workflow;

/// <summary>
/// 工作流实例控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "workflow")]
public class LeanWorkflowInstanceController : LeanBaseController
{
    private readonly ILeanWorkflowInstanceService _service;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="service">工作流实例服务</param>
    /// <param name="localizationService">本地化服务</param>
    /// <param name="configuration">配置</param>
    public LeanWorkflowInstanceController(
        ILeanWorkflowInstanceService service,
        ILeanLocalizationService localizationService,
        IConfiguration configuration)
        : base(localizationService, configuration)
    {
        _service = service;
    }

    /// <summary>
    /// 获取工作流实例
    /// </summary>
    /// <param name="id">工作流实例ID</param>
    /// <returns>工作流实例</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(long id)
    {
        var result = await _service.GetAsync(id);
        return Success(result, LeanBusinessType.Query);
    }

    /// <summary>
    /// 根据业务主键获取工作流实例
    /// </summary>
    /// <param name="businessKey">业务主键</param>
    /// <returns>工作流实例</returns>
    [HttpGet("business-key/{businessKey}")]
    public async Task<IActionResult> GetByBusinessKeyAsync(string businessKey)
    {
        var result = await _service.GetByBusinessKeyAsync(businessKey);
        return Success(result, LeanBusinessType.Query);
    }

    /// <summary>
    /// 创建工作流实例
    /// </summary>
    /// <param name="dto">工作流实例</param>
    /// <returns>工作流实例ID</returns>
    [HttpPost]
    public async Task<IActionResult> CreateAsync(LeanWorkflowInstanceDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return Success(result, LeanBusinessType.Create);
    }

    /// <summary>
    /// 更新工作流实例
    /// </summary>
    /// <param name="id">工作流实例ID</param>
    /// <param name="dto">工作流实例</param>
    /// <returns>是否成功</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(long id, LeanWorkflowInstanceDto dto)
    {
        if (id != dto.Id)
        {
            return Error("ID不匹配");
        }
        var result = await _service.UpdateAsync(dto);
        return Success(result, LeanBusinessType.Update);
    }

    /// <summary>
    /// 删除工作流实例
    /// </summary>
    /// <param name="id">工作流实例ID</param>
    /// <returns>是否成功</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(long id)
    {
        var result = await _service.DeleteAsync(id);
        return Success(result, LeanBusinessType.Delete);
    }

    /// <summary>
    /// 启动工作流实例
    /// </summary>
    /// <param name="id">工作流实例ID</param>
    /// <returns>是否成功</returns>
    [HttpPost("{id}/start")]
    public async Task<IActionResult> StartAsync(long id)
    {
        var result = await _service.StartAsync(id);
        return Success(result, LeanBusinessType.Update);
    }

    /// <summary>
    /// 暂停工作流实例
    /// </summary>
    /// <param name="id">工作流实例ID</param>
    /// <returns>是否成功</returns>
    [HttpPost("{id}/suspend")]
    public async Task<IActionResult> SuspendAsync(long id)
    {
        var result = await _service.SuspendAsync(id);
        return Success(result, LeanBusinessType.Update);
    }

    /// <summary>
    /// 恢复工作流实例
    /// </summary>
    /// <param name="id">工作流实例ID</param>
    /// <returns>是否成功</returns>
    [HttpPost("{id}/resume")]
    public async Task<IActionResult> ResumeAsync(long id)
    {
        var result = await _service.ResumeAsync(id);
        return Success(result, LeanBusinessType.Update);
    }

    /// <summary>
    /// 终止工作流实例
    /// </summary>
    /// <param name="id">工作流实例ID</param>
    /// <returns>是否成功</returns>
    [HttpPost("{id}/terminate")]
    public async Task<IActionResult> TerminateAsync(long id)
    {
        var result = await _service.TerminateAsync(id);
        return Success(result, LeanBusinessType.Update);
    }

    /// <summary>
    /// 归档工作流实例
    /// </summary>
    /// <param name="id">工作流实例ID</param>
    /// <returns>是否成功</returns>
    [HttpPost("{id}/archive")]
    public async Task<IActionResult> ArchiveAsync(long id)
    {
        var result = await _service.ArchiveAsync(id);
        return Success(result, LeanBusinessType.Update);
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
    public async Task<IActionResult> GetPagedListAsync(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] long? definitionId = null,
        [FromQuery] string? businessKey = null,
        [FromQuery] string? businessType = null,
        [FromQuery] string? title = null,
        [FromQuery] long? initiatorId = null,
        [FromQuery] int? workflowStatus = null)
    {
        var result = await _service.GetPagedListAsync(pageIndex, pageSize, definitionId, businessKey, businessType, title, initiatorId, workflowStatus);
        return Success(result, LeanBusinessType.Query);
    }
}