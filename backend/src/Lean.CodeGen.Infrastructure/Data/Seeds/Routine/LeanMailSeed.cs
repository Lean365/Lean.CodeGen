using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Domain.Entities.Routine;
using SqlSugar;
using System.Threading.Tasks;
using NLog;
using ILogger = NLog.ILogger;
using Lean.CodeGen.Infrastructure.Data.Seeds.Extensions;

namespace Lean.CodeGen.Infrastructure.Data.Seeds.Routine;

/// <summary>
/// 邮件种子数据
/// </summary>
public class LeanMailSeed
{
  private readonly ISqlSugarClient _db;
  private readonly ILogger _logger;

  public LeanMailSeed(ISqlSugarClient db)
  {
    _db = db;
    _logger = LogManager.GetCurrentClassLogger();
  }

  public async Task InitializeAsync()
  {
    _logger.Info("开始初始化邮件数据...");

    var defaultMails = new List<LeanMail>
    {
      new()
      {
        Subject = "系统测试邮件",
        FromAddress = "admin@lean.com",
        FromName = "系统管理员",
        ToAddresses = "test@lean.com",
        Body = "<p>这是一封测试邮件。</p>",
        IsBodyHtml = 1,
        Priority = 2,
        SendStatus = 1,
        SendTime = DateTime.Now,
        RetryCount = 0
      }
    };

    foreach (var mail in defaultMails)
    {
      await mail.SeedDataAsync(_db);
    }

    _logger.Info("邮件数据初始化完成");
  }
}