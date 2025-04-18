using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Domain.Entities.Identity;
using SqlSugar;
using System.Threading.Tasks;
using NLog;
using ILogger = NLog.ILogger;
using Lean.CodeGen.Infrastructure.Data.Seeds.Extensions;

namespace Lean.CodeGen.Infrastructure.Data.Seeds.Identity;

/// <summary>
/// 岗位种子数据
/// </summary>
public class LeanPostSeed
{
  private readonly ISqlSugarClient _db;
  private readonly ILogger _logger;

  public LeanPostSeed(ISqlSugarClient db)
  {
    _db = db;
    _logger = LogManager.GetCurrentClassLogger();
  }

  public async Task InitializeAsync()
  {
    _logger.Info("开始初始化岗位数据...");

    var defaultPosts = new List<LeanPost>
        {
            new()
            {
                PostCode = "ceo",
                PostName = "董事长",
                OrderNum = 1,
                PostStatus = 1,
                IsBuiltin = 1
            },
            new()
            {
                PostCode = "se",
                PostName = "项目经理",
                OrderNum = 2,
                PostStatus = 1,
                IsBuiltin = 1
            },
            new()
            {
                PostCode = "hr",
                PostName = "人力资源",
                OrderNum = 3,
                PostStatus = 1,
                IsBuiltin = 1
            }
        };

    foreach (var post in defaultPosts)
    {
      var exists = await _db.Queryable<LeanPost>()
          .FirstAsync(x => x.PostCode == post.PostCode);

      if (exists != null)
      {
        post.Id = exists.Id;
        // 复制原有审计信息并初始化更新信息
        post.CopyAuditFields(exists).InitAuditFields(true);
        await _db.Updateable(post).ExecuteCommandAsync();
        _logger.Info($"更新岗位: {post.PostName}");
      }
      else
      {
        // 初始化审计字段
        post.InitAuditFields();
        await _db.Insertable(post).ExecuteCommandAsync();
        _logger.Info($"新增岗位: {post.PostName}");
      }
    }

    _logger.Info("岗位数据初始化完成");
  }
}