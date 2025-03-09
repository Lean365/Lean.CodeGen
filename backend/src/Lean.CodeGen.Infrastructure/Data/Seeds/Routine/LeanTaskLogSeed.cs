using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Domain.Entities.Routine;
using SqlSugar;
using System.Threading.Tasks;
using NLog;
using ILogger = NLog.ILogger;
using Lean.CodeGen.Infrastructure.Data.Seeds.Extensions;

namespace Lean.CodeGen.Infrastructure.Data.Seeds.Routine;

/// <summary>
/// 任务日志种子数据
/// </summary>
public class LeanTaskLogSeed
{
  private readonly ISqlSugarClient _db;
  private readonly ILogger _logger;

  public LeanTaskLogSeed(ISqlSugarClient db)
  {
    _db = db;
    _logger = LogManager.GetCurrentClassLogger();
  }

  public async Task InitializeAsync()
  {
    _logger.Info("开始初始化任务日志数据...");

    // 任务日志一般不需要初始化种子数据
    _logger.Info("任务日志数据初始化完成");
  }
}