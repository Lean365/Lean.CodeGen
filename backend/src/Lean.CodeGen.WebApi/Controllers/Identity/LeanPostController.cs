using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Application.Services.Identity;
using Lean.CodeGen.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.WebApi.Controllers;

namespace Lean.CodeGen.WebApi.Controllers.Identity;

/// <summary>
/// 岗位管理
/// </summary>
[Route("api/identity/[controller]")]
[Tags("身份认证")]
[ApiController]
public class LeanPostController : LeanBaseController
{
    private readonly ILeanPostService _postService;

    public LeanPostController(ILeanPostService postService)
    {
        _postService = postService;
    }

    /// <summary>
    /// 创建岗位
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] LeanCreatePostDto input)
    {
        var result = await _postService.CreateAsync(input);
        return ApiResult(result);
    }

    /// <summary>
    /// 更新岗位
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] LeanUpdatePostDto input)
    {
        var result = await _postService.UpdateAsync(input);
        return ApiResult(result);
    }

    /// <summary>
    /// 删除岗位
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] long id)
    {
        var result = await _postService.DeleteAsync(id);
        return ApiResult(result);
    }

    /// <summary>
    /// 批量删除岗位
    /// </summary>
    [HttpDelete("batch")]
    public async Task<IActionResult> BatchDeleteAsync([FromBody] List<long> ids)
    {
        var result = await _postService.BatchDeleteAsync(ids);
        return ApiResult(result);
    }

    /// <summary>
    /// 获取岗位信息
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync([FromRoute] long id)
    {
        var result = await _postService.GetAsync(id);
        return ApiResult(result);
    }

    /// <summary>
    /// 分页查询岗位
    /// </summary>
    [HttpGet("page")]
    public async Task<IActionResult> GetPageAsync([FromQuery] LeanQueryPostDto input)
    {
        var result = await _postService.GetPageAsync(input);
        return ApiResult(result);
    }

    /// <summary>
    /// 修改岗位状态
    /// </summary>
    [HttpPut("status")]
    public Task<LeanApiResult> SetStatusAsync([FromBody] LeanChangePostStatusDto input)
    {
        return _postService.SetStatusAsync(input);
    }

    /// <summary>
    /// 获取用户岗位列表
    /// </summary>
    [HttpGet("user/{userId}/posts")]
    public Task<LeanApiResult<List<LeanPostDto>>> GetUserPostsAsync([FromRoute] long userId)
    {
        return _postService.GetUserPostsAsync(userId);
    }

    /// <summary>
    /// 获取部门岗位列表
    /// </summary>
    [HttpGet("dept/{deptId}/posts")]
    public Task<LeanApiResult<List<LeanPostDto>>> GetDeptPostsAsync([FromRoute] long deptId)
    {
        return _postService.GetDeptPostsAsync(deptId);
    }

    /// <summary>
    /// 获取用户的主岗位
    /// </summary>
    [HttpGet("user/{userId}/primary")]
    public Task<LeanApiResult<LeanPostDto>> GetUserPrimaryPostAsync([FromRoute] long userId)
    {
        return _postService.GetUserPrimaryPostAsync(userId);
    }

    /// <summary>
    /// 验证岗位编码是否唯一
    /// </summary>
    [HttpGet("validate-code")]
    public Task<LeanApiResult<bool>> ValidatePostCodeUniqueAsync([FromQuery] string postCode, [FromQuery] long? id = null)
    {
        return _postService.ValidatePostCodeUniqueAsync(postCode, id);
    }

    /// <summary>
    /// 获取岗位的所有用户ID列表
    /// </summary>
    [HttpGet("{postId}/users")]
    public Task<LeanApiResult<List<long>>> GetPostUserIdsAsync([FromRoute] long postId)
    {
        return _postService.GetPostUserIdsAsync(postId);
    }

    /// <summary>
    /// 导入岗位数据
    /// </summary>
    [HttpPost("import")]
    public Task<LeanApiResult<LeanImportPostResultDto>> ImportAsync([FromBody] List<LeanImportTemplatePostDto> posts)
    {
        return _postService.ImportAsync(posts);
    }

    /// <summary>
    /// 导出岗位数据
    /// </summary>
    [HttpGet("export")]
    public Task<LeanApiResult<byte[]>> ExportAsync([FromQuery] LeanExportPostDto input)
    {
        return _postService.ExportAsync(input);
    }

    /// <summary>
    /// 获取岗位继承关系树
    /// </summary>
    [HttpGet("inheritance/tree")]
    public Task<LeanApiResult<List<LeanPostTreeDto>>> GetPostInheritanceTreeAsync()
    {
        return _postService.GetPostInheritanceTreeAsync();
    }

    /// <summary>
    /// 设置岗位继承关系
    /// </summary>
    [HttpPut("inheritance")]
    public Task<LeanApiResult> SetPostInheritanceAsync([FromBody] LeanSetPostInheritanceDto input)
    {
        return _postService.SetPostInheritanceAsync(input);
    }

    /// <summary>
    /// 获取岗位的所有继承权限
    /// </summary>
    [HttpGet("{postId}/inherited-permissions")]
    public Task<LeanApiResult<LeanPostPermissionsDto>> GetPostInheritedPermissionsAsync([FromRoute] long postId)
    {
        return _postService.GetPostInheritedPermissionsAsync(postId);
    }

    /// <summary>
    /// 设置岗位权限
    /// </summary>
    [HttpPut("permissions")]
    public Task<LeanApiResult> SetPostPermissionsAsync([FromBody] LeanSetPostPermissionDto input)
    {
        return _postService.SetPostPermissionsAsync(input);
    }

    /// <summary>
    /// 验证用户是否具有指定岗位的权限
    /// </summary>
    [HttpGet("user/{userId}/validate-permission/{postId}")]
    public Task<LeanApiResult<bool>> ValidateUserPostPermissionAsync([FromRoute] long userId, [FromRoute] long postId)
    {
        return _postService.ValidateUserPostPermissionAsync(userId, postId);
    }
}