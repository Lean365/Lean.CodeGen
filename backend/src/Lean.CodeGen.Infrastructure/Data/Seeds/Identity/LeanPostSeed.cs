using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Domain.Entities.Identity;
using Lean.CodeGen.Infrastructure.Data.Context;
using SqlSugar;
using System.Threading.Tasks;

namespace Lean.CodeGen.Infrastructure.Data.Seeds.Identity;

/// <summary>
/// 岗位种子数据
/// </summary>
public class LeanPostSeed : ILeanDataSeed
{
  public int Order => 4;

  public async Task SeedAsync(LeanDbContext dbContext)
  {
    var db = dbContext.GetDatabase();
    if (await db.Queryable<LeanPost>().AnyAsync())
    {
      return;
    }

    await db.Insertable(new[]
    {
            new LeanPost
            {
                PostName = "系统管理员",
                PostCode = "ADMIN",
                PostDescription = "系统管理员岗位",
                OrderNum = 1,
                PostStatus = LeanPostStatus.Normal,
                IsBuiltin = LeanBuiltinStatus.Yes
            },
            new LeanPost
            {
                PostName = "开发工程师",
                PostCode = "DEV",
                PostDescription = "开发工程师岗位",
                OrderNum = 2,
                PostStatus =LeanPostStatus.Normal,
                IsBuiltin = LeanBuiltinStatus.Yes
            },
            new LeanPost
            {
                PostName = "测试工程师",
                PostCode = "QA",
                PostDescription = "测试工程师岗位",
                OrderNum = 3,
                PostStatus = LeanPostStatus.Normal,
                IsBuiltin = LeanBuiltinStatus.Yes
            }
        }).ExecuteCommandAsync();
  }
}