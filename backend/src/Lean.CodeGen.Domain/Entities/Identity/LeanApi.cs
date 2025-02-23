//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: LeanApi.cs
// 功能描述: API权限实体
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Domain.Entities;
using SqlSugar;

namespace Lean.CodeGen.Domain.Entities.Identity;

/// <summary>
/// API权限实体
/// </summary>
[SugarTable("lean_id_api", "API权限表")]
[SugarIndex("uk_path", nameof(Path), OrderByType.Asc, true)]
public class LeanApi : LeanBaseEntity
{
    /// <summary>
    /// API名称
    /// </summary>
    /// <remarks>
    /// API的显示名称，用于界面展示
    /// </remarks>
    [SugarColumn(ColumnName = "api_name", ColumnDescription = "API名称", Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
    public string ApiName { get; set; } = string.Empty;

    /// <summary>
    /// API路径
    /// </summary>
    /// <remarks>
    /// API的请求路径，如：/api/system/user/list
    /// </remarks>
    [SugarColumn(ColumnName = "path", ColumnDescription = "API路径", Length = 200, IsNullable = false, UniqueGroupNameList = new[] { "uk_path" }, ColumnDataType = "nvarchar")]
    public string Path { get; set; } = string.Empty;

    /// <summary>
    /// 请求方法
    /// </summary>
    /// <remarks>
    /// HTTP请求方法：GET、POST、PUT、DELETE等
    /// </remarks>
    [SugarColumn(ColumnName = "method", ColumnDescription = "请求方法", Length = 10, IsNullable = false, ColumnDataType = "nvarchar")]
    public string Method { get; set; } = string.Empty;

    /// <summary>
    /// 所属模块
    /// </summary>
    /// <remarks>
    /// API所属的业务模块，如：系统管理、用户管理等
    /// </remarks>
    [SugarColumn(ColumnName = "module", ColumnDescription = "所属模块", Length = 50, IsNullable = false, ColumnDataType = "nvarchar")]
    public string Module { get; set; } = string.Empty;

    /// <summary>
    /// API状态
    /// </summary>
    [SugarColumn(ColumnName = "api_status", ColumnDescription = "API状态", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
    public LeanApiStatus ApiStatus { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    [SugarColumn(ColumnName = "order_num", ColumnDescription = "排序号", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
    public int OrderNum { get; set; }

    /// <summary>
    /// 是否内置
    /// </summary>
    /// <remarks>
    /// 是否为系统内置API：No-否，Yes-是
    /// 内置API不允许删除
    /// </remarks>
    [SugarColumn(ColumnName = "is_builtin", ColumnDescription = "是否内置", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
    public LeanBuiltinStatus IsBuiltin { get; set; }

    /// <summary>
    /// 角色API关联
    /// </summary>
    /// <remarks>
    /// API与角色的多对多关系
    /// </remarks>
    [Navigate(NavigateType.OneToMany, nameof(LeanRoleApi.ApiId))]
    public virtual ICollection<LeanRoleApi> RoleApis { get; set; } = new List<LeanRoleApi>();
}