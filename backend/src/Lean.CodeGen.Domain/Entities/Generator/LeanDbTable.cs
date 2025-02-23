//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: LeanDbTable.cs
// 功能描述: 数据库表信息实体
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using System;
using System.Collections.Generic;
using SqlSugar;
using Lean.CodeGen.Domain.Entities;

namespace Lean.CodeGen.Domain.Entities.Generator
{
    /// <summary>
    /// 数据库表信息实体
    /// </summary>
    [SugarTable("lean_gen_db_table", "数据库表信息")]
    public class LeanDbTable : LeanBaseEntity
    {
        /// <summary>
        /// 表名称
        /// </summary>
        /// <remarks>
        /// 数据库中的实际表名
        /// </remarks>
        [SugarColumn(ColumnName = "table_name", ColumnDescription = "表名称", Length = 200, IsNullable = false, ColumnDataType = "nvarchar")]
        public string TableName { get; set; } = default!;

        /// <summary>
        /// 表描述
        /// </summary>
        /// <remarks>
        /// 表的中文名称或描述信息
        /// </remarks>
        [SugarColumn(ColumnName = "table_comment", ColumnDescription = "表描述", Length = 500, IsNullable = true, ColumnDataType = "nvarchar")]
        public string? TableComment { get; set; }

        /// <summary>
        /// 实体类名称
        /// </summary>
        [SugarColumn(ColumnName = "class_name", ColumnDescription = "实体类名称", Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
        public string ClassName { get; set; } = default!;

        /// <summary>
        /// 使用的模板（crud单表操作、tree树表操作、sub主子表操作）
        /// </summary>
        [SugarColumn(ColumnName = "template_category", ColumnDescription = "使用的模板", Length = 50, IsNullable = false, ColumnDataType = "nvarchar")]
        public string TemplateCategory { get; set; } = default!;

        /// <summary>
        /// 生成包路径
        /// </summary>
        [SugarColumn(ColumnName = "package_name", ColumnDescription = "生成包路径", Length = 200, IsNullable = false, ColumnDataType = "nvarchar")]
        public string PackageName { get; set; } = default!;

        /// <summary>
        /// 生成模块名
        /// </summary>
        [SugarColumn(ColumnName = "module_name", ColumnDescription = "生成模块名", Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
        public string ModuleName { get; set; } = default!;

        /// <summary>
        /// 生成业务名
        /// </summary>
        [SugarColumn(ColumnName = "business_name", ColumnDescription = "生成业务名", Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
        public string BusinessName { get; set; } = default!;

        /// <summary>
        /// 生成功能名
        /// </summary>
        [SugarColumn(ColumnName = "function_name", ColumnDescription = "生成功能名", Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
        public string FunctionName { get; set; } = default!;

        /// <summary>
        /// 生成功能作者
        /// </summary>
        [SugarColumn(ColumnName = "function_author", ColumnDescription = "生成功能作者", Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
        public string FunctionAuthor { get; set; } = default!;

        /// <summary>
        /// 父表名称
        /// </summary>
        [SugarColumn(ColumnName = "parent_table_name", ColumnDescription = "父表名称", Length = 200, IsNullable = true, ColumnDataType = "nvarchar")]
        public string? ParentTableName { get; set; }

        /// <summary>
        /// 父表主键
        /// </summary>
        [SugarColumn(ColumnName = "parent_table_fk", ColumnDescription = "父表主键", Length = 100, IsNullable = true, ColumnDataType = "nvarchar")]
        public string? ParentTableFk { get; set; }

        /// <summary>
        /// 主键信息
        /// </summary>
        [SugarColumn(ColumnName = "pk_column", ColumnDescription = "主键信息", Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
        public string PkColumn { get; set; } = default!;

        /// <summary>
        /// 其它生成选项
        /// </summary>
        [SugarColumn(ColumnName = "options", ColumnDescription = "其它生成选项", Length = 1000, IsNullable = true, ColumnDataType = "nvarchar")]
        public string? Options { get; set; }

        /// <summary>
        /// 数据源Id
        /// </summary>
        [SugarColumn(ColumnName = "data_source_id", ColumnDescription = "数据源Id", IsNullable = false, ColumnDataType = "bigint")]
        public long DataSourceId { get; set; }

        /// <summary>
        /// 表来源（0手动创建 1数据库导入）
        /// </summary>
        [SugarColumn(ColumnName = "source_type", ColumnDescription = "表来源", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
        public int SourceType { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        /// <remarks>
        /// 显示顺序，数值越小越靠前
        /// </remarks>
        [SugarColumn(ColumnName = "order_num", ColumnDescription = "排序号", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
        public int OrderNum { get; set; }

        /// <summary>
        /// 所属数据源
        /// </summary>
        [Navigate(NavigateType.ManyToOne, nameof(DataSourceId))]
        public virtual LeanDataSource DataSource { get; set; }

        /// <summary>
        /// 表的列信息
        /// </summary>
        /// <remarks>
        /// 与数据库列的一对多关系
        /// </remarks>
        [Navigate(NavigateType.OneToMany, nameof(LeanDbColumn.TableId))]
        public virtual ICollection<LeanDbColumn> Columns { get; set; } = new List<LeanDbColumn>();
    }
}