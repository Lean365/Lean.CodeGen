using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Domain.Entities.Routine;
using SqlSugar;
using System.Threading.Tasks;
using NLog;
using ILogger = NLog.ILogger;
using Lean.CodeGen.Infrastructure.Data.Seeds.Extensions;

namespace Lean.CodeGen.Infrastructure.Data.Seeds.Routine;

/// <summary>
/// 邮件模板种子数据
/// </summary>
public class LeanMailTmplSeed
{
  private readonly ISqlSugarClient _db;
  private readonly ILogger _logger;

  public LeanMailTmplSeed(ISqlSugarClient db)
  {
    _db = db;
    _logger = LogManager.GetCurrentClassLogger();
  }

  public async Task InitializeAsync()
  {
    _logger.Info("开始初始化邮件模板数据...");

    var defaultTemplates = new List<LeanMailTmpl>
    {
      new()
      {
        TmplCode = "welcome",
        TmplName = "欢迎邮件",
        TmplSubject = "欢迎加入我们",
        TmplContent = @"<div>
          <h3>尊敬的{UserName}：</h3>
          <p>欢迎加入我们！</p>
          <p>您的账号已经创建成功，初始密码为：{Password}</p>
          <p>请及时登录系统修改密码。</p>
        </div>",
        TmplSignature = "系统管理员",
        TmplIsHtml = 1,
        TmplPriority = 2,
        TmplRemark = "新用户欢迎邮件模板",
        TmplStatus = 1
      },
      new()
      {
        TmplCode = "reset_password",
        TmplName = "重置密码",
        TmplSubject = "密码重置通知",
        TmplContent = @"<div>
          <h3>尊敬的{UserName}：</h3>
          <p>您的密码已经重置。</p>
          <p>新密码为：{Password}</p>
          <p>请及时登录系统修改密码。</p>
        </div>",
        TmplSignature = "系统管理员",
        TmplIsHtml = 1,
        TmplPriority = 2,
        TmplRemark = "密码重置邮件模板",
        TmplStatus = 1
      }
    };

    foreach (var template in defaultTemplates)
    {
      await template.SeedDataAsync(_db);
    }

    _logger.Info("邮件模板数据初始化完成");
  }
}