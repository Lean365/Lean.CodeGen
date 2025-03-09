using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Domain.Entities.Identity;
using SqlSugar;
using System.Threading.Tasks;
using NLog;
using ILogger = NLog.ILogger;
using Lean.CodeGen.Infrastructure.Data.Seeds.Extensions;

namespace Lean.CodeGen.Infrastructure.Data.Seeds.Identity;

/// <summary>
/// 身份关系种子数据
/// </summary>
/// <remarks>
/// 负责初始化以下关联关系：
/// 1. 用户-角色关联
/// 2. 角色-菜单关联
/// 3. 用户-部门关联
/// 4. 用户-岗位关联
/// </remarks>
public class LeanIdentityRelationSeed
{
  private readonly ISqlSugarClient _db;
  private readonly ILogger _logger;

  public LeanIdentityRelationSeed(ISqlSugarClient db)
  {
    _db = db;
    _logger = LogManager.GetCurrentClassLogger();
  }

  public async Task InitializeAsync()
  {
    _logger.Info("开始初始化身份关系数据...");

    await InitializeUserRoleRelationsAsync();
    await InitializeRoleMenuRelationsAsync();
    await InitializeUserDeptRelationsAsync();
    await InitializeUserPostRelationsAsync();
    await InitializeRoleDeptRelationsAsync();

    _logger.Info("身份关系数据初始化完成");
  }

  /// <summary>
  /// 初始化用户-角色关联
  /// </summary>
  private async Task InitializeUserRoleRelationsAsync()
  {
    _logger.Info("初始化用户-角色关联...");

    // 获取管理员用户和角色
    var adminUser = await _db.Queryable<LeanUser>()
        .FirstAsync(u => u.UserName == "admin");
    var adminRole = await _db.Queryable<LeanRole>()
        .FirstAsync(r => r.RoleCode == "admin");

    // 获取测试用户和普通角色
    var testUser = await _db.Queryable<LeanUser>()
        .FirstAsync(u => u.UserName == "test");
    var userRole = await _db.Queryable<LeanRole>()
        .FirstAsync(r => r.RoleCode == "user");

    if (adminUser != null && adminRole != null)
    {
      var adminUserRole = new LeanUserRole
      {
        UserId = adminUser.Id,
        RoleId = adminRole.Id
      }.InitAuditFields();

      var exists = await _db.Queryable<LeanUserRole>()
          .FirstAsync(ur => ur.UserId == adminUser.Id && ur.RoleId == adminRole.Id);

      if (exists == null)
      {
        await _db.Insertable(adminUserRole).ExecuteCommandAsync();
        _logger.Info($"新增用户角色关联: {adminUser.UserName} - {adminRole.RoleName}");
      }
    }

    if (testUser != null && userRole != null)
    {
      var testUserRole = new LeanUserRole
      {
        UserId = testUser.Id,
        RoleId = userRole.Id
      }.InitAuditFields();

      var exists = await _db.Queryable<LeanUserRole>()
          .FirstAsync(ur => ur.UserId == testUser.Id && ur.RoleId == userRole.Id);

      if (exists == null)
      {
        await _db.Insertable(testUserRole).ExecuteCommandAsync();
        _logger.Info($"新增用户角色关联: {testUser.UserName} - {userRole.RoleName}");
      }
    }
  }

  /// <summary>
  /// 初始化角色-菜单关联
  /// </summary>
  private async Task InitializeRoleMenuRelationsAsync()
  {
    _logger.Info("初始化角色-菜单关联...");

    // 获取管理员角色
    var adminRole = await _db.Queryable<LeanRole>()
        .FirstAsync(r => r.RoleCode == "admin");

    // 获取所有菜单
    var allMenus = await _db.Queryable<LeanMenu>().ToListAsync();

    if (adminRole != null && allMenus.Any())
    {
      foreach (var menu in allMenus)
      {
        var roleMenu = new LeanRoleMenu
        {
          RoleId = adminRole.Id,
          MenuId = menu.Id
        }.InitAuditFields();

        var exists = await _db.Queryable<LeanRoleMenu>()
            .FirstAsync(rm => rm.RoleId == adminRole.Id && rm.MenuId == menu.Id);

        if (exists == null)
        {
          await _db.Insertable(roleMenu).ExecuteCommandAsync();
          _logger.Info($"新增角色菜单关联: {adminRole.RoleName} - {menu.MenuName}");
        }
      }
    }

    // 获取普通用户角色
    var userRole = await _db.Queryable<LeanRole>()
        .FirstAsync(r => r.RoleCode == "user");

    // 获取基础菜单（这里假设用户管理菜单）
    var userMenu = await _db.Queryable<LeanMenu>()
        .FirstAsync(m => m.MenuName == "用户管理");

    if (userRole != null && userMenu != null)
    {
      var roleMenu = new LeanRoleMenu
      {
        RoleId = userRole.Id,
        MenuId = userMenu.Id
      }.InitAuditFields();

      var exists = await _db.Queryable<LeanRoleMenu>()
          .FirstAsync(rm => rm.RoleId == userRole.Id && rm.MenuId == userMenu.Id);

      if (exists == null)
      {
        await _db.Insertable(roleMenu).ExecuteCommandAsync();
        _logger.Info($"新增角色菜单关联: {userRole.RoleName} - {userMenu.MenuName}");
      }
    }
  }

  /// <summary>
  /// 初始化用户-部门关联
  /// </summary>
  private async Task InitializeUserDeptRelationsAsync()
  {
    _logger.Info("初始化用户-部门关联...");

    // 获取管理员用户和总部部门
    var adminUser = await _db.Queryable<LeanUser>()
        .FirstAsync(u => u.UserName == "admin");
    var headDept = await _db.Queryable<LeanDept>()
        .FirstAsync(d => d.DeptCode == "group");

    // 获取测试用户和研发部门
    var testUser = await _db.Queryable<LeanUser>()
        .FirstAsync(u => u.UserName == "test");
    var devDept = await _db.Queryable<LeanDept>()
        .FirstAsync(d => d.DeptCode == "dev");

    if (adminUser != null && headDept != null)
    {
      var userDept = new LeanUserDept
      {
        UserId = adminUser.Id,
        DeptId = headDept.Id,
        IsPrimary = 1
      }.InitAuditFields();

      var exists = await _db.Queryable<LeanUserDept>()
          .FirstAsync(ud => ud.UserId == adminUser.Id && ud.DeptId == headDept.Id);

      if (exists == null)
      {
        await _db.Insertable(userDept).ExecuteCommandAsync();
        _logger.Info($"新增用户部门关联: {adminUser.UserName} - {headDept.DeptName}");
      }
    }

    if (testUser != null && devDept != null)
    {
      var userDept = new LeanUserDept
      {
        UserId = testUser.Id,
        DeptId = devDept.Id,
        IsPrimary = 1
      }.InitAuditFields();

      var exists = await _db.Queryable<LeanUserDept>()
          .FirstAsync(ud => ud.UserId == testUser.Id && ud.DeptId == devDept.Id);

      if (exists == null)
      {
        await _db.Insertable(userDept).ExecuteCommandAsync();
        _logger.Info($"新增用户部门关联: {testUser.UserName} - {devDept.DeptName}");
      }
    }
  }

  /// <summary>
  /// 初始化用户-岗位关联
  /// </summary>
  private async Task InitializeUserPostRelationsAsync()
  {
    _logger.Info("初始化用户-岗位关联...");

    // 获取管理员用户和CEO岗位
    var adminUser = await _db.Queryable<LeanUser>()
        .FirstAsync(u => u.UserName == "admin");
    var ceoPost = await _db.Queryable<LeanPost>()
        .FirstAsync(p => p.PostCode == "ceo");

    // 获取测试用户和项目经理岗位
    var testUser = await _db.Queryable<LeanUser>()
        .FirstAsync(u => u.UserName == "test");
    var sePost = await _db.Queryable<LeanPost>()
        .FirstAsync(p => p.PostCode == "se");

    if (adminUser != null && ceoPost != null)
    {
      var userPost = new LeanUserPost
      {
        UserId = adminUser.Id,
        PostId = ceoPost.Id
      }.InitAuditFields();

      var exists = await _db.Queryable<LeanUserPost>()
          .FirstAsync(up => up.UserId == adminUser.Id && up.PostId == ceoPost.Id);

      if (exists == null)
      {
        await _db.Insertable(userPost).ExecuteCommandAsync();
        _logger.Info($"新增用户岗位关联: {adminUser.UserName} - {ceoPost.PostName}");
      }
    }

    if (testUser != null && sePost != null)
    {
      var userPost = new LeanUserPost
      {
        UserId = testUser.Id,
        PostId = sePost.Id
      }.InitAuditFields();

      var exists = await _db.Queryable<LeanUserPost>()
          .FirstAsync(up => up.UserId == testUser.Id && up.PostId == sePost.Id);

      if (exists == null)
      {
        await _db.Insertable(userPost).ExecuteCommandAsync();
        _logger.Info($"新增用户岗位关联: {testUser.UserName} - {sePost.PostName}");
      }
    }
  }

  /// <summary>
  /// 初始化角色-部门关联
  /// </summary>
  /// <remarks>
  /// 为角色分配数据权限范围内的部门
  /// - 超级管理员角色：可以访问所有部门数据
  /// - 普通用户角色：只能访问自己所在部门的数据
  /// </remarks>
  private async Task InitializeRoleDeptRelationsAsync()
  {
    _logger.Info("初始化角色-部门关联...");

    // 获取管理员角色
    var adminRole = await _db.Queryable<LeanRole>()
        .FirstAsync(r => r.RoleCode == "admin");

    // 获取所有部门
    var allDepts = await _db.Queryable<LeanDept>().ToListAsync();

    // 为管理员角色分配所有部门权限
    if (adminRole != null && allDepts.Any())
    {
      foreach (var dept in allDepts)
      {
        var roleDept = new LeanRoleDept
        {
          RoleId = adminRole.Id,
          DeptId = dept.Id
        }.InitAuditFields();

        var exists = await _db.Queryable<LeanRoleDept>()
            .FirstAsync(rd => rd.RoleId == adminRole.Id && rd.DeptId == dept.Id);

        if (exists == null)
        {
          await _db.Insertable(roleDept).ExecuteCommandAsync();
          _logger.Info($"新增角色部门关联: {adminRole.RoleName} - {dept.DeptName}");
        }
      }
    }

    // 获取普通用户角色
    var userRole = await _db.Queryable<LeanRole>()
        .FirstAsync(r => r.RoleCode == "user");

    // 获取研发部门（普通用户只能访问研发部门数据）
    var devDept = await _db.Queryable<LeanDept>()
        .FirstAsync(d => d.DeptCode == "dev");

    if (userRole != null && devDept != null)
    {
      var roleDept = new LeanRoleDept
      {
        RoleId = userRole.Id,
        DeptId = devDept.Id
      }.InitAuditFields();

      var exists = await _db.Queryable<LeanRoleDept>()
          .FirstAsync(rd => rd.RoleId == userRole.Id && rd.DeptId == devDept.Id);

      if (exists == null)
      {
        await _db.Insertable(roleDept).ExecuteCommandAsync();
        _logger.Info($"新增角色部门关联: {userRole.RoleName} - {devDept.DeptName}");
      }
    }
  }
}