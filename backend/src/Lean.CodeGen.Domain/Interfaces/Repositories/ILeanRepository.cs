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
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Domain.Interfaces.Repositories;

/// <summary>
/// 通用仓储接口
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
/// <typeparam name="TKey">主键类型</typeparam>
public interface ILeanRepository<TEntity, TKey> where TEntity : class
{
  /// <summary>
  /// 创建实体
  /// </summary>
  Task<TKey> CreateAsync(TEntity entity);

  /// <summary>
  /// 批量创建实体
  /// </summary>
  Task<bool> CreateRangeAsync(List<TEntity> entities);

  /// <summary>
  /// 更新实体
  /// </summary>
  Task<bool> UpdateAsync(TEntity entity);

  /// <summary>
  /// 批量更新实体
  /// </summary>
  Task<bool> UpdateRangeAsync(List<TEntity> entities);

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

  /// <summary>
  /// 根据ID获取实体
  /// </summary>
  Task<TEntity?> GetByIdAsync(TKey id);

  /// <summary>
  /// 根据条件获取第一个实体
  /// </summary>
  Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

  /// <summary>
  /// 根据条件获取实体列表
  /// </summary>
  Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate);

  /// <summary>
  /// 根据条件获取分页列表
  /// </summary>
  Task<(long Total, List<TEntity> Items)> GetPageListAsync(Expression<Func<TEntity, bool>> predicate, int pageSize, int pageIndex);

  /// <summary>
  /// 根据条件判断是否存在
  /// </summary>
  Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

  /// <summary>
  /// 根据条件获取数量
  /// </summary>
  Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate);
}

/// <summary>
/// 通用仓储接口（使用 long 作为主键类型）
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
public interface ILeanRepository<TEntity> : ILeanRepository<TEntity, long> where TEntity : class
{
}
