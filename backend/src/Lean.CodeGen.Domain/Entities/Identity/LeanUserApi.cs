//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: LeanUserApi.cs
// 功能描述: 用户API关联实体
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using SqlSugar;

namespace Lean.CodeGen.Domain.Entities.Identity;

/// <summary>
/// 用户API关联实体
/// </summary>
[SugarTable("lean_user_api", "用户API关联表")]
[SugarIndex("uk_user_api", nameof(UserId), OrderByType.Asc, nameof(ApiId), OrderByType.Asc)]
public class LeanUserApi : LeanBaseEntity
{
    /// <summary>
    /// 用户ID
    /// </summary>
    /// <remarks>
    /// 关联的用户ID
    /// </remarks>
    [SugarColumn(ColumnName = "user_id", ColumnDescription = "用户ID", IsNullable = false, ColumnDataType = "bigint")]
    public long UserId { get; set; }

    /// <summary>
    /// API ID
    /// </summary>
    /// <remarks>
    /// 关联的API ID
    /// </remarks>
    [SugarColumn(ColumnName = "api_id", ColumnDescription = "API ID", IsNullable = false, ColumnDataType = "bigint")]
    public long ApiId { get; set; }

    /// <summary>
    /// 用户
    /// </summary>
    /// <remarks>
    /// 关联的用户实体
    /// </remarks>
    [Navigate(NavigateType.OneToOne, nameof(UserId))]
    public virtual LeanUser User { get; set; } = default!;

    /// <summary>
    /// API
    /// </summary>
    /// <remarks>
    /// 关联的API实体
    /// </remarks>
    [Navigate(NavigateType.OneToOne, nameof(ApiId))]
    public virtual LeanApi Api { get; set; } = default!;
}