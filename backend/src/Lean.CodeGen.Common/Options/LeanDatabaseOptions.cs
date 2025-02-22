using SqlSugar;

namespace Lean.CodeGen.Common.Options;

/// <summary>
/// 数据库配置选项
/// </summary>
public class LeanDatabaseOptions
{
  /// <summary>
  /// 配置节点
  /// </summary>
  public const string Position = "Database";

  /// <summary>
  /// 数据库类型
  /// </summary>
  public DbType DbType { get; set; }

  /// <summary>
  /// 连接字符串
  /// </summary>
  public string ConnectionString { get; set; } = string.Empty;

  /// <summary>
  /// 是否启用下划线命名
  /// </summary>
  public bool EnableUnderLine { get; set; } = true;

  /// <summary>
  /// 是否启用SQL日志
  /// </summary>
  public bool EnableSqlLog { get; set; }

  /// <summary>
  /// 是否自动迁移
  /// </summary>
  public bool EnableAutoMigrate { get; set; }

  /// <summary>
  /// 是否初始化数据
  /// </summary>
  public bool EnableInitData { get; set; }
}