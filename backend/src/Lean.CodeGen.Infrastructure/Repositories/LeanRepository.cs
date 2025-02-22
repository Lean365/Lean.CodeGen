using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Domain.Entities;
using Lean.CodeGen.Domain.Interfaces.Repositories;
using SqlSugar;

namespace Lean.CodeGen.Infrastructure.Repositories;

/// <summary>
/// 通用仓储实现（使用 long 作为主键类型）
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
public class LeanRepository<TEntity> : LeanRepository<TEntity, long>, ILeanRepository<TEntity>
  where TEntity : LeanBaseEntity, new()
{
  public LeanRepository(ISqlSugarClient db) : base(db)
  {
  }
}

/// <summary>
/// 通用仓储实现
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
/// <typeparam name="TKey">主键类型</typeparam>
public class LeanRepository<TEntity, TKey> : ILeanRepository<TEntity, TKey>
  where TEntity : class, new()
{
  protected readonly ISqlSugarClient Db;
  protected readonly ISimpleClient<TEntity> Entity;

  public LeanRepository(ISqlSugarClient db)
  {
    Db = db;
    Entity = db.GetSimpleClient<TEntity>();
  }

  /// <summary>
  /// 创建实体
  /// </summary>
  public virtual async Task<TKey> CreateAsync(TEntity entity)
  {
    var id = await Entity.InsertReturnIdentityAsync(entity);
    return (TKey)Convert.ChangeType(id, typeof(TKey));
  }

  /// <summary>
  /// 批量创建实体
  /// </summary>
  public virtual async Task<bool> CreateRangeAsync(List<TEntity> entities)
  {
    return await Entity.InsertRangeAsync(entities);
  }

  /// <summary>
  /// 更新实体
  /// </summary>
  public virtual async Task<bool> UpdateAsync(TEntity entity)
  {
    return await Entity.UpdateAsync(entity);
  }

  /// <summary>
  /// 批量更新实体
  /// </summary>
  public virtual async Task<bool> UpdateRangeAsync(List<TEntity> entities)
  {
    return await Entity.UpdateRangeAsync(entities);
  }

  /// <summary>
  /// 删除实体
  /// </summary>
  public virtual async Task<bool> DeleteAsync(TEntity entity)
  {
    return await Entity.DeleteAsync(entity);
  }

  /// <summary>
  /// 根据条件删除实体
  /// </summary>
  public virtual async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
  {
    return await Entity.DeleteAsync(predicate);
  }

  /// <summary>
  /// 批量删除实体
  /// </summary>
  public virtual async Task<bool> DeleteRangeAsync(List<TEntity> entities)
  {
    return await Db.Deleteable<TEntity>(entities).ExecuteCommandAsync() > 0;
  }

  /// <summary>
  /// 根据ID获取实体
  /// </summary>
  public virtual async Task<TEntity?> GetByIdAsync(TKey id)
  {
    return await Entity.GetByIdAsync(id);
  }

  /// <summary>
  /// 根据条件获取第一个实体
  /// </summary>
  public virtual async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
  {
    return await Entity.GetFirstAsync(predicate);
  }

  /// <summary>
  /// 根据条件获取实体列表
  /// </summary>
  public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate)
  {
    return await Entity.GetListAsync(predicate);
  }

  /// <summary>
  /// 分页查询
  /// </summary>
  public virtual async Task<(long Total, List<TEntity> Items)> GetPageListAsync(Expression<Func<TEntity, bool>> predicate, int pageSize, int pageIndex)
  {
    var query = Db.Queryable<TEntity>().Where(predicate);
    RefAsync<int> total = 0;
    var items = await query.ToPageListAsync(pageIndex, pageSize, total);

    return (total, items);
  }

  /// <summary>
  /// 检查是否存在
  /// </summary>
  public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
  {
    return await Entity.IsAnyAsync(predicate);
  }

  /// <summary>
  /// 根据条件获取数量
  /// </summary>
  public virtual async Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate)
  {
    return await Entity.CountAsync(predicate);
  }
}