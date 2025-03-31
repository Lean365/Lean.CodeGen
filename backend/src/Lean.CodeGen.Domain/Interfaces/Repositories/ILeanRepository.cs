//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: ILeanRepository.cs
// 功能描述: 通用仓储接口
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Lean.CodeGen.Domain.Interfaces.Repositories
{
  /// <summary>
  /// 基础仓储接口
  /// </summary>
  /// <typeparam name="TEntity">实体类型</typeparam>
  /// <typeparam name="TKey">主键类型</typeparam>
  public interface ILeanRepository<TEntity, TKey> where TEntity : class
  {
    #region 查询操作

    /// <summary>
    /// 根据ID获取实体
    /// </summary>
    Task<TEntity> GetByIdAsync(TKey id);

    /// <summary>
    /// 获取第一个匹配的实体
    /// </summary>
    Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// 获取所有实体
    /// </summary>
    Task<List<TEntity>> GetListAsync();

    /// <summary>
    /// 根据条件获取实体列表
    /// </summary>
    Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// 是否存在匹配的实体
    /// </summary>
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// 获取匹配的实体数量
    /// </summary>
    Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate);

    #endregion

    #region 分页查询

    /// <summary>
    /// 分页查询（支持排序）
    /// </summary>
    Task<(long Total, List<TEntity> Items)> GetPageListAsync(
        Expression<Func<TEntity, bool>> predicate,
        int pageIndex,
        int pageSize,
        Expression<Func<TEntity, object>> orderBy = null,
        bool isAsc = true);

    /// <summary>
    /// 分批获取数据
    /// </summary>
    Task<List<TEntity>> GetBatchAsync(
        Expression<Func<TEntity, bool>> predicate,
        int batchSize = 1000);

    #endregion

    #region 新增操作

    /// <summary>
    /// 新增实体
    /// </summary>
    Task<TKey> CreateAsync(TEntity entity);

    /// <summary>
    /// 批量新增实体
    /// </summary>
    Task<bool> CreateRangeAsync(List<TEntity> entities);

    #endregion

    #region 更新操作

    /// <summary>
    /// 更新实体
    /// </summary>
    Task<bool> UpdateAsync(TEntity entity);

    /// <summary>
    /// 批量更新实体
    /// </summary>
    Task<bool> UpdateRangeAsync(List<TEntity> entities);

    #endregion

    #region 审核操作

    /// <summary>
    /// 审核实体
    /// </summary>
    Task<bool> AuditAsync(TEntity entity);

    /// <summary>
    /// 批量审核实体
    /// </summary>
    Task<bool> AuditRangeAsync(List<TEntity> entities);

    #endregion

    #region 撤销操作

    /// <summary>
    /// 撤销实体
    /// </summary>
    Task<bool> RevokeAsync(TEntity entity);

    /// <summary>
    /// 批量撤销实体
    /// </summary>
    Task<bool> RevokeRangeAsync(List<TEntity> entities);

    #endregion

    #region 删除操作

    /// <summary>
    /// 删除实体
    /// </summary>
    Task<bool> DeleteAsync(TEntity entity);

    /// <summary>
    /// 根据条件删除实体
    /// </summary>
    Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// 批量删除实体
    /// </summary>
    Task<bool> DeleteRangeAsync(List<TEntity> entities);

    #endregion
  }

  /// <summary>
  /// 业务仓储接口
  /// </summary>
  /// <typeparam name="TEntity">实体类型</typeparam>
  public interface ILeanRepository<TEntity> : ILeanRepository<TEntity, long> where TEntity : class
  {
  }
}
