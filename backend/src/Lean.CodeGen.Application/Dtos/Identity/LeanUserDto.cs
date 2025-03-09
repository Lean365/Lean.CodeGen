using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Application.Dtos;
using Lean.CodeGen.Common.Excel;

namespace Lean.CodeGen.Application.Dtos.Identity;

/// <summary>
/// 用户基础信息
/// </summary>
public class LeanUserDto : LeanBaseDto
{
  /// <summary>
  /// 用户名
  /// </summary>
  public string UserName { get; set; } = string.Empty;

  /// <summary>
  /// 真实姓名
  /// </summary>
  public string RealName { get; set; } = string.Empty;

  /// <summary>
  /// 英文名称
  /// </summary>
  public string? EnglishName { get; set; }

  /// <summary>
  /// 昵称
  /// </summary>
  public string? Nickname { get; set; }

  /// <summary>
  /// 用户类型
  /// 0-普通用户
  /// 1-管理员
  /// </summary>
  public int UserType { get; set; }

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
  /// 用户状态
  /// 0-正常
  /// 1-停用
  /// </summary>
  public int UserStatus { get; set; }

  /// <summary>
  /// 是否内置
  /// 0-否
  /// 1-是
  /// </summary>
  public int IsBuiltin { get; set; }

  /// <summary>
  /// 是否信任设备
  /// 0-否
  /// 1-是
  /// </summary>
  public int IsTrusted { get; set; }

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
  public LeanLoginDto? LastLoginInfo { get; set; }
}

/// <summary>
/// 用户查询参数
/// </summary>
public class LeanUserQueryDto : LeanPage
{
  /// <summary>
  /// ID
  /// </summary>
  public long? Id { get; set; }

  /// <summary>
  /// 租户ID
  /// </summary>
  public long? TenantId { get; set; }

  /// <summary>
  /// 用户名
  /// </summary>
  public string? UserName { get; set; }

  /// <summary>
  /// 真实姓名
  /// </summary>
  public string? RealName { get; set; }

  /// <summary>
  /// 英文名称
  /// </summary>
  public string? EnglishName { get; set; }

  /// <summary>
  /// 昵称
  /// </summary>
  public string? Nickname { get; set; }

  /// <summary>
  /// 用户类型
  /// 0-普通用户
  /// 1-管理员
  /// </summary>
  public int? UserType { get; set; }

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
  /// 用户状态
  /// 0-正常
  /// 1-停用
  /// </summary>
  public int? UserStatus { get; set; }

  /// <summary>
  /// 是否内置
  /// 0-否
  /// 1-是
  /// </summary>
  public int? IsBuiltin { get; set; }

  /// <summary>
  /// 创建者ID
  /// </summary>
  public long? CreateUserId { get; set; }

  /// <summary>
  /// 创建者
  /// </summary>
  public string? CreateUserName { get; set; }

  /// <summary>
  /// 创建时间范围-开始
  /// </summary>
  public DateTime? StartTime { get; set; }

  /// <summary>
  /// 创建时间范围-结束
  /// </summary>
  public DateTime? EndTime { get; set; }

  /// <summary>
  /// 更新者ID
  /// </summary>
  public long? UpdateUserId { get; set; }

  /// <summary>
  /// 更新者
  /// </summary>
  public string? UpdateUserName { get; set; }

  /// <summary>
  /// 更新时间范围-开始
  /// </summary>
  public DateTime? UpdateStartTime { get; set; }

  /// <summary>
  /// 更新时间范围-结束
  /// </summary>
  public DateTime? UpdateEndTime { get; set; }

  /// <summary>
  /// 审核状态
  /// 0-未审核
  /// 1-已审核
  /// 2-已驳回
  /// </summary>
  public int? AuditStatus { get; set; }

  /// <summary>
  /// 审核人员ID
  /// </summary>
  public long? AuditUserId { get; set; }

  /// <summary>
  /// 审核人员
  /// </summary>
  public string? AuditUserName { get; set; }

  /// <summary>
  /// 审核时间范围-开始
  /// </summary>
  public DateTime? AuditStartTime { get; set; }

  /// <summary>
  /// 审核时间范围-结束
  /// </summary>
  public DateTime? AuditEndTime { get; set; }

  /// <summary>
  /// 审核意见
  /// </summary>
  public string? AuditOpinion { get; set; }

  /// <summary>
  /// 撤销人员ID
  /// </summary>
  public long? RevokeUserId { get; set; }

  /// <summary>
  /// 撤销人员
  /// </summary>
  public string? RevokeUserName { get; set; }

  /// <summary>
  /// 撤销时间范围-开始
  /// </summary>
  public DateTime? RevokeStartTime { get; set; }

  /// <summary>
  /// 撤销时间范围-结束
  /// </summary>
  public DateTime? RevokeEndTime { get; set; }

  /// <summary>
  /// 撤销意见
  /// </summary>
  public string? RevokeOpinion { get; set; }

  /// <summary>
  /// 是否删除
  /// </summary>
  public int? IsDeleted { get; set; }

  /// <summary>
  /// 删除人员ID
  /// </summary>
  public long? DeleteUserId { get; set; }

  /// <summary>
  /// 删除人员
  /// </summary>
  public string? DeleteUserName { get; set; }

  /// <summary>
  /// 删除时间范围-开始
  /// </summary>
  public DateTime? DeleteStartTime { get; set; }

  /// <summary>
  /// 删除时间范围-结束
  /// </summary>
  public DateTime? DeleteEndTime { get; set; }

  /// <summary>
  /// 备注
  /// </summary>
  public string? Remark { get; set; }

  /// <summary>
  /// 角色ID列表
  /// </summary>
  public List<long>? RoleIds { get; set; }

  /// <summary>
  /// 部门ID列表
  /// </summary>
  public List<long>? DeptIds { get; set; }
}

/// <summary>
/// 用户创建参数
/// </summary>
public class LeanUserCreateDto
{
  /// <summary>
  /// 用户名
  /// </summary>
  [Required(ErrorMessage = "用户名不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "用户名长度必须在2-50个字符之间")]
  public string UserName { get; set; } = string.Empty;

  /// <summary>
  /// 密码
  /// </summary>
  [Required(ErrorMessage = "密码不能为空")]
  [StringLength(50, MinimumLength = 6, ErrorMessage = "密码长度必须在6-50个字符之间")]
  public string Password { get; set; } = string.Empty;

  /// <summary>
  /// 真实姓名
  /// </summary>
  [Required(ErrorMessage = "真实姓名不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "真实姓名长度必须在2-50个字符之间")]
  public string RealName { get; set; } = string.Empty;

  /// <summary>
  /// 英文名称
  /// </summary>
  [StringLength(50, ErrorMessage = "英文名称长度不能超过50个字符")]
  public string? EnglishName { get; set; }

  /// <summary>
  /// 昵称
  /// </summary>
  [StringLength(50, ErrorMessage = "昵称长度不能超过50个字符")]
  public string? Nickname { get; set; }

  /// <summary>
  /// 用户类型
  /// 0-普通用户
  /// 1-管理员
  /// </summary>
  public int UserType { get; set; } = 0;

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
  /// 用户状态
  /// 0-正常
  /// 1-停用
  /// </summary>
  public int UserStatus { get; set; } = 0;

  /// <summary>
  /// 是否内置
  /// 0-否
  /// 1-是
  /// </summary>
  public int IsBuiltin { get; set; } = 0;
}

/// <summary>
/// 用户更新参数
/// </summary>
public class LeanUserUpdateDto
{
  /// <summary>
  /// 用户ID
  /// </summary>
  [Required(ErrorMessage = "用户ID不能为空")]
  public long Id { get; set; }

  /// <summary>
  /// 真实姓名
  /// </summary>
  [Required(ErrorMessage = "真实姓名不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "真实姓名长度必须在2-50个字符之间")]
  public string RealName { get; set; } = string.Empty;

  /// <summary>
  /// 英文名称
  /// </summary>
  [StringLength(50, ErrorMessage = "英文名称长度不能超过50个字符")]
  public string? EnglishName { get; set; }

  /// <summary>
  /// 昵称
  /// </summary>
  [StringLength(50, ErrorMessage = "昵称长度不能超过50个字符")]
  public string? Nickname { get; set; }

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
  /// 用户状态
  /// 0-正常
  /// 1-停用
  /// </summary>
  public int UserStatus { get; set; }
}

/// <summary>
/// 用户状态变更参数
/// </summary>
public class LeanUserChangeStatusDto
{
  /// <summary>
  /// 用户ID
  /// </summary>
  [Required(ErrorMessage = "用户ID不能为空")]
  public long Id { get; set; }

  /// <summary>
  /// 用户状态
  /// 0-正常
  /// 1-停用
  /// </summary>
  [Required(ErrorMessage = "用户状态不能为空")]
  public int UserStatus { get; set; }
}

/// <summary>
/// 用户重置密码参数
/// </summary>
public class LeanUserResetPasswordDto
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
  [StringLength(50, MinimumLength = 6, ErrorMessage = "密码长度必须在6-50个字符之间")]
  public string NewPassword { get; set; } = string.Empty;
}

/// <summary>
/// 用户修改密码参数
/// </summary>
public class LeanUserChangePasswordDto
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
  [StringLength(50, MinimumLength = 6, ErrorMessage = "密码长度必须在6-50个字符之间")]
  public string OldPassword { get; set; } = string.Empty;

  /// <summary>
  /// 新密码
  /// </summary>
  [Required(ErrorMessage = "新密码不能为空")]
  [StringLength(50, MinimumLength = 6, ErrorMessage = "密码长度必须在6-50个字符之间")]
  public string NewPassword { get; set; } = string.Empty;

  /// <summary>
  /// 确认密码
  /// </summary>
  [Required(ErrorMessage = "确认密码不能为空")]
  [Compare("NewPassword", ErrorMessage = "两次输入的密码不一致")]
  public string ConfirmPassword { get; set; } = string.Empty;
}

/// <summary>
/// 用户导入模板参数
/// </summary>
public class LeanUserImportTemplateDto
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
  [StringLength(50, MinimumLength = 6, ErrorMessage = "密码长度必须在6-50个字符之间")]
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
  /// 昵称
  /// </summary>
  [StringLength(50, ErrorMessage = "昵称长度不能超过50个字符")]
  public string? Nickname { get; set; }

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
}

/// <summary>
/// 用户导入错误参数
/// </summary>
public class LeanUserImportErrorDto : LeanImportError
{
  /// <summary>
  /// 用户名
  /// </summary>
  public string UserName { get; set; } = default!;
}

/// <summary>
/// 用户导入结果参数
/// </summary>
public class LeanUserImportResultDto : LeanImportResult
{
  /// <summary>
  /// 错误列表
  /// </summary>
  public new List<LeanUserImportErrorDto> Errors { get; set; } = new();

  /// <summary>
  /// 添加错误
  /// </summary>
  /// <param name="userName">用户名</param>
  /// <param name="errorMessage">错误消息</param>
  public override void AddError(string userName, string errorMessage)
  {
    Errors.Add(new LeanUserImportErrorDto
    {
      UserName = userName,
      ErrorMessage = errorMessage
    });
  }
}

/// <summary>
/// 用户导出查询参数
/// </summary>
public class LeanUserExportQueryDto : LeanUserQueryDto
{
  /// <summary>
  /// 导出字段列表
  /// </summary>
  public List<string> ExportFields { get; set; } = new();

  /// <summary>
  /// 文件格式
  /// </summary>
  [Required(ErrorMessage = "文件格式不能为空")]
  public string FileType { get; set; } = default!;

  /// <summary>
  /// 是否导出全部
  /// 0-否
  /// 1-是
  /// </summary>
  public int IsExportAll { get; set; }

  /// <summary>
  /// 选中的ID列表
  /// </summary>
  public List<long> SelectedIds { get; set; } = new();
}

/// <summary>
/// 用户删除参数
/// </summary>
public class LeanUserDeleteDto
{
  /// <summary>
  /// 用户ID
  /// </summary>
  [Required(ErrorMessage = "用户ID不能为空")]
  public long Id { get; set; }
}

/// <summary>
/// 用户导出参数
/// </summary>
public class LeanUserExportDto
{
  /// <summary>
  /// 用户名
  /// </summary>
  [LeanExcelColumn("用户名", DataType = LeanExcelDataType.String)]
  public string UserName { get; set; } = default!;

  /// <summary>
  /// 真实姓名
  /// </summary>
  [LeanExcelColumn("真实姓名", DataType = LeanExcelDataType.String)]
  public string RealName { get; set; } = default!;

  /// <summary>
  /// 英文名称
  /// </summary>
  [LeanExcelColumn("英文名称", DataType = LeanExcelDataType.String)]
  public string? EnglishName { get; set; }

  /// <summary>
  /// 昵称
  /// </summary>
  [LeanExcelColumn("昵称", DataType = LeanExcelDataType.String)]
  public string? Nickname { get; set; }

  /// <summary>
  /// 邮箱
  /// </summary>
  [LeanExcelColumn("邮箱", DataType = LeanExcelDataType.String)]
  public string? Email { get; set; }

  /// <summary>
  /// 手机号
  /// </summary>
  [LeanExcelColumn("手机号", DataType = LeanExcelDataType.String)]
  public string? PhoneNumber { get; set; }

  /// <summary>
  /// 用户状态
  /// 0-正常
  /// 1-停用
  /// </summary>
  [LeanExcelColumn("用户状态", DataType = LeanExcelDataType.Int)]
  public int UserStatus { get; set; }

  /// <summary>
  /// 是否内置
  /// 0-否
  /// 1-是
  /// </summary>
  [LeanExcelColumn("是否内置", DataType = LeanExcelDataType.Int)]
  public int IsBuiltin { get; set; }

  /// <summary>
  /// 创建时间
  /// </summary>
  [LeanExcelColumn("创建时间", DataType = LeanExcelDataType.DateTime)]
  public DateTime CreateTime { get; set; }
}

/// <summary>
/// 用户导入参数
/// </summary>
public class LeanUserImportDto
{
  /// <summary>
  /// 用户名
  /// </summary>
  [LeanExcelColumn("用户名")]
  public string UserName { get; set; } = default!;

  /// <summary>
  /// 真实姓名
  /// </summary>
  [LeanExcelColumn("真实姓名")]
  public string RealName { get; set; } = default!;

  /// <summary>
  /// 英文名称
  /// </summary>
  [LeanExcelColumn("英文名称")]
  public string? EnglishName { get; set; }

  /// <summary>
  /// 昵称
  /// </summary>
  [LeanExcelColumn("昵称")]
  public string? Nickname { get; set; }

  /// <summary>
  /// 邮箱
  /// </summary>
  [LeanExcelColumn("邮箱")]
  public string? Email { get; set; }

  /// <summary>
  /// 手机号
  /// </summary>
  [LeanExcelColumn("手机号")]
  public string? PhoneNumber { get; set; }
}