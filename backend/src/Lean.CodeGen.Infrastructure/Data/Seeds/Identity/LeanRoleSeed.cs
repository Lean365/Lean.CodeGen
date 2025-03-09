using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Domain.Entities.Identity;
using SqlSugar;
using System.Threading.Tasks;
using NLog;
using ILogger = NLog.ILogger;
using Lean.CodeGen.Infrastructure.Data.Seeds.Extensions;

namespace Lean.CodeGen.Infrastructure.Data.Seeds.Identity;

/// <summary>
/// 角色种子数据
/// </summary>
public class LeanRoleSeed
{
  private readonly ISqlSugarClient _db;
  private readonly ILogger _logger;

  public LeanRoleSeed(ISqlSugarClient db)
  {
    _db = db;
    _logger = LogManager.GetCurrentClassLogger();
  }

  public async Task InitializeAsync()
  {
    _logger.Info("开始初始化角色数据...");

    var defaultRoles = new List<LeanRole>
        {
            new()
            {
                RoleCode = "admin",
                RoleName = "超级管理员",
                RoleDescription = "系统超级管理员，拥有所有权限",
                OrderNum = 1,
                RoleStatus = 1,
                DataScope = 0,
                IsBuiltin = 1
            },
            new()
            {
                RoleCode = "user",
                RoleName = "普通用户",
                RoleDescription = "普通用户，拥有基本权限",
                OrderNum = 2,
                RoleStatus = 1,
                DataScope = 2,
                IsBuiltin = 1
            }
        };

    foreach (var role in defaultRoles)
    {
      var exists = await _db.Queryable<LeanRole>()
          .FirstAsync(x => x.RoleCode == role.RoleCode);

      if (exists != null)
      {
        role.Id = exists.Id;
        // 复制原有审计信息并初始化更新信息
        role.CopyAuditFields(exists).InitAuditFields(true);
        await _db.Updateable(role).ExecuteCommandAsync();
        _logger.Info($"更新角色: {role.RoleName}");
      }
      else
      {
        // 初始化审计字段
        role.InitAuditFields();
        await _db.Insertable(role).ExecuteCommandAsync();
        _logger.Info($"新增角色: {role.RoleName}");
      }
    }

    _logger.Info("角色数据初始化完成");
  }
}