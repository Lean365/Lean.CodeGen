using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Domain.Entities.Admin;
using Lean.CodeGen.Infrastructure.Data.Context;
using SqlSugar;
using System.Threading.Tasks;
using NLog;
using ILogger = NLog.ILogger;
using Lean.CodeGen.Infrastructure.Data.Seeds.Extensions;

namespace Lean.CodeGen.Infrastructure.Data.Seeds.Admin;

/// <summary>
/// 语言种子数据
/// </summary>
public class LeanLanguageSeed
{
  private readonly ISqlSugarClient _db;
  private readonly ILogger _logger;

  public LeanLanguageSeed(ISqlSugarClient db)
  {
    _db = db;
    _logger = LogManager.GetCurrentClassLogger();
  }

  public async Task InitializeAsync()
  {
    _logger.Info("开始初始化语言数据...");

    var defaultLanguages = new List<LeanLanguage>
        {
            // 东亚语言
            new() { LangCode = "zh-CN", LangName = "简体中文", LangIcon = "🇨🇳", IsDefault = 1 },
            new() { LangCode = "zh-TW", LangName = "繁體中文", LangIcon = "🇹🇼", IsDefault = 0 },
            new() { LangCode = "ja-JP", LangName = "日本語", LangIcon = "🇯🇵", IsDefault = 0 },
            new() { LangCode = "ko-KR", LangName = "한국어", LangIcon = "🇰🇷", IsDefault = 0 },
            // 联合国官方语言
            new() { LangCode = "en-US", LangName = "English", LangIcon = "🇺🇸", IsDefault = 0 },
            new() { LangCode = "fr-FR", LangName = "Français", LangIcon = "🇫🇷", IsDefault = 0 },
            new() { LangCode = "es-ES", LangName = "Español", LangIcon = "🇪🇸", IsDefault = 0 },
            new() { LangCode = "ru-RU", LangName = "Русский", LangIcon = "🇷🇺", IsDefault = 0 },
            new() { LangCode = "ar-SA", LangName = "العربية", LangIcon = "🇸🇦", IsDefault = 0 }
        };

    // 为所有语言设置通用属性
    foreach (var lang in defaultLanguages)
    {
      lang.OrderNum = 1;
      lang.Remark = "简体中文";
      lang.LangStatus = 0; // 正常
      lang.IsBuiltin = 1; // 是
      lang.OrderNum = defaultLanguages.IndexOf(lang) + 1;
    }

    // 更新或插入语言数据
    foreach (var lang in defaultLanguages)
    {
      var exists = await _db.Queryable<LeanLanguage>()
          .FirstAsync(x => x.LangCode == lang.LangCode);

      if (exists != null)
      {
        lang.Id = exists.Id;
        // 复制原有审计信息并初始化更新信息
        lang.CopyAuditFields(exists).InitAuditFields(true);
        await _db.Updateable(lang).ExecuteCommandAsync();
        _logger.Info($"更新语言: {lang.LangName} ({lang.LangCode})");
      }
      else
      {
        // 初始化审计字段
        lang.InitAuditFields();
        await _db.Insertable(lang).ExecuteCommandAsync();
        _logger.Info($"新增语言: {lang.LangName} ({lang.LangCode})");
      }
    }

    _logger.Info("语言数据初始化完成");
  }
}