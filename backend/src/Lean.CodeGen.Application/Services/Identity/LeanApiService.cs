using System.Linq.Expressions;
using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Domain.Entities.Identity;
using Lean.CodeGen.Domain.Interfaces.Repositories;
using Lean.CodeGen.Domain.Validators;
using Mapster;
using Lean.CodeGen.Common.Options;
using Lean.CodeGen.Application.Services.Base;
using Lean.CodeGen.Application.Services.Security;
using Microsoft.Extensions.Options;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Extensions;

namespace Lean.CodeGen.Application.Services.Identity;

/// <summary>
/// API服务实现
/// </summary>
public class LeanApiService : LeanBaseService, ILeanApiService
{
  private readonly ILeanRepository<LeanApi> _apiRepository;
  private readonly ILeanRepository<LeanUserApi> _userApiRepository;
  private readonly ILeanRepository<LeanRoleApi> _roleApiRepository;
  private readonly ILeanRepository<LeanApiRateLimit> _apiRateLimitRepository;
  private readonly ILeanRepository<LeanApiAccessLog> _apiAccessLogRepository;
  private readonly LeanUniqueValidator<LeanApi> _uniqueValidator;

  public LeanApiService(
      ILeanRepository<LeanApi> apiRepository,
      ILeanRepository<LeanUserApi> userApiRepository,
      ILeanRepository<LeanRoleApi> roleApiRepository,
      ILeanRepository<LeanApiRateLimit> apiRateLimitRepository,
      ILeanRepository<LeanApiAccessLog> apiAccessLogRepository,
      ILeanSqlSafeService sqlSafeService,
      IOptions<LeanSecurityOptions> securityOptions)
      : base(sqlSafeService, securityOptions)
  {
    _apiRepository = apiRepository;
    _userApiRepository = userApiRepository;
    _roleApiRepository = roleApiRepository;
    _apiRateLimitRepository = apiRateLimitRepository;
    _apiAccessLogRepository = apiAccessLogRepository;
    _uniqueValidator = new LeanUniqueValidator<LeanApi>(_apiRepository);
  }

  /// <summary>
  /// 创建API
  /// </summary>
  public async Task<LeanApiResult<long>> CreateAsync(LeanCreateApiDto input)
  {
    try
    {
      // 验证输入
      if (!await ValidateApiInputAsync(input.ApiName, input.Path, input.Method))
      {
        return LeanApiResult<long>.Error("API输入验证失败");
      }

      // 创建API
      var api = input.Adapt<LeanApi>();
      var id = await _apiRepository.CreateAsync(api);

      return LeanApiResult<long>.Ok(id);
    }
    catch (Exception ex)
    {
      return LeanApiResult<long>.Error($"创建API失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 更新API
  /// </summary>
  public async Task<LeanApiResult> UpdateAsync(LeanUpdateApiDto input)
  {
    try
    {
      var api = await GetApiByIdAsync(input.Id);
      if (api == null)
      {
        return LeanApiResult.Error($"API {input.Id} 不存在");
      }

      // 验证输入
      if (!await ValidateApiInputAsync(input.ApiName, input.Path, input.Method, input.Id))
      {
        return LeanApiResult.Error("API输入验证失败");
      }

      // 更新API
      input.Adapt(api);
      await _apiRepository.UpdateAsync(api);

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"更新API失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 删除API
  /// </summary>
  public async Task<LeanApiResult> DeleteAsync(long id)
  {
    try
    {
      var api = await GetApiByIdAsync(id);
      if (api == null)
      {
        return LeanApiResult.Error($"API {id} 不存在");
      }

      if (api.IsBuiltin == LeanBuiltinStatus.Yes)
      {
        return LeanApiResult.Error($"API {api.ApiName} 是内置API，不能删除");
      }

      // 删除API及其关联数据
      await DeleteApiRelationsAsync(id);
      await _apiRepository.DeleteAsync(api);

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"删除API失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 批量删除API
  /// </summary>
  public async Task<LeanApiResult> BatchDeleteAsync(List<long> ids)
  {
    try
    {
      var apis = await _apiRepository.GetListAsync(a => ids.Contains(a.Id));
      if (apis.Any(a => a.IsBuiltin == LeanBuiltinStatus.Yes))
      {
        return LeanApiResult.Error("选中的API中包含内置API，不能删除");
      }

      foreach (var id in ids)
      {
        await DeleteApiRelationsAsync(id);
      }
      await _apiRepository.DeleteAsync(a => ids.Contains(a.Id));

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"批量删除API失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取API信息
  /// </summary>
  public async Task<LeanApiResult<LeanApiDto>> GetAsync(long id)
  {
    try
    {
      var api = await GetApiByIdAsync(id);
      if (api == null)
      {
        return LeanApiResult<LeanApiDto>.Error($"API {id} 不存在");
      }

      var dto = api.Adapt<LeanApiDto>();
      return LeanApiResult<LeanApiDto>.Ok(dto);
    }
    catch (Exception ex)
    {
      return LeanApiResult<LeanApiDto>.Error($"获取API信息失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 分页查询API
  /// </summary>
  public async Task<LeanApiResult<LeanPageResult<LeanApiDto>>> GetPageAsync(LeanQueryApiDto input)
  {
    try
    {
      var predicate = BuildApiQueryPredicate(input);
      var (total, apis) = await _apiRepository.GetPageListAsync(predicate, input.PageSize, input.PageIndex);
      var dtos = apis.Adapt<List<LeanApiDto>>();

      return LeanApiResult<LeanPageResult<LeanApiDto>>.Ok(new LeanPageResult<LeanApiDto>
      {
        Total = total,
        Items = dtos
      });
    }
    catch (Exception ex)
    {
      return LeanApiResult<LeanPageResult<LeanApiDto>>.Error($"分页查询API失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 修改API状态
  /// </summary>
  public async Task<LeanApiResult> SetStatusAsync(LeanChangeApiStatusDto input)
  {
    try
    {
      var api = await GetApiByIdAsync(input.Id);
      if (api == null)
      {
        return LeanApiResult.Error($"API {input.Id} 不存在");
      }

      if (api.IsBuiltin == LeanBuiltinStatus.Yes && input.ApiStatus == LeanApiStatus.Disabled)
      {
        return LeanApiResult.Error($"API {api.ApiName} 是内置API，不能禁用");
      }

      api.ApiStatus = input.ApiStatus;
      await _apiRepository.UpdateAsync(api);

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"修改API状态失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取API树形结构
  /// </summary>
  public async Task<LeanApiResult<List<LeanApiTreeDto>>> GetTreeAsync()
  {
    try
    {
      var apis = await _apiRepository.GetListAsync(_ => true);
      var result = BuildApiTree(apis);
      return LeanApiResult<List<LeanApiTreeDto>>.Ok(result);
    }
    catch (Exception ex)
    {
      return LeanApiResult<List<LeanApiTreeDto>>.Error($"获取API树形结构失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取用户API权限列表
  /// </summary>
  public async Task<LeanApiResult<List<string>>> GetUserApiPermissionsAsync(long userId)
  {
    try
    {
      var userApis = await _userApiRepository.GetListAsync(ua => ua.UserId == userId);
      var apiIds = userApis.Select(ua => ua.ApiId).ToList();
      var apis = await _apiRepository.GetListAsync(a => apiIds.Contains(a.Id));
      var permissions = apis.Select(a => $"{a.Method}:{a.Path}").ToList();

      return LeanApiResult<List<string>>.Ok(permissions);
    }
    catch (Exception ex)
    {
      return LeanApiResult<List<string>>.Error($"获取用户API权限列表失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取角色API树
  /// </summary>
  public async Task<LeanApiResult<List<LeanApiTreeDto>>> GetRoleApiTreeAsync(long roleId)
  {
    try
    {
      var roleApis = await _roleApiRepository.GetListAsync(ra => ra.RoleId == roleId);
      var apiIds = roleApis.Select(ra => ra.ApiId).ToList();
      var apis = await _apiRepository.GetListAsync(_ => true);
      var result = BuildApiTree(apis);

      // 标记角色已分配的API
      MarkAssignedApis(result, apiIds);

      return LeanApiResult<List<LeanApiTreeDto>>.Ok(result);
    }
    catch (Exception ex)
    {
      return LeanApiResult<List<LeanApiTreeDto>>.Error($"获取角色API树失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 同步API数据
  /// </summary>
  public async Task<LeanApiResult> SyncApisAsync()
  {
    try
    {
      // TODO: 实现API数据同步逻辑
      await Task.CompletedTask;
      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"同步API数据失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 设置API访问频率限制
  /// </summary>
  public async Task<LeanApiResult> SetApiRateLimitAsync(LeanSetApiRateLimitDto input)
  {
    try
    {
      var api = await GetApiByIdAsync(input.ApiId);
      if (api == null)
      {
        return LeanApiResult.Error($"API {input.ApiId} 不存在");
      }

      await _apiRateLimitRepository.DeleteAsync(rl => rl.ApiId == input.ApiId);
      var rateLimit = input.Adapt<LeanApiRateLimit>();
      await _apiRateLimitRepository.CreateAsync(rateLimit);

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"设置API访问频率限制失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取API访问频率限制
  /// </summary>
  public async Task<LeanApiResult<LeanApiRateLimitDto>> GetApiRateLimitAsync(long apiId)
  {
    try
    {
      var rateLimit = await _apiRateLimitRepository.FirstOrDefaultAsync(rl => rl.ApiId == apiId);
      if (rateLimit == null)
      {
        return LeanApiResult<LeanApiRateLimitDto>.Error($"API {apiId} 未设置访问频率限制");
      }

      var dto = rateLimit.Adapt<LeanApiRateLimitDto>();
      return LeanApiResult<LeanApiRateLimitDto>.Ok(dto);
    }
    catch (Exception ex)
    {
      return LeanApiResult<LeanApiRateLimitDto>.Error($"获取API访问频率限制失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 记录API调用日志
  /// </summary>
  public async Task<LeanApiResult> LogApiAccessAsync(LeanApiAccessLogDto input)
  {
    try
    {
      var log = input.Adapt<LeanApiAccessLog>();
      await _apiAccessLogRepository.CreateAsync(log);
      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"记录API调用日志失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取API调用日志
  /// </summary>
  public async Task<LeanApiResult<LeanPageResult<LeanApiAccessLogDto>>> GetApiAccessLogsAsync(LeanQueryApiAccessLogDto input)
  {
    try
    {
      var predicate = BuildApiAccessLogQueryPredicate(input);
      var (total, logs) = await _apiAccessLogRepository.GetPageListAsync(predicate, input.PageSize, input.PageIndex);
      var dtos = logs.Adapt<List<LeanApiAccessLogDto>>();

      return LeanApiResult<LeanPageResult<LeanApiAccessLogDto>>.Ok(new LeanPageResult<LeanApiAccessLogDto>
      {
        Total = total,
        Items = dtos
      });
    }
    catch (Exception ex)
    {
      return LeanApiResult<LeanPageResult<LeanApiAccessLogDto>>.Error($"获取API调用日志失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 验证API访问频率
  /// </summary>
  public async Task<LeanApiResult<bool>> ValidateApiRateLimitAsync(long userId, string path, string method)
  {
    try
    {
      var api = await _apiRepository.FirstOrDefaultAsync(a => a.Path == path && a.Method == method);
      if (api == null)
      {
        return LeanApiResult<bool>.Error($"API {method}:{path} 不存在");
      }

      var rateLimit = await _apiRateLimitRepository.FirstOrDefaultAsync(rl => rl.ApiId == api.Id);
      if (rateLimit == null)
      {
        return LeanApiResult<bool>.Ok(true);
      }

      var now = DateTime.Now;
      var startTime = now.AddSeconds(-rateLimit.TimeWindow);
      var count = await _apiAccessLogRepository.CountAsync(log =>
          log.UserId == userId &&
          log.Path == path &&
          log.Method == method &&
          log.AccessTime >= startTime &&
          log.AccessTime <= now);

      return LeanApiResult<bool>.Ok(count < rateLimit.MaxRequests);
    }
    catch (Exception ex)
    {
      return LeanApiResult<bool>.Error($"验证API访问频率失败: {ex.Message}");
    }
  }

  #region 私有方法

  /// <summary>
  /// 获取API
  /// </summary>
  private async Task<LeanApi?> GetApiByIdAsync(long id)
  {
    return await _apiRepository.GetByIdAsync(id);
  }

  /// <summary>
  /// 验证API输入
  /// </summary>
  private async Task<bool> ValidateApiInputAsync(string apiName, string path, string method, long? id = null)
  {
    try
    {
      await _uniqueValidator.ValidateAsync(
          (a => a.Path, path, id, $"API路径 {path} 已存在")
      );
      return true;
    }
    catch (Exception)
    {
      return false;
    }
  }

  /// <summary>
  /// 删除API关联数据
  /// </summary>
  private async Task DeleteApiRelationsAsync(long apiId)
  {
    await _userApiRepository.DeleteAsync(ua => ua.ApiId == apiId);
    await _roleApiRepository.DeleteAsync(ra => ra.ApiId == apiId);
    await _apiRateLimitRepository.DeleteAsync(rl => rl.ApiId == apiId);
    await _apiAccessLogRepository.DeleteAsync(log => log.ApiId == apiId);
  }

  /// <summary>
  /// 构建API查询条件
  /// </summary>
  private Expression<Func<LeanApi, bool>> BuildApiQueryPredicate(LeanQueryApiDto input)
  {
    Expression<Func<LeanApi, bool>> predicate = a => true;

    if (!string.IsNullOrEmpty(input.ApiName))
    {
      var apiName = CleanInput(input.ApiName);
      predicate = predicate.And(a => a.ApiName.Contains(apiName));
    }

    if (!string.IsNullOrEmpty(input.Path))
    {
      var path = CleanInput(input.Path);
      predicate = predicate.And(a => a.Path.Contains(path));
    }

    if (!string.IsNullOrEmpty(input.Method))
    {
      var method = CleanInput(input.Method);
      predicate = predicate.And(a => a.Method == method);
    }

    if (!string.IsNullOrEmpty(input.Module))
    {
      var module = CleanInput(input.Module);
      predicate = predicate.And(a => a.Module.Contains(module));
    }

    if (input.ApiStatus.HasValue)
    {
      predicate = predicate.And(a => a.ApiStatus == input.ApiStatus.Value);
    }

    if (input.StartTime.HasValue)
    {
      predicate = predicate.And(a => a.CreateTime >= input.StartTime.Value);
    }

    if (input.EndTime.HasValue)
    {
      predicate = predicate.And(a => a.CreateTime <= input.EndTime.Value);
    }

    return predicate;
  }

  /// <summary>
  /// 构建API访问日志查询条件
  /// </summary>
  private Expression<Func<LeanApiAccessLog, bool>> BuildApiAccessLogQueryPredicate(LeanQueryApiAccessLogDto input)
  {
    Expression<Func<LeanApiAccessLog, bool>> predicate = log => true;

    if (!string.IsNullOrEmpty(input.Path))
    {
      var path = CleanInput(input.Path);
      predicate = predicate.And(log => log.Path.Contains(path));
    }

    if (!string.IsNullOrEmpty(input.Method))
    {
      var method = CleanInput(input.Method);
      predicate = predicate.And(log => log.Method == method);
    }

    if (input.UserId.HasValue)
    {
      predicate = predicate.And(log => log.UserId == input.UserId.Value);
    }

    if (input.StartTime.HasValue)
    {
      predicate = predicate.And(log => log.AccessTime >= input.StartTime.Value);
    }

    if (input.EndTime.HasValue)
    {
      predicate = predicate.And(log => log.AccessTime <= input.EndTime.Value);
    }

    return predicate;
  }

  /// <summary>
  /// 构建API树形结构
  /// </summary>
  private List<LeanApiTreeDto> BuildApiTree(List<LeanApi> apis)
  {
    var result = new List<LeanApiTreeDto>();
    var moduleGroups = apis.GroupBy(a => a.Module).OrderBy(g => g.Key);

    foreach (var group in moduleGroups)
    {
      var moduleNode = new LeanApiTreeDto
      {
        Id = 0,
        ApiName = group.Key,
        Path = "#",
        Method = "GROUP",
        Module = group.Key,
        Children = group.OrderBy(a => a.OrderNum)
                      .ThenBy(a => a.ApiName)
                      .Select(a => a.Adapt<LeanApiTreeDto>())
                      .ToList()
      };
      result.Add(moduleNode);
    }

    return result;
  }

  /// <summary>
  /// 标记已分配的API
  /// </summary>
  private void MarkAssignedApis(List<LeanApiTreeDto> tree, List<long> assignedApiIds)
  {
    foreach (var node in tree)
    {
      if (node.Children?.Any() == true)
      {
        MarkAssignedApis(node.Children, assignedApiIds);
      }
      else if (assignedApiIds.Contains(node.Id))
      {
        node.IsAssigned = true;
      }
    }
  }

  #endregion
}