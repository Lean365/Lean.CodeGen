using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Domain.Entities.Identity;
using SqlSugar;
using System.Threading.Tasks;
using NLog;
using ILogger = NLog.ILogger;

namespace Lean.CodeGen.Infrastructure.Data.Seeds.Identity;

/// <summary>
/// 部门种子数据
/// </summary>
public class LeanDeptSeed
{
  private readonly ISqlSugarClient _db;
  private readonly ILogger _logger;

  public LeanDeptSeed(ISqlSugarClient db)
  {
    _db = db;
    _logger = LogManager.GetCurrentClassLogger();
  }

  public async Task InitializeAsync()
  {
    _logger.Info("开始初始化部门数据...");

    // 集团总部部门
    var headquarterDepts = new List<LeanDept>
    {
      new()
      {
        DeptCode = "group",
        DeptName = "集团总部",
        ParentId = 0,
        OrderNum = 1,
        Leader = "张三",
        Phone = "15888888888",
        Email = "admin@lean.com",
        DeptStatus = LeanDeptStatus.Normal,
        IsBuiltin = LeanBuiltinStatus.Yes
      }
    };

    // 业务部门
    var businessDepts = new List<LeanDept>
    {
      new()
      {
        DeptCode = "dev",
        DeptName = "研发部门",
        ParentId = 1,
        OrderNum = 2,
        Leader = "李四",
        Phone = "15666666666",
        Email = "dev@lean.com",
        DeptStatus = LeanDeptStatus.Normal,
        IsBuiltin = LeanBuiltinStatus.Yes
      },
      new()
      {
        DeptCode = "sales",
        DeptName = "销售部门",
        ParentId = 1,
        OrderNum = 3,
        Leader = "王五",
        Phone = "15777777777",
        Email = "sales@lean.com",
        DeptStatus = LeanDeptStatus.Normal,
        IsBuiltin = LeanBuiltinStatus.Yes
      }
    };

    // 合并所有部门
    var defaultDepts = new List<LeanDept>();
    defaultDepts.AddRange(headquarterDepts);
    defaultDepts.AddRange(businessDepts);

    // 更新或插入部门数据
    foreach (var dept in defaultDepts)
    {
      var exists = await _db.Queryable<LeanDept>()
          .FirstAsync(x => x.DeptCode == dept.DeptCode);

      if (exists != null)
      {
        dept.Id = exists.Id;
        await _db.Updateable(dept).ExecuteCommandAsync();
        _logger.Info($"更新部门: {dept.DeptName}");
      }
      else
      {
        await _db.Insertable(dept).ExecuteCommandAsync();
        _logger.Info($"新增部门: {dept.DeptName}");
      }
    }

    _logger.Info("部门数据初始化完成");
  }
}