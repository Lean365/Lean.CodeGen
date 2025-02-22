using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Domain.Entities.Identity;
using Lean.CodeGen.Infrastructure.Data.Context;
using SqlSugar;
using System.Threading.Tasks;
using System.Linq;

namespace Lean.CodeGen.Infrastructure.Data.Seeds.Identity;

/// <summary>
/// 关联关系种子数据
/// </summary>
public class LeanRelationSeed : ILeanDataSeed
{
  public int Order => 6;

  public async Task SeedAsync(LeanDbContext dbContext)
  {
    var db = dbContext.GetDatabase();

    // 获取角色
    var adminRole = await db.Queryable<LeanRole>()
        .Where(r => r.RoleCode == "admin")
        .FirstAsync();

    var userRole = await db.Queryable<LeanRole>()
        .Where(r => r.RoleCode == "user")
        .FirstAsync();

    if (adminRole == null || userRole == null)
    {
      return;
    }

    // 获取所有菜单
    var menus = await db.Queryable<LeanMenu>().ToListAsync();
    if (menus.Count == 0)
    {
      return;
    }

    // 获取所有部门
    var depts = await db.Queryable<LeanDept>().ToListAsync();
    if (depts.Count == 0)
    {
      return;
    }

    // 超级管理员角色关联所有菜单
    var roleMenus = menus.Select(m => new LeanRoleMenu
    {
      RoleId = adminRole.Id,
      MenuId = m.Id
    }).ToList();
    await db.Insertable(roleMenus).ExecuteCommandAsync();

    // 普通用户角色关联基本菜单(除了菜单管理)
    var userMenus = menus.Where(m => m.Perms != "system:menu")
        .Select(m => new LeanRoleMenu
        {
          RoleId = userRole.Id,
          MenuId = m.Id
        }).ToList();
    await db.Insertable(userMenus).ExecuteCommandAsync();

    // 角色-部门数据权限
    var roleDepts = depts.Select(d => new LeanRoleDept
    {
      RoleId = adminRole.Id,
      DeptId = d.Id
    }).ToList();
    await db.Insertable(roleDepts).ExecuteCommandAsync();

    // 为普通用户角色分配其所在部门的数据权限
    var techDept = depts.FirstOrDefault(d => d.DeptCode == "TECH");
    if (techDept != null)
    {
      await db.Insertable(new LeanRoleDept
      {
        RoleId = userRole.Id,
        DeptId = techDept.Id
      }).ExecuteCommandAsync();
    }
  }
}