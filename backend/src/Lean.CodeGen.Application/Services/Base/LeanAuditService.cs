using System;
using System.Threading.Tasks;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Exceptions;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Domain.Entities;
using Lean.CodeGen.Domain.Interfaces.Repositories;

namespace Lean.CodeGen.Application.Services.Base;

/// <summary>
/// 审核服务基类
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
public abstract class LeanAuditService<TEntity> : LeanBaseService where TEntity : LeanBaseEntity
{
  protected readonly ILeanRepository<TEntity> Repository;

  protected LeanAuditService(
      ILeanRepository<TEntity> repository,
      LeanBaseServiceContext context)
      : base(context)
  {
    Repository = repository;
  }

  /// <summary>
  /// 提交审核
  /// </summary>
  public virtual async Task<LeanApiResult> SubmitAuditAsync(long id)
  {
    return await ExecuteInTransactionAsync(async () =>
    {
      var entity = await Repository.GetByIdAsync(id);
      if (entity == null)
      {
        throw new LeanException("记录不存在");
      }

      if (entity.AuditStatus != 0)
      {
        throw new LeanException("当前状态不能提交审核");
      }

      entity.AuditStatus = 1;
      await Repository.UpdateAsync(entity);

      return LeanApiResult.Ok();
    }, "提交审核");
  }

  /// <summary>
  /// 审核通过
  /// </summary>
  public virtual async Task<LeanApiResult> ApproveAsync(long id)
  {
    return await ExecuteInTransactionAsync(async () =>
    {
      var entity = await Repository.GetByIdAsync(id);
      if (entity == null)
      {
        throw new LeanException("记录不存在");
      }

      if (entity.AuditStatus != 1)
      {
        throw new LeanException("当前状态不能审核通过");
      }

      entity.AuditStatus = 2;
      entity.AuditUserId = Context.CurrentUserId;
      await Repository.UpdateAsync(entity);

      return LeanApiResult.Ok();
    }, "审核通过");
  }

  /// <summary>
  /// 审核驳回
  /// </summary>
  public virtual async Task<LeanApiResult> RejectAsync(long id, string reason)
  {
    return await ExecuteInTransactionAsync(async () =>
    {
      var entity = await Repository.GetByIdAsync(id);
      if (entity == null)
      {
        throw new LeanException("记录不存在");
      }

      if (entity.AuditStatus != 1)
      {
        throw new LeanException("当前状态不能驳回");
      }

      entity.AuditStatus = 3;
      entity.AuditUserId = Context.CurrentUserId;
      await Repository.UpdateAsync(entity);

      // 记录驳回原因
      LogAudit("Reject", $"驳回原因: {reason}");

      return LeanApiResult.Ok();
    }, "审核驳回");
  }

  /// <summary>
  /// 撤销审核
  /// </summary>
  public virtual async Task<LeanApiResult> CancelAuditAsync(long id)
  {
    return await ExecuteInTransactionAsync(async () =>
    {
      var entity = await Repository.GetByIdAsync(id);
      if (entity == null)
      {
        throw new LeanException("记录不存在");
      }

      if (entity.AuditStatus != 1)
      {
        throw new LeanException("当前状态不能撤销审核");
      }

      entity.AuditStatus = 0;
      await Repository.UpdateAsync(entity);

      return LeanApiResult.Ok();
    }, "撤销审核");
  }
}