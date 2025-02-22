using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Domain.Entities.Identity;
using Lean.CodeGen.Infrastructure.Data.Context;
using SqlSugar;
using System.Threading.Tasks;

namespace Lean.CodeGen.Infrastructure.Data.Seeds.Identity;

/// <summary>
/// 用户种子数据
/// </summary>
public class LeanUserSeed : ILeanDataSeed
{
  public int Order => 2;

  public async Task SeedAsync(LeanDbContext dbContext)
  {
    var db = dbContext.GetDatabase();
    if (await db.Queryable<LeanUser>().AnyAsync())
    {
      return;
    }

    await db.Insertable(new[]
    {
            new LeanUser
            {
                UserName = "admin",
                RealName = "系统管理员",
                Password = "123456", // 注意：实际应用中应该使用加密密码
                UserType = LeanUserType.System,
                UserStatus = LeanUserStatus.Normal,
                IsBuiltin = LeanBuiltinStatus.Yes,

            },
            new LeanUser
            {
                UserName = "test",
                RealName = "测试用户",
                Password = "123456", // 注意：实际应用中应该使用加密密码
                UserType = LeanUserType.Normal,
                UserStatus = LeanUserStatus.Normal,
                IsBuiltin = LeanBuiltinStatus.Yes,

            }
        }).ExecuteCommandAsync();
  }
}
