using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Domain.Entities;
using Lean.CodeGen.Domain.Interfaces.Repositories;
using Lean.CodeGen.Infrastructure.Data.Context;
using SqlSugar;

namespace Lean.CodeGen.Infrastructure.Repositories;

/// <summary>
/// 通用仓储实现（使用 long 作为主键类型）
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
/// <remarks>
/// 该类继承自 LeanRepository{TEntity, long}，专门用于处理使用 long 类型作为主键的实体。
/// 通常用于系统中的大多数实体，因为它们都继承自 LeanBaseEntity。
/// </remarks>
public class LeanRepository<TEntity> : LeanRepository<TEntity, long>, ILeanRepository<TEntity>
  where TEntity : LeanBaseEntity, new()
{
  /// <summary>
  /// 初始化仓储实例
  /// </summary>
  /// <param name="dbContext">数据库上下文</param>
  public LeanRepository(LeanDbContext dbContext) : base(dbContext)
  {
  }
}

/// <summary>
/// 通用仓储实现
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
/// <typeparam name="TKey">主键类型</typeparam>
/// <remarks>
/// 该类提供了对实体的基本CRUD操作和一些高级查询功能：
/// 1. 基本的增删改查操作
/// 2. 批量操作支持
/// 3. 事务管理
/// 4. 分页查询
/// 5. 条件查询等
/// </remarks>
public class LeanRepository<TEntity, TKey> : ILeanRepository<TEntity, TKey>
  where TEntity : class, new()
{
  /// <summary>
  /// 数据库上下文
  /// </summary>
  protected readonly LeanDbContext DbContext;

  /// <summary>
  /// 初始化仓储实例
  /// </summary>
  /// <param name="dbContext">数据库上下文</param>
  public LeanRepository(LeanDbContext dbContext)
  {
    DbContext = dbContext;
  }

  /// <summary>
  /// 开始事务
  /// </summary>
  /// <remarks>
  /// 如果当前没有活动的事务，则开启新事务。
  /// 如果已有事务在进行中，则忽略此调用。
  /// </remarks>
  public virtual async Task BeginTransactionAsync()
  {
    await DbContext.BeginTransactionAsync();
  }

  /// <summary>
  /// 提交事务
  /// </summary>
  /// <remarks>
  /// 只有在事务已经开启的情况下才会执行提交操作。
  /// 提交后会清除事务状态标记。
  /// </remarks>
  public virtual async Task CommitAsync()
  {
    await DbContext.CommitTransactionAsync();
  }

  /// <summary>
  /// 回滚事务
  /// </summary>
  /// <remarks>
  /// 只有在事务已经开启的情况下才会执行回滚操作。
  /// 回滚后会清除事务状态标记。
  /// </remarks>
  public virtual async Task RollbackAsync()
  {
    await DbContext.RollbackTransactionAsync();
  }

  /// <summary>
  /// 创建实体
  /// </summary>
  /// <param name="entity">要创建的实体对象</param>
  /// <returns>新创建实体的主键值</returns>
  /// <remarks>
  /// 使用 InsertReturnIdentity 方法插入实体并返回自增主键值。
  /// 支持不同类型的主键通过类型转换返回。
  /// </remarks>
  public virtual async Task<TKey> CreateAsync(TEntity entity)
  {
    var id = await DbContext.InsertReturnIdentityAsync(entity);
    return (TKey)Convert.ChangeType(id, typeof(TKey));
  }

  /// <summary>
  /// 批量创建实体
  /// </summary>
  /// <param name="entities">要创建的实体列表</param>
  /// <returns>是否创建成功</returns>
  /// <remarks>
  /// 使用批量插入方式提高性能。
  /// 如果列表为空则返回 false。
  /// </remarks>
  public virtual async Task<bool> CreateRangeAsync(List<TEntity> entities)
  {
    if (entities == null || !entities.Any())
      return false;

    return await DbContext.InsertRangeAsync(entities);
  }

  /// <summary>
  /// 更新实体
  /// </summary>
  /// <param name="entity">要更新的实体对象</param>
  /// <returns>是否更新成功</returns>
  public virtual async Task<bool> UpdateAsync(TEntity entity)
  {
    return await DbContext.UpdateAsync(entity);
  }

  /// <summary>
  /// 批量更新实体
  /// </summary>
  /// <param name="entities">要更新的实体列表</param>
  /// <returns>是否更新成功</returns>
  /// <remarks>
  /// 使用批量更新方式提高性能。
  /// 如果列表为空则返回 false。
  /// </remarks>
  public virtual async Task<bool> UpdateRangeAsync(List<TEntity> entities)
  {
    if (entities == null || !entities.Any())
      return false;

    return await DbContext.UpdateRangeAsync(entities);
  }

  /// <summary>
  /// 删除实体
  /// </summary>
  /// <param name="entity">要删除的实体对象</param>
  /// <returns>是否删除成功</returns>
  public virtual async Task<bool> DeleteAsync(TEntity entity)
  {
    return await DbContext.DeleteAsync(entity);
  }

  /// <summary>
  /// 根据条件删除实体
  /// </summary>
  /// <param name="predicate">删除条件表达式</param>
  /// <returns>是否删除成功</returns>
  public virtual async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
  {
    return await DbContext.DeleteAsync(predicate);
  }

  /// <summary>
  /// 批量删除实体
  /// </summary>
  /// <param name="entities">要删除的实体列表</param>
  /// <returns>是否删除成功</returns>
  /// <remarks>
  /// 使用批量删除方式提高性能。
  /// 如果列表为空则返回 false。
  /// </remarks>
  public virtual async Task<bool> DeleteRangeAsync(List<TEntity> entities)
  {
    if (entities == null || !entities.Any())
      return false;

    return await DbContext.DeleteRangeAsync(entities);
  }

  /// <summary>
  /// 根据ID获取实体
  /// </summary>
  /// <param name="id">实体ID</param>
  /// <returns>实体对象，如果不存在则返回null</returns>
  public virtual async Task<TEntity?> GetByIdAsync(TKey id)
  {
    return await DbContext.GetByIdAsync<TEntity>(id);
  }

  /// <summary>
  /// 根据条件获取第一个实体
  /// </summary>
  /// <param name="predicate">查询条件表达式</param>
  /// <returns>实体对象，如果不存在则返回null</returns>
  public virtual async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
  {
    return await DbContext.FirstOrDefaultAsync(predicate);
  }

  /// <summary>
  /// 根据条件获取实体列表
  /// </summary>
  /// <param name="predicate">查询条件表达式</param>
  /// <returns>实体列表</returns>
  public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate)
  {
    return await DbContext.GetListAsync(predicate);
  }

  /// <summary>
  /// 分页查询
  /// </summary>
  /// <param name="predicate">查询条件表达式</param>
  /// <param name="pageSize">每页大小</param>
  /// <param name="pageIndex">页码</param>
  /// <param name="orderByExpression">排序表达式</param>
  /// <param name="isAsc">是否升序</param>
  /// <returns>总记录数和当前页数据列表</returns>
  /// <remarks>
  /// 支持动态排序和条件查询。
  /// 使用 RefAsync 优化性能。
  /// </remarks>
  public virtual async Task<(long Total, List<TEntity> Items)> GetPageListAsync(
    Expression<Func<TEntity, bool>> predicate,
    int pageSize,
    int pageIndex,
    Expression<Func<TEntity, object>>? orderByExpression = null,
    bool isAsc = true)
  {
    return await DbContext.GetPageListAsync(predicate, pageSize, pageIndex, orderByExpression, isAsc);
  }

  /// <summary>
  /// 检查是否存在
  /// </summary>
  /// <param name="predicate">查询条件表达式</param>
  /// <returns>是否存在符合条件的记录</returns>
  public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
  {
    return await DbContext.AnyAsync(predicate);
  }

  /// <summary>
  /// 根据条件获取数量
  /// </summary>
  /// <param name="predicate">查询条件表达式</param>
  /// <returns>符合条件的记录数</returns>
  public virtual async Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate)
  {
    return await DbContext.CountAsync(predicate);
  }

  /// <summary>
  /// 批量查询
  /// </summary>
  /// <param name="predicate">查询条件表达式</param>
  /// <param name="batchSize">批次大小</param>
  /// <returns>实体列表</returns>
  /// <remarks>
  /// 用于处理大数据量查询，通过分批次查询避免内存溢出。
  /// 每次查询 batchSize 条记录，直到没有更多数据。
  /// </remarks>
  public virtual async Task<List<TEntity>> GetBatchAsync(Expression<Func<TEntity, bool>> predicate, int batchSize = 1000)
  {
    return await DbContext.GetBatchAsync(predicate, batchSize);
  }
}