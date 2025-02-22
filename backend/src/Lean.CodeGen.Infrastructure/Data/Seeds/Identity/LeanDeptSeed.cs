using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Domain.Entities.Identity;
using Lean.CodeGen.Infrastructure.Data.Context;
using SqlSugar;
using System.Threading.Tasks;

namespace Lean.CodeGen.Infrastructure.Data.Seeds.Identity
{
  /// <summary>
  /// 部门数据种子
  /// </summary>
  public class LeanDeptSeed : ILeanDataSeed
  {
    public int Order => 3;

    public async Task SeedAsync(LeanDbContext dbContext)
    {
      var db = dbContext.GetDatabase();
      if (await db.Queryable<LeanDept>().AnyAsync())
      {
        return;
      }

      await db.Insertable(new[]
      {
                new LeanDept
                {
                    DeptName = "总公司",
                    DeptCode = "HQ",
                    DeptDescription = "总公司",
                    OrderNum = 1,
                    DeptStatus = LeanDeptStatus.Normal,
                    IsBuiltin = LeanBuiltinStatus.Yes,
                    Status = LeanStatus.Enable
                },
                new LeanDept
                {
                    DeptName = "技术部",
                    DeptCode = "TECH",
                    DeptDescription = "技术部门",
                    OrderNum = 2,
                    DeptStatus = LeanDeptStatus.Normal,
                    IsBuiltin = LeanBuiltinStatus.Yes,
                    Status = LeanStatus.Enable
                }
            }).ExecuteCommandAsync();
    }
  }
}
