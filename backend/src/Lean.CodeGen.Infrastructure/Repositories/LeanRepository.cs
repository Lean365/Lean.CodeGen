using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Lean.CodeGen.Domain.Entities;
using Lean.CodeGen.Domain.Interfaces.Repositories;
using Lean.CodeGen.Infrastructure.Data.Context;
using Lean.CodeGen.Infrastructure.Extensions;
using Lean.CodeGen.Application.Services.Base;
using Lean.CodeGen.Domain.Context;
using SqlSugar;

namespace Lean.CodeGen.Infrastructure.Repositories
{
  /// <summary>
  /// 基础仓储实现
  /// </summary>
  /// <typeparam name="TEntity">实体类型</typeparam>
  /// <typeparam name="TKey">主键类型</typeparam>
  public class LeanRepository<TEntity, TKey> : ILeanRepository<TEntity, TKey>
      where TEntity : class, new()
  {
    protected readonly ISqlSugarClient DbContext;
    protected readonly LeanBaseServiceContext _serviceContext;
    protected readonly ILeanUserContext _userContext;

    protected LeanRepository(ISqlSugarClient dbContext, LeanBaseServiceContext serviceContext, ILeanUserContext userContext)
    {
      DbContext = dbContext;
      _serviceContext = serviceContext;
      _userContext = userContext;
    }

    #region 查询操作

    /// <summary>
    /// 获取单个实体
    /// </summary>
    public virtual async Task<TEntity> GetAsync(TKey id)
    {
      return await DbContext.Queryable<TEntity>().InSingleAsync(id);
    }

    /// <summary>
    /// 根据条件获取单个实体
    /// </summary>
    public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
    {
      return await DbContext.Queryable<TEntity>().FirstAsync(predicate);
    }

    /// <summary>
    /// 获取所有实体
    /// </summary>
    public virtual async Task<List<TEntity>> GetListAsync()
    {
      return await DbContext.Queryable<TEntity>().ToListAsync();
    }

    /// <summary>
    /// 根据条件获取实体列表
    /// </summary>
    public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate)
    {
      return await DbContext.Queryable<TEntity>().Where(predicate).ToListAsync();
    }

    /// <summary>
    /// 根据ID获取实体
    /// </summary>
    public virtual async Task<TEntity> GetByIdAsync(TKey id)
    {
      return await DbContext.Queryable<TEntity>().InSingleAsync(id);
    }

    /// <summary>
    /// 获取第一个匹配的实体
    /// </summary>
    public virtual async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
    {
      return await DbContext.Queryable<TEntity>().FirstAsync(predicate);
    }

    /// <summary>
    /// 是否存在匹配的实体
    /// </summary>
    public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
    {
      return await DbContext.Queryable<TEntity>().AnyAsync(predicate);
    }

    /// <summary>
    /// 获取匹配的实体数量
    /// </summary>
    public virtual async Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate)
    {
      return await DbContext.Queryable<TEntity>().CountAsync(predicate);
    }

    /// <summary>
    /// 分页查询（支持排序）
    /// </summary>
    public virtual async Task<(long Total, List<TEntity> Items)> GetPageListAsync(
        Expression<Func<TEntity, bool>> predicate,
        int pageIndex,
        int pageSize,
        Expression<Func<TEntity, object>> orderBy = null,
        bool isAsc = true)
    {
      RefAsync<int> total = 0;
      var query = DbContext.Queryable<TEntity>().Where(predicate);

      if (orderBy != null)
      {
        query = isAsc ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);
      }

      var items = await query.ToPageListAsync(pageIndex, pageSize, total);
      return (total, items);
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    public virtual async Task<(List<TEntity> Items, long Total)> GetPagedListAsync(
        Expression<Func<TEntity, bool>> predicate,
        int pageIndex,
        int pageSize)
    {
      RefAsync<int> total = 0;
      var items = await DbContext.Queryable<TEntity>()
          .Where(predicate)
          .ToPageListAsync(pageIndex, pageSize, total);
      return (items, total);
    }

    /// <summary>
    /// 分批获取数据
    /// </summary>
    public virtual async Task<List<TEntity>> GetBatchAsync(
        Expression<Func<TEntity, bool>> predicate,
        int batchSize = 1000)
    {
      var result = new List<TEntity>();
      var total = await DbContext.Queryable<TEntity>().CountAsync(predicate);
      var pageCount = (int)Math.Ceiling(total / (double)batchSize);

      for (var i = 1; i <= pageCount; i++)
      {
        var items = await DbContext.Queryable<TEntity>()
            .Where(predicate)
            .ToPageListAsync(i, batchSize);
        result.AddRange(items);
      }

      return result;
    }

    #endregion

    #region 新增操作

    /// <summary>
    /// 新增实体
    /// </summary>
    public virtual async Task<TKey> CreateAsync(TEntity entity)
    {
      if (entity is LeanBaseEntity baseEntity)
      {
        baseEntity.CreateTime = DateTime.Now;
        baseEntity.CreateBy = _userContext.GetCurrentUserName();
        baseEntity.TenantId = _userContext.GetCurrentTenantId();
      }
      await DbContext.Insertable(entity).ExecuteCommandAsync();
      return (TKey)entity.GetType().GetProperty("Id").GetValue(entity);
    }

    /// <summary>
    /// 批量新增实体
    /// </summary>
    public virtual async Task<bool> CreateRangeAsync(List<TEntity> entities)
    {
      if (entities == null || !entities.Any())
        return false;

      foreach (var entity in entities)
      {
        if (entity is LeanBaseEntity baseEntity)
        {
          baseEntity.CreateTime = DateTime.Now;
          baseEntity.CreateBy = _userContext.GetCurrentUserName();
          baseEntity.TenantId = _userContext.GetCurrentTenantId();
        }
      }

      return await DbContext.Insertable(entities).ExecuteCommandAsync() > 0;
    }

    #endregion

    #region 更新操作

    /// <summary>
    /// 更新实体
    /// </summary>
    public virtual async Task<bool> UpdateAsync(TEntity entity)
    {
      if (entity is LeanBaseEntity baseEntity)
      {
        baseEntity.UpdateTime = DateTime.Now;
        baseEntity.UpdateBy = _userContext.GetCurrentUserName();
      }
      return await DbContext.Updateable(entity).ExecuteCommandAsync() > 0;
    }

    /// <summary>
    /// 批量更新实体
    /// </summary>
    public virtual async Task<bool> UpdateRangeAsync(List<TEntity> entities)
    {
      if (entities == null || !entities.Any())
        return false;

      foreach (var entity in entities)
      {
        if (entity is LeanBaseEntity baseEntity)
        {
          baseEntity.UpdateTime = DateTime.Now;
          baseEntity.UpdateBy = _userContext.GetCurrentUserName();
        }
      }

      return await DbContext.Updateable(entities).ExecuteCommandAsync() > 0;
    }

    #endregion

    #region 审核操作

    /// <summary>
    /// 审核实体
    /// </summary>
    public virtual async Task<bool> AuditAsync(TEntity entity)
    {
      if (entity is LeanBaseEntity baseEntity)
      {
        baseEntity.AuditTime = DateTime.Now;
        baseEntity.AuditBy = _userContext.GetCurrentUserName();
      }
      return await DbContext.Updateable(entity).ExecuteCommandAsync() > 0;
    }

    /// <summary>
    /// 批量审核实体
    /// </summary>
    public virtual async Task<bool> AuditRangeAsync(List<TEntity> entities)
    {
      if (entities == null || !entities.Any())
        return false;

      foreach (var entity in entities)
      {
        if (entity is LeanBaseEntity baseEntity)
        {
          baseEntity.AuditTime = DateTime.Now;
          baseEntity.AuditBy = _userContext.GetCurrentUserName();
        }
      }

      return await DbContext.Updateable(entities).ExecuteCommandAsync() > 0;
    }

    #endregion

    #region 撤销操作

    /// <summary>
    /// 撤销实体
    /// </summary>
    public virtual async Task<bool> RevokeAsync(TEntity entity)
    {
      if (entity is LeanBaseEntity baseEntity)
      {
        baseEntity.AuditStatus = 0;
        baseEntity.AuditTime = null;
        baseEntity.AuditBy = null;
      }
      return await DbContext.Updateable(entity).ExecuteCommandAsync() > 0;
    }

    /// <summary>
    /// 批量撤销实体
    /// </summary>
    public virtual async Task<bool> RevokeRangeAsync(List<TEntity> entities)
    {
      if (entities == null || !entities.Any())
        return false;

      foreach (var entity in entities)
      {
        if (entity is LeanBaseEntity baseEntity)
        {
          baseEntity.AuditStatus = 0;
          baseEntity.AuditTime = null;
          baseEntity.AuditBy = null;
        }
      }

      return await DbContext.Updateable(entities).ExecuteCommandAsync() > 0;
    }

    #endregion

    #region 删除操作

    /// <summary>
    /// 删除实体
    /// </summary>
    public virtual async Task<bool> DeleteAsync(TEntity entity)
    {
      if (entity is LeanBaseEntity baseEntity)
      {
        baseEntity.IsDeleted = 1;
        baseEntity.DeleteTime = DateTime.Now;
        baseEntity.DeleteBy = _userContext.GetCurrentUserName();
      }
      return await DbContext.Updateable(entity).ExecuteCommandAsync() > 0;
    }

    /// <summary>
    /// 根据条件删除实体
    /// </summary>
    public virtual async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
    {
      var entities = await DbContext.Queryable<TEntity>().Where(predicate).ToListAsync();
      if (!entities.Any())
        return false;

      foreach (var entity in entities)
      {
        if (entity is LeanBaseEntity baseEntity)
        {
          baseEntity.IsDeleted = 1;
          baseEntity.DeleteTime = DateTime.Now;
          baseEntity.DeleteBy = _userContext.GetCurrentUserName();
        }
      }

      return await DbContext.Updateable(entities).ExecuteCommandAsync() > 0;
    }

    /// <summary>
    /// 批量删除实体
    /// </summary>
    public virtual async Task<bool> DeleteRangeAsync(List<TEntity> entities)
    {
      if (entities == null || !entities.Any())
        return false;

      foreach (var entity in entities)
      {
        if (entity is LeanBaseEntity baseEntity)
        {
          baseEntity.IsDeleted = 1;
          baseEntity.DeleteTime = DateTime.Now;
          baseEntity.DeleteBy = _userContext.GetCurrentUserName();
        }
      }

      return await DbContext.Updateable(entities).ExecuteCommandAsync() > 0;
    }

    #endregion
  }

  /// <summary>
  /// 业务仓储实现
  /// </summary>
  /// <typeparam name="TEntity">实体类型</typeparam>
  public class LeanRepository<TEntity> : LeanRepository<TEntity, long>, ILeanRepository<TEntity>
      where TEntity : LeanBaseEntity, new()
  {
    public LeanRepository(ISqlSugarClient dbContext, LeanBaseServiceContext serviceContext, ILeanUserContext userContext)
        : base(dbContext, serviceContext, userContext)
    {
    }

    #region 查询方法重写

    public override async Task<List<TEntity>> GetListAsync()
    {
      return await DbContext.Queryable<TEntity>().Where(e => e.IsDeleted != 1).ToListAsync();
    }

    public override async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate)
    {
      var notDeletedPredicate = Expression.Lambda<Func<TEntity, bool>>(
          Expression.AndAlso(
              Expression.Equal(Expression.Property(Expression.Parameter(typeof(TEntity)), "IsDeleted"), Expression.Constant(0)),
              predicate.Body),
          predicate.Parameters);
      return await DbContext.Queryable<TEntity>().Where(notDeletedPredicate).ToListAsync();
    }

    public override async Task<(long Total, List<TEntity> Items)> GetPageListAsync(
        Expression<Func<TEntity, bool>> predicate,
        int pageIndex,
        int pageSize,
        Expression<Func<TEntity, object>> orderBy = null,
        bool isAsc = true)
    {
      var notDeletedPredicate = Expression.Lambda<Func<TEntity, bool>>(
          Expression.AndAlso(
              Expression.Equal(Expression.Property(Expression.Parameter(typeof(TEntity)), "IsDeleted"), Expression.Constant(0)),
              predicate.Body),
          predicate.Parameters);

      RefAsync<int> total = 0;
      var query = DbContext.Queryable<TEntity>().Where(notDeletedPredicate);

      if (orderBy != null)
      {
        query = isAsc ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);
      }

      var items = await query.ToPageListAsync(pageIndex, pageSize, total);
      return (total, items);
    }

    #endregion
  }
}
