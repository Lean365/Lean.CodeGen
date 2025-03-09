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
/// 工作流活动属性控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "workflow")]
public class LeanWorkflowActivityPropertyController : LeanBaseController
{
    private readonly ILeanWorkflowActivityPropertyService _service;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="service">工作流活动属性服务</param>
    /// <param name="localizationService">本地化服务</param>
    /// <param name="configuration">配置</param>
    public LeanWorkflowActivityPropertyController(
        ILeanWorkflowActivityPropertyService service,
        ILeanLocalizationService localizationService,
        IConfiguration configuration)
        : base(localizationService, configuration)
    {
        _service = service;
    }

    /// <summary>
    /// 获取活动属性
    /// </summary>
    /// <param name="id">属性ID</param>
    /// <returns>活动属性</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(long id)
    {
        var result = await _service.GetAsync(id);
        return Success(result, LeanBusinessType.Query);
    }

    /// <summary>
    /// 根据活动ID和属性名称获取活动属性
    /// </summary>
    /// <param name="activityId">活动ID</param>
    /// <param name="propertyName">属性名称</param>
    /// <returns>活动属性</returns>
    [HttpGet("activity/{activityId}/property/{propertyName}")]
    public async Task<IActionResult> GetByNameAsync(long activityId, string propertyName)
    {
        var result = await _service.GetByNameAsync(activityId, propertyName);
        return Success(result, LeanBusinessType.Query);
    }

    /// <summary>
    /// 创建活动属性
    /// </summary>
    /// <param name="dto">活动属性</param>
    /// <returns>属性ID</returns>
    [HttpPost]
    public async Task<IActionResult> CreateAsync(LeanWorkflowActivityPropertyDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return Success(result, LeanBusinessType.Create);
    }

    /// <summary>
    /// 更新活动属性
    /// </summary>
    /// <param name="id">属性ID</param>
    /// <param name="dto">活动属性</param>
    /// <returns>是否成功</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(long id, LeanWorkflowActivityPropertyDto dto)
    {
        if (id != dto.Id)
        {
            return await ErrorAsync("workflow.activity.property.error.id_mismatch");
        }
        var result = await _service.UpdateAsync(dto);
        return Success(result, LeanBusinessType.Update);
    }

    /// <summary>
    /// 删除活动属性
    /// </summary>
    /// <param name="id">属性ID</param>
    /// <returns>是否成功</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(long id)
    {
        var result = await _service.DeleteAsync(id);
        return Success(result, LeanBusinessType.Delete);
    }
}