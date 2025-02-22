using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Domain.Entities.Identity;
using Lean.CodeGen.Infrastructure.Data.Context;
using SqlSugar;
using System.Threading.Tasks;

namespace Lean.CodeGen.Infrastructure.Data.Seeds.Identity;

/// <summary>
/// 角色种子数据
/// </summary>
public class LeanRoleSeed : ILeanDataSeed
{
  public int Order => 1;

  public async Task SeedAsync(LeanDbContext dbContext)
  {
    var db = dbContext.GetDatabase();
    if (await db.Queryable<LeanRole>().AnyAsync())
    {
      return;
    }

    await db.Insertable(new[]
    {
            new LeanRole
            {
                RoleName = "超级管理员",
                RoleCode = "admin",
                RoleDescription = "系统内置超级管理员角色",
                OrderNum = 1,
                RoleStatus = LeanRoleStatus.Normal,
                IsBuiltin = LeanBuiltinStatus.Yes
            },
            new LeanRole
            {
                RoleName = "普通用户",
                RoleCode = "user",
                RoleDescription = "系统内置普通用户角色",
                OrderNum = 2,
                RoleStatus = LeanRoleStatus.Normal,
                IsBuiltin = LeanBuiltinStatus.Yes
            }
        }).ExecuteCommandAsync();
  }
}