using System.Linq.Expressions;
using Lean.CodeGen.Domain.Entities;
using Lean.CodeGen.Domain.Interfaces.Repositories;

namespace Lean.CodeGen.Domain.Extensions;

/// <summary>
/// Lean仓储扩展方法
/// </summary>
public static class LeanRepositoryExtensions
{
  /// <summary>
  /// 获取第一条记录
  /// </summary>
  public static async Task<T?> GetFirstAsync<T>(this ILeanRepository<T> repository, Expression<Func<T, bool>> predicate)
      where T : LeanBaseEntity
  {
    var list = await repository.GetListAsync(predicate);
    return list.FirstOrDefault();
  }
}