using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Application.Services.Identity;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Enums;

namespace Lean.CodeGen.WebApi.Controllers.Identity;

/// <summary>
/// 岗位管理控制器
/// </summary>
/// <remarks>
/// 提供岗位管理相关的API接口，包括：
/// 1. 岗位的增删改查
/// 2. 岗位状态管理
/// </remarks>
[Route("api/[controller]")]
public class LeanPostController : LeanBaseController
{
  private readonly ILeanPostService _postService;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanPostController(ILeanPostService postService)
  {
    _postService = postService;
  }

  /// <summary>
  /// 创建岗位
  /// </summary>
  /// <param name="input">岗位创建参数</param>
  /// <returns>创建成功的岗位信息</returns>
  [HttpPost]
  public async Task<IActionResult> CreateAsync([FromBody] LeanCreatePostDto input)
  {
    var result = await _postService.CreateAsync(input);
    return Success(result, LeanBusinessType.Create);
  }

  /// <summary>
  /// 更新岗位
  /// </summary>
  /// <param name="input">岗位更新参数</param>
  /// <returns>更新后的岗位信息</returns>
  [HttpPut]
  public async Task<IActionResult> UpdateAsync([FromBody] LeanUpdatePostDto input)
  {
    var result = await _postService.UpdateAsync(input);
    return Success(result, LeanBusinessType.Update);
  }

  /// <summary>
  /// 删除岗位
  /// </summary>
  /// <param name="input">岗位删除参数</param>
  [HttpDelete]
  public async Task<IActionResult> DeleteAsync([FromBody] LeanDeletePostDto input)
  {
    await _postService.DeleteAsync(input);
    return Success(LeanBusinessType.Delete);
  }

  /// <summary>
  /// 获取岗位信息
  /// </summary>
  /// <param name="id">岗位ID</param>
  /// <returns>岗位详细信息</returns>
  [HttpGet("{id}")]
  public async Task<IActionResult> GetAsync(long id)
  {
    var result = await _postService.GetAsync(id);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 查询岗位列表
  /// </summary>
  /// <param name="input">查询参数</param>
  /// <returns>岗位列表</returns>
  [HttpGet]
  public async Task<IActionResult> QueryAsync([FromQuery] LeanQueryPostDto input)
  {
    var result = await _postService.QueryAsync(input);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 修改岗位状态
  /// </summary>
  /// <param name="input">状态修改参数</param>
  [HttpPut("status")]
  public async Task<IActionResult> ChangeStatusAsync([FromBody] LeanChangePostStatusDto input)
  {
    await _postService.ChangeStatusAsync(input);
    return Success(LeanBusinessType.Update);
  }
}