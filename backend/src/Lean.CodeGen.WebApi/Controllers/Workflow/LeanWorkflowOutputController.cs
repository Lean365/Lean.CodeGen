using Lean.CodeGen.Application.Dtos.Workflow;
using Lean.CodeGen.Application.Services.Workflow;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Application.Services.Admin;
using Microsoft.Extensions.Configuration;

namespace Lean.CodeGen.WebApi.Controllers.Workflow;

/// <summary>
/// 工作流输出控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "workflow")]
public class LeanWorkflowOutputController : LeanBaseController
{
    private readonly ILeanWorkflowOutputService _service;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="service">工作流输出服务</param>
    /// <param name="localizationService">本地化服务</param>
    /// <param name="configuration">配置</param>
    public LeanWorkflowOutputController(
        ILeanWorkflowOutputService service,
        ILeanLocalizationService localizationService,
        IConfiguration configuration)
        : base(localizationService, configuration)
    {
        _service = service;
    }

    /// <summary>
    /// 获取输出记录
    /// </summary>
    /// <param name="id">输出ID</param>
    /// <returns>输出记录</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(long id)
    {
        var result = await _service.GetAsync(id);
        return Success(result, LeanBusinessType.Query);
    }

    /// <summary>
    /// 创建输出记录
    /// </summary>
    /// <param name="dto">输出记录</param>
    /// <returns>输出ID</returns>
    [HttpPost]
    public async Task<IActionResult> CreateAsync(LeanWorkflowOutputDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return Success(result, LeanBusinessType.Create);
    }

    /// <summary>
    /// 更新输出记录
    /// </summary>
    /// <param name="id">输出ID</param>
    /// <param name="dto">输出记录</param>
    /// <returns>是否成功</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(long id, LeanWorkflowOutputDto dto)
    {
        if (id != dto.Id)
        {
            return await ErrorAsync("workflow.output.error.id_mismatch");
        }
        var result = await _service.UpdateAsync(dto);
        return Success(result, LeanBusinessType.Update);
    }

    /// <summary>
    /// 删除输出记录
    /// </summary>
    /// <param name="id">输出ID</param>
    /// <returns>是否成功</returns>
    [HttpDelete("{id}")]
    public async Task<LeanApiResult<bool>> DeleteAsync(long id)
    {
        var result = await _service.DeleteAsync(id);
        return LeanApiResult<bool>.Ok(result);
    }

    /// <summary>
    /// 分页查询输出记录
    /// </summary>
    /// <param name="pageIndex">页码</param>
    /// <param name="pageSize">每页大小</param>
    /// <param name="activityInstanceId">活动实例ID</param>
    /// <param name="outputName">输出名称</param>
    /// <param name="outputType">输出类型</param>
    /// <returns>分页结果</returns>
    [HttpGet]
    public async Task<LeanApiResult<LeanPageResult<LeanWorkflowOutputDto>>> GetPagedListAsync(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] long? activityInstanceId = null,
        [FromQuery] string? outputName = null,
        [FromQuery] string? outputType = null)
    {
        var result = await _service.GetPagedListAsync(pageIndex, pageSize, activityInstanceId, outputName, outputType);
        return LeanApiResult<LeanPageResult<LeanWorkflowOutputDto>>.Ok(result);
    }
}