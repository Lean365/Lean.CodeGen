using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Domain.Entities.Identity;
using SqlSugar;
using System.Threading.Tasks;
using NLog;
using ILogger = NLog.ILogger;
using Lean.CodeGen.Common.Security;
using Lean.CodeGen.Common.Options;
using Microsoft.Extensions.Options;
using Lean.CodeGen.Infrastructure.Data.Seeds.Extensions;

namespace Lean.CodeGen.Infrastructure.Data.Seeds.Identity;

/// <summary>
/// 用户种子数据初始化类
/// </summary>
/// <remarks>
/// 负责系统初始化时创建默认用户数据，包括：
/// 1. 系统管理员账号
/// 2. 测试用户账号
/// 3. 处理用户密码加密和盐值生成
/// 4. 处理用户数据的更新和插入
/// </remarks>
public class LeanUserSeed
{
  /// <summary>
  /// 数据库访问客户端
  /// </summary>
  private readonly ISqlSugarClient _db;

  /// <summary>
  /// 日志记录器
  /// </summary>
  private readonly ILogger _logger;

  /// <summary>
  /// 安全配置选项
  /// </summary>
  private readonly LeanSecurityOptions _securityOptions;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="db">数据库访问客户端</param>
  /// <param name="securityOptions">安全配置选项</param>
  public LeanUserSeed(
      ISqlSugarClient db,
      IOptions<LeanSecurityOptions> securityOptions)
  {
    _db = db;
    _logger = LogManager.GetCurrentClassLogger();
    _securityOptions = securityOptions.Value;
  }

  /// <summary>
  /// 初始化用户数据
  /// </summary>
  /// <remarks>
  /// 执行以下操作：
  /// 1. 创建默认用户列表（管理员和测试用户）
  /// 2. 为用户生成加密密码和盐值
  /// 3. 检查用户是否已存在
  /// 4. 根据情况更新或新增用户数据
  /// </remarks>
  public async Task InitializeAsync()
  {
    _logger.Info("开始初始化用户数据...");

    // 定义默认用户列表
    var defaultUsers = new List<LeanUser>
    {
      // 创建系统管理员账号
      new LeanUser()
      {
        UserName = "admin",
        RealName = "系统管理员",
        UserType = LeanUserType.System,      // 系统用户类型
        UserStatus = LeanUserStatus.Normal,   // 正常状态
        IsBuiltin = LeanBuiltinStatus.Yes     // 内置用户
      },
      // 创建测试用户账号
      new LeanUser()
      {
        UserName = "test",
        RealName = "测试用户",
        UserType = LeanUserType.Normal,      // 普通用户类型
        UserStatus = LeanUserStatus.Normal,   // 正常状态
        IsBuiltin = LeanBuiltinStatus.Yes     // 内置用户
      }
    };

    // 为每个用户设置密码和盐值
    foreach (var user in defaultUsers)
    {
      var (hashedPassword, salt) = LeanPassword.CreatePassword(_securityOptions.DefaultPassword);
      user.Password = hashedPassword;
      user.Salt = salt;
    }

    // 处理每个默认用户
    foreach (var user in defaultUsers)
    {
      // 检查用户是否已存在
      var exists = await _db.Queryable<LeanUser>()
          .FirstAsync(x => x.UserName == user.UserName);

      if (exists != null)
      {
        // 更新现有用户信息
        user.Id = exists.Id;
        // 如果未指定密码，保留原密码
        if (string.IsNullOrEmpty(user.Password))
        {
          user.Password = exists.Password;
          user.Salt = exists.Salt;
        }
        // 复制原有审计信息并初始化更新信息
        user.CopyAuditFields(exists).InitAuditFields(true);

        await _db.Updateable(user).ExecuteCommandAsync();
        _logger.Info($"更新用户: {user.UserName}");
      }
      else
      {
        // 初始化审计字段
        user.InitAuditFields();
        // 新增用户
        await _db.Insertable(user).ExecuteCommandAsync();
        _logger.Info($"新增用户: {user.UserName}");
      }
    }

    _logger.Info("用户数据初始化完成");
  }
}