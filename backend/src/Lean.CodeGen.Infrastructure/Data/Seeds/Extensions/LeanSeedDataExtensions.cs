using Lean.CodeGen.Domain.Entities;
using SqlSugar;

namespace Lean.CodeGen.Infrastructure.Data.Seeds.Extensions;

/// <summary>
/// 种子数据扩展方法
/// </summary>
public static class LeanSeedDataExtensions
{
  /// <summary>
  /// 初始化审计字段
  /// </summary>
  public static T InitAuditFields<T>(this T entity, bool isUpdate = false) where T : LeanBaseEntity
  {
    if (!isUpdate)
    {
      entity.CreateTime = DateTime.Now;
      entity.CreateUserId = 0;
      entity.CreateUserName = "system";
      entity.TenantId = 0; // 设置默认租户ID为0，表示系统租户
    }
    else
    {
      entity.UpdateTime = DateTime.Now;
      entity.UpdateUserId = 0;
      entity.UpdateUserName = "system";
    }
    return entity;
  }

  /// <summary>
  /// 从现有实体复制审计字段
  /// </summary>
  public static T CopyAuditFields<T>(this T target, T source) where T : LeanBaseEntity
  {
    target.CreateTime = source.CreateTime;
    target.CreateUserId = source.CreateUserId;
    target.CreateUserName = source.CreateUserName;
    target.TenantId = source.TenantId; // 复制租户ID
    return target;
  }

  /// <summary>
  /// 初始化种子数据
  /// </summary>
  public static async Task SeedDataAsync<T>(this T entity, ISqlSugarClient db)
      where T : LeanBaseEntity, new()
  {
    entity.InitAuditFields();
    await db.Insertable(entity).ExecuteCommandAsync();
  }
}