using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Domain.Entities.Routine;
using SqlSugar;
using System.Threading.Tasks;
using NLog;
using ILogger = NLog.ILogger;
using Lean.CodeGen.Infrastructure.Data.Seeds.Extensions;

namespace Lean.CodeGen.Infrastructure.Data.Seeds.Routine;

/// <summary>
/// 任务种子数据
/// </summary>
public class LeanQuartzTaskSeed
{
  private readonly ISqlSugarClient _db;
  private readonly ILogger _logger;

  public LeanQuartzTaskSeed(ISqlSugarClient db)
  {
    _db = db;
    _logger = LogManager.GetCurrentClassLogger();
  }

  public async Task InitializeAsync()
  {
    _logger.Info("开始初始化任务数据...");

    var defaultTasks = new List<LeanQuartzTask>
    {
      new()
      {
        TaskId = 1,
        TaskName = "清理临时文件",
        GroupName = "系统维护",
        AssemblyName = "Lean.CodeGen.Tasks",
        ClassName = "Lean.CodeGen.Tasks.CleanTempFilesTask",
        TriggerType = 2,
        CronExpression = "0 0 2 * * ?",
        TaskType = 1,
        StartTime = DateTime.Now,
        RunResult = 0,
        RetryCount = 0,
        TaskStatus = 0
      }
    };

    foreach (var task in defaultTasks)
    {
      await task.SeedDataAsync(_db);
    }

    _logger.Info("任务数据初始化完成");
  }
}