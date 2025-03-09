using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Domain.Entities.Admin;
using SqlSugar;
using System.Threading.Tasks;
using NLog;
using ILogger = NLog.ILogger;
using Lean.CodeGen.Infrastructure.Data.Seeds.Extensions;

namespace Lean.CodeGen.Infrastructure.Data.Seeds.Admin;

/// <summary>
/// 系统配置种子数据
/// </summary>
public class LeanConfigSeed
{
  /// <summary>
  /// 数据库访问对象
  /// </summary>
  private readonly ISqlSugarClient _db;

  /// <summary>
  /// 日志记录器
  /// </summary>
  private readonly ILogger _logger;

  /// <summary>
  /// 初始化系统配置种子数据类
  /// </summary>
  /// <param name="db">数据库访问对象</param>
  public LeanConfigSeed(ISqlSugarClient db)
  {
    _db = db;
    _logger = LogManager.GetCurrentClassLogger();
  }

  /// <summary>
  /// 初始化系统配置数据
  /// </summary>
  /// <returns>异步任务</returns>
  public async Task InitializeAsync()
  {
    _logger.Info("开始初始化系统配置数据...");

    // 数据库配置
    await CreateOrUpdateConfig("Database:DbType", "1", "数据库类型", "数据库类型：0-SqlServer, 1-MySql, 2-PostgreSql, 3-Oracle, 4-Sqlite", "system");
    await CreateOrUpdateConfig("Database:EnableUnderLine", "true", "启用下划线", "数据库表和字段命名是否使用下划线", "system");
    await CreateOrUpdateConfig("Database:EnableSqlLog", "false", "启用SQL日志", "是否启用SQL执行日志记录", "system");
    await CreateOrUpdateConfig("Database:EnableAutoMigrate", "true", "启用自动迁移", "是否启用数据库自动迁移", "system");
    await CreateOrUpdateConfig("Database:EnableInitData", "true", "启用初始数据", "是否启用初始化种子数据", "system");

    // 缓存配置
    await CreateOrUpdateConfig("Cache:DefaultExpiration", "01:00:00", "默认过期时间", "缓存默认过期时间", "system");
    await CreateOrUpdateConfig("Cache:EnableRedis", "false", "启用Redis", "是否启用Redis缓存", "system");
    await CreateOrUpdateConfig("Cache:KeyPrefix", "lean:", "缓存前缀", "缓存键前缀", "system");
    await CreateOrUpdateConfig("Cache:EnableStats", "true", "启用统计", "是否启用缓存统计", "system");
    await CreateOrUpdateConfig("Cache:EnableCompression", "true", "启用压缩", "是否启用缓存压缩", "system");
    await CreateOrUpdateConfig("Cache:CompressionThreshold", "1024", "压缩阈值", "启用压缩的大小阈值(字节)", "system");

    // Redis配置
    await CreateOrUpdateConfig("Cache:Redis:ConnectionString", "localhost:6379", "连接字符串", "Redis连接字符串", "system");
    await CreateOrUpdateConfig("Cache:Redis:Database", "0", "数据库索引", "Redis数据库索引", "system");
    await CreateOrUpdateConfig("Cache:Redis:PoolSize", "50", "连接池大小", "Redis连接池大小", "system");
    await CreateOrUpdateConfig("Cache:Redis:ConnectionTimeout", "5000", "连接超时", "Redis连接超时时间(毫秒)", "system");
    await CreateOrUpdateConfig("Cache:Redis:OperationTimeout", "1000", "操作超时", "Redis操作超时时间(毫秒)", "system");
    await CreateOrUpdateConfig("Cache:Redis:RetryCount", "3", "重试次数", "Redis操作重试次数", "system");
    await CreateOrUpdateConfig("Cache:Redis:RetryInterval", "1000", "重试间隔", "Redis操作重试间隔(毫秒)", "system");
    await CreateOrUpdateConfig("Cache:Redis:EnableSsl", "false", "启用SSL", "是否启用SSL连接", "system");
    await CreateOrUpdateConfig("Cache:Redis:EnableCluster", "false", "启用集群", "是否启用Redis集群", "system");

    // 内存缓存配置
    await CreateOrUpdateConfig("Cache:Memory:SizeLimit", "1024", "大小限制", "内存缓存大小限制(MB)", "system");
    await CreateOrUpdateConfig("Cache:Memory:CompactionPercentage", "0.05", "压缩比例", "内存缓存压缩比例", "system");
    await CreateOrUpdateConfig("Cache:Memory:ExpirationScanFrequency", "60", "扫描频率", "过期项扫描频率(秒)", "system");
    await CreateOrUpdateConfig("Cache:Memory:EnableSlidingExpiration", "true", "启用滑动过期", "是否启用滑动过期", "system");
    await CreateOrUpdateConfig("Cache:Memory:SlidingExpiration", "00:30:00", "滑动过期时间", "滑动过期时间", "system");

    // JWT配置
    await CreateOrUpdateConfig("JwtSettings:Issuer", "LeanCodeGen", "发行者", "JWT令牌发行者", "system");
    await CreateOrUpdateConfig("JwtSettings:Audience", "LeanCodeGen.WebApi", "受众", "JWT令牌受众", "system");
    await CreateOrUpdateConfig("JwtSettings:ExpiresInMinutes", "60", "过期时间", "JWT令牌过期时间(分钟)", "system");

    // 安全配置
    await CreateOrUpdateConfig("Security:EnableAntiforgery", "true", "启用防伪", "是否启用防伪验证", "system");
    await CreateOrUpdateConfig("Security:EnableRateLimit", "true", "启用限流", "是否启用请求限流", "system");
    await CreateOrUpdateConfig("Security:EnableSqlInjection", "true", "启用SQL注入防护", "是否启用SQL注入防护", "system");
    await CreateOrUpdateConfig("Security:DefaultPassword", "123456", "默认密码", "用户默认密码", "system");

    // 防伪配置
    await CreateOrUpdateConfig("Security:Antiforgery:HeaderName", "X-XSRF-TOKEN", "请求头名称", "防伪请求头名称", "system");
    await CreateOrUpdateConfig("Security:Antiforgery:CookieName", "XSRF-TOKEN", "Cookie名称", "防伪Cookie名称", "system");

    // 限流配置
    await CreateOrUpdateConfig("Security:RateLimit:DefaultRateLimit:Seconds", "60", "默认时间窗口", "默认限流时间窗口(秒)", "system");
    await CreateOrUpdateConfig("Security:RateLimit:DefaultRateLimit:MaxRequests", "100", "默认最大请求数", "默认限流最大请求数", "system");
    await CreateOrUpdateConfig("Security:RateLimit:IpRateLimit:Seconds", "1", "IP时间窗口", "IP限流时间窗口(秒)", "system");
    await CreateOrUpdateConfig("Security:RateLimit:IpRateLimit:MaxRequests", "10", "IP最大请求数", "IP限流最大请求数", "system");
    await CreateOrUpdateConfig("Security:RateLimit:UserRateLimit:Seconds", "60", "用户时间窗口", "用户限流时间窗口(秒)", "system");
    await CreateOrUpdateConfig("Security:RateLimit:UserRateLimit:MaxRequests", "1000", "用户最大请求数", "用户限流最大请求数", "system");

    // 本地化配置
    await CreateOrUpdateConfig("LocalizationSettings:DefaultLanguage", "zh-CN", "默认语言", "系统默认语言", "system");
    await CreateOrUpdateConfig("LocalizationSettings:SystemLanguage", "zh-CN", "系统语言", "系统内部语言", "system");
    await CreateOrUpdateConfig("LocalizationSettings:EnableAutoDetectLanguage", "true", "启用语言自动检测", "是否启用语言自动检测", "system");
    await CreateOrUpdateConfig("LocalizationSettings:EnableLanguageCookie", "true", "启用语言Cookie", "是否启用语言Cookie", "system");
    await CreateOrUpdateConfig("LocalizationSettings:LanguageCookieName", "lang", "语言Cookie名称", "语言Cookie名称", "system");
    await CreateOrUpdateConfig("LocalizationSettings:LanguageCookieExpireDays", "30", "Cookie过期天数", "语言Cookie过期天数", "system");

    _logger.Info("系统配置数据初始化完成");
  }

  /// <summary>
  /// 创建或更新配置项
  /// </summary>
  /// <param name="key">配置键</param>
  /// <param name="value">配置值</param>
  /// <param name="name">配置名称</param>
  /// <param name="description">配置描述</param>
  /// <param name="group">配置分组</param>
  private async Task CreateOrUpdateConfig(string key, string value, string name, string description, string group)
  {
    var config = new LeanConfig
    {
      ConfigKey = key,
      ConfigValue = value,
      ConfigName = name,
      ConfigType = 0,
      ConfigGroup = group,
      ConfigStatus = 0,
      IsBuiltin = 0
    };

    var exists = await _db.Queryable<LeanConfig>()
        .FirstAsync(c => c.ConfigKey == key);

    if (exists != null)
    {
      config.Id = exists.Id;
      config.CopyAuditFields(exists).InitAuditFields(true);
      await _db.Updateable(config).ExecuteCommandAsync();
      _logger.Info($"更新配置项: {key}");
    }
    else
    {
      config.InitAuditFields();
      await _db.Insertable(config).ExecuteCommandAsync();
      _logger.Info($"新增配置项: {key}");
    }
  }
}