using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Application.Services.Base;
using Lean.CodeGen.Common.Exceptions;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Domain.Entities.Identity;
using Lean.CodeGen.Domain.Interfaces.Repositories;
using Lean.CodeGen.Domain.Validators;
using Mapster;

namespace Lean.CodeGen.Application.Services.Identity;

/// <summary>
/// 岗位服务实现
/// </summary>
public class LeanPostService : LeanBaseService, ILeanPostService
{
  private readonly ILeanRepository<LeanPost> _postRepository;
  private readonly ILeanRepository<LeanUserPost> _userPostRepository;
  private readonly LeanUniqueValidator<LeanPost> _uniqueValidator;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanPostService(
      ILeanRepository<LeanPost> postRepository,
      ILeanRepository<LeanUserPost> userPostRepository,
      LeanBaseServiceContext context)
      : base(context)
  {
    _postRepository = postRepository;
    _userPostRepository = userPostRepository;
    _uniqueValidator = new LeanUniqueValidator<LeanPost>(_postRepository);
  }

  /// <summary>
  /// 创建岗位
  /// </summary>
  public async Task<LeanPostDto> CreateAsync(LeanCreatePostDto input)
  {
    // 验证岗位编码唯一性
    await _uniqueValidator.ValidateAsync<string>(x => x.PostCode, input.PostCode);

    // 创建岗位
    var post = input.Adapt<LeanPost>();
    await _postRepository.CreateAsync(post);

    return post.Adapt<LeanPostDto>();
  }

  /// <summary>
  /// 更新岗位
  /// </summary>
  public async Task<LeanPostDto> UpdateAsync(LeanUpdatePostDto input)
  {
    // 获取岗位
    var post = await _postRepository.GetByIdAsync(input.Id);
    if (post == null)
    {
      throw new LeanException("岗位不存在");
    }

    // 验证内置岗位
    if (post.IsBuiltin == Common.Enums.LeanBuiltinStatus.Yes)
    {
      throw new LeanException("内置岗位不允许修改");
    }

    // 更新岗位
    post = input.Adapt(post);
    await _postRepository.UpdateAsync(post);

    return post.Adapt<LeanPostDto>();
  }

  /// <summary>
  /// 删除岗位
  /// </summary>
  public async Task DeleteAsync(LeanDeletePostDto input)
  {
    // 获取岗位
    var post = await _postRepository.GetByIdAsync(input.Id);
    if (post == null)
    {
      throw new LeanException("岗位不存在");
    }

    // 验证内置岗位
    if (post.IsBuiltin == Common.Enums.LeanBuiltinStatus.Yes)
    {
      throw new LeanException("内置岗位不允许删除");
    }

    // 验证是否有用户关联
    var hasUsers = await _userPostRepository.AnyAsync(x => x.PostId == input.Id);
    if (hasUsers)
    {
      throw new LeanException("岗位已被用户使用，不允许删除");
    }

    // 删除岗位
    await _postRepository.DeleteAsync(post);
  }

  /// <summary>
  /// 获取岗位信息
  /// </summary>
  public async Task<LeanPostDto> GetAsync(long id)
  {
    var post = await _postRepository.GetByIdAsync(id);
    if (post == null)
    {
      throw new LeanException("岗位不存在");
    }

    return post.Adapt<LeanPostDto>();
  }

  /// <summary>
  /// 查询岗位列表
  /// </summary>
  public async Task<List<LeanPostDto>> QueryAsync(LeanQueryPostDto input)
  {
    // 构建查询条件
    var posts = await _postRepository.GetListAsync(x =>
        (string.IsNullOrEmpty(input.PostName) || x.PostName.Contains(input.PostName)) &&
        (string.IsNullOrEmpty(input.PostCode) || x.PostCode.Contains(input.PostCode)) &&
        (!input.PostStatus.HasValue || x.PostStatus == input.PostStatus) &&
        (!input.IsBuiltin.HasValue || x.IsBuiltin == input.IsBuiltin) &&
        (!input.StartTime.HasValue || x.CreateTime >= input.StartTime) &&
        (!input.EndTime.HasValue || x.CreateTime <= input.EndTime));

    return posts.OrderBy(x => x.OrderNum)
               .Select(x => x.Adapt<LeanPostDto>())
               .ToList();
  }

  /// <summary>
  /// 修改岗位状态
  /// </summary>
  public async Task ChangeStatusAsync(LeanChangePostStatusDto input)
  {
    // 获取岗位
    var post = await _postRepository.GetByIdAsync(input.Id);
    if (post == null)
    {
      throw new LeanException("岗位不存在");
    }

    // 验证内置岗位
    if (post.IsBuiltin == Common.Enums.LeanBuiltinStatus.Yes)
    {
      throw new LeanException("内置岗位不允许修改状态");
    }

    // 更新状态
    post.PostStatus = input.PostStatus;
    await _postRepository.UpdateAsync(post);
  }
}