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
/// 工作流活动类型控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "workflow")]
public class LeanWorkflowActivityTypeController : LeanBaseController
{
  private readonly ILeanWorkflowActivityTypeService _service;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="service">工作流活动类型服务</param>
  /// <param name="localizationService">本地化服务</param>
  /// <param name="configuration">配置</param>
  /// <param name="userContext">用户上下文</param>
  public LeanWorkflowActivityTypeController(
      ILeanWorkflowActivityTypeService service,
      ILeanLocalizationService localizationService,
      IConfiguration configuration,
      ILeanUserContext userContext)
      : base(localizationService, configuration, userContext)
  {
    _service = service;
  }

  /// <summary>
  /// 获取活动类型列表
  /// </summary>
  /// <returns>活动类型列表</returns>
  [HttpGet("list")]
  public async Task<IActionResult> GetListAsync()
  {
    var result = await _service.GetListAsync();
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 获取活动类型
  /// </summary>
  /// <param name="typeName">活动类型名称</param>
  /// <returns>活动类型</returns>
  [HttpGet("{typeName}")]
  public async Task<IActionResult> GetAsync(string typeName)
  {
    var result = await _service.GetAsync(typeName);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 创建活动类型
  /// </summary>
  /// <param name="dto">活动类型</param>
  /// <returns>是否成功</returns>
  [HttpPost]
  public async Task<IActionResult> CreateAsync(LeanWorkflowActivityTypeDto dto)
  {
    var result = await _service.CreateAsync(dto);
    return Success(result, LeanBusinessType.Create);
  }

  /// <summary>
  /// 更新活动类型
  /// </summary>
  /// <param name="typeName">活动类型名称</param>
  /// <param name="dto">活动类型</param>
  /// <returns>是否成功</returns>
  [HttpPut("{typeName}")]
  public async Task<IActionResult> UpdateAsync(string typeName, LeanWorkflowActivityTypeDto dto)
  {
    if (typeName != dto.TypeName)
    {
      return await ErrorAsync("workflow.activity.type.error.name_mismatch");
    }
    var result = await _service.UpdateAsync(dto);
    return Success(result, LeanBusinessType.Update);
  }

  /// <summary>
  /// 删除活动类型
  /// </summary>
  /// <param name="typeName">活动类型名称</param>
  /// <returns>是否成功</returns>
  [HttpDelete("{typeName}")]
  public async Task<IActionResult> DeleteAsync(string typeName)
  {
    var result = await _service.DeleteAsync(typeName);
    return Success(result, LeanBusinessType.Delete);
  }
}