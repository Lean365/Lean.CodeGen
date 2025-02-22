using System.Reflection;
using Lean.CodeGen.Infrastructure.Data.Context;

namespace Lean.CodeGen.Infrastructure.Data.Seeds;

/// <summary>
/// 种子数据执行器
/// </summary>
public class LeanDataSeeder
{
  private readonly LeanDbContext _dbContext;
  private readonly NLog.ILogger _logger;

  public LeanDataSeeder(LeanDbContext dbContext)
  {
    _dbContext = dbContext;
    _logger = NLog.LogManager.GetCurrentClassLogger();
  }

  /// <summary>
  /// 执行所有种子数据
  /// </summary>
  public async Task SeedAllAsync()
  {
    try
    {
      // 获取所有种子数据类
      var seedTypes = Assembly.GetExecutingAssembly()
          .GetTypes()
          .Where(t => !t.IsAbstract && typeof(ILeanDataSeed).IsAssignableFrom(t));

      // 创建种子数据实例并按Order排序
      var seeds = seedTypes
          .Select(t => (ILeanDataSeed)Activator.CreateInstance(t)!)
          .OrderBy(s => s.Order)
          .ToList();

      // 执行种子数据
      foreach (var seed in seeds)
      {
        await seed.SeedAsync(_dbContext);
        _logger.Info($"执行种子数据: {seed.GetType().Name}");
      }

      _logger.Info("所有种子数据执行完成");
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "执行种子数据失败");
      throw;
    }
  }
}