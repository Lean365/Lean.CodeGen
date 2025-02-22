using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Application.Dtos.Identity.Login;
using Lean.CodeGen.Common.Models;
using System.Threading.Tasks;

namespace Lean.CodeGen.Application.Services.Identity;

/// <summary>
/// 用户服务接口
/// </summary>
public interface ILeanUserService
{
  /// <summary>
  /// 用户登录
  /// </summary>
  /// <param name="input">登录信息</param>
  /// <returns>登录结果</returns>
  Task<LeanLoginResultDto> LoginAsync(LeanLoginDto input);

  /// <summary>
  /// 创建用户
  /// </summary>
  Task<LeanApiResult<long>> CreateAsync(LeanCreateUserDto input);

  /// <summary>
  /// 更新用户
  /// </summary>
  Task<LeanApiResult> UpdateAsync(LeanUpdateUserDto input);

  /// <summary>
  /// 删除用户
  /// </summary>
  Task<LeanApiResult> DeleteAsync(long id);

  /// <summary>
  /// 批量删除用户
  /// </summary>
  Task<LeanApiResult> BatchDeleteAsync(List<long> ids);

  /// <summary>
  /// 获取用户信息
  /// </summary>
  Task<LeanApiResult<LeanUserDto>> GetAsync(long id);

  /// <summary>
  /// 分页查询用户
  /// </summary>
  Task<LeanApiResult<LeanPageResult<LeanUserDto>>> GetPageAsync(LeanQueryUserDto input);

  /// <summary>
  /// 修改用户状态
  /// </summary>
  Task<LeanApiResult> SetStatusAsync(LeanChangeUserStatusDto input);

  /// <summary>
  /// 重置密码
  /// </summary>
  Task<LeanApiResult> ResetPasswordAsync(LeanResetUserPasswordDto input);

  /// <summary>
  /// 修改密码
  /// </summary>
  Task<LeanApiResult> ChangePasswordAsync(LeanChangeUserPasswordDto input);

  /// <summary>
  /// 导入用户
  /// </summary>
  Task<LeanApiResult<LeanImportUserResultDto>> ImportAsync(List<LeanImportTemplateUserDto> users);

  /// <summary>
  /// 导出用户
  /// </summary>
  Task<LeanApiResult<byte[]>> ExportAsync(LeanExportUserDto input);

  /// <summary>
  /// 获取用户直接分配的菜单权限列表
  /// </summary>
  Task<LeanApiResult<List<long>>> GetUserDirectMenusAsync(long userId);

  /// <summary>
  /// 分配用户直接菜单权限
  /// </summary>
  Task<LeanApiResult> AssignUserMenusAsync(LeanAssignUserMenuDto input);

  /// <summary>
  /// 获取用户直接分配的API权限列表
  /// </summary>
  Task<LeanApiResult<List<long>>> GetUserDirectApisAsync(long userId);

  /// <summary>
  /// 分配用户直接API权限
  /// </summary>
  Task<LeanApiResult> AssignUserApisAsync(LeanAssignUserApiDto input);

  /// <summary>
  /// 获取用户的所有权限（包括角色继承的权限）
  /// </summary>
  Task<LeanApiResult<LeanUserPermissionsDto>> GetUserAllPermissionsAsync(long userId);

  /// <summary>
  /// 验证用户是否具有指定权限
  /// </summary>
  Task<LeanApiResult<bool>> ValidateUserPermissionAsync(string permission, long userId);

  /// <summary>
  /// 批量验证用户权限
  /// </summary>
  Task<LeanApiResult<Dictionary<string, bool>>> ValidateUserPermissionsAsync(List<string> permissions, long userId);

  /// <summary>
  /// 验证用户是否具有指定资源的访问权限
  /// </summary>
  Task<LeanApiResult<bool>> ValidateUserResourceAccessAsync(LeanValidateUserResourceAccessDto input);

  /// <summary>
  /// 获取当前登录用户信息
  /// </summary>
  Task<LeanApiResult<LeanUserDto>> GetCurrentUserInfoAsync();
}
