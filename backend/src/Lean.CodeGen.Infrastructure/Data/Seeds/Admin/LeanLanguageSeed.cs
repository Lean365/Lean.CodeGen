using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Domain.Entities.Admin;
using Lean.CodeGen.Infrastructure.Data.Context;
using SqlSugar;
using System.Threading.Tasks;
using NLog;
using ILogger = NLog.ILogger;

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
            new() { LangCode = "zh-CN", LangName = "简体中文", LangIcon = "🇨🇳", IsDefault = LeanYesNo.Yes },
            new() { LangCode = "zh-TW", LangName = "繁體中文", LangIcon = "🇹🇼", IsDefault = LeanYesNo.No },
            new() { LangCode = "ja-JP", LangName = "日本語", LangIcon = "🇯🇵", IsDefault = LeanYesNo.No },
            new() { LangCode = "ko-KR", LangName = "한국어", LangIcon = "🇰🇷", IsDefault = LeanYesNo.No },
            // 联合国官方语言
            new() { LangCode = "en-US", LangName = "English", LangIcon = "🇺🇸", IsDefault = LeanYesNo.No },
            new() { LangCode = "fr-FR", LangName = "Français", LangIcon = "🇫🇷", IsDefault = LeanYesNo.No },
            new() { LangCode = "es-ES", LangName = "Español", LangIcon = "🇪🇸", IsDefault = LeanYesNo.No },
            new() { LangCode = "ru-RU", LangName = "Русский", LangIcon = "🇷🇺", IsDefault = LeanYesNo.No },
            new() { LangCode = "ar-SA", LangName = "العربية", LangIcon = "🇸🇦", IsDefault = LeanYesNo.No }
        };

    // 为所有语言设置通用属性
    foreach (var lang in defaultLanguages)
    {
      lang.Status = LeanStatus.Enable;
      lang.IsBuiltin = LeanBuiltinStatus.Yes;
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
        await _db.Updateable(lang).ExecuteCommandAsync();
        _logger.Info($"更新语言: {lang.LangName} ({lang.LangCode})");
      }
      else
      {
        await _db.Insertable(lang).ExecuteCommandAsync();
        _logger.Info($"新增语言: {lang.LangName} ({lang.LangCode})");
      }
    }

    _logger.Info("语言数据初始化完成");
  }
}