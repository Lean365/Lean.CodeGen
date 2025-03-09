using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Domain.Entities.Identity;
using SqlSugar;
using System.Threading.Tasks;
using NLog;
using ILogger = NLog.ILogger;
using Lean.CodeGen.Infrastructure.Data.Seeds.Extensions;
using System.IO;
using System.Collections.Generic;

namespace Lean.CodeGen.Infrastructure.Data.Seeds.Identity;

/// <summary>
/// 菜单种子数据
/// </summary>
/// <remarks>
/// 该类负责根据控制器目录结构自动生成系统菜单数据。
/// 遵循以下规则：
/// 1. 控制器目录作为顶级菜单
/// 2. 目录下的控制器文件作为子菜单
/// 3. 为每个子菜单自动生成标准按钮（新增、编辑、删除等）
/// 4. 权限标识符遵循 {目录}:{控制器}:{操作} 的命名规范
/// </remarks>
public class LeanMenuSeed
{
  /// <summary>
  /// 数据库访问对象
  /// </summary>
  private readonly ISqlSugarClient _db;

  /// <summary>
  /// 日志记录器
  /// </summary>
  private readonly ILogger _logger;

  /// <summary>
  /// 控制器目录路径
  /// </summary>
  private readonly string _controllerPath;

  /// <summary>
  /// 用于跟踪每个父级下的排序号
  /// </summary>
  private Dictionary<long, int> _parentOrderNums = new();

  /// <summary>
  /// 获取下一个排序号
  /// </summary>
  /// <param name="parentId">父级菜单ID</param>
  /// <returns>下一个排序号</returns>
  private int GetNextOrderNum(long parentId)
  {
    if (!_parentOrderNums.ContainsKey(parentId))
    {
      _parentOrderNums[parentId] = 1;
    }
    return _parentOrderNums[parentId]++;
  }

  /// <summary>
  /// 初始化菜单种子数据类
  /// </summary>
  /// <param name="db">数据库访问对象</param>
  public LeanMenuSeed(ISqlSugarClient db)
  {
    _db = db;
    _logger = LogManager.GetCurrentClassLogger();

    // 获取当前程序集所在目录
    var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
    // 从当前目录向上导航到解决方案根目录，然后定位到Controllers文件夹
    _controllerPath = Path.GetFullPath(Path.Combine(baseDirectory, "..", "..", "..", "..", "..", "src", "Lean.CodeGen.WebApi", "Controllers"));
    _logger.Info($"控制器目录路径: {_controllerPath}");
  }

  /// <summary>
  /// 初始化菜单数据
  /// </summary>
  /// <returns>异步任务</returns>
  public async Task InitializeAsync()
  {
    _logger.Info("开始初始化菜单数据...");

    // 第一步：创建所有顶级菜单（目录）
    var directoryMenus = new Dictionary<string, LeanMenu>();
    var topMenus = new[]
    {
      ("Identity", "身份认证", "TeamOutlined"),
      ("Admin", "系统管理", "ControlOutlined"),
      ("Generator", "代码生成", "CodeOutlined"),
      ("Workflow", "工作流程", "DeploymentUnitOutlined"),
      ("Signalr", "即时通讯", "MessageOutlined"),
      ("Audit", "审计日志", "AuditOutlined")
    };

    _logger.Info("开始创建顶级菜单...");
    foreach (var (name, displayName, icon) in topMenus)
    {
      var menu = new LeanMenu
      {
        MenuName = displayName,
        ParentId = 0,
        OrderNum = GetNextOrderNum(0),
        Path = name.ToLower(),
        Component = "Layout",
        IsFrame = 0,
        IsCached = 0,
        Visible = 0,
        MenuStatus = 1,
        MenuType = 2,
        Icon = icon,
        TransKey = $"menu.{name.ToLower()}",
        Perms = $"{name.ToLower()}:list",
        IsBuiltin = 1
      };

      var exists = await _db.Queryable<LeanMenu>()
          .FirstAsync(m => m.MenuName == menu.MenuName && m.ParentId == 0);

      if (exists != null)
      {
        menu.Id = exists.Id;
        menu.CopyAuditFields(exists).InitAuditFields(true);
        await _db.Updateable(menu).ExecuteCommandAsync();
        _logger.Info($"更新目录菜单: {menu.MenuName}");
        directoryMenus.Add(name, menu);
      }
      else
      {
        menu.InitAuditFields();
        await _db.Insertable(menu).ExecuteCommandAsync();
        _logger.Info($"新增目录菜单: {menu.MenuName}");
        directoryMenus.Add(name, menu);
      }
    }
    _logger.Info("顶级菜单创建完成");

    // 第二步：创建所有子菜单（控制器）
    _logger.Info("开始创建子菜单...");
    var controllerMenus = new List<(LeanMenu Menu, string DirName)>();

    // 定义每个目录下的控制器
    var directoryControllers = new Dictionary<string, string[]>
    {
      ["Identity"] = new[]
      {
        "User",      // 用户管理
        "Role",      // 角色管理
        "Menu",      // 菜单管理
        "Dept",      // 部门管理
        "Post"       // 岗位管理
      },
      ["Admin"] = new[]
      {
        "DictType",      // 字典类型
        "DictData",      // 字典数据
        "Config",        // 参数配置
        "Language",      // 语言管理
        "Translation",   // 翻译管理
        "Localization"   // 本地化
      },
      ["Generator"] = new[]
      {
        "GenTask",      // 生成任务
        "GenTemplate",  // 生成模板
        "GenConfig",    // 生成配置
        "GenHistory",   // 生成历史
        "DataSource",   // 数据源
        "DbTable",      // 数据表
        "TableConfig"   // 表配置
      },
      ["Workflow"] = new[]
      {
        "WorkflowDefinition",      // 流程定义
        "WorkflowInstance",        // 流程实例
        "WorkflowTask",           // 流程任务
        "WorkflowForm",           // 流程表单
        "WorkflowVariable",       // 流程变量
        "WorkflowVariableData",   // 变量数据
        "WorkflowActivityType",   // 活动类型
        "WorkflowActivityProperty", // 活动属性
        "WorkflowActivityInstance", // 活动实例
        "WorkflowOutput",         // 流程输出
        "WorkflowOutcome",        // 流程结果
        "WorkflowHistory",        // 流程历史
        "WorkflowCorrelation"     // 流程关联
      },
      ["Signalr"] = new[]
      {
        "OnlineUser",    // 在线用户
        "OnlineMessage"  // 在线消息
      },
      ["Audit"] = new[]
      {
        "AuditLog",      // 审计日志
        "OperationLog",  // 操作日志
        "LoginLog",      // 登录日志
        "ExceptionLog",  // 异常日志
        "SqlDiffLog"     // SQL差异日志
      }
    };

    foreach (var dirEntry in directoryControllers)
    {
      var dirName = dirEntry.Key;
      var controllers = dirEntry.Value;

      // 先获取顶级菜单
      var parentMenu = await _db.Queryable<LeanMenu>()
          .FirstAsync(m => m.MenuName == topMenus.First(t => t.Item1 == dirName).Item2 && m.ParentId == 0);

      if (parentMenu == null)
      {
        _logger.Info($"跳过未找到的顶级菜单: {dirName}");
        continue;
      }

      foreach (var controller in controllers)
      {
        // 创建子菜单并获取返回的ID
        var menu = await CreateControllerMenu($"{controller}Controller.cs", parentMenu.Id, dirName);

        // 确保获取到最新的子菜单记录
        var subMenu = await _db.Queryable<LeanMenu>()
            .FirstAsync(m => m.Perms == $"{dirName.ToLower()}:{menu.MenuName.ToLower()}:list");

        if (subMenu != null)
        {
          controllerMenus.Add((subMenu, dirName));
        }
      }
    }
    _logger.Info("子菜单创建完成");

    // 第三步：创建所有按钮
    _logger.Info("开始创建按钮...");
    foreach (var (menu, dirName) in controllerMenus)
    {
      // 获取正确的routeName，从菜单名称转换为小写
      var routeName = menu.MenuName.ToLower();
      await CreateMenuButtons(menu.Id, routeName, dirName);
    }
    _logger.Info("按钮创建完成");

    _logger.Info("菜单数据初始化完成");
  }

  /// <summary>
  /// 创建目录菜单
  /// </summary>
  /// <param name="dirName">目录名称</param>
  /// <returns>创建的目录菜单对象</returns>
  /// <remarks>
  /// 根据控制器目录创建顶级菜单，具有以下特点：
  /// 1. ParentId 为 0，表示顶级菜单
  /// 2. 菜单类型为 Directory
  /// 3. 使用目录名作为权限标识前缀
  /// </remarks>
  private async Task<LeanMenu> CreateDirectoryMenu(string dirName)
  {
    var menu = new LeanMenu
    {
      MenuName = dirName,
      ParentId = 0,
      OrderNum = GetNextOrderNum(0),
      Path = dirName.ToLower(),
      Component = "Layout",
      IsFrame = 0,
      IsCached = 0,
      Visible = 0,
      MenuStatus = 1,
      MenuType = 2,
      Icon = GetDirectoryIcon(dirName.ToLower()),
      TransKey = $"menu.{dirName.ToLower()}",
      Perms = $"{dirName.ToLower()}:list",
      IsBuiltin = 1
    };

    var exists = await _db.Queryable<LeanMenu>()
        .FirstAsync(m => m.MenuName == menu.MenuName && m.ParentId == 0);

    if (exists != null)
    {
      menu.Id = exists.Id;
      menu.CopyAuditFields(exists).InitAuditFields(true);
      await _db.Updateable(menu).ExecuteCommandAsync();
      _logger.Info($"更新目录菜单: {menu.MenuName}");
      return menu;
    }
    else
    {
      menu.InitAuditFields();
      await _db.Insertable(menu).ExecuteCommandAsync();
      _logger.Info($"新增目录菜单: {menu.MenuName}");
      return menu;
    }
  }

  /// <summary>
  /// 获取目录的图标
  /// </summary>
  /// <param name="dirName">目录名称</param>
  /// <returns>图标名称</returns>
  private string GetDirectoryIcon(string dirName)
  {
    return dirName.ToLower() switch
    {
      "identity" => "TeamOutlined",       // 身份认证
      "admin" => "ControlOutlined",       // 系统管理
      "generator" => "CodeOutlined",      // 代码生成
      "workflow" => "DeploymentUnitOutlined", // 工作流程
      "signalr" => "MessageOutlined",     // 即时通讯
      "audit" => "AuditOutlined",         // 审计日志
      _ => "FolderOutlined"               // 默认文件夹图标
    };
  }

  /// <summary>
  /// 创建控制器菜单
  /// </summary>
  /// <param name="filePath">控制器文件路径</param>
  /// <param name="parentId">父级菜单ID</param>
  /// <param name="permPrefix">权限前缀（目录名）</param>
  /// <returns>创建的控制器菜单对象</returns>
  /// <remarks>
  /// 根据控制器文件创建子菜单，具有以下特点：
  /// 1. 自动去除文件名中的"Controller"后缀
  /// 2. 继承父级目录的权限前缀
  /// 3. 自动生成路由路径和组件路径
  /// </remarks>
  private async Task<LeanMenu> CreateControllerMenu(string filePath, long parentId, string permPrefix)
  {
    var fileName = Path.GetFileNameWithoutExtension(filePath);
    var menuName = fileName.Replace("Controller", "").Replace("Lean", "");
    var routeName = menuName.ToLower();

    // 创建菜单
    var menu = new LeanMenu
    {
      MenuName = menuName,
      ParentId = parentId,
      OrderNum = GetNextOrderNum(parentId),
      Path = routeName,
      Component = $"{permPrefix.ToLower()}{routeName}/index",
      IsFrame = 0,
      IsCached = 0,
      Visible = 0,
      MenuStatus = 1,
      MenuType = 1,
      Icon = GetControllerIcon(menuName.ToLower()),
      TransKey = $"menu.{permPrefix.ToLower()}.{routeName}",
      Perms = $"{permPrefix.ToLower()}:{routeName}:list",
      IsBuiltin = 1
    };

    // 检查是否存在（优先使用权限标识符检查）
    var exists = await _db.Queryable<LeanMenu>()
        .FirstAsync(m => m.Perms == menu.Perms);

    if (exists != null)
    {
      // 更新现有记录
      menu.Id = exists.Id;
      menu.CopyAuditFields(exists).InitAuditFields(true);
      await _db.Updateable(menu).ExecuteCommandAsync();
      _logger.Info($"更新菜单: {menu.MenuName}, 权限: {menu.Perms}");
      return menu;
    }
    else
    {
      // 插入新记录
      menu.InitAuditFields();
      var newId = await _db.Insertable(menu).ExecuteReturnIdentityAsync();
      menu.Id = newId;
      _logger.Info($"新增菜单: {menu.MenuName}, 权限: {menu.Perms}");
      return menu;
    }
  }

  /// <summary>
  /// 获取控制器菜单的图标
  /// </summary>
  /// <param name="controllerName">控制器名称</param>
  /// <returns>图标名称</returns>
  private string GetControllerIcon(string controllerName)
  {
    return controllerName.ToLower() switch
    {
      // Identity模块
      "user" => "UserOutlined",           // 用户管理
      "role" => "SafetyOutlined",         // 角色管理
      "menu" => "MenuOutlined",           // 菜单管理
      "dept" => "ApartmentOutlined",      // 部门管理
      "post" => "IdcardOutlined",         // 岗位管理

      // Admin模块
      "dicttype" => "BookOutlined",       // 字典类型
      "dictdata" => "OrderedListOutlined", // 字典数据
      "config" => "SettingOutlined",      // 参数配置
      "language" => "GlobalOutlined",     // 语言管理
      "translation" => "TranslationOutlined", // 翻译管理
      "localization" => "GlobalOutlined", // 本地化

      // Generator模块
      "gentask" => "CodeOutlined",        // 生成任务
      "gentemplate" => "FileOutlined",    // 生成模板
      "genconfig" => "ToolOutlined",      // 生成配置
      "genhistory" => "HistoryOutlined",  // 生成历史
      "datasource" => "DatabaseOutlined", // 数据源
      "dbtable" => "TableOutlined",       // 数据表
      "tableconfig" => "ProfileOutlined", // 表配置

      // Workflow模块
      "workflowdefinition" => "ApiOutlined",      // 流程定义
      "workflowinstance" => "DeploymentUnitOutlined", // 流程实例
      "workflowtask" => "CarryOutOutlined",      // 流程任务
      "workflowform" => "FormOutlined",          // 流程表单
      "workflowvariable" => "TagsOutlined",      // 流程变量
      "workflowvariabledata" => "DatabaseOutlined", // 变量数据
      "workflowactivitytype" => "AppstoreOutlined", // 活动类型
      "workflowactivityproperty" => "SettingOutlined", // 活动属性
      "workflowactivityinstance" => "ApartmentOutlined", // 活动实例
      "workflowoutput" => "ExportOutlined",      // 流程输出
      "workflowoutcome" => "CheckCircleOutlined", // 流程结果
      "workflowhistory" => "HistoryOutlined",    // 流程历史
      "workflowcorrelation" => "LinkOutlined",   // 流程关联

      // Signalr模块
      "onlineuser" => "UserSwitchOutlined",     // 在线用户
      "onlinemessage" => "MessageOutlined",     // 在线消息

      // Audit模块
      "auditlog" => "AuditOutlined",           // 审计日志
      "operationlog" => "ToolOutlined",        // 操作日志
      "loginlog" => "LoginOutlined",           // 登录日志
      "exceptionlog" => "BugOutlined",         // 异常日志
      "sqldifflog" => "ConsoleSqlOutlined",    // SQL差异日志

      // 默认图标
      _ => "AppstoreOutlined"             // 默认应用图标
    };
  }

  /// <summary>
  /// 创建菜单按钮
  /// </summary>
  /// <param name="parentId">父级菜单ID</param>
  /// <param name="routeName">路由名称</param>
  /// <param name="permPrefix">权限前缀</param>
  /// <remarks>
  /// 为菜单创建标准按钮，包括：
  /// 1. 新增、更新、删除等基本操作按钮
  /// 2. 导入、导出等数据操作按钮
  /// 3. 预览、打印等查看按钮
  /// 4. 审核、撤消等业务按钮
  /// 每个按钮都继承父级菜单的权限前缀，并添加相应的操作权限
  /// </remarks>
  private async Task CreateMenuButtons(long parentId, string routeName, string permPrefix)
  {
    // 验证父菜单是否存在
    var parentMenu = await _db.Queryable<LeanMenu>()
        .FirstAsync(m => m.Id == parentId);

    if (parentMenu == null)
    {
      _logger.Error($"未找到父菜单: {parentId}");
      return;
    }

    // 所有按钮的统一定义
    var buttons = new[]
    {
      ("查询", "query", "SearchOutlined"),         // 查询数据按钮
      ("新增", "create", "PlusOutlined"),          // 新增数据按钮
      ("更新", "update", "EditOutlined"),          // 更新数据按钮
      ("删除", "delete", "DeleteOutlined"),        // 删除数据按钮
      ("清空", "clear", "ClearOutlined"),          // 清空数据按钮
      ("模板", "template", "FileExcelOutlined"),   // 下载模板按钮
      ("导入", "import", "ImportOutlined"),        // 导入数据按钮
      ("导出", "export", "ExportOutlined"),        // 导出数据按钮
      ("预览", "preview", "EyeOutlined"),          // 预览数据按钮
      ("打印", "print", "PrinterOutlined"),        // 打印数据按钮
      ("审核", "audit", "CheckOutlined"),          // 审核数据按钮
      ("撤消", "revoke", "UndoOutlined"),         // 撤消操作按钮
      ("翻译", "translate", "TranslationOutlined"), // 翻译按钮
      ("图标", "icon", "FontColorsOutlined")       // 图标按钮
    };

    // 为每个按钮创建菜单项
    foreach (var (name, action, icon) in buttons)
    {
      var button = new LeanMenu
      {
        MenuName = name,
        ParentId = parentId,
        OrderNum = GetNextOrderNum(parentId),
        Path = "#",
        Component = "",
        IsFrame = 0,
        IsCached = 0,
        Visible = 0,
        MenuStatus = 1,
        MenuType = 2,
        Icon = icon,
        Perms = $"{permPrefix.ToLower()}:{routeName.Replace("lean", "")}:{action}",
        IsBuiltin = 1
      };

      // 检查是否存在
      var exists = await _db.Queryable<LeanMenu>()
          .FirstAsync(m => m.Perms == button.Perms);

      if (exists != null)
      {
        // 更新现有记录
        button.Id = exists.Id;
        button.CopyAuditFields(exists).InitAuditFields(true);
        await _db.Updateable(button).ExecuteCommandAsync();
        _logger.Info($"更新按钮: {button.MenuName}, 权限: {button.Perms}");
      }
      else
      {
        // 插入新记录
        button.InitAuditFields();
        await _db.Insertable(button).ExecuteCommandAsync();
        _logger.Info($"新增按钮: {button.MenuName}, 权限: {button.Perms}");
      }
    }
  }
}