using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Domain.Entities.Identity;
using Lean.CodeGen.Infrastructure.Data.Context;
using SqlSugar;
using System.Threading.Tasks;

namespace Lean.CodeGen.Infrastructure.Data.Seeds.Identity;

/// <summary>
/// 用户关系种子数据
/// </summary>
public class LeanUserRelationSeed : ILeanDataSeed
{
  public int Order => 5;

  public async Task SeedAsync(LeanDbContext dbContext)
  {
    var db = dbContext.GetDatabase();

    // 获取用户
    var adminUser = await db.Queryable<LeanUser>()
        .Where(u => u.UserName == "admin")
        .FirstAsync();

    var testUser = await db.Queryable<LeanUser>()
        .Where(u => u.UserName == "test")
        .FirstAsync();

    if (adminUser == null || testUser == null)
    {
      return;
    }

    // 获取角色
    var adminRole = await db.Queryable<LeanRole>()
        .Where(r => r.RoleCode == "admin")
        .FirstAsync();

    var userRole = await db.Queryable<LeanRole>()
        .Where(r => r.RoleCode == "user")
        .FirstAsync();

    // 获取部门
    var hqDept = await db.Queryable<LeanDept>()
        .Where(d => d.DeptCode == "HQ")
        .FirstAsync();

    var techDept = await db.Queryable<LeanDept>()
        .Where(d => d.DeptCode == "TECH")
        .FirstAsync();

    // 获取岗位
    var adminPost = await db.Queryable<LeanPost>()
        .Where(p => p.PostCode == "ADMIN")
        .FirstAsync();

    var devPost = await db.Queryable<LeanPost>()
        .Where(p => p.PostCode == "DEV")
        .FirstAsync();

    // 建立管理员关系
    await db.Insertable(new LeanUserRole
    {
      UserId = adminUser.Id,
      RoleId = adminRole.Id
    }).ExecuteCommandAsync();

    await db.Insertable(new LeanUserDept
    {
      UserId = adminUser.Id,
      DeptId = hqDept.Id,
      IsPrimary = LeanPrimaryStatus.Yes
    }).ExecuteCommandAsync();

    await db.Insertable(new LeanUserPost
    {
      UserId = adminUser.Id,
      PostId = adminPost.Id,
      IsPrimary = LeanPrimaryStatus.Yes
    }).ExecuteCommandAsync();

    // 建立测试用户关系
    await db.Insertable(new LeanUserRole
    {
      UserId = testUser.Id,
      RoleId = userRole.Id
    }).ExecuteCommandAsync();

    await db.Insertable(new LeanUserDept
    {
      UserId = testUser.Id,
      DeptId = techDept.Id,
      IsPrimary = LeanPrimaryStatus.Yes
    }).ExecuteCommandAsync();

    await db.Insertable(new LeanUserPost
    {
      UserId = testUser.Id,
      PostId = devPost.Id,
      IsPrimary = LeanPrimaryStatus.Yes
    }).ExecuteCommandAsync();
  }
}