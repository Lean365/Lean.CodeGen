//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: LeanTableConfig.cs
// 功能描述: 表和配置的关联实体
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using System;
using SqlSugar;
using Lean.CodeGen.Domain.Entities;

namespace Lean.CodeGen.Domain.Entities.Generator
{
    /// <summary>
    /// 表和配置的关联实体
    /// </summary>
    [SugarTable("lean_gen_table_config", "表和配置关联")]
    public class LeanTableConfig : LeanBaseEntity
    {
        /// <summary>
        /// 表Id
        /// </summary>
        /// <remarks>
        /// 关联的数据库表Id
        /// </remarks>
        [SugarColumn(ColumnName = "table_id", ColumnDescription = "表Id", IsNullable = false, ColumnDataType = "bigint")]
        public long TableId { get; set; }

        /// <summary>
        /// 配置Id
        /// </summary>
        /// <remarks>
        /// 关联的代码生成配置Id
        /// </remarks>
        [SugarColumn(ColumnName = "config_id", ColumnDescription = "配置Id", IsNullable = false, ColumnDataType = "bigint")]
        public long ConfigId { get; set; }

        /// <summary>
        /// 实体名称
        /// </summary>
        /// <remarks>
        /// 生成的实体类名称
        /// </remarks>
        [SugarColumn(ColumnName = "entity_name", ColumnDescription = "实体名称", Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
        public string EntityName { get; set; } = default!;

        /// <summary>
        /// 业务名称
        /// </summary>
        /// <remarks>
        /// 业务模块名称
        /// </remarks>
        [SugarColumn(ColumnName = "business_name", ColumnDescription = "业务名称", Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
        public string BusinessName { get; set; } = default!;

        /// <summary>
        /// 功能名称
        /// </summary>
        /// <remarks>
        /// 功能的中文名称
        /// </remarks>
        [SugarColumn(ColumnName = "function_name", ColumnDescription = "功能名称", Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
        public string FunctionName { get; set; } = default!;

        /// <summary>
        /// 所属表
        /// </summary>
        /// <remarks>
        /// 关联的数据库表
        /// </remarks>
        [Navigate(NavigateType.ManyToOne, nameof(TableId))]
        public virtual LeanDbTable Table { get; set; }

        /// <summary>
        /// 所属配置
        /// </summary>
        /// <remarks>
        /// 关联的代码生成配置
        /// </remarks>
        [Navigate(NavigateType.ManyToOne, nameof(ConfigId))]
        public virtual LeanGenConfig Config { get; set; }
    }
}