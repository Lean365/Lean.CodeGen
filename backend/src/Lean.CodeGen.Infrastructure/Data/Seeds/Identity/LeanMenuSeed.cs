using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Domain.Entities.Identity;
using SqlSugar;
using System.Threading.Tasks;
using NLog;
using ILogger = NLog.ILogger;

namespace Lean.CodeGen.Infrastructure.Data.Seeds.Identity;

/// <summary>
/// 菜单种子数据
/// </summary>
public class LeanMenuSeed
{
  private readonly ISqlSugarClient _db;
  private readonly ILogger _logger;

  public LeanMenuSeed(ISqlSugarClient db)
  {
    _db = db;
    _logger = LogManager.GetCurrentClassLogger();
  }

  public async Task InitializeAsync()
  {
    _logger.Info("开始初始化菜单数据...");

    var defaultMenus = new List<LeanMenu>();

    // 系统管理模块菜单
    var systemMenus = new List<LeanMenu>
    {
      new()
      {
        MenuName = "系统管理",
        ParentId = 0,
        OrderNum = 1,
        Path = "/system",
        Component = "Layout",
        IsFrame = 0,
        IsCached = 0,
        MenuType = LeanMenuType.Directory,
        Visible = 0,
        MenuStatus = LeanMenuStatus.Normal,
        Perms = "system",
        Icon = "system",
        IsBuiltin = LeanBuiltinStatus.Yes
      }
    };

    // 用户权限管理菜单
    var userMenus = new List<LeanMenu>
    {
      new()
      {
        MenuName = "用户管理",
        ParentId = 1,
        OrderNum = 1,
        Path = "user",
        Component = "system/user/index",
        IsFrame = 0,
        IsCached = 0,
        MenuType = LeanMenuType.Menu,
        Visible = 0,
        MenuStatus = LeanMenuStatus.Normal,
        Perms = "system:user:list",
        Icon = "user",
        IsBuiltin = LeanBuiltinStatus.Yes
      },
      new()
      {
        MenuName = "角色管理",
        ParentId = 1,
        OrderNum = 2,
        Path = "role",
        Component = "system/role/index",
        IsFrame = 0,
        IsCached = 0,
        MenuType = LeanMenuType.Menu,
        Visible = 0,
        MenuStatus = LeanMenuStatus.Normal,
        Perms = "system:role:list",
        Icon = "peoples",
        IsBuiltin = LeanBuiltinStatus.Yes
      }
    };

    // 合并所有菜单
    defaultMenus.AddRange(systemMenus);
    defaultMenus.AddRange(userMenus);

    // 更新或插入菜单数据
    foreach (var menu in defaultMenus)
    {
      var exists = await _db.Queryable<LeanMenu>()
          .FirstAsync(x => x.MenuName == menu.MenuName);

      if (exists != null)
      {
        menu.Id = exists.Id;
        await _db.Updateable(menu).ExecuteCommandAsync();
        _logger.Info($"更新菜单: {menu.MenuName}");
      }
      else
      {
        await _db.Insertable(menu).ExecuteCommandAsync();
        _logger.Info($"新增菜单: {menu.MenuName}");
      }
    }

    _logger.Info("菜单数据初始化完成");
  }
}