using SqlSugar;
using Lean.CodeGen.Infrastructure.Data.Context;
using Lean.CodeGen.Infrastructure.Data.Seeds;

namespace Lean.CodeGen.Infrastructure.Data.Initializer;

/// <summary>
/// 数据库初始化器
/// </summary>
public class LeanDbInitializer
{
  private readonly LeanDbContext _dbContext;
  private readonly NLog.ILogger _logger;

  public LeanDbInitializer(LeanDbContext dbContext)
  {
    _dbContext = dbContext;
    _logger = NLog.LogManager.GetCurrentClassLogger();
  }

  /// <summary>
  /// 初始化数据库
  /// </summary>
  public async Task InitializeAsync()
  {
    try
    {
      // 创建数据库(如果不存在)
      _dbContext.GetDatabase().DbMaintenance.CreateDatabase();
      _logger.Info("数据库创建成功");

      // 初始化表结构
      _dbContext.ConfigureEntities();
      _logger.Info("表结构初始化成功");

      // 初始化种子数据
      var seeder = new LeanDataSeeder(_dbContext);
      await seeder.SeedAllAsync();
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "初始化数据库失败");
      throw;
    }
  }
}

