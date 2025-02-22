using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Services.Identity;

/// <summary>
/// API服务接口
/// </summary>
public interface ILeanApiService
{
  /// <summary>
  /// 创建API
  /// </summary>
  Task<LeanApiResult<long>> CreateAsync(LeanCreateApiDto input);

  /// <summary>
  /// 更新API
  /// </summary>
  Task<LeanApiResult> UpdateAsync(LeanUpdateApiDto input);

  /// <summary>
  /// 删除API
  /// </summary>
  Task<LeanApiResult> DeleteAsync(long id);

  /// <summary>
  /// 批量删除API
  /// </summary>
  Task<LeanApiResult> BatchDeleteAsync(List<long> ids);

  /// <summary>
  /// 获取API信息
  /// </summary>
  Task<LeanApiResult<LeanApiDto>> GetAsync(long id);

  /// <summary>
  /// 分页查询API
  /// </summary>
  Task<LeanApiResult<LeanPageResult<LeanApiDto>>> GetPageAsync(LeanQueryApiDto input);

  /// <summary>
  /// 修改API状态
  /// </summary>
  Task<LeanApiResult> SetStatusAsync(LeanChangeApiStatusDto input);

  /// <summary>
  /// 获取API树形结构
  /// </summary>
  Task<LeanApiResult<List<LeanApiTreeDto>>> GetTreeAsync();

  /// <summary>
  /// 获取用户API权限列表
  /// </summary>
  Task<LeanApiResult<List<string>>> GetUserApiPermissionsAsync(long userId);

  /// <summary>
  /// 获取角色API树
  /// </summary>
  Task<LeanApiResult<List<LeanApiTreeDto>>> GetRoleApiTreeAsync(long roleId);

  /// <summary>
  /// 同步API数据
  /// </summary>
  Task<LeanApiResult> SyncApisAsync();

  /// <summary>
  /// 设置API访问频率限制
  /// </summary>
  Task<LeanApiResult> SetApiRateLimitAsync(LeanSetApiRateLimitDto input);

  /// <summary>
  /// 获取API访问频率限制
  /// </summary>
  Task<LeanApiResult<LeanApiRateLimitDto>> GetApiRateLimitAsync(long apiId);

  /// <summary>
  /// 记录API调用日志
  /// </summary>
  Task<LeanApiResult> LogApiAccessAsync(LeanApiAccessLogDto input);

  /// <summary>
  /// 获取API调用日志
  /// </summary>
  Task<LeanApiResult<LeanPageResult<LeanApiAccessLogDto>>> GetApiAccessLogsAsync(LeanQueryApiAccessLogDto input);

  /// <summary>
  /// 验证API访问频率
  /// </summary>
  Task<LeanApiResult<bool>> ValidateApiRateLimitAsync(long userId, string path, string method);
}