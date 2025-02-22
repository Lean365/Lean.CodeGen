using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Application.Dtos.Identity.Login;

namespace Lean.CodeGen.Application.Dtos.Identity;

/// <summary>
/// 用户查询参数
/// </summary>
public class LeanQueryUserDto : LeanPage
{
  /// <summary>
  /// 用户名
  /// </summary>
  public string? UserName { get; set; }

  /// <summary>
  /// 真实姓名
  /// </summary>
  public string? RealName { get; set; }

  /// <summary>
  /// 手机号
  /// </summary>
  public string? PhoneNumber { get; set; }

  /// <summary>
  /// 用户状态
  /// </summary>
  public LeanUserStatus? UserStatus { get; set; }

  /// <summary>
  /// 部门ID
  /// </summary>
  public long? DeptId { get; set; }

  /// <summary>
  /// 岗位ID
  /// </summary>
  public long? PostId { get; set; }

  /// <summary>
  /// 开始时间
  /// </summary>
  public DateTime? StartTime { get; set; }

  /// <summary>
  /// 结束时间
  /// </summary>
  public DateTime? EndTime { get; set; }
}

/// <summary>
/// 用户创建参数
/// </summary>
public class LeanCreateUserDto
{
  /// <summary>
  /// 用户名
  /// </summary>
  [Required(ErrorMessage = "用户名不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "用户名长度必须在2-50个字符之间")]
  public string UserName { get; set; } = default!;

  /// <summary>
  /// 密码
  /// </summary>
  [Required(ErrorMessage = "密码不能为空")]
  [StringLength(20, MinimumLength = 6, ErrorMessage = "密码长度必须在6-20个字符之间")]
  public string Password { get; set; } = default!;

  /// <summary>
  /// 真实姓名
  /// </summary>
  [Required(ErrorMessage = "真实姓名不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "真实姓名长度必须在2-50个字符之间")]
  public string RealName { get; set; } = default!;

  /// <summary>
  /// 英文名称
  /// </summary>
  [StringLength(50, ErrorMessage = "英文名称长度不能超过50个字符")]
  public string? EnglishName { get; set; }

  /// <summary>
  /// 用户类型
  /// </summary>
  public LeanUserType UserType { get; set; } = LeanUserType.Normal;

  /// <summary>
  /// 邮箱
  /// </summary>
  [EmailAddress(ErrorMessage = "邮箱格式不正确")]
  [StringLength(100, ErrorMessage = "邮箱长度不能超过100个字符")]
  public string? Email { get; set; }

  /// <summary>
  /// 手机号
  /// </summary>
  [Phone(ErrorMessage = "手机号格式不正确")]
  [StringLength(20, ErrorMessage = "手机号长度不能超过20个字符")]
  public string? PhoneNumber { get; set; }

  /// <summary>
  /// 头像
  /// </summary>
  [StringLength(500, ErrorMessage = "头像URL长度不能超过500个字符")]
  public string? Avatar { get; set; }

  /// <summary>
  /// 主部门ID
  /// </summary>
  [Required(ErrorMessage = "主部门不能为空")]
  public long PrimaryDeptId { get; set; }

  /// <summary>
  /// 部门ID列表
  /// </summary>
  public List<long> DeptIds { get; set; } = new();

  /// <summary>
  /// 主岗位ID
  /// </summary>
  [Required(ErrorMessage = "主岗位不能为空")]
  public long PrimaryPostId { get; set; }

  /// <summary>
  /// 岗位ID列表
  /// </summary>
  public List<long> PostIds { get; set; } = new();

  /// <summary>
  /// 角色ID列表
  /// </summary>
  public List<long> RoleIds { get; set; } = new();
}

/// <summary>
/// 用户更新参数
/// </summary>
public class LeanUpdateUserDto : LeanCreateUserDto
{
  /// <summary>
  /// 用户ID
  /// </summary>
  [Required(ErrorMessage = "用户ID不能为空")]
  public long Id { get; set; }

  /// <summary>
  /// 密码
  /// </summary>
  [StringLength(20, MinimumLength = 6, ErrorMessage = "密码长度必须在6-20个字符之间")]
  public new string? Password { get; set; }
}

/// <summary>
/// 用户详情
/// </summary>
public class LeanUserDetailDto
{
  /// <summary>
  /// 用户ID
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 用户名
  /// </summary>
  public string UserName { get; set; } = default!;

  /// <summary>
  /// 真实姓名
  /// </summary>
  public string RealName { get; set; } = default!;

  /// <summary>
  /// 英文名称
  /// </summary>
  public string? EnglishName { get; set; }

  /// <summary>
  /// 用户类型
  /// </summary>
  public LeanUserType UserType { get; set; }

  /// <summary>
  /// 邮箱
  /// </summary>
  public string? Email { get; set; }

  /// <summary>
  /// 手机号
  /// </summary>
  public string? PhoneNumber { get; set; }

  /// <summary>
  /// 头像
  /// </summary>
  public string? Avatar { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  public LeanUserStatus UserStatus { get; set; }

  /// <summary>
  /// 是否内置
  /// </summary>
  public LeanBuiltinStatus IsBuiltin { get; set; }

  /// <summary>
  /// 最后登录时间
  /// </summary>
  public DateTime? LoginTime { get; set; }

  /// <summary>
  /// 部门列表
  /// </summary>
  public List<LeanUserDeptDto> Depts { get; set; } = new();

  /// <summary>
  /// 岗位列表
  /// </summary>
  public List<LeanUserPostDto> Posts { get; set; } = new();

  /// <summary>
  /// 角色列表
  /// </summary>
  public List<LeanUserRoleDto> Roles { get; set; } = new();

  /// <summary>
  /// 创建时间
  /// </summary>
  public DateTime CreateTime { get; set; }

  /// <summary>
  /// 创建者
  /// </summary>
  public string? CreateUserName { get; set; }

  /// <summary>
  /// 更新时间
  /// </summary>
  public DateTime? UpdateTime { get; set; }

  /// <summary>
  /// 更新者
  /// </summary>
  public string? UpdateUserName { get; set; }
}

/// <summary>
/// 用户导入模板
/// </summary>
public class LeanImportTemplateUserDto
{
  /// <summary>
  /// 用户名
  /// </summary>
  [Required(ErrorMessage = "用户名不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "用户名长度必须在2-50个字符之间")]
  public string UserName { get; set; } = default!;

  /// <summary>
  /// 密码
  /// </summary>
  [Required(ErrorMessage = "密码不能为空")]
  [StringLength(20, MinimumLength = 6, ErrorMessage = "密码长度必须在6-20个字符之间")]
  public string Password { get; set; } = default!;

  /// <summary>
  /// 真实姓名
  /// </summary>
  [Required(ErrorMessage = "真实姓名不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "真实姓名长度必须在2-50个字符之间")]
  public string RealName { get; set; } = default!;

  /// <summary>
  /// 英文名称
  /// </summary>
  [StringLength(50, ErrorMessage = "英文名称长度不能超过50个字符")]
  public string? EnglishName { get; set; }

  /// <summary>
  /// 邮箱
  /// </summary>
  [EmailAddress(ErrorMessage = "邮箱格式不正确")]
  [StringLength(100, ErrorMessage = "邮箱长度不能超过100个字符")]
  public string? Email { get; set; }

  /// <summary>
  /// 手机号
  /// </summary>
  [Phone(ErrorMessage = "手机号格式不正确")]
  [StringLength(20, ErrorMessage = "手机号长度不能超过20个字符")]
  public string? PhoneNumber { get; set; }

  /// <summary>
  /// 主部门编码
  /// </summary>
  [Required(ErrorMessage = "主部门编码不能为空")]
  [StringLength(50, ErrorMessage = "主部门编码长度不能超过50个字符")]
  public string PrimaryDeptCode { get; set; } = default!;

  /// <summary>
  /// 主岗位编码
  /// </summary>
  [Required(ErrorMessage = "主岗位编码不能为空")]
  [StringLength(50, ErrorMessage = "主岗位编码长度不能超过50个字符")]
  public string PrimaryPostCode { get; set; } = default!;

  /// <summary>
  /// 角色编码列表
  /// </summary>
  /// <remarks>
  /// 多个角色编码用逗号分隔
  /// </remarks>
  [StringLength(500, ErrorMessage = "角色编码列表长度不能超过500个字符")]
  public string? RoleCodes { get; set; }
}

/// <summary>
/// 用户导入错误信息
/// </summary>
public class LeanImportUserErrorDto : LeanImportError
{
  /// <summary>
  /// 用户名
  /// </summary>
  public string UserName
  {
    get => Key;
    set => Key = value;
  }
}

/// <summary>
/// 用户导入结果
/// </summary>
public class LeanImportUserResultDto : LeanImportResult
{
  /// <summary>
  /// 错误信息列表
  /// </summary>
  public new List<LeanImportUserErrorDto> Errors { get; set; } = new();

  /// <summary>
  /// 添加错误信息
  /// </summary>
  public override void AddError(string userName, string errorMessage)
  {
    base.AddError(userName, errorMessage);
    Errors.Add(new LeanImportUserErrorDto
    {
      RowIndex = TotalCount,
      UserName = userName,
      ErrorMessage = errorMessage
    });
  }
}

/// <summary>
/// 用户导出参数
/// </summary>
public class LeanExportUserDto : LeanQueryUserDto
{
  /// <summary>
  /// 导出字段列表
  /// </summary>
  public List<string> ExportFields { get; set; } = new();

  /// <summary>
  /// 文件格式
  /// </summary>
  [Required(ErrorMessage = "文件格式不能为空")]
  public string FileFormat { get; set; } = "xlsx";

  /// <summary>
  /// 是否导出全部
  /// </summary>
  public bool IsExportAll { get; set; }

  /// <summary>
  /// 选中的ID列表
  /// </summary>
  public List<long> SelectedIds { get; set; } = new();
}

/// <summary>
/// 用户状态变更参数
/// </summary>
public class LeanChangeUserStatusDto
{
  /// <summary>
  /// 用户ID
  /// </summary>
  [Required(ErrorMessage = "用户ID不能为空")]
  public long Id { get; set; }

  /// <summary>
  /// 用户状态
  /// </summary>
  [Required(ErrorMessage = "用户状态不能为空")]
  public LeanUserStatus UserStatus { get; set; }
}

/// <summary>
/// 重置密码参数
/// </summary>
public class LeanResetUserPasswordDto
{
  /// <summary>
  /// 用户ID
  /// </summary>
  [Required(ErrorMessage = "用户ID不能为空")]
  public long Id { get; set; }

  /// <summary>
  /// 新密码
  /// </summary>
  [Required(ErrorMessage = "新密码不能为空")]
  [StringLength(20, MinimumLength = 6, ErrorMessage = "密码长度必须在6-20个字符之间")]
  public string NewPassword { get; set; } = default!;
}

/// <summary>
/// 修改密码参数
/// </summary>
public class LeanChangeUserPasswordDto
{
  /// <summary>
  /// 用户ID
  /// </summary>
  [Required(ErrorMessage = "用户ID不能为空")]
  public long Id { get; set; }

  /// <summary>
  /// 旧密码
  /// </summary>
  [Required(ErrorMessage = "旧密码不能为空")]
  [StringLength(20, MinimumLength = 6, ErrorMessage = "密码长度必须在6-20个字符之间")]
  public string OldPassword { get; set; } = default!;

  /// <summary>
  /// 新密码
  /// </summary>
  [Required(ErrorMessage = "新密码不能为空")]
  [StringLength(20, MinimumLength = 6, ErrorMessage = "密码长度必须在6-20个字符之间")]
  public string NewPassword { get; set; } = default!;

  /// <summary>
  /// 确认密码
  /// </summary>
  [Required(ErrorMessage = "确认密码不能为空")]
  [Compare(nameof(NewPassword), ErrorMessage = "两次输入的密码不一致")]
  public string ConfirmPassword { get; set; } = default!;
}

/// <summary>
/// 批量删除参数
/// </summary>
public class LeanDeleteUserDto
{
  /// <summary>
  /// 用户ID列表
  /// </summary>
  [Required(ErrorMessage = "用户ID列表不能为空")]
  public List<long> UserIds { get; set; } = new();
}

/// <summary>
/// 批量导出参数
/// </summary>
public class LeanBatchExportUserDto
{
  /// <summary>
  /// 用户ID列表
  /// </summary>
  [Required(ErrorMessage = "用户ID列表不能为空")]
  public List<long> UserIds { get; set; } = new();

  /// <summary>
  /// 导出字段列表
  /// </summary>
  public List<string> ExportFields { get; set; } = new();

  /// <summary>
  /// 文件格式
  /// </summary>
  /// <remarks>
  /// 支持的格式：xlsx、csv
  /// </remarks>
  [Required(ErrorMessage = "文件格式不能为空")]
  public string FileFormat { get; set; } = "xlsx";
}

/// <summary>
/// 用户 DTO
/// </summary>
public class LeanUserDto
{
  /// <summary>
  /// 用户ID
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 用户名
  /// </summary>
  public string UserName { get; set; } = null!;

  /// <summary>
  /// 真实姓名
  /// </summary>
  public string? RealName { get; set; }

  /// <summary>
  /// 邮箱
  /// </summary>
  public string? Email { get; set; }

  /// <summary>
  /// 手机号
  /// </summary>
  public string? PhoneNumber { get; set; }

  /// <summary>
  /// 用户状态
  /// </summary>
  public LeanUserStatus UserStatus { get; set; }

  /// <summary>
  /// 角色ID列表
  /// </summary>
  public List<long> RoleIds { get; set; } = new();

  /// <summary>
  /// 部门ID列表
  /// </summary>
  public List<long> DeptIds { get; set; } = new();

  /// <summary>
  /// 主部门ID
  /// </summary>
  public long? PrimaryDeptId { get; set; }

  /// <summary>
  /// 岗位ID列表
  /// </summary>
  public List<long> PostIds { get; set; } = new();

  /// <summary>
  /// 主岗位ID
  /// </summary>
  public long? PrimaryPostId { get; set; }

  /// <summary>
  /// 部门列表
  /// </summary>
  public List<LeanUserDeptDto> Depts { get; set; } = new();

  /// <summary>
  /// 岗位列表
  /// </summary>
  public List<LeanUserPostDto> Posts { get; set; } = new();

  /// <summary>
  /// 最后登录信息
  /// </summary>
  public LeanUserLoginInfo? LastLoginInfo { get; set; }

  /// <summary>
  /// 创建时间
  /// </summary>
  public DateTime CreateTime { get; set; }
}

/// <summary>
/// 分配用户菜单权限参数
/// </summary>
public class LeanAssignUserMenuDto
{
  /// <summary>
  /// 用户ID
  /// </summary>
  [Required(ErrorMessage = "用户ID不能为空")]
  public long UserId { get; set; }

  /// <summary>
  /// 菜单ID列表
  /// </summary>
  public List<long> MenuIds { get; set; } = new();
}

/// <summary>
/// 分配用户API权限参数
/// </summary>
public class LeanAssignUserApiDto
{
  /// <summary>
  /// 用户ID
  /// </summary>
  [Required(ErrorMessage = "用户ID不能为空")]
  public long UserId { get; set; }

  /// <summary>
  /// API ID列表
  /// </summary>
  public List<long> ApiIds { get; set; } = new();
}

/// <summary>
/// 用户权限信息
/// </summary>
public class LeanUserPermissionsDto
{
  /// <summary>
  /// 用户ID
  /// </summary>
  public long UserId { get; set; }

  /// <summary>
  /// 角色ID列表
  /// </summary>
  public List<long> RoleIds { get; set; } = new();

  /// <summary>
  /// 部门ID列表
  /// </summary>
  public List<long> DeptIds { get; set; } = new();

  /// <summary>
  /// 岗位ID列表
  /// </summary>
  public List<long> PostIds { get; set; } = new();

  /// <summary>
  /// 菜单ID列表
  /// </summary>
  public List<long> MenuIds { get; set; } = new();

  /// <summary>
  /// API ID列表
  /// </summary>
  public List<long> ApiIds { get; set; } = new();
}

/// <summary>
/// 验证用户资源访问参数
/// </summary>
public class LeanValidateUserResourceAccessDto
{
  /// <summary>
  /// 用户ID
  /// </summary>
  [Required(ErrorMessage = "用户ID不能为空")]
  public long UserId { get; set; }

  /// <summary>
  /// 资源类型
  /// </summary>
  [Required(ErrorMessage = "资源类型不能为空")]
  public LeanResourceType ResourceType { get; set; }

  /// <summary>
  /// 资源ID
  /// </summary>
  [Required(ErrorMessage = "资源ID不能为空")]
  public long ResourceId { get; set; }

  /// <summary>
  /// 操作类型
  /// </summary>
  [Required(ErrorMessage = "操作类型不能为空")]
  public LeanOperationType Operation { get; set; }
}