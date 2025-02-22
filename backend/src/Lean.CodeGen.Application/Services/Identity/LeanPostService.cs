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
using Microsoft.Extensions.Logging;
using static Lean.CodeGen.Common.Extensions.LeanExpressionExtensions;

namespace Lean.CodeGen.Application.Services.Identity;

/// <summary>
/// 岗位服务实现
/// </summary>
public class LeanPostService : LeanBaseService, ILeanPostService
{
  private readonly ILeanRepository<LeanPost> _postRepository;
  private readonly ILeanRepository<LeanUserPost> _userPostRepository;
  private readonly ILeanRepository<LeanPostInheritance> _postInheritanceRepository;
  private readonly ILeanRepository<LeanPostPermission> _postPermissionRepository;
  private readonly LeanUniqueValidator<LeanPost> _uniqueValidator;
  private readonly ILogger<LeanPostService> _logger;

  public LeanPostService(
      ILeanRepository<LeanPost> postRepository,
      ILeanRepository<LeanUserPost> userPostRepository,
      ILeanRepository<LeanPostInheritance> postInheritanceRepository,
      ILeanRepository<LeanPostPermission> postPermissionRepository,
      ILeanSqlSafeService sqlSafeService,
      IOptions<LeanSecurityOptions> securityOptions,
      ILogger<LeanPostService> logger)
      : base(sqlSafeService, securityOptions, logger)
  {
    _postRepository = postRepository;
    _userPostRepository = userPostRepository;
    _postInheritanceRepository = postInheritanceRepository;
    _postPermissionRepository = postPermissionRepository;
    _uniqueValidator = new LeanUniqueValidator<LeanPost>(postRepository);
    _logger = logger;
  }

  /// <summary>
  /// 创建岗位
  /// </summary>
  public async Task<LeanApiResult<long>> CreateAsync(LeanCreatePostDto input)
  {
    try
    {
      if (!await ValidatePostInputAsync(input.PostName, input.PostCode))
      {
        return LeanApiResult<long>.Error("岗位输入验证失败");
      }

      var post = input.Adapt<LeanPost>();
      var id = await _postRepository.CreateAsync(post);

      return LeanApiResult<long>.Ok(id);
    }
    catch (Exception ex)
    {
      return LeanApiResult<long>.Error($"创建岗位失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 更新岗位
  /// </summary>
  public async Task<LeanApiResult> UpdateAsync(LeanUpdatePostDto input)
  {
    try
    {
      var post = await GetPostByIdAsync(input.Id);
      if (post == null)
      {
        return LeanApiResult.Error($"岗位 {input.Id} 不存在");
      }

      if (!await ValidatePostInputAsync(input.PostName, input.PostCode, input.Id))
      {
        return LeanApiResult.Error("岗位输入验证失败");
      }

      input.Adapt(post);
      await _postRepository.UpdateAsync(post);

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"更新岗位失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 删除岗位
  /// </summary>
  public async Task<LeanApiResult> DeleteAsync(long id)
  {
    try
    {
      var post = await GetPostByIdAsync(id);
      if (post == null)
      {
        return LeanApiResult.Error($"岗位 {id} 不存在");
      }

      if (post.IsBuiltin == LeanBuiltinStatus.Yes)
      {
        return LeanApiResult.Error($"岗位 {post.PostName} 是内置岗位，不能删除");
      }

      await DeletePostRelationsAsync(id);
      await _postRepository.DeleteAsync(post);

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"删除岗位失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 批量删除岗位
  /// </summary>
  public async Task<LeanApiResult> BatchDeleteAsync(List<long> ids)
  {
    try
    {
      var posts = await _postRepository.GetListAsync(p => ids.Contains(p.Id));
      if (posts.Any(p => p.IsBuiltin == LeanBuiltinStatus.Yes))
      {
        return LeanApiResult.Error("选中的岗位中包含内置岗位，不能删除");
      }

      foreach (var id in ids)
      {
        await DeletePostRelationsAsync(id);
      }
      await _postRepository.DeleteAsync(p => ids.Contains(p.Id));

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"批量删除岗位失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取岗位信息
  /// </summary>
  public async Task<LeanApiResult<LeanPostDto>> GetAsync(long id)
  {
    try
    {
      var post = await GetPostByIdAsync(id);
      if (post == null)
      {
        return LeanApiResult<LeanPostDto>.Error($"岗位 {id} 不存在");
      }

      var dto = post.Adapt<LeanPostDto>();
      return LeanApiResult<LeanPostDto>.Ok(dto);
    }
    catch (Exception ex)
    {
      return LeanApiResult<LeanPostDto>.Error($"获取岗位信息失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 分页查询岗位
  /// </summary>
  public async Task<LeanApiResult<LeanPageResult<LeanPostDto>>> GetPageAsync(LeanQueryPostDto input)
  {
    try
    {
      var predicate = BuildPostQueryPredicate(input);
      var (total, posts) = await _postRepository.GetPageListAsync(predicate, input.PageSize, input.PageIndex);
      var dtos = posts.Adapt<List<LeanPostDto>>();

      return LeanApiResult<LeanPageResult<LeanPostDto>>.Ok(new LeanPageResult<LeanPostDto>
      {
        Total = total,
        Items = dtos
      });
    }
    catch (Exception ex)
    {
      return LeanApiResult<LeanPageResult<LeanPostDto>>.Error($"分页查询岗位失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 修改岗位状态
  /// </summary>
  public async Task<LeanApiResult> SetStatusAsync(LeanChangePostStatusDto input)
  {
    try
    {
      var post = await GetPostByIdAsync(input.Id);
      if (post == null)
      {
        return LeanApiResult.Error($"岗位 {input.Id} 不存在");
      }

      if (post.IsBuiltin == LeanBuiltinStatus.Yes && input.PostStatus == LeanPostStatus.Disabled)
      {
        return LeanApiResult.Error($"岗位 {post.PostName} 是内置岗位，不能禁用");
      }

      post.PostStatus = input.PostStatus;
      await _postRepository.UpdateAsync(post);

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"修改岗位状态失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取用户岗位列表
  /// </summary>
  public async Task<LeanApiResult<List<LeanPostDto>>> GetUserPostsAsync(long userId)
  {
    try
    {
      var userPosts = await _userPostRepository.GetListAsync(up => up.UserId == userId);
      var postIds = userPosts.Select(up => up.PostId).ToList();
      var posts = await _postRepository.GetListAsync(p => postIds.Contains(p.Id));
      var dtos = posts.Adapt<List<LeanPostDto>>();

      return LeanApiResult<List<LeanPostDto>>.Ok(dtos);
    }
    catch (Exception ex)
    {
      return LeanApiResult<List<LeanPostDto>>.Error($"获取用户岗位列表失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取部门岗位列表
  /// </summary>
  public async Task<LeanApiResult<List<LeanPostDto>>> GetDeptPostsAsync(long deptId)
  {
    try
    {
      var userPosts = await _userPostRepository.GetListAsync(up => up.User.UserDepts.Any(ud => ud.DeptId == deptId));
      var postIds = userPosts.Select(up => up.PostId).Distinct().ToList();
      var posts = await _postRepository.GetListAsync(p => postIds.Contains(p.Id));
      var dtos = posts.Adapt<List<LeanPostDto>>();

      return LeanApiResult<List<LeanPostDto>>.Ok(dtos);
    }
    catch (Exception ex)
    {
      return LeanApiResult<List<LeanPostDto>>.Error($"获取部门岗位列表失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取用户主岗位
  /// </summary>
  public async Task<LeanApiResult<LeanPostDto>> GetUserPrimaryPostAsync(long userId)
  {
    try
    {
      var userPost = await _userPostRepository.FirstOrDefaultAsync(up => up.UserId == userId && up.IsPrimary == LeanPrimaryStatus.Yes);
      if (userPost == null)
      {
        return LeanApiResult<LeanPostDto>.Error($"用户 {userId} 没有主岗位");
      }

      var post = await _postRepository.GetByIdAsync(userPost.PostId);
      if (post == null)
      {
        return LeanApiResult<LeanPostDto>.Error($"岗位 {userPost.PostId} 不存在");
      }

      var dto = post.Adapt<LeanPostDto>();
      return LeanApiResult<LeanPostDto>.Ok(dto);
    }
    catch (Exception ex)
    {
      return LeanApiResult<LeanPostDto>.Error($"获取用户主岗位失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 验证岗位编码是否唯一
  /// </summary>
  public async Task<LeanApiResult<bool>> ValidatePostCodeUniqueAsync(string postCode, long? id = null)
  {
    try
    {
      var exists = await _postRepository.AnyAsync(p => p.PostCode == postCode && (id == null || p.Id != id));
      return LeanApiResult<bool>.Ok(!exists);
    }
    catch (Exception ex)
    {
      return LeanApiResult<bool>.Error($"验证岗位编码唯一性失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取岗位的所有用户ID列表
  /// </summary>
  public async Task<LeanApiResult<List<long>>> GetPostUserIdsAsync(long postId)
  {
    try
    {
      var userPosts = await _userPostRepository.GetListAsync(up => up.PostId == postId);
      var userIds = userPosts.Select(up => up.UserId).ToList();

      return LeanApiResult<List<long>>.Ok(userIds);
    }
    catch (Exception ex)
    {
      return LeanApiResult<List<long>>.Error($"获取岗位用户列表失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 导入岗位数据
  /// </summary>
  public async Task<LeanApiResult<LeanImportPostResultDto>> ImportAsync(List<LeanImportTemplatePostDto> posts)
  {
    var result = new LeanImportPostResultDto();
    try
    {
      foreach (var postDto in posts)
      {
        if (await _postRepository.AnyAsync(p => p.PostCode == postDto.PostCode))
        {
          result.ErrorMessages.Add($"岗位编码 {postDto.PostCode} 已存在");
          continue;
        }

        var post = new LeanPost
        {
          PostName = postDto.PostName,
          PostCode = postDto.PostCode,
          PostStatus = LeanPostStatus.Normal,
          IsBuiltin = LeanBuiltinStatus.No
        };

        await _postRepository.CreateAsync(post);
        result.SuccessCount++;
      }

      result.TotalCount = posts.Count;
      result.FailCount = result.TotalCount - result.SuccessCount;

      return LeanApiResult<LeanImportPostResultDto>.Ok(result);
    }
    catch (Exception ex)
    {
      return LeanApiResult<LeanImportPostResultDto>.Error($"导入岗位数据失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 导出岗位数据
  /// </summary>
  public async Task<LeanApiResult<byte[]>> ExportAsync(LeanExportPostDto input)
  {
    try
    {
      var predicate = BuildPostQueryPredicate(input);
      var posts = await _postRepository.GetListAsync(predicate);
      var dtos = posts.Adapt<List<LeanPostDto>>();
      var bytes = await ExportToExcel(dtos);

      return LeanApiResult<byte[]>.Ok(bytes);
    }
    catch (Exception ex)
    {
      return LeanApiResult<byte[]>.Error($"导出岗位数据失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取岗位继承关系树
  /// </summary>
  public async Task<LeanApiResult<List<LeanPostTreeDto>>> GetPostInheritanceTreeAsync()
  {
    try
    {
      var posts = await _postRepository.GetListAsync(_ => true);
      var inheritances = await _postInheritanceRepository.GetListAsync(_ => true);

      var result = BuildPostInheritanceTree(posts, inheritances);
      return LeanApiResult<List<LeanPostTreeDto>>.Ok(result);
    }
    catch (Exception ex)
    {
      return LeanApiResult<List<LeanPostTreeDto>>.Error($"获取岗位继承关系树失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 设置岗位继承关系
  /// </summary>
  public async Task<LeanApiResult> SetPostInheritanceAsync(LeanSetPostInheritanceDto input)
  {
    try
    {
      var post = await GetPostByIdAsync(input.PostId);
      if (post == null)
      {
        return LeanApiResult.Error($"岗位 {input.PostId} 不存在");
      }

      await _postInheritanceRepository.DeleteAsync(pi => pi.PostId == input.PostId);

      if (input.InheritedPostIds?.Any() == true)
      {
        var inheritances = input.InheritedPostIds.Select(inheritedPostId => new LeanPostInheritance
        {
          PostId = input.PostId,
          InheritedPostId = inheritedPostId,
        }).ToList();

        await _postInheritanceRepository.CreateRangeAsync(inheritances);
      }

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"设置岗位继承关系失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取岗位继承的权限
  /// </summary>
  public async Task<LeanApiResult<LeanPostPermissionsDto>> GetPostInheritedPermissionsAsync(long postId)
  {
    try
    {
      var post = await GetPostByIdAsync(postId);
      if (post == null)
      {
        return LeanApiResult<LeanPostPermissionsDto>.Error($"岗位 {postId} 不存在");
      }

      var inheritedPostIds = await GetInheritedPostIdsAsync(postId);
      var permissions = await _postPermissionRepository.GetListAsync(pp => inheritedPostIds.Contains(pp.PostId));

      var result = new LeanPostPermissionsDto
      {
        MenuPermissions = permissions.Where(p => p.ResourceType == LeanResourceType.Menu)
                                   .Select(p => p.PermissionName)
                                   .ToList(),
        ApiPermissions = permissions.Where(p => p.ResourceType == LeanResourceType.Api)
                                  .Select(p => p.PermissionName)
                                  .ToList()
      };

      return LeanApiResult<LeanPostPermissionsDto>.Ok(result);
    }
    catch (Exception ex)
    {
      return LeanApiResult<LeanPostPermissionsDto>.Error($"获取岗位继承权限失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 设置岗位权限
  /// </summary>
  public async Task<LeanApiResult> SetPostPermissionsAsync(LeanSetPostPermissionDto input)
  {
    try
    {
      var post = await GetPostByIdAsync(input.PostId);
      if (post == null)
      {
        return LeanApiResult.Error($"岗位 {input.PostId} 不存在");
      }

      await _postPermissionRepository.DeleteAsync(pp => pp.PostId == input.PostId);

      var permissions = new List<LeanPostPermission>();

      if (input.MenuIds?.Any() == true)
      {
        permissions.AddRange(input.MenuIds.Select(menuId => new LeanPostPermission
        {
          PostId = input.PostId,
          ResourceType = LeanResourceType.Menu,
          ResourceId = menuId,
          PermissionName = $"menu:{menuId}",
          Operation = LeanOperationType.View
        }));
      }

      if (input.ApiIds?.Any() == true)
      {
        permissions.AddRange(input.ApiIds.Select(apiId => new LeanPostPermission
        {
          PostId = input.PostId,
          ResourceType = LeanResourceType.Api,
          ResourceId = apiId,
          PermissionName = $"api:{apiId}",
          Operation = LeanOperationType.View
        }));
      }

      if (permissions.Any())
      {
        await _postPermissionRepository.CreateRangeAsync(permissions);
      }

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"设置岗位权限失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 验证用户岗位权限
  /// </summary>
  public async Task<LeanApiResult<bool>> ValidateUserPostPermissionAsync(long userId, long postId)
  {
    try
    {
      var userPost = await _userPostRepository.FirstOrDefaultAsync(up => up.UserId == userId && up.PostId == postId);
      if (userPost == null)
      {
        return LeanApiResult<bool>.Ok(false);
      }

      var post = await _postRepository.GetByIdAsync(postId);
      if (post == null || post.PostStatus == LeanPostStatus.Disabled)
      {
        return LeanApiResult<bool>.Ok(false);
      }

      return LeanApiResult<bool>.Ok(true);
    }
    catch (Exception ex)
    {
      return LeanApiResult<bool>.Error($"验证用户岗位权限失败: {ex.Message}");
    }
  }

  #region 私有方法

  /// <summary>
  /// 获取岗位
  /// </summary>
  private async Task<LeanPost?> GetPostByIdAsync(long id)
  {
    return await _postRepository.GetByIdAsync(id);
  }

  /// <summary>
  /// 验证岗位输入
  /// </summary>
  private async Task<bool> ValidatePostInputAsync(string postName, string postCode, long? id = null)
  {
    try
    {
      await _uniqueValidator.ValidateAsync(
          (p => p.PostCode, postCode, id, $"岗位编码 {postCode} 已存在")
      );
      return true;
    }
    catch (Exception)
    {
      return false;
    }
  }

  /// <summary>
  /// 删除岗位关联数据
  /// </summary>
  private async Task DeletePostRelationsAsync(long postId)
  {
    await _userPostRepository.DeleteAsync(up => up.PostId == postId);
    await _postInheritanceRepository.DeleteAsync(pi => pi.PostId == postId || pi.InheritedPostId == postId);
    await _postPermissionRepository.DeleteAsync(pp => pp.PostId == postId);
  }

  /// <summary>
  /// 构建岗位查询条件
  /// </summary>
  private Expression<Func<LeanPost, bool>> BuildPostQueryPredicate(LeanQueryPostDto input)
  {
    Expression<Func<LeanPost, bool>> predicate = p => true;

    if (!string.IsNullOrEmpty(input.PostName))
    {
      var postName = CleanInput(input.PostName);
      predicate = predicate.And(p => p.PostName.Contains(postName));
    }

    if (!string.IsNullOrEmpty(input.PostCode))
    {
      var postCode = CleanInput(input.PostCode);
      predicate = predicate.And(p => p.PostCode.Contains(postCode));
    }

    if (input.PostStatus.HasValue)
    {
      predicate = predicate.And(p => p.PostStatus == input.PostStatus.Value);
    }

    if (input.StartTime.HasValue)
    {
      predicate = predicate.And(p => p.CreateTime >= input.StartTime.Value);
    }

    if (input.EndTime.HasValue)
    {
      predicate = predicate.And(p => p.CreateTime <= input.EndTime.Value);
    }

    return predicate;
  }

  /// <summary>
  /// 导出到Excel
  /// </summary>
  private async Task<byte[]> ExportToExcel(List<LeanPostDto> posts)
  {
    // TODO: 实现导出到Excel的逻辑
    return Array.Empty<byte>();
  }

  /// <summary>
  /// 构建岗位继承关系树
  /// </summary>
  private List<LeanPostTreeDto> BuildPostInheritanceTree(List<LeanPost> posts, List<LeanPostInheritance> inheritances)
  {
    var result = new List<LeanPostTreeDto>();
    var postDict = posts.ToDictionary(p => p.Id, p => p.Adapt<LeanPostTreeDto>());
    var inheritanceDict = inheritances.GroupBy(i => i.PostId)
                                    .ToDictionary(g => g.Key, g => g.Select(i => i.InheritedPostId).ToList());

    foreach (var post in posts)
    {
      var postNode = postDict[post.Id];
      if (inheritanceDict.TryGetValue(post.Id, out var inheritedPostIds))
      {
        foreach (var inheritedPostId in inheritedPostIds)
        {
          if (postDict.TryGetValue(inheritedPostId, out var inheritedPostNode))
          {
            postNode.Children.Add(inheritedPostNode);
          }
        }
      }
      result.Add(postNode);
    }

    return result;
  }

  /// <summary>
  /// 获取继承的岗位ID列表
  /// </summary>
  private async Task<List<long>> GetInheritedPostIdsAsync(long postId)
  {
    var result = new List<long> { postId };
    var inheritances = await _postInheritanceRepository.GetListAsync(pi => pi.PostId == postId);

    foreach (var inheritance in inheritances)
    {
      var inheritedIds = await GetInheritedPostIdsAsync(inheritance.InheritedPostId);
      result.AddRange(inheritedIds);
    }

    return result.Distinct().ToList();
  }

  #endregion
}