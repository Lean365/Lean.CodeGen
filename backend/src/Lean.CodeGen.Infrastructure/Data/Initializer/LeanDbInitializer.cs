using SqlSugar;
using NLog;
using Lean.CodeGen.Infrastructure.Data.Context;
using Lean.CodeGen.Infrastructure.Data.Seeds.Admin;
using Lean.CodeGen.Infrastructure.Data.Seeds.Identity;
using ILogger = NLog.ILogger;
using Microsoft.Extensions.Options;
using Lean.CodeGen.Common.Options;

namespace Lean.CodeGen.Infrastructure.Data.Initializer;

/// <summary>
/// 数据库初始化器
/// </summary>
public class LeanDbInitializer
{
  private readonly LeanDbContext _dbContext;
  private readonly ILogger _logger;
  private readonly IOptions<LeanSecurityOptions> _securityOptions;

  public LeanDbInitializer(
      LeanDbContext dbContext,
      IOptions<LeanSecurityOptions> securityOptions)
  {
    _dbContext = dbContext;
    _logger = LogManager.GetCurrentClassLogger();
    _securityOptions = securityOptions;
  }

  /// <summary>
  /// 初始化数据库
  /// </summary>
  public async Task InitializeAsync()
  {
    try
    {
      var db = _dbContext.GetDatabase();

      // 创建数据库(如果不存在)
      db.DbMaintenance.CreateDatabase();
      _logger.Info("数据库创建成功");

      // 初始化表结构
      _dbContext.ConfigureEntities();
      _logger.Info("表结构初始化成功");

      // 初始化种子数据
      _logger.Info("开始初始化种子数据...");

      // 1. 初始化语言数据
      await new LeanLanguageSeed(db).InitializeAsync();

      // 2. 初始化翻译数据
      await new LeanTranslationSeed(db).InitializeAsync();

      // 3. 初始化用户数据
      await new LeanUserSeed(db, _securityOptions).InitializeAsync();

      // 4. 初始化角色数据
      await new LeanRoleSeed(db).InitializeAsync();

      // 5. 初始化岗位数据
      await new LeanPostSeed(db).InitializeAsync();

      // 6. 初始化部门数据
      await new LeanDeptSeed(db).InitializeAsync();

      // 7. 初始化菜单数据
      await new LeanMenuSeed(db).InitializeAsync();

      _logger.Info("种子数据初始化完成");
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "初始化数据库失败");
      throw;
    }
  }
}

