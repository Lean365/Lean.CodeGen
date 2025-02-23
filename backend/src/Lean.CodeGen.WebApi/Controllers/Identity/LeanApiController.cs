using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Application.Services.Identity;
using Lean.CodeGen.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.WebApi.Controllers;

namespace Lean.CodeGen.WebApi.Controllers.Identity;

/// <summary>
/// API管理
/// </summary>
[Route("api/identity/[controller]")]
[Tags("身份认证")]
[ApiController]
public class LeanApiController : LeanBaseController
{
    private readonly ILeanApiService _apiService;

    public LeanApiController(ILeanApiService apiService)
    {
        _apiService = apiService;
    }

    /// <summary>
    /// 创建API
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] LeanCreateApiDto input)
    {
        var result = await _apiService.CreateAsync(input);
        return ApiResult(result);
    }

    /// <summary>
    /// 更新API
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] LeanUpdateApiDto input)
    {
        var result = await _apiService.UpdateAsync(input);
        return ApiResult(result);
    }

    /// <summary>
    /// 删除API
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] long id)
    {
        var result = await _apiService.DeleteAsync(id);
        return ApiResult(result);
    }

    /// <summary>
    /// 批量删除API
    /// </summary>
    [HttpDelete("batch")]
    public async Task<IActionResult> BatchDeleteAsync([FromBody] List<long> ids)
    {
        var result = await _apiService.BatchDeleteAsync(ids);
        return ApiResult(result);
    }

    /// <summary>
    /// 获取API信息
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync([FromRoute] long id)
    {
        var result = await _apiService.GetAsync(id);
        return ApiResult(result);
    }

    /// <summary>
    /// 分页查询API
    /// </summary>
    [HttpGet("page")]
    public async Task<IActionResult> GetPageAsync([FromQuery] LeanQueryApiDto input)
    {
        var result = await _apiService.GetPageAsync(input);
        return ApiResult(result);
    }

    /// <summary>
    /// 修改API状态
    /// </summary>
    [HttpPut("status")]
    public async Task<IActionResult> SetStatusAsync([FromBody] LeanChangeApiStatusDto input)
    {
        var result = await _apiService.SetStatusAsync(input);
        return ApiResult(result);
    }

    /// <summary>
    /// 获取API树形结构
    /// </summary>
    [HttpGet("tree")]
    public async Task<IActionResult> GetTreeAsync()
    {
        var result = await _apiService.GetTreeAsync();
        return ApiResult(result);
    }

    /// <summary>
    /// 获取用户API权限列表
    /// </summary>
    [HttpGet("user/{userId}/permissions")]
    public async Task<IActionResult> GetUserApiPermissionsAsync([FromRoute] long userId)
    {
        var result = await _apiService.GetUserApiPermissionsAsync(userId);
        return ApiResult(result);
    }

    /// <summary>
    /// 获取角色API树
    /// </summary>
    [HttpGet("role/{roleId}/tree")]
    public async Task<IActionResult> GetRoleApiTreeAsync([FromRoute] long roleId)
    {
        var result = await _apiService.GetRoleApiTreeAsync(roleId);
        return ApiResult(result);
    }

    /// <summary>
    /// 同步API数据
    /// </summary>
    [HttpPost("sync")]
    public async Task<IActionResult> SyncApisAsync()
    {
        var result = await _apiService.SyncApisAsync();
        return ApiResult(result);
    }

    /// <summary>
    /// 设置API访问频率限制
    /// </summary>
    [HttpPut("rate-limit")]
    public async Task<IActionResult> SetApiRateLimitAsync([FromBody] LeanSetApiRateLimitDto input)
    {
        var result = await _apiService.SetApiRateLimitAsync(input);
        return ApiResult(result);
    }

    /// <summary>
    /// 获取API访问频率限制
    /// </summary>
    [HttpGet("{apiId}/rate-limit")]
    public async Task<IActionResult> GetApiRateLimitAsync([FromRoute] long apiId)
    {
        var result = await _apiService.GetApiRateLimitAsync(apiId);
        return ApiResult(result);
    }

    /// <summary>
    /// 记录API调用日志
    /// </summary>
    [HttpPost("access-log")]
    public async Task<IActionResult> LogApiAccessAsync([FromBody] LeanApiAccessLogDto input)
    {
        var result = await _apiService.LogApiAccessAsync(input);
        return ApiResult(result);
    }

    /// <summary>
    /// 获取API调用日志
    /// </summary>
    [HttpGet("access-logs")]
    public async Task<IActionResult> GetApiAccessLogsAsync([FromQuery] LeanQueryApiAccessLogDto input)
    {
        var result = await _apiService.GetApiAccessLogsAsync(input);
        return ApiResult(result);
    }

    /// <summary>
    /// 验证API访问频率
    /// </summary>
    [HttpGet("validate-rate-limit")]
    public async Task<IActionResult> ValidateApiRateLimitAsync([FromQuery] long userId, [FromQuery] string path, [FromQuery] string method)
    {
        var result = await _apiService.ValidateApiRateLimitAsync(userId, path, method);
        return ApiResult(result);
    }
}