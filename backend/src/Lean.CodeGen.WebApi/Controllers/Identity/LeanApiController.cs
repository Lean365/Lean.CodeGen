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
  public Task<LeanApiResult<long>> CreateAsync([FromBody] LeanCreateApiDto input)
  {
    return _apiService.CreateAsync(input);
  }

  /// <summary>
  /// 更新API
  /// </summary>
  [HttpPut]
  public Task<LeanApiResult> UpdateAsync([FromBody] LeanUpdateApiDto input)
  {
    return _apiService.UpdateAsync(input);
  }

  /// <summary>
  /// 删除API
  /// </summary>
  [HttpDelete("{id}")]
  public Task<LeanApiResult> DeleteAsync([FromRoute] long id)
  {
    return _apiService.DeleteAsync(id);
  }

  /// <summary>
  /// 批量删除API
  /// </summary>
  [HttpDelete("batch")]
  public Task<LeanApiResult> BatchDeleteAsync([FromBody] List<long> ids)
  {
    return _apiService.BatchDeleteAsync(ids);
  }

  /// <summary>
  /// 获取API信息
  /// </summary>
  [HttpGet("{id}")]
  public Task<LeanApiResult<LeanApiDto>> GetAsync([FromRoute] long id)
  {
    return _apiService.GetAsync(id);
  }

  /// <summary>
  /// 分页查询API
  /// </summary>
  [HttpGet("page")]
  public Task<LeanApiResult<LeanPageResult<LeanApiDto>>> GetPageAsync([FromQuery] LeanQueryApiDto input)
  {
    return _apiService.GetPageAsync(input);
  }

  /// <summary>
  /// 修改API状态
  /// </summary>
  [HttpPut("status")]
  public Task<LeanApiResult> SetStatusAsync([FromBody] LeanChangeApiStatusDto input)
  {
    return _apiService.SetStatusAsync(input);
  }

  /// <summary>
  /// 获取API树形结构
  /// </summary>
  [HttpGet("tree")]
  public Task<LeanApiResult<List<LeanApiTreeDto>>> GetTreeAsync()
  {
    return _apiService.GetTreeAsync();
  }

  /// <summary>
  /// 获取用户API权限列表
  /// </summary>
  [HttpGet("user/{userId}/permissions")]
  public Task<LeanApiResult<List<string>>> GetUserApiPermissionsAsync([FromRoute] long userId)
  {
    return _apiService.GetUserApiPermissionsAsync(userId);
  }

  /// <summary>
  /// 获取角色API树
  /// </summary>
  [HttpGet("role/{roleId}/tree")]
  public Task<LeanApiResult<List<LeanApiTreeDto>>> GetRoleApiTreeAsync([FromRoute] long roleId)
  {
    return _apiService.GetRoleApiTreeAsync(roleId);
  }

  /// <summary>
  /// 同步API数据
  /// </summary>
  [HttpPost("sync")]
  public Task<LeanApiResult> SyncApisAsync()
  {
    return _apiService.SyncApisAsync();
  }

  /// <summary>
  /// 设置API访问频率限制
  /// </summary>
  [HttpPut("rate-limit")]
  public Task<LeanApiResult> SetApiRateLimitAsync([FromBody] LeanSetApiRateLimitDto input)
  {
    return _apiService.SetApiRateLimitAsync(input);
  }

  /// <summary>
  /// 获取API访问频率限制
  /// </summary>
  [HttpGet("{apiId}/rate-limit")]
  public Task<LeanApiResult<LeanApiRateLimitDto>> GetApiRateLimitAsync([FromRoute] long apiId)
  {
    return _apiService.GetApiRateLimitAsync(apiId);
  }

  /// <summary>
  /// 记录API调用日志
  /// </summary>
  [HttpPost("access-log")]
  public Task<LeanApiResult> LogApiAccessAsync([FromBody] LeanApiAccessLogDto input)
  {
    return _apiService.LogApiAccessAsync(input);
  }

  /// <summary>
  /// 获取API调用日志
  /// </summary>
  [HttpGet("access-logs")]
  public Task<LeanApiResult<LeanPageResult<LeanApiAccessLogDto>>> GetApiAccessLogsAsync([FromQuery] LeanQueryApiAccessLogDto input)
  {
    return _apiService.GetApiAccessLogsAsync(input);
  }

  /// <summary>
  /// 验证API访问频率
  /// </summary>
  [HttpGet("validate-rate-limit")]
  public Task<LeanApiResult<bool>> ValidateApiRateLimitAsync([FromQuery] long userId, [FromQuery] string path, [FromQuery] string method)
  {
    return _apiService.ValidateApiRateLimitAsync(userId, path, method);
  }
}