//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: LeanDbColumn.cs
// 功能描述: 数据库列信息实体
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
    /// 数据库列信息实体
    /// </summary>
    [SugarTable("lean_db_column", "数据库列信息")]
    public class LeanDbColumn : LeanBaseEntity
    {
        #region 数据库元数据

        /// <summary>
        /// 列名
        /// </summary>
        /// <remarks>
        /// 数据库中的实际列名
        /// </remarks>
        [SugarColumn(ColumnName = "column_name", ColumnDescription = "列名", Length = 200, IsNullable = false, ColumnDataType = "nvarchar")]
        public string ColumnName { get; set; } = default!;

        /// <summary>
        /// 列描述
        /// </summary>
        /// <remarks>
        /// 列的中文名称或描述信息
        /// </remarks>
        [SugarColumn(ColumnName = "column_comment", ColumnDescription = "列描述", Length = 500, IsNullable = true, ColumnDataType = "nvarchar")]
        public string? ColumnComment { get; set; }

        /// <summary>
        /// 列类型
        /// </summary>
        /// <remarks>
        /// 列在数据库中的原始类型
        /// </remarks>
        [SugarColumn(ColumnName = "column_type", ColumnDescription = "列类型", Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
        public string ColumnType { get; set; } = default!;

        /// <summary>
        /// .NET类型
        /// </summary>
        /// <remarks>
        /// 对应的.NET类型名称
        /// </remarks>
        [SugarColumn(ColumnName = "net_type", ColumnDescription = ".NET类型", Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
        public string NetType { get; set; } = default!;

        /// <summary>
        /// Java类型
        /// </summary>
        /// <remarks>
        /// 对应的Java类型名称
        /// </remarks>
        [SugarColumn(ColumnName = "java_type", ColumnDescription = "Java类型", Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
        public string JavaType { get; set; } = default!;

        /// <summary>
        /// TypeScript类型
        /// </summary>
        /// <remarks>
        /// 对应的TypeScript类型名称
        /// </remarks>
        [SugarColumn(ColumnName = "ts_type", ColumnDescription = "TypeScript类型", Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
        public string TsType { get; set; } = default!;

        /// <summary>
        /// 是否主键
        /// </summary>
        /// <remarks>
        /// 标识该列是否为主键
        /// </remarks>
        [SugarColumn(ColumnName = "is_pk", ColumnDescription = "是否主键", IsNullable = false, ColumnDataType = "bit")]
        public bool IsPk { get; set; }

        /// <summary>
        /// 是否自增
        /// </summary>
        /// <remarks>
        /// 标识该列是否为自增列
        /// </remarks>
        [SugarColumn(ColumnName = "is_increment", ColumnDescription = "是否自增", IsNullable = false, ColumnDataType = "bit")]
        public bool IsIncrement { get; set; }

        /// <summary>
        /// 是否允许为空
        /// </summary>
        /// <remarks>
        /// 标识该列是否允许为空值
        /// </remarks>
        [SugarColumn(ColumnName = "is_nullable", ColumnDescription = "是否允许为空", IsNullable = false, ColumnDataType = "bit")]
        public bool IsNullable { get; set; }

        /// <summary>
        /// 列长度
        /// </summary>
        /// <remarks>
        /// 字符串类型的最大长度
        /// </remarks>
        [SugarColumn(ColumnName = "length", ColumnDescription = "列长度", IsNullable = true, ColumnDataType = "int")]
        public int? Length { get; set; }

        /// <summary>
        /// 精度
        /// </summary>
        /// <remarks>
        /// 数值类型的总位数
        /// </remarks>
        [SugarColumn(ColumnName = "precision", ColumnDescription = "精度", IsNullable = true, ColumnDataType = "int")]
        public int? Precision { get; set; }

        /// <summary>
        /// 小数位数
        /// </summary>
        /// <remarks>
        /// 数值类型的小数位数
        /// </remarks>
        [SugarColumn(ColumnName = "scale", ColumnDescription = "小数位数", IsNullable = true, ColumnDataType = "int")]
        public int? Scale { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        /// <remarks>
        /// 列的默认值
        /// </remarks>
        [SugarColumn(ColumnName = "default_value", ColumnDescription = "默认值", Length = 200, IsNullable = true, ColumnDataType = "nvarchar")]
        public string? DefaultValue { get; set; }

        #endregion 数据库元数据

        #region 表单配置

        /// <summary>
        /// 是否必填
        /// </summary>
        /// <remarks>
        /// 标识该字段在表单中是否必填
        /// </remarks>
        [SugarColumn(ColumnName = "is_required", ColumnDescription = "是否必填", IsNullable = false, ColumnDataType = "bit")]
        public bool IsRequired { get; set; }

        /// <summary>
        /// 是否为插入字段
        /// </summary>
        /// <remarks>
        /// 标识该字段是否为插入字段
        /// </remarks>
        [SugarColumn(ColumnName = "is_insert", ColumnDescription = "是否为插入字段", IsNullable = false, ColumnDataType = "bit")]
        public bool IsInsert { get; set; }

        /// <summary>
        /// 是否编辑字段
        /// </summary>
        /// <remarks>
        /// 标识该字段是否为编辑字段
        /// </remarks>
        [SugarColumn(ColumnName = "is_edit", ColumnDescription = "是否编辑字段", IsNullable = false, ColumnDataType = "bit")]
        public bool IsEdit { get; set; }

        /// <summary>
        /// 显示类型
        /// </summary>
        /// <remarks>
        /// 字段显示类型（文本框、文本域、下拉框、单选框、复选框、日期控件等）
        /// </remarks>
        [SugarColumn(ColumnName = "html_type", ColumnDescription = "显示类型", Length = 100, IsNullable = true, ColumnDataType = "nvarchar")]
        public string? HtmlType { get; set; }

        /// <summary>
        /// 字典类型
        /// </summary>
        /// <remarks>
        /// 字段对应的数据字典类型
        /// </remarks>
        [SugarColumn(ColumnName = "dict_type", ColumnDescription = "字典类型", Length = 200, IsNullable = true, ColumnDataType = "nvarchar")]
        public string? DictType { get; set; }

        /// <summary>
        /// 验证规则
        /// </summary>
        /// <remarks>
        /// 字段验证规则（如：邮箱、手机号、URL等）
        /// </remarks>
        [SugarColumn(ColumnName = "validate_rule", ColumnDescription = "验证规则", Length = 500, IsNullable = true, ColumnDataType = "nvarchar")]
        public string? ValidateRule { get; set; }

        #endregion 表单配置

        #region 列表配置

        /// <summary>
        /// 是否列表显示
        /// </summary>
        /// <remarks>
        /// 标识该字段是否在列表页面显示
        /// </remarks>
        [SugarColumn(ColumnName = "is_list", ColumnDescription = "是否列表字段", IsNullable = false, ColumnDataType = "bit")]
        public bool IsList { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        /// <remarks>
        /// 字段在表中的排序号，数值越小越靠前
        /// </remarks>
        [SugarColumn(ColumnName = "order_num", ColumnDescription = "排序号", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
        public int OrderNum { get; set; }

        #endregion 列表配置

        #region 查询配置

        /// <summary>
        /// 是否查询条件
        /// </summary>
        /// <remarks>
        /// 标识该字段是否作为查询条件
        /// </remarks>
        [SugarColumn(ColumnName = "is_query", ColumnDescription = "是否查询字段", IsNullable = false, ColumnDataType = "bit")]
        public bool IsQuery { get; set; }

        /// <summary>
        /// 查询方式
        /// </summary>
        /// <remarks>
        /// 字段查询方式（等于、不等于、大于、小于、范围、模糊查询等）
        /// </remarks>
        [SugarColumn(ColumnName = "query_type", ColumnDescription = "查询方式", Length = 50, IsNullable = true, ColumnDataType = "nvarchar")]
        public string? QueryType { get; set; }

        #endregion 查询配置

        #region 导入导出

        /// <summary>
        /// 是否导入字段
        /// </summary>
        /// <remarks>
        /// 标识该字段是否支持导入
        /// </remarks>
        [SugarColumn(ColumnName = "is_import", ColumnDescription = "是否导入字段", IsNullable = false, ColumnDataType = "bit")]
        public bool IsImport { get; set; }

        /// <summary>
        /// 是否导出字段
        /// </summary>
        /// <remarks>
        /// 标识该字段是否支持导出
        /// </remarks>
        [SugarColumn(ColumnName = "is_export", ColumnDescription = "是否导出字段", IsNullable = false, ColumnDataType = "bit")]
        public bool IsExport { get; set; }

        #endregion 导入导出

        #region 业务标识

        /// <summary>
        /// 是否业务键
        /// </summary>
        /// <remarks>
        /// 标识该字段是否为业务主键（除数据库主键外的唯一标识）
        /// </remarks>
        [SugarColumn(ColumnName = "is_business_key", ColumnDescription = "是否业务键", IsNullable = false, ColumnDataType = "bit")]
        public bool IsBusinessKey { get; set; }

        /// <summary>
        /// 是否版本字段
        /// </summary>
        /// <remarks>
        /// 标识该字段是否为版本控制字段
        /// </remarks>
        [SugarColumn(ColumnName = "is_version", ColumnDescription = "是否版本字段", IsNullable = false, ColumnDataType = "bit")]
        public bool IsVersion { get; set; }

        #endregion 业务标识

        #region 关联关系

        /// <summary>
        /// 所属表Id
        /// </summary>
        /// <remarks>
        /// 关联的数据库表Id
        /// </remarks>
        [SugarColumn(ColumnName = "table_id", ColumnDescription = "所属表Id", IsNullable = false, ColumnDataType = "bigint")]
        public long TableId { get; set; }

        /// <summary>
        /// 所属表
        /// </summary>
        /// <remarks>
        /// 关联的数据库表信息
        /// </remarks>
        [Navigate(NavigateType.ManyToOne, nameof(TableId))]
        public virtual LeanDbTable Table { get; set; } = default!;

        #endregion 关联关系
    }
}