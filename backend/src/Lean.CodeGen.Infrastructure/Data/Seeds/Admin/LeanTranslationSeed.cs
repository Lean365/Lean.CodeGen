using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Domain.Entities.Admin;
using SqlSugar;
using System.Threading.Tasks;
using NLog;
using ILogger = NLog.ILogger;
using Lean.CodeGen.Infrastructure.Data.Seeds.Extensions;

namespace Lean.CodeGen.Infrastructure.Data.Seeds.Admin;

/// <summary>
/// 翻译种子数据
/// </summary>
public class LeanTranslationSeed
{
  private readonly ISqlSugarClient _db;
  private readonly ILogger _logger;

  public LeanTranslationSeed(ISqlSugarClient db)
  {
    _db = db;
    _logger = LogManager.GetCurrentClassLogger();
  }

  public async Task InitializeAsync()
  {
    _logger.Info("开始初始化翻译数据...");

    // 获取中文和英文语言的ID
    var zhLang = await _db.Queryable<LeanLanguage>()
        .FirstAsync(l => l.LangCode == "zh-CN");
    var enLang = await _db.Queryable<LeanLanguage>()
        .FirstAsync(l => l.LangCode == "en-US");

    if (zhLang == null || enLang == null)
    {
      _logger.Error("未找到必需的中文或英文语言配置");
      return;
    }

    var defaultTranslations = new List<LeanTranslation>();

    // 通用模块翻译
    var commonTranslations = new List<LeanTranslation>
        {
            // 中文翻译
            new()
            {
                LangId = zhLang.Id,
                TransKey = "common.success",
                TransValue = "操作成功",
                ModuleName = "common",
                OrderNum = 1,
                TransStatus = 1,
                IsBuiltin = 1,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            },
            new()
            {
                LangId = zhLang.Id,
                TransKey = "common.error",
                TransValue = "操作失败",
                ModuleName = "common",
                OrderNum = 2,
                TransStatus = 1,
                IsBuiltin = 1,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            },
            // 英文翻译
            new()
            {
                LangId = enLang.Id,
                TransKey = "common.success",
                TransValue = "Operation successful",
                ModuleName = "common",
                OrderNum = 1,
                TransStatus = 1,
                IsBuiltin = 1,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            },
            new()
            {
                LangId = enLang.Id,
                TransKey = "common.error",
                TransValue = "Operation failed",
                ModuleName = "common",
                OrderNum = 2,
                TransStatus = 1,
                IsBuiltin = 1,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            }
        };

    // 按钮模块翻译
    var buttonTranslations = new List<LeanTranslation>
        {
            // 中文翻译
            new()
            {
                LangId = zhLang.Id,
                TransKey = "button.save",
                TransValue = "保存",
                ModuleName = "button",
                OrderNum = 1,
                TransStatus = 1,
                IsBuiltin = 1,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            },
            new()
            {
                LangId = zhLang.Id,
                TransKey = "button.cancel",
                TransValue = "取消",
                ModuleName = "button",
                OrderNum = 2,
                TransStatus = 1,
                IsBuiltin = 1,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            },
            // 英文翻译
            new()
            {
                LangId = enLang.Id,
                TransKey = "button.save",
                TransValue = "Save",
                ModuleName = "button",
                OrderNum = 1,
                TransStatus = 1,
                IsBuiltin = 1,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            },
            new()
            {
                LangId = enLang.Id,
                TransKey = "button.cancel",
                TransValue = "Cancel",
                ModuleName = "button",
                OrderNum = 2,
                TransStatus = 1,
                IsBuiltin = 1,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            }
        };

    // 合并所有翻译
    defaultTranslations.AddRange(commonTranslations);
    defaultTranslations.AddRange(buttonTranslations);

    // 更新或插入翻译数据
    foreach (var trans in defaultTranslations)
    {
      var exists = await _db.Queryable<LeanTranslation>()
          .FirstAsync(t => t.LangId == trans.LangId && t.TransKey == trans.TransKey);

      if (exists != null)
      {
        trans.Id = exists.Id;
        // 复制原有审计信息并初始化更新信息
        trans.CopyAuditFields(exists).InitAuditFields(true);
        await _db.Updateable(trans).ExecuteCommandAsync();
        _logger.Info($"更新翻译: {trans.TransKey} = {trans.TransValue}");
      }
      else
      {
        // 初始化审计字段
        trans.InitAuditFields();
        await _db.Insertable(trans).ExecuteCommandAsync();
        _logger.Info($"新增翻译: {trans.TransKey} = {trans.TransValue}");
      }
    }

    _logger.Info("翻译数据初始化完成");
  }
}