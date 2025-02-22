using Lean.CodeGen.Infrastructure.Data.Context;

namespace Lean.CodeGen.Infrastructure.Data.Seeds;

/// <summary>
/// 种子数据接口
/// </summary>
public interface ILeanDataSeed
{
  /// <summary>
  /// 获取排序号
  /// </summary>
  int Order { get; }

  /// <summary>
  /// 执行种子数据初始化
  /// </summary>
  Task SeedAsync(LeanDbContext dbContext);
}