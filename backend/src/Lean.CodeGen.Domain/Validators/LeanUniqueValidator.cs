using System.Linq.Expressions;
using Lean.CodeGen.Domain.Entities;
using Lean.CodeGen.Domain.Interfaces.Repositories;

namespace Lean.CodeGen.Domain.Validators;

/// <summary>
/// 唯一性校验器
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
public class LeanUniqueValidator<TEntity> where TEntity : LeanBaseEntity
{
  private readonly ILeanRepository<TEntity> _repository;

  public LeanUniqueValidator(ILeanRepository<TEntity> repository)
  {
    _repository = repository;
  }

  /// <summary>
  /// 校验属性值是否唯一
  /// </summary>
  /// <param name="propertyExpression">属性表达式</param>
  /// <param name="value">属性值</param>
  /// <param name="id">排除的ID</param>
  /// <param name="errorMessage">错误消息</param>
  /// <returns>校验结果</returns>
  public async Task ValidateAsync<TProperty>(
      Expression<Func<TEntity, TProperty>> propertyExpression,
      TProperty value,
      long? id = null,
      string? errorMessage = null)
  {
    if (value == null)
    {
      return;
    }

    // 获取属性名
    var propertyName = ((MemberExpression)propertyExpression.Body).Member.Name;

    // 构建查询条件
    var parameter = Expression.Parameter(typeof(TEntity), "x");
    var property = Expression.Property(parameter, propertyName);
    var constant = Expression.Constant(value);
    var equal = Expression.Equal(property, constant);

    Expression<Func<TEntity, bool>> predicate;
    if (id.HasValue)
    {
      var idProperty = Expression.Property(parameter, "Id");
      var idConstant = Expression.Constant(id.Value);
      var notEqual = Expression.NotEqual(idProperty, idConstant);
      var and = Expression.AndAlso(equal, notEqual);
      predicate = Expression.Lambda<Func<TEntity, bool>>(and, parameter);
    }
    else
    {
      predicate = Expression.Lambda<Func<TEntity, bool>>(equal, parameter);
    }

    // 检查是否存在
    if (await _repository.AnyAsync(predicate))
    {
      throw new Exception(errorMessage ?? $"{propertyName} {value} 已存在");
    }
  }

  /// <summary>
  /// 批量校验属性值是否唯一
  /// </summary>
  /// <param name="validations">校验项</param>
  public async Task ValidateAsync(params (Expression<Func<TEntity, object>> PropertyExpression, object Value, long? Id, string? ErrorMessage)[] validations)
  {
    foreach (var validation in validations)
    {
      await ValidateAsync(validation.PropertyExpression, validation.Value, validation.Id, validation.ErrorMessage);
    }
  }
}