using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Lean.CodeGen.Domain.Entities;
using Lean.CodeGen.Domain.Interfaces.Repositories;
using SqlSugar;
using NLog;

namespace Lean.CodeGen.Infrastructure.Data.Repositories;

/// <summary>
/// 仓储基类实现
/// </summary>
public class LeanRepository<TEntity> : ILeanRepository<TEntity> where TEntity : LeanBaseEntity, new()
{
  private readonly ISqlSugarClient _db;
  private readonly ILogger _logger;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanRepository(ISqlSugarClient db, ILogger logger)
  {
    _db = db;
    _logger = logger;
  }

  /// <summary>
  /// 开始事务
  /// </summary>
  public async Task BeginTransactionAsync()
  {
    await _db.Ado.BeginTranAsync();
  }

  /// <summary>
  /// 提交事务
  /// </summary>
  public async Task CommitAsync()
  {
    await _db.Ado.CommitTranAsync();
  }

  /// <summary>
  /// 回滚事务
  /// </summary>
  public async Task RollbackAsync()
  {
    await _db.Ado.RollbackTranAsync();
  }

  /// <summary>
  /// 根据主键获取实体
  /// </summary>
  public async Task<TEntity?> GetByIdAsync(long id)
  {
    try
    {
      return await _db.Queryable<TEntity>().InSingleAsync(id);
    }
    catch (Exception ex)
    {
      _logger.Error(ex, $"Error getting entity by id: {id}");
      throw;
    }
  }

  /// <summary>
  /// 获取所有实体
  /// </summary>
  public async Task<List<TEntity>> GetAllAsync()
  {
    try
    {
      return await _db.Queryable<TEntity>().ToListAsync();
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "Error getting all entities");
      throw;
    }
  }

  /// <summary>
  /// 根据条件获取实体列表
  /// </summary>
  public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate)
  {
    try
    {
      return await _db.Queryable<TEntity>().Where(predicate).ToListAsync();
    }
    catch (Exception ex)
    {
      _logger.Error(ex, $"Error getting entities with predicate: {predicate}");
      throw;
    }
  }

  /// <summary>
  /// 根据条件获取第一个实体
  /// </summary>
  public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
  {
    try
    {
      return await _db.Queryable<TEntity>().FirstAsync(predicate);
    }
    catch (Exception ex)
    {
      _logger.Error(ex, $"Error getting first entity with predicate: {predicate}");
      throw;
    }
  }

  /// <summary>
  /// 添加实体
  /// </summary>
  public async Task<long> CreateAsync(TEntity entity)
  {
    try
    {
      return await _db.Insertable(entity).ExecuteReturnIdentityAsync();
    }
    catch (Exception ex)
    {
      _logger.Error(ex, $"Error adding entity: {entity}");
      throw;
    }
  }

  /// <summary>
  /// 批量添加实体
  /// </summary>
  public async Task<bool> CreateRangeAsync(List<TEntity> entities)
  {
    try
    {
      return await _db.Insertable(entities).ExecuteCommandAsync() > 0;
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "Error adding entities");
      throw;
    }
  }

  /// <summary>
  /// 更新实体
  /// </summary>
  public async Task<bool> UpdateAsync(TEntity entity)
  {
    try
    {
      return await _db.Updateable(entity).ExecuteCommandAsync() > 0;
    }
    catch (Exception ex)
    {
      _logger.Error(ex, $"Error updating entity: {entity}");
      throw;
    }
  }

  /// <summary>
  /// 批量更新实体
  /// </summary>
  public async Task<bool> UpdateRangeAsync(List<TEntity> entities)
  {
    try
    {
      return await _db.Updateable(entities).ExecuteCommandAsync() > 0;
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "Error updating entities");
      throw;
    }
  }

  /// <summary>
  /// 删除实体
  /// </summary>
  public async Task<bool> DeleteAsync(TEntity entity)
  {
    try
    {
      return await _db.Deleteable(entity).ExecuteCommandAsync() > 0;
    }
    catch (Exception ex)
    {
      _logger.Error(ex, $"Error deleting entity: {entity}");
      throw;
    }
  }

  /// <summary>
  /// 批量删除实体
  /// </summary>
  public async Task<bool> DeleteRangeAsync(List<TEntity> entities)
  {
    try
    {
      return await _db.Deleteable<TEntity>().In(entities).ExecuteCommandAsync() > 0;
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "Error deleting entities");
      throw;
    }
  }

  /// <summary>
  /// 根据条件删除实体
  /// </summary>
  public async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
  {
    try
    {
      return await _db.Deleteable<TEntity>().Where(predicate).ExecuteCommandAsync() > 0;
    }
    catch (Exception ex)
    {
      _logger.Error(ex, $"Error deleting entities with predicate: {predicate}");
      throw;
    }
  }

  /// <summary>
  /// 检查是否存在满足条件的实体
  /// </summary>
  public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
  {
    try
    {
      return await _db.Queryable<TEntity>().AnyAsync(predicate);
    }
    catch (Exception ex)
    {
      _logger.Error(ex, $"Error checking existence with predicate: {predicate}");
      throw;
    }
  }

  /// <summary>
  /// 获取满足条件的实体数量
  /// </summary>
  public async Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate)
  {
    try
    {
      return await _db.Queryable<TEntity>().CountAsync(predicate);
    }
    catch (Exception ex)
    {
      _logger.Error(ex, $"Error counting entities with predicate: {predicate}");
      throw;
    }
  }

  /// <summary>
  /// 分页查询
  /// </summary>
  public async Task<(long Total, List<TEntity> Items)> GetPageListAsync(
    Expression<Func<TEntity, bool>> predicate,
    int pageSize,
    int pageIndex,
    Expression<Func<TEntity, object>>? orderByExpression = null,
    bool isAsc = true)
  {
    try
    {
      var query = _db.Queryable<TEntity>().Where(predicate);
      if (orderByExpression != null)
      {
        query = isAsc ? query.OrderBy(orderByExpression) : query.OrderByDescending(orderByExpression);
      }
      RefAsync<int> total = 0;
      var items = await query.ToPageListAsync(pageIndex, pageSize, total);
      return ((long)total, items);
    }
    catch (Exception ex)
    {
      _logger.Error(ex, $"Error getting page list with predicate: {predicate}");
      throw;
    }
  }

  /// <summary>
  /// 批量获取实体
  /// </summary>
  public async Task<List<TEntity>> GetBatchAsync(Expression<Func<TEntity, bool>> predicate, int batchSize = 1000)
  {
    try
    {
      return await _db.Queryable<TEntity>().Where(predicate).Take(batchSize).ToListAsync();
    }
    catch (Exception ex)
    {
      _logger.Error(ex, $"Error getting batch with predicate: {predicate}");
      throw;
    }
  }
}