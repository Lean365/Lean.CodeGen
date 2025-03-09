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
/// 工作流变量数据控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "workflow")]
public class LeanWorkflowVariableDataController : LeanBaseController
{
    private readonly ILeanWorkflowVariableDataService _service;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="service">工作流变量数据服务</param>
    /// <param name="localizationService">本地化服务</param>
    /// <param name="configuration">配置</param>
    public LeanWorkflowVariableDataController(
        ILeanWorkflowVariableDataService service,
        ILeanLocalizationService localizationService,
        IConfiguration configuration)
        : base(localizationService, configuration)
    {
        _service = service;
    }

    /// <summary>
    /// 获取变量数据
    /// </summary>
    /// <param name="id">变量数据ID</param>
    /// <returns>变量数据</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(long id)
    {
        var result = await _service.GetAsync(id);
        return Success(result, LeanBusinessType.Query);
    }

    /// <summary>
    /// 根据实例ID和变量名称获取变量数据
    /// </summary>
    /// <param name="instanceId">工作流实例ID</param>
    /// <param name="variableName">变量名称</param>
    /// <returns>变量数据</returns>
    [HttpGet("instance/{instanceId}/variable/{variableName}")]
    public async Task<IActionResult> GetByNameAsync(long instanceId, string variableName)
    {
        var result = await _service.GetByNameAsync(instanceId, variableName);
        return Success(result, LeanBusinessType.Query);
    }

    /// <summary>
    /// 创建变量数据
    /// </summary>
    /// <param name="dto">变量数据</param>
    /// <returns>变量数据ID</returns>
    [HttpPost]
    public async Task<IActionResult> CreateAsync(LeanWorkflowVariableDataDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return Success(result, LeanBusinessType.Create);
    }

    /// <summary>
    /// 更新变量数据
    /// </summary>
    /// <param name="id">变量数据ID</param>
    /// <param name="dto">变量数据</param>
    /// <returns>是否成功</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(long id, LeanWorkflowVariableDataDto dto)
    {
        if (id != dto.Id)
        {
            return await ErrorAsync("workflow.error.id_mismatch");
        }
        var result = await _service.UpdateAsync(dto);
        return Success(result);
    }

    /// <summary>
    /// 删除变量数据
    /// </summary>
    /// <param name="id">变量数据ID</param>
    /// <returns>是否成功</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(long id)
    {
        var result = await _service.DeleteAsync(id);
        return Success(result);
    }
}