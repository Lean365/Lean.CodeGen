using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Domain.Entities.Identity;
using SqlSugar;
using System.Threading.Tasks;
using NLog;
using ILogger = NLog.ILogger;
using Lean.CodeGen.Common.Security;
using Lean.CodeGen.Common.Options;
using Microsoft.Extensions.Options;

namespace Lean.CodeGen.Infrastructure.Data.Seeds.Identity;

/// <summary>
/// 用户种子数据
/// </summary>
public class LeanUserSeed
{
  private readonly ISqlSugarClient _db;
  private readonly ILogger _logger;
  private readonly LeanSecurityOptions _securityOptions;

  public LeanUserSeed(
      ISqlSugarClient db,
      IOptions<LeanSecurityOptions> securityOptions)
  {
    _db = db;
    _logger = LogManager.GetCurrentClassLogger();
    _securityOptions = securityOptions.Value;
  }

  public async Task InitializeAsync()
  {
    _logger.Info("开始初始化用户数据...");

    var defaultUsers = new List<LeanUser>
        {
            new()
            {
                UserName = "admin",
                RealName = "系统管理员",
                UserType = LeanUserType.System,
                UserStatus = LeanUserStatus.Normal,
                IsBuiltin = LeanBuiltinStatus.Yes
            },
            new()
            {
                UserName = "test",
                RealName = "测试用户",
                UserType = LeanUserType.Normal,
                UserStatus = LeanUserStatus.Normal,
                IsBuiltin = LeanBuiltinStatus.Yes
            }
        };

    // 设置密码和盐值
    foreach (var user in defaultUsers)
    {
      var (hashedPassword, salt) = LeanPassword.CreatePassword(_securityOptions.DefaultPassword);
      user.Password = hashedPassword;
      user.Salt = salt;
    }

    foreach (var user in defaultUsers)
    {
      var exists = await _db.Queryable<LeanUser>()
          .FirstAsync(x => x.UserName == user.UserName);

      if (exists != null)
      {
        user.Id = exists.Id;
        if (string.IsNullOrEmpty(user.Password))
        {
          user.Password = exists.Password;
          user.Salt = exists.Salt;
        }
        await _db.Updateable(user).ExecuteCommandAsync();
        _logger.Info($"更新用户: {user.UserName}");
      }
      else
      {
        await _db.Insertable(user).ExecuteCommandAsync();
        _logger.Info($"新增用户: {user.UserName}");
      }
    }

    _logger.Info("用户数据初始化完成");
  }
}