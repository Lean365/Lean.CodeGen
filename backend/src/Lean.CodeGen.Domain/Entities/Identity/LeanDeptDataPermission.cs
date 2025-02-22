using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Domain.Entities;

namespace Lean.CodeGen.Domain.Entities.Identity;

/// <summary>
/// 部门数据权限
/// </summary>
public class LeanDeptDataPermission : LeanBaseEntity
{
  /// <summary>
  /// 部门ID
  /// </summary>
  public long DeptId { get; set; }

  /// <summary>
  /// 可访问的部门ID
  /// </summary>
  public long AccessibleDeptId { get; set; }

  /// <summary>
  /// 数据权限类型
  /// </summary>
  public LeanDataScopeType DataScope { get; set; }
}