using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Application.Services.Identity;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Attributes;
using Microsoft.Extensions.Configuration;
using Lean.CodeGen.Application.Services.Admin;

namespace Lean.CodeGen.WebApi.Controllers.Identity;

/// <summary>
/// 岗位控制器
/// </summary>
/// <remarks>
/// 提供岗位管理相关的API接口，包括：
/// 1. 岗位的增删改查
/// 2. 岗位状态管理
/// </remarks>
[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "identity")]
[LeanPermission("identity:post", "岗位管理")]
public class LeanPostController : LeanBaseController
{
  private readonly ILeanPostService _postService;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="postService">岗位服务</param>
  /// <param name="localizationService">本地化服务</param>
  /// <param name="configuration">配置</param>
  /// <param name="userContext">用户上下文</param>
  public LeanPostController(
      ILeanPostService postService,
      ILeanLocalizationService localizationService,
      IConfiguration configuration,
      ILeanUserContext userContext)
      : base(localizationService, configuration, userContext)
  {
    _postService = postService;
  }

  /// <summary>
  /// 分页查询岗位列表
  /// </summary>
  [LeanPermission("identity:post:list", "查询岗位")]
  [HttpGet("list")]
  public async Task<LeanApiResult<LeanPageResult<LeanPostDto>>> GetPageAsync([FromQuery] LeanPostQueryDto input)
  {
    return await _postService.GetPageAsync(input);
  }

  /// <summary>
  /// 获取岗位详情
  /// </summary>
  [LeanPermission("identity:post:query", "查询岗位")]
  [HttpGet("{id}")]
  public async Task<LeanApiResult<LeanPostDto>> GetAsync(long id)
  {
    return await _postService.GetAsync(id);
  }

  /// <summary>
  /// 新增岗位
  /// </summary>
  [LeanPermission("identity:post:add", "新增岗位")]
  [HttpPost]
  public async Task<LeanApiResult> CreateAsync([FromBody] LeanPostCreateDto input)
  {
    return await _postService.CreateAsync(input);
  }

  /// <summary>
  /// 更新岗位
  /// </summary>
  [LeanPermission("identity:post:edit", "修改岗位")]
  [HttpPut]
  public async Task<LeanApiResult> UpdateAsync([FromBody] LeanPostUpdateDto input)
  {
    return await _postService.UpdateAsync(input);
  }

  /// <summary>
  /// 删除岗位
  /// </summary>
  [LeanPermission("identity:post:delete", "删除岗位")]
  [HttpDelete("{id}")]
  public async Task<LeanApiResult> DeleteAsync(long id)
  {
    return await _postService.DeleteAsync(id);
  }

  /// <summary>
  /// 批量删除岗位
  /// </summary>
  [LeanPermission("identity:post:delete", "删除岗位")]
  [HttpDelete("batch")]
  public async Task<LeanApiResult> BatchDeleteAsync([FromBody] List<long> ids)
  {
    return await _postService.BatchDeleteAsync(ids);
  }

  /// <summary>
  /// 导出岗位列表
  /// </summary>
  [LeanPermission("identity:post:export", "导出岗位")]
  [HttpGet("export")]
  public async Task<IActionResult> ExportAsync([FromQuery] LeanPostQueryDto input)
  {
    var bytes = await _postService.ExportAsync(input);
    return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "posts.xlsx");
  }

  /// <summary>
  /// 获取岗位导入模板
  /// </summary>
  [LeanPermission("identity:post:import", "导入岗位")]
  [HttpGet("template")]
  public async Task<IActionResult> GetTemplateAsync()
  {
    var bytes = await _postService.GetTemplateAsync();
    return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "post-template.xlsx");
  }

  /// <summary>
  /// 导入岗位列表
  /// </summary>
  [LeanPermission("identity:post:import", "导入岗位")]
  [HttpPost("import")]
  public async Task<IActionResult> ImportAsync([FromForm] LeanFileInfo file)
  {
    var result = await _postService.ImportAsync(file);
    return Success(result, LeanBusinessType.Import);
  }

  /// <summary>
  /// 获取用户岗位列表
  /// </summary>
  [LeanPermission("identity:post:query", "查询岗位")]
  [HttpGet("user/{userId}")]
  public async Task<LeanApiResult<List<LeanPostDto>>> GetUserPostsAsync(long userId)
  {
    return await _postService.GetUserPostsAsync(userId);
  }

  /// <summary>
  /// 设置用户岗位
  /// </summary>
  [LeanPermission("identity:post:edit", "修改岗位")]
  [HttpPut("user")]
  public async Task<LeanApiResult> SetUserPostsAsync([FromBody] LeanUserPostDto input)
  {
    return await _postService.SetUserPostsAsync(input);
  }
}