using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Application.Services.Base;
using Lean.CodeGen.Common.Excel;
using Lean.CodeGen.Common.Exceptions;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Extensions;
using Lean.CodeGen.Domain.Entities.Identity;
using Lean.CodeGen.Domain.Interfaces.Repositories;
using Lean.CodeGen.Domain.Validators;
using Mapster;
using System.Linq.Expressions;

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
      LeanBaseServiceContext context,
      ILeanRepository<LeanPost> postRepository,
      ILeanRepository<LeanUserPost> userPostRepository) : base(context)
  {
    _postRepository = postRepository;
    _userPostRepository = userPostRepository;
    _uniqueValidator = new LeanUniqueValidator<LeanPost>(_postRepository);
  }

  /// <summary>
  /// 分页查询岗位列表
  /// </summary>
  public async Task<LeanApiResult<LeanPageResult<LeanPostDto>>> GetPageAsync(LeanPostQueryDto input)
  {
    Expression<Func<LeanPost, bool>> predicate = x => true;

    if (!string.IsNullOrEmpty(input.PostName))
    {
      var postName = CleanInput(input.PostName);
      predicate = predicate.And(x => x.PostName.Contains(postName));
    }

    if (!string.IsNullOrEmpty(input.PostCode))
    {
      var postCode = CleanInput(input.PostCode);
      predicate = predicate.And(x => x.PostCode.Contains(postCode));
    }

    if (input.PostStatus.HasValue)
    {
      predicate = predicate.And(x => x.PostStatus == input.PostStatus);
    }

    if (input.StartTime.HasValue)
    {
      predicate = predicate.And(x => x.CreateTime >= input.StartTime);
    }

    if (input.EndTime.HasValue)
    {
      predicate = predicate.And(x => x.CreateTime <= input.EndTime);
    }

    var (total, items) = await _postRepository.GetPageListAsync(predicate, input.PageSize, input.PageIndex);
    var list = items.Adapt<List<LeanPostDto>>();

    var result = new LeanPageResult<LeanPostDto>
    {
      Total = total,
      Items = list,
      PageIndex = input.PageIndex,
      PageSize = input.PageSize
    };

    return LeanApiResult<LeanPageResult<LeanPostDto>>.Ok(result);
  }

  /// <summary>
  /// 获取岗位详情
  /// </summary>
  public async Task<LeanApiResult<LeanPostDto>> GetAsync(long id)
  {
    var post = await _postRepository.GetByIdAsync(id);
    if (post == null)
    {
      throw new LeanException("岗位不存在");
    }

    return LeanApiResult<LeanPostDto>.Ok(post.Adapt<LeanPostDto>());
  }

  /// <summary>
  /// 新增岗位
  /// </summary>
  public async Task<LeanApiResult> CreateAsync(LeanPostCreateDto input)
  {
    // 验证岗位编码唯一性
    await _uniqueValidator.ValidateAsync(x => x.PostCode, input.PostCode, null, $"岗位编码 {input.PostCode} 已存在");

    var post = new LeanPost
    {
      PostName = input.PostName,
      PostCode = input.PostCode,
      PostStatus = input.PostStatus,
      OrderNum = input.OrderNum,
      CreateTime = DateTime.Now,
      UpdateTime = DateTime.Now
    };

    await _postRepository.CreateAsync(post);
    LogAudit("CreatePost", $"新增岗位: {post.PostName}");
    return LeanApiResult.Ok();
  }

  /// <summary>
  /// 更新岗位
  /// </summary>
  public async Task<LeanApiResult> UpdateAsync(LeanPostUpdateDto input)
  {
    // 获取岗位
    var post = await _postRepository.GetByIdAsync(input.Id);
    if (post == null)
    {
      throw new LeanException("岗位不存在");
    }

    // 验证内置岗位
    if (post.IsBuiltin == 1)
    {
      throw new LeanException("内置岗位不允许修改");
    }

    // 更新岗位
    post = input.Adapt(post);
    await _postRepository.UpdateAsync(post);

    return LeanApiResult.Ok();
  }

  /// <summary>
  /// 删除岗位
  /// </summary>
  public async Task<LeanApiResult> DeleteAsync(long id)
  {
    // 获取岗位
    var post = await _postRepository.GetByIdAsync(id);
    if (post == null)
    {
      throw new LeanException("岗位不存在");
    }

    // 验证内置岗位
    if (post.IsBuiltin == 1)
    {
      throw new LeanException("内置岗位不允许删除");
    }

    // 验证是否有用户关联
    var hasUsers = await _userPostRepository.AnyAsync(x => x.PostId == id);
    if (hasUsers)
    {
      throw new LeanException("岗位已被用户使用，不允许删除");
    }

    // 删除岗位
    await _postRepository.DeleteAsync(post);
    return LeanApiResult.Ok();
  }

  /// <summary>
  /// 批量删除岗位
  /// </summary>
  public async Task<LeanApiResult> BatchDeleteAsync(List<long> ids)
  {
    foreach (var id in ids)
    {
      await DeleteAsync(id);
    }
    return LeanApiResult.Ok();
  }

  /// <summary>
  /// 导出岗位列表
  /// </summary>
  public async Task<byte[]> ExportAsync(LeanPostQueryDto input)
  {
    var posts = await GetPageAsync(input);
    var exportDtos = posts.Data.Items.Select(x => new LeanPostExportDto
    {
      PostName = x.PostName,
      PostCode = x.PostCode,
      PostStatus = x.PostStatus,
      OrderNum = x.OrderNum,
      IsBuiltin = x.IsBuiltin,
      CreateTime = x.CreateTime
    }).ToList();

    return LeanExcelHelper.Export(exportDtos);
  }

  /// <summary>
  /// 获取岗位导入模板
  /// </summary>
  public async Task<byte[]> GetTemplateAsync()
  {
    var template = new List<LeanPostImportDto>
    {
      new LeanPostImportDto
      {
        PostName = "示例岗位",
        PostCode = "POST001",
        OrderNum = 1
      }
    };

    return await Task.FromResult(LeanExcelHelper.Export(template));
  }

  /// <summary>
  /// 导入岗位列表
  /// </summary>
  public async Task<LeanPostImportResultDto> ImportAsync(LeanFileInfo file)
  {
    var result = new LeanPostImportResultDto();
    try
    {
      var bytes = new byte[file.Stream.Length];
      await file.Stream.ReadAsync(bytes, 0, (int)file.Stream.Length);
      var importResult = LeanExcelHelper.Import<LeanPostImportDto>(bytes);

      foreach (var item in importResult.Data)
      {
        if (await _postRepository.AnyAsync(x => x.PostCode == item.PostCode))
        {
          result.AddError(item.PostCode, $"岗位编码 {item.PostCode} 已存在");
          continue;
        }
        var post = item.Adapt<LeanPost>();
        post.PostStatus = 2;
        post.IsBuiltin = 0;
        await _postRepository.CreateAsync(post);
        result.SuccessCount++;
      }
      return result;
    }
    catch (Exception ex)
    {
      result.ErrorMessage = $"导入失败：{ex.Message}";
      return result;
    }
  }

  /// <summary>
  /// 获取用户岗位列表
  /// </summary>
  public async Task<LeanApiResult<List<LeanPostDto>>> GetUserPostsAsync(long userId)
  {
    var userPosts = await _userPostRepository.GetListAsync(x => x.UserId == userId);
    var postIds = userPosts.Select(x => x.PostId).ToList();

    var posts = await _postRepository.GetListAsync(x => postIds.Contains(x.Id));
    var orderedPosts = posts.OrderBy(x => x.OrderNum).ToList();

    return LeanApiResult<List<LeanPostDto>>.Ok(orderedPosts.Adapt<List<LeanPostDto>>());
  }

  /// <summary>
  /// 设置用户岗位
  /// </summary>
  public async Task<LeanApiResult> SetUserPostsAsync(LeanUserPostDto input)
  {
    // 删除原有用户岗位关联
    await _userPostRepository.DeleteAsync(x => x.UserId == input.UserId);

    // 添加新的用户岗位关联
    var userPosts = input.PostIds.Select(postId => new LeanUserPost
    {
      UserId = input.UserId,
      PostId = postId,
      CreateTime = DateTime.Now,
      UpdateTime = DateTime.Now
    }).ToList();

    await _userPostRepository.CreateRangeAsync(userPosts);
    LogAudit("SetUserPosts", $"设置用户岗位，用户ID: {input.UserId}, 岗位数量: {input.PostIds.Count}");
    return LeanApiResult.Ok();
  }
}