using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Domain.Entities.Routine;
using SqlSugar;
using System.Threading.Tasks;
using NLog;
using ILogger = NLog.ILogger;
using Lean.CodeGen.Infrastructure.Data.Seeds.Extensions;

namespace Lean.CodeGen.Infrastructure.Data.Seeds.Routine;

/// <summary>
/// 文件种子数据
/// </summary>
public class LeanFileSeed
{
  private readonly ISqlSugarClient _db;
  private readonly ILogger _logger;

  public LeanFileSeed(ISqlSugarClient db)
  {
    _db = db;
    _logger = LogManager.GetCurrentClassLogger();
  }

  public async Task InitializeAsync()
  {
    _logger.Info("开始初始化文件数据...");

    var defaultFiles = new List<LeanFile>
    {
      new()
      {
        FileName = "示例文件.txt",
        OriginalFileName = "example.txt",
        Extension = ".txt",
        FileSize = 1024,
        ContentType = "text/plain",
        FilePath = "/files/example.txt",
        FileMD5 = "e10adc3949ba59abbe56e057f20f883e",
        StorageType = 1,
        AccessUrl = "/files/example.txt",
        FileType = 1,
        BusinessModule = "system",
        BusinessId = 0,
        IsTemporary = 0,
        DownloadCount = 0
      }
    };

    foreach (var file in defaultFiles)
    {
      var exists = await _db.Queryable<LeanFile>()
          .FirstAsync(x => x.FileName == file.FileName && x.FilePath == file.FilePath);

      if (exists == null)
      {
        file.InitAuditFields();
        await _db.Insertable(file).ExecuteCommandAsync();
        _logger.Info($"新增文件: {file.FileName}");
      }
    }

    _logger.Info("文件数据初始化完成");
  }
}