using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Domain.Entities.Routine;
using SqlSugar;
using System.Threading.Tasks;
using NLog;
using ILogger = NLog.ILogger;
using Lean.CodeGen.Infrastructure.Data.Seeds.Extensions;

namespace Lean.CodeGen.Infrastructure.Data.Seeds.Routine;

/// <summary>
/// 通知种子数据
/// </summary>
public class LeanNotificationSeed
{
  private readonly ISqlSugarClient _db;
  private readonly ILogger _logger;

  public LeanNotificationSeed(ISqlSugarClient db)
  {
    _db = db;
    _logger = LogManager.GetCurrentClassLogger();
  }

  public async Task InitializeAsync()
  {
    _logger.Info("开始初始化通知数据...");

    var defaultNotifications = new List<LeanNotification>
    {
      new()
      {
        NotificationTitle = "系统升级通知",
        NotificationContent = "系统将于今晚22:00进行升级维护，预计耗时2小时。",
        NotificationType = 1,
        NotificationLevel = 2,
        ReceiverType = 4,
        RequireConfirmation = 1,
        ConfirmationDeadline = DateTime.Now.AddDays(1),
        BusinessModule = "system",
        PublishStatus = 1,
        PublishTime = DateTime.Now,
        IsTop = 1,
        TopEndTime = DateTime.Now.AddDays(7),
        ReadCount = 0,
        ConfirmationCount = 0
      }
    };

    foreach (var notification in defaultNotifications)
    {
      await notification.SeedDataAsync(_db);
    }

    _logger.Info("通知数据初始化完成");
  }
}