using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Application.Services.Base;
using Lean.CodeGen.Common.Excel;
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
  public async Task<LeanApiResult<long>> CreateAsync(LeanCreatePostDto input)
  {
    // 验证岗位编码唯一性
    await _uniqueValidator.ValidateAsync<string>(x => x.PostCode, input.PostCode);

    // 创建岗位
    var post = input.Adapt<LeanPost>();
    await _postRepository.CreateAsync(post);

    return LeanApiResult<long>.Ok(post.Id);
  }

  /// <summary>
  /// 更新岗位
  /// </summary>
  public async Task<LeanApiResult> UpdateAsync(LeanUpdatePostDto input)
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
    if (post.IsBuiltin == Common.Enums.LeanBuiltinStatus.Yes)
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
  /// 获取岗位信息
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
  /// 分页查询岗位
  /// </summary>
  public async Task<LeanApiResult<LeanPageResult<LeanPostDto>>> GetPageAsync(LeanQueryPostDto input)
  {
    var (total, items) = await _postRepository.GetPageListAsync(x =>
        (string.IsNullOrEmpty(input.PostName) || x.PostName.Contains(input.PostName)) &&
        (string.IsNullOrEmpty(input.PostCode) || x.PostCode.Contains(input.PostCode)) &&
        (!input.PostStatus.HasValue || x.PostStatus == input.PostStatus) &&
        (!input.IsBuiltin.HasValue || x.IsBuiltin == input.IsBuiltin) &&
        (!input.StartTime.HasValue || x.CreateTime >= input.StartTime) &&
        (!input.EndTime.HasValue || x.CreateTime <= input.EndTime),
        input.PageSize,
        input.PageIndex);

    var result = new LeanPageResult<LeanPostDto>
    {
      Total = total,
      Items = items.Select(x => x.Adapt<LeanPostDto>()).ToList(),
      PageIndex = input.PageIndex,
      PageSize = input.PageSize
    };

    return LeanApiResult<LeanPageResult<LeanPostDto>>.Ok(result);
  }

  /// <summary>
  /// 设置岗位状态
  /// </summary>
  public async Task<LeanApiResult> SetStatusAsync(LeanChangePostStatusDto input)
  {
    await ChangeStatusAsync(input);
    return LeanApiResult.Ok();
  }

  /// <summary>
  /// 导出岗位数据
  /// </summary>
  public async Task<byte[]> ExportAsync(LeanQueryPostDto input)
  {
    var posts = await QueryAsync(input);
    var exportDtos = posts.Select(x => new LeanPostExportDto
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
  /// 导入岗位数据
  /// </summary>
  public async Task<LeanImportPostResultDto> ImportAsync(LeanFileInfo file)
  {
    var result = new LeanImportPostResultDto();
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
        post.PostStatus = Common.Enums.LeanPostStatus.Normal;
        post.IsBuiltin = Common.Enums.LeanBuiltinStatus.No;
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
  /// 获取导入模板
  /// </summary>
  public async Task<byte[]> GetImportTemplateAsync()
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
}