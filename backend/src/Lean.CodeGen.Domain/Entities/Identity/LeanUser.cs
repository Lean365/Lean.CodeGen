//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: LeanUser.cs
// 功能描述: 用户身份实体
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Domain.Entities;
using SqlSugar;
using System;

namespace Lean.CodeGen.Domain.Entities.Identity;

/// <summary>
/// 用户身份实体
/// </summary>
[SugarTable("lean_user", "用户身份表")]
[SugarIndex("uk_username", nameof(UserName), OrderByType.Asc, true)]
[SugarIndex("idx_email", nameof(Email), OrderByType.Asc)]
[SugarIndex("idx_phone", nameof(PhoneNumber), OrderByType.Asc)]
public class LeanUser : LeanBaseEntity
{
  /// <summary>
  /// 用户名
  /// </summary>
  /// <remarks>
  /// 用户登录账号，唯一标识
  /// </remarks>
  [SugarColumn(ColumnName = "user_name", ColumnDescription = "用户名", Length = 50, IsNullable = false, UniqueGroupNameList = new[] { "uk_username" }, ColumnDataType = "nvarchar")]
  public string UserName { get; set; } = string.Empty;

  /// <summary>
  /// 密码
  /// </summary>
  /// <remarks>
  /// 用户登录密码，加密存储
  /// </remarks>
  [SugarColumn(ColumnName = "password", ColumnDescription = "密码", Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
  public string Password { get; set; } = string.Empty;

  /// <summary>
  /// 密码盐值
  /// </summary>
  /// <remarks>
  /// 用于密码加密的盐值
  /// </remarks>
  [SugarColumn(ColumnName = "salt", ColumnDescription = "密码盐值", Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
  public string Salt { get; set; } = string.Empty;

  /// <summary>
  /// 真实姓名
  /// </summary>
  /// <remarks>
  /// 用户的真实姓名
  /// </remarks>
  [SugarColumn(ColumnName = "real_name", ColumnDescription = "真实姓名", Length = 50, IsNullable = false, ColumnDataType = "nvarchar")]
  public string RealName { get; set; } = string.Empty;

  /// <summary>
  /// 英文名称
  /// </summary>
  /// <remarks>
  /// 用户的英文名称
  /// </remarks>
  [SugarColumn(ColumnName = "english_name", ColumnDescription = "英文名称", Length = 50, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? EnglishName { get; set; }

  /// <summary>
  /// 昵称
  /// </summary>
  /// <remarks>
  /// 用户的昵称
  /// </remarks>
  [SugarColumn(ColumnName = "nickname", ColumnDescription = "昵称", Length = 50, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? Nickname { get; set; }

  /// <summary>
  /// 用户类型
  /// </summary>
  /// <remarks>
  /// 用户类型：0-超级管理员，1-普通用户
  /// </remarks>
  [SugarColumn(ColumnName = "user_type", ColumnDescription = "用户类型", IsNullable = false, DefaultValue = "1", ColumnDataType = "int")]
  public LeanUserType UserType { get; set; }

  /// <summary>
  /// 邮箱
  /// </summary>
  /// <remarks>
  /// 用户邮箱地址
  /// </remarks>
  [SugarColumn(ColumnName = "email", ColumnDescription = "邮箱", Length = 100, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? Email { get; set; }

  /// <summary>
  /// 手机号
  /// </summary>
  /// <remarks>
  /// 用户手机号码
  /// </remarks>
  [SugarColumn(ColumnName = "phone_number", ColumnDescription = "手机号", Length = 20, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? PhoneNumber { get; set; }

  /// <summary>
  /// 头像
  /// </summary>
  /// <remarks>
  /// 用户头像URL地址
  /// </remarks>
  [SugarColumn(ColumnName = "avatar", ColumnDescription = "头像", Length = 500, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? Avatar { get; set; }

  /// <summary>
  /// 用户状态
  /// </summary>
  /// <remarks>
  /// 用户状态：0-正常，1-停用
  /// </remarks>
  [SugarColumn(ColumnName = "user_status", ColumnDescription = "状态", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
  public LeanUserStatus UserStatus { get; set; }

  /// <summary>
  /// 是否内置
  /// </summary>
  /// <remarks>
  /// 是否为系统内置用户：No-否，Yes-是
  /// </remarks>
  [SugarColumn(ColumnName = "is_builtin", ColumnDescription = "是否内置", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
  public LeanBuiltinStatus IsBuiltin { get; set; }

  /// <summary>
  /// 用户角色列表
  /// </summary>
  /// <remarks>
  /// 用户与角色的一对多关系
  /// </remarks>
  [Navigate(NavigateType.OneToMany, nameof(LeanUserRole.UserId))]
  public virtual List<LeanUserRole> UserRoles { get; set; } = new();

  /// <summary>
  /// 用户部门列表
  /// </summary>
  /// <remarks>
  /// 用户与部门的一对多关系
  /// </remarks>
  [Navigate(NavigateType.OneToMany, nameof(LeanUserDept.UserId))]
  public virtual List<LeanUserDept> UserDepts { get; set; } = new();
}