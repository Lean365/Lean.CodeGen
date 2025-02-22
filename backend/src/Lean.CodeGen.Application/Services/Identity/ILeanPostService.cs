using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Services.Identity;

/// <summary>
/// 岗位服务接口
/// </summary>
public interface ILeanPostService
{
  /// <summary>
  /// 创建岗位
  /// </summary>
  Task<LeanApiResult<long>> CreateAsync(LeanCreatePostDto input);

  /// <summary>
  /// 更新岗位
  /// </summary>
  Task<LeanApiResult> UpdateAsync(LeanUpdatePostDto input);

  /// <summary>
  /// 删除岗位
  /// </summary>
  Task<LeanApiResult> DeleteAsync(long id);

  /// <summary>
  /// 批量删除岗位
  /// </summary>
  Task<LeanApiResult> BatchDeleteAsync(List<long> ids);

  /// <summary>
  /// 获取岗位信息
  /// </summary>
  Task<LeanApiResult<LeanPostDto>> GetAsync(long id);

  /// <summary>
  /// 分页查询岗位
  /// </summary>
  Task<LeanApiResult<LeanPageResult<LeanPostDto>>> GetPageAsync(LeanQueryPostDto input);

  /// <summary>
  /// 修改岗位状态
  /// </summary>
  Task<LeanApiResult> SetStatusAsync(LeanChangePostStatusDto input);

  /// <summary>
  /// 获取用户岗位列表
  /// </summary>
  Task<LeanApiResult<List<LeanPostDto>>> GetUserPostsAsync(long userId);

  /// <summary>
  /// 获取部门岗位列表
  /// </summary>
  Task<LeanApiResult<List<LeanPostDto>>> GetDeptPostsAsync(long deptId);

  /// <summary>
  /// 获取用户的主岗位
  /// </summary>
  Task<LeanApiResult<LeanPostDto>> GetUserPrimaryPostAsync(long userId);

  /// <summary>
  /// 验证岗位编码是否唯一
  /// </summary>
  Task<LeanApiResult<bool>> ValidatePostCodeUniqueAsync(string postCode, long? id = null);

  /// <summary>
  /// 获取岗位的所有用户ID列表
  /// </summary>
  Task<LeanApiResult<List<long>>> GetPostUserIdsAsync(long postId);

  /// <summary>
  /// 导入岗位数据
  /// </summary>
  Task<LeanApiResult<LeanImportPostResultDto>> ImportAsync(List<LeanImportTemplatePostDto> posts);

  /// <summary>
  /// 导出岗位数据
  /// </summary>
  Task<LeanApiResult<byte[]>> ExportAsync(LeanExportPostDto input);

  /// <summary>
  /// 获取岗位继承关系树
  /// </summary>
  Task<LeanApiResult<List<LeanPostTreeDto>>> GetPostInheritanceTreeAsync();

  /// <summary>
  /// 设置岗位继承关系
  /// </summary>
  Task<LeanApiResult> SetPostInheritanceAsync(LeanSetPostInheritanceDto input);

  /// <summary>
  /// 获取岗位的所有继承权限
  /// </summary>
  Task<LeanApiResult<LeanPostPermissionsDto>> GetPostInheritedPermissionsAsync(long postId);

  /// <summary>
  /// 设置岗位权限
  /// </summary>
  Task<LeanApiResult> SetPostPermissionsAsync(LeanSetPostPermissionDto input);

  /// <summary>
  /// 验证用户是否具有指定岗位的权限
  /// </summary>
  Task<LeanApiResult<bool>> ValidateUserPostPermissionAsync(long userId, long postId);
}