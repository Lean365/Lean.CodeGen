using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Domain.Entities.Identity;
using Lean.CodeGen.Infrastructure.Data.Context;
using SqlSugar;
using System.Threading.Tasks;

namespace Lean.CodeGen.Infrastructure.Data.Seeds.Identity;

/// <summary>
/// 菜单种子数据
/// </summary>
public class LeanMenuSeed : ILeanDataSeed
{
  public int Order => 3;

  public async Task SeedAsync(LeanDbContext dbContext)
  {
    var db = dbContext.GetDatabase();
    if (await db.Queryable<LeanMenu>().AnyAsync())
    {
      return;
    }

    // 1. 系统管理
    await db.Insertable(new LeanMenu
    {
      MenuName = "系统管理",
      Perms = "system",
      MenuType = LeanMenuType.Directory,
      Icon = "system",
      Path = "/system",
      OrderNum = 1,
      MenuStatus = LeanMenuStatus.Normal,
      IsBuiltin = LeanBuiltinStatus.Yes,
      ParentId = 0,
      Visible = 0,
      IsFrame = 0,
      IsCached = 0,
      TransKey = "menu.system"
    }).ExecuteReturnIdentityAsync();

    // 2. 代码生成
    await db.Insertable(new LeanMenu
    {
      MenuName = "代码生成",
      Perms = "generator",
      MenuType = LeanMenuType.Directory,
      Icon = "code",
      Path = "/generator",
      OrderNum = 2,
      MenuStatus = LeanMenuStatus.Normal,
      IsBuiltin = LeanBuiltinStatus.Yes,
      ParentId = 0,
      Visible = 0,
      IsFrame = 0,
      IsCached = 0,
      TransKey = "menu.generator"
    }).ExecuteReturnIdentityAsync();

    // 3. 审计日志
    await db.Insertable(new LeanMenu
    {
      MenuName = "审计日志",
      Perms = "audit",
      MenuType = LeanMenuType.Directory,
      Icon = "audit",
      Path = "/audit",
      OrderNum = 3,
      MenuStatus = LeanMenuStatus.Normal,
      IsBuiltin = LeanBuiltinStatus.Yes,
      ParentId = 0,
      Visible = 0,
      IsFrame = 0,
      IsCached = 0,
      TransKey = "menu.audit"
    }).ExecuteReturnIdentityAsync();

    // 4. 在线管理
    await db.Insertable(new LeanMenu
    {
      MenuName = "在线管理",
      Perms = "online",
      MenuType = LeanMenuType.Directory,
      Icon = "online",
      Path = "/online",
      OrderNum = 4,
      MenuStatus = LeanMenuStatus.Normal,
      IsBuiltin = LeanBuiltinStatus.Yes,
      ParentId = 0,
      Visible = 0,
      IsFrame = 0,
      IsCached = 0,
      TransKey = "menu.online"
    }).ExecuteReturnIdentityAsync();

    // 5. 系统工具
    await db.Insertable(new LeanMenu
    {
      MenuName = "系统工具",
      Perms = "tool",
      MenuType = LeanMenuType.Directory,
      Icon = "tool",
      Path = "/tool",
      OrderNum = 5,
      MenuStatus = LeanMenuStatus.Normal,
      IsBuiltin = LeanBuiltinStatus.Yes,
      ParentId = 0,
      Visible = 0,
      IsFrame = 0,
      IsCached = 0,
      TransKey = "menu.tool"
    }).ExecuteReturnIdentityAsync();

    // 获取父级菜单ID
    var systemMenu = await db.Queryable<LeanMenu>().Where(m => m.Perms == "system").FirstAsync();
    var generatorMenu = await db.Queryable<LeanMenu>().Where(m => m.Perms == "generator").FirstAsync();
    var auditMenu = await db.Queryable<LeanMenu>().Where(m => m.Perms == "audit").FirstAsync();
    var onlineMenu = await db.Queryable<LeanMenu>().Where(m => m.Perms == "online").FirstAsync();
    var toolMenu = await db.Queryable<LeanMenu>().Where(m => m.Perms == "tool").FirstAsync();

    // 系统管理子菜单
    var systemMenus = new List<LeanMenu>
    {
      new LeanMenu
      {
        MenuName = "用户管理",
        Perms = "system:user",
        MenuType = LeanMenuType.Menu,
        Icon = "user",
        Path = "/system/user",
        Component = "system/user/index",
        OrderNum = 1,
        MenuStatus = LeanMenuStatus.Normal,
        IsBuiltin = LeanBuiltinStatus.Yes,
        ParentId = systemMenu.Id,
        Visible = 0,
        IsFrame = 0,
        IsCached = 1,
        TransKey = "menu.system.user"
      },
      new LeanMenu
      {
        MenuName = "角色管理",
        Perms = "system:role",
        MenuType = LeanMenuType.Menu,
        Icon = "role",
        Path = "/system/role",
        Component = "system/role/index",
        OrderNum = 2,
        MenuStatus = LeanMenuStatus.Normal,
        IsBuiltin = LeanBuiltinStatus.Yes,
        ParentId = systemMenu.Id,
        Visible = 0,
        IsFrame = 0,
        IsCached = 1,
        TransKey = "menu.system.role"
      },
      new LeanMenu
      {
        MenuName = "菜单管理",
        Perms = "system:menu",
        MenuType = LeanMenuType.Menu,
        Icon = "menu",
        Path = "/system/menu",
        Component = "system/menu/index",
        OrderNum = 3,
        MenuStatus = LeanMenuStatus.Normal,
        IsBuiltin = LeanBuiltinStatus.Yes,
        ParentId = systemMenu.Id,
        Visible = 0,
        IsFrame = 0,
        IsCached = 1,
        TransKey = "menu.system.menu"
      },
      new LeanMenu
      {
        MenuName = "部门管理",
        Perms = "system:dept",
        MenuType = LeanMenuType.Menu,
        Icon = "dept",
        Path = "/system/dept",
        Component = "system/dept/index",
        OrderNum = 4,
        MenuStatus = LeanMenuStatus.Normal,
        IsBuiltin = LeanBuiltinStatus.Yes,
        ParentId = systemMenu.Id,
        Visible = 0,
        IsFrame = 0,
        IsCached = 1,
        TransKey = "menu.system.dept"
      },
      new LeanMenu
      {
        MenuName = "岗位管理",
        Perms = "system:post",
        MenuType = LeanMenuType.Menu,
        Icon = "post",
        Path = "/system/post",
        Component = "system/post/index",
        OrderNum = 5,
        MenuStatus = LeanMenuStatus.Normal,
        IsBuiltin = LeanBuiltinStatus.Yes,
        ParentId = systemMenu.Id,
        Visible = 0,
        IsFrame = 0,
        IsCached = 1,
        TransKey = "menu.system.post"
      },
      new LeanMenu
      {
        MenuName = "字典管理",
        Perms = "system:dict",
        MenuType = LeanMenuType.Menu,
        Icon = "dict",
        Path = "/system/dict",
        Component = "system/dict/index",
        OrderNum = 6,
        MenuStatus = LeanMenuStatus.Normal,
        IsBuiltin = LeanBuiltinStatus.Yes,
        ParentId = systemMenu.Id,
        Visible = 0,
        IsFrame = 0,
        IsCached = 1,
        TransKey = "menu.system.dict"
      },
      new LeanMenu
      {
        MenuName = "参数设置",
        Perms = "system:config",
        MenuType = LeanMenuType.Menu,
        Icon = "config",
        Path = "/system/config",
        Component = "system/config/index",
        OrderNum = 7,
        MenuStatus = LeanMenuStatus.Normal,
        IsBuiltin = LeanBuiltinStatus.Yes,
        ParentId = systemMenu.Id,
        Visible = 0,
        IsFrame = 0,
        IsCached = 1,
        TransKey = "menu.system.config"
      }
    };

    // 代码生成子菜单
    var generatorMenus = new List<LeanMenu>
    {
      new LeanMenu
      {
        MenuName = "数据源管理",
        Perms = "generator:datasource",
        MenuType = LeanMenuType.Menu,
        Icon = "datasource",
        Path = "/generator/datasource",
        Component = "generator/datasource/index",
        OrderNum = 1,
        MenuStatus = LeanMenuStatus.Normal,
        IsBuiltin = LeanBuiltinStatus.Yes,
        ParentId = generatorMenu.Id,
        Visible = 0,
        IsFrame = 0,
        IsCached = 1,
        TransKey = "menu.generator.datasource"
      },
      new LeanMenu
      {
        MenuName = "数据表管理",
        Perms = "generator:table",
        MenuType = LeanMenuType.Menu,
        Icon = "table",
        Path = "/generator/table",
        Component = "generator/table/index",
        OrderNum = 2,
        MenuStatus = LeanMenuStatus.Normal,
        IsBuiltin = LeanBuiltinStatus.Yes,
        ParentId = generatorMenu.Id,
        Visible = 0,
        IsFrame = 0,
        IsCached = 1,
        TransKey = "menu.generator.table"
      },
      new LeanMenu
      {
        MenuName = "生成配置",
        Perms = "generator:config",
        MenuType = LeanMenuType.Menu,
        Icon = "config",
        Path = "/generator/config",
        Component = "generator/config/index",
        OrderNum = 3,
        MenuStatus = LeanMenuStatus.Normal,
        IsBuiltin = LeanBuiltinStatus.Yes,
        ParentId = generatorMenu.Id,
        Visible = 0,
        IsFrame = 0,
        IsCached = 1,
        TransKey = "menu.generator.config"
      },
      new LeanMenu
      {
        MenuName = "生成模板",
        Perms = "generator:template",
        MenuType = LeanMenuType.Menu,
        Icon = "template",
        Path = "/generator/template",
        Component = "generator/template/index",
        OrderNum = 4,
        MenuStatus = LeanMenuStatus.Normal,
        IsBuiltin = LeanBuiltinStatus.Yes,
        ParentId = generatorMenu.Id,
        Visible = 0,
        IsFrame = 0,
        IsCached = 1,
        TransKey = "menu.generator.template"
      },
      new LeanMenu
      {
        MenuName = "生成任务",
        Perms = "generator:task",
        MenuType = LeanMenuType.Menu,
        Icon = "task",
        Path = "/generator/task",
        Component = "generator/task/index",
        OrderNum = 5,
        MenuStatus = LeanMenuStatus.Normal,
        IsBuiltin = LeanBuiltinStatus.Yes,
        ParentId = generatorMenu.Id,
        Visible = 0,
        IsFrame = 0,
        IsCached = 1,
        TransKey = "menu.generator.task"
      },
      new LeanMenu
      {
        MenuName = "生成历史",
        Perms = "generator:history",
        MenuType = LeanMenuType.Menu,
        Icon = "history",
        Path = "/generator/history",
        Component = "generator/history/index",
        OrderNum = 6,
        MenuStatus = LeanMenuStatus.Normal,
        IsBuiltin = LeanBuiltinStatus.Yes,
        ParentId = generatorMenu.Id,
        Visible = 0,
        IsFrame = 0,
        IsCached = 1,
        TransKey = "menu.generator.history"
      }
    };

    // 审计日志子菜单
    var auditMenus = new List<LeanMenu>
    {
      new LeanMenu
      {
        MenuName = "操作日志",
        Perms = "audit:operation",
        MenuType = LeanMenuType.Menu,
        Icon = "operation",
        Path = "/audit/operation",
        Component = "audit/operation/index",
        OrderNum = 1,
        MenuStatus = LeanMenuStatus.Normal,
        IsBuiltin = LeanBuiltinStatus.Yes,
        ParentId = auditMenu.Id,
        Visible = 0,
        IsFrame = 0,
        IsCached = 1,
        TransKey = "menu.audit.operation"
      },
      new LeanMenu
      {
        MenuName = "登录日志",
        Perms = "audit:login",
        MenuType = LeanMenuType.Menu,
        Icon = "login",
        Path = "/audit/login",
        Component = "audit/login/index",
        OrderNum = 2,
        MenuStatus = LeanMenuStatus.Normal,
        IsBuiltin = LeanBuiltinStatus.Yes,
        ParentId = auditMenu.Id,
        Visible = 0,
        IsFrame = 0,
        IsCached = 1,
        TransKey = "menu.audit.login"
      },
      new LeanMenu
      {
        MenuName = "异常日志",
        Perms = "audit:exception",
        MenuType = LeanMenuType.Menu,
        Icon = "exception",
        Path = "/audit/exception",
        Component = "audit/exception/index",
        OrderNum = 3,
        MenuStatus = LeanMenuStatus.Normal,
        IsBuiltin = LeanBuiltinStatus.Yes,
        ParentId = auditMenu.Id,
        Visible = 0,
        IsFrame = 0,
        IsCached = 1,
        TransKey = "menu.audit.exception"
      }
    };

    // 在线管理子菜单
    var onlineMenus = new List<LeanMenu>
    {
      new LeanMenu
      {
        MenuName = "在线用户",
        Perms = "online:user",
        MenuType = LeanMenuType.Menu,
        Icon = "online-user",
        Path = "/online/user",
        Component = "online/user/index",
        OrderNum = 1,
        MenuStatus = LeanMenuStatus.Normal,
        IsBuiltin = LeanBuiltinStatus.Yes,
        ParentId = onlineMenu.Id,
        Visible = 0,
        IsFrame = 0,
        IsCached = 1,
        TransKey = "menu.online.user"
      },
      new LeanMenu
      {
        MenuName = "在线消息",
        Perms = "online:message",
        MenuType = LeanMenuType.Menu,
        Icon = "message",
        Path = "/online/message",
        Component = "online/message/index",
        OrderNum = 2,
        MenuStatus = LeanMenuStatus.Normal,
        IsBuiltin = LeanBuiltinStatus.Yes,
        ParentId = onlineMenu.Id,
        Visible = 0,
        IsFrame = 0,
        IsCached = 1,
        TransKey = "menu.online.message"
      }
    };

    // 系统工具子菜单
    var toolMenus = new List<LeanMenu>
    {
      new LeanMenu
      {
        MenuName = "系统接口",
        Perms = "tool:swagger",
        MenuType = LeanMenuType.Menu,
        Icon = "swagger",
        Path = "/tool/swagger",
        Component = "tool/swagger/index",
        OrderNum = 1,
        MenuStatus = LeanMenuStatus.Normal,
        IsBuiltin = LeanBuiltinStatus.Yes,
        ParentId = toolMenu.Id,
        Visible = 0,
        IsFrame = 0,
        IsCached = 1,
        TransKey = "menu.tool.swagger"
      }
    };

    // 插入所有子菜单
    await db.Insertable(systemMenus).ExecuteCommandAsync();
    await db.Insertable(generatorMenus).ExecuteCommandAsync();
    await db.Insertable(auditMenus).ExecuteCommandAsync();
    await db.Insertable(onlineMenus).ExecuteCommandAsync();
    await db.Insertable(toolMenus).ExecuteCommandAsync();

    // 获取所有菜单
    var menus = await db.Queryable<LeanMenu>().ToListAsync();

    // 为每个菜单添加按钮权限
    var buttons = new List<LeanMenu>();
    foreach (var menu in menus.Where(m => m.MenuType == LeanMenuType.Menu))
    {
      buttons.AddRange(new[]
      {
        new LeanMenu
        {
          MenuName = "查询",
          Perms = $"{menu.Perms}:query",
          MenuType = LeanMenuType.Button,
          OrderNum = 1,
          MenuStatus = LeanMenuStatus.Normal,
          IsBuiltin = LeanBuiltinStatus.Yes,
          ParentId = menu.Id,
          Visible = 0,
          IsFrame = 0,
          IsCached = 0,
          TransKey = $"{menu.TransKey}.query"
        },
        new LeanMenu
        {
          MenuName = "新增",
          Perms = $"{menu.Perms}:create",
          MenuType = LeanMenuType.Button,
          OrderNum = 2,
          MenuStatus = LeanMenuStatus.Normal,
          IsBuiltin = LeanBuiltinStatus.Yes,
          ParentId = menu.Id,
          Visible = 0,
          IsFrame = 0,
          IsCached = 0,
          TransKey = $"{menu.TransKey}.create"
        },
        new LeanMenu
        {
          MenuName = "修改",
          Perms = $"{menu.Perms}:update",
          MenuType = LeanMenuType.Button,
          OrderNum = 3,
          MenuStatus = LeanMenuStatus.Normal,
          IsBuiltin = LeanBuiltinStatus.Yes,
          ParentId = menu.Id,
          Visible = 0,
          IsFrame = 0,
          IsCached = 0,
          TransKey = $"{menu.TransKey}.update"
        },
        new LeanMenu
        {
          MenuName = "删除",
          Perms = $"{menu.Perms}:delete",
          MenuType = LeanMenuType.Button,
          OrderNum = 4,
          MenuStatus = LeanMenuStatus.Normal,
          IsBuiltin = LeanBuiltinStatus.Yes,
          ParentId = menu.Id,
          Visible = 0,
          IsFrame = 0,
          IsCached = 0,
          TransKey = $"{menu.TransKey}.delete"
        },
        new LeanMenu
        {
          MenuName = "导入",
          Perms = $"{menu.Perms}:import",
          MenuType = LeanMenuType.Button,
          OrderNum = 5,
          MenuStatus = LeanMenuStatus.Normal,
          IsBuiltin = LeanBuiltinStatus.Yes,
          ParentId = menu.Id,
          Visible = 0,
          IsFrame = 0,
          IsCached = 0,
          TransKey = $"{menu.TransKey}.import"
        },
        new LeanMenu
        {
          MenuName = "导出",
          Perms = $"{menu.Perms}:export",
          MenuType = LeanMenuType.Button,
          OrderNum = 6,
          MenuStatus = LeanMenuStatus.Normal,
          IsBuiltin = LeanBuiltinStatus.Yes,
          ParentId = menu.Id,
          Visible = 0,
          IsFrame = 0,
          IsCached = 0,
          TransKey = $"{menu.TransKey}.export"
        },
        new LeanMenu
        {
          MenuName = "预览",
          Perms = $"{menu.Perms}:preview",
          MenuType = LeanMenuType.Button,
          OrderNum = 7,
          MenuStatus = LeanMenuStatus.Normal,
          IsBuiltin = LeanBuiltinStatus.Yes,
          ParentId = menu.Id,
          Visible = 0,
          IsFrame = 0,
          IsCached = 0,
          TransKey = $"{menu.TransKey}.preview"
        },
        new LeanMenu
        {
          MenuName = "审批",
          Perms = $"{menu.Perms}:approve",
          MenuType = LeanMenuType.Button,
          OrderNum = 8,
          MenuStatus = LeanMenuStatus.Normal,
          IsBuiltin = LeanBuiltinStatus.Yes,
          ParentId = menu.Id,
          Visible = 0,
          IsFrame = 0,
          IsCached = 0,
          TransKey = $"{menu.TransKey}.approve"
        },
        new LeanMenu
        {
          MenuName = "撤销",
          Perms = $"{menu.Perms}:cancel",
          MenuType = LeanMenuType.Button,
          OrderNum = 9,
          MenuStatus = LeanMenuStatus.Normal,
          IsBuiltin = LeanBuiltinStatus.Yes,
          ParentId = menu.Id,
          Visible = 0,
          IsFrame = 0,
          IsCached = 0,
          TransKey = $"{menu.TransKey}.cancel"
        }
      });
    }

    // 插入所有按钮
    await db.Insertable(buttons).ExecuteCommandAsync();
  }
}