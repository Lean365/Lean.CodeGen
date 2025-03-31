using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Excel;

namespace Lean.CodeGen.Application.Services.Identity;

/// <summary>
/// 岗位服务接口
/// </summary>
/// <remarks>
/// 提供岗位管理相关的业务功能，包括：
/// 1. 岗位的增删改查
/// 2. 用户岗位管理
/// </remarks>
public interface ILeanPostService
{
  /// <summary>
  /// 分页查询岗位列表
  /// </summary>
  Task<LeanApiResult<LeanPageResult<LeanPostDto>>> GetPageAsync(LeanPostQueryDto input);

  /// <summary>
  /// 获取岗位详情
  /// </summary>
  Task<LeanApiResult<LeanPostDto>> GetAsync(long id);

  /// <summary>
  /// 新增岗位
  /// </summary>
  Task<LeanApiResult> CreateAsync(LeanPostCreateDto input);

  /// <summary>
  /// 更新岗位
  /// </summary>
  Task<LeanApiResult> UpdateAsync(LeanPostUpdateDto input);

  /// <summary>
  /// 删除岗位
  /// </summary>
  Task<LeanApiResult> DeleteAsync(long id);

  /// <summary>
  /// 批量删除岗位
  /// </summary>
  Task<LeanApiResult> BatchDeleteAsync(List<long> ids);

  /// <summary>
  /// 导出岗位列表
  /// </summary>
  Task<byte[]> ExportAsync(LeanPostQueryDto input);

  /// <summary>
  /// 获取岗位导入模板
  /// </summary>
  Task<byte[]> GetTemplateAsync();

  /// <summary>
  /// 导入岗位列表
  /// </summary>
  Task<LeanPostImportResultDto> ImportAsync(LeanFileInfo file);

  /// <summary>
  /// 获取用户岗位列表
  /// </summary>
  Task<LeanApiResult<List<LeanPostDto>>> GetUserPostsAsync(long userId);

  /// <summary>
  /// 设置用户岗位
  /// </summary>
  Task<LeanApiResult> SetUserPostsAsync(LeanUserPostDto input);
}