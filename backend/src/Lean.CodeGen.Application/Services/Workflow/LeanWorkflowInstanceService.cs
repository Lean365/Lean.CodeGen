using Lean.CodeGen.Application.Dtos.Workflow;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Domain.Entities.Workflow;
using Lean.CodeGen.Domain.Interfaces.Repositories;
using System.Linq.Expressions;
using Mapster;
using NLog;
using Lean.CodeGen.Application.Services.Base;

namespace Lean.CodeGen.Application.Services.Workflow;

/// <summary>
/// 工作流实例服务
/// </summary>
public class LeanWorkflowInstanceService : LeanBaseService, ILeanWorkflowInstanceService
{
  private readonly ILeanRepository<LeanWorkflowInstance> _repository;
  private readonly ILogger _logger;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanWorkflowInstanceService(
      ILeanRepository<LeanWorkflowInstance> repository,
      LeanBaseServiceContext context)
      : base(context)
  {
    _repository = repository;
    _logger = context.Logger;
  }

  /// <inheritdoc/>
  public async Task<LeanWorkflowInstanceDto?> GetAsync(long id)
  {
    var entity = await _repository.GetByIdAsync(id);
    return entity?.Adapt<LeanWorkflowInstanceDto>();
  }

  /// <inheritdoc/>
  public async Task<LeanWorkflowInstanceDto?> GetByBusinessKeyAsync(string businessKey)
  {
    var entity = await _repository.FirstOrDefaultAsync(x => x.BusinessKey == businessKey);
    return entity?.Adapt<LeanWorkflowInstanceDto>();
  }

  /// <inheritdoc/>
  public async Task<long> CreateAsync(LeanWorkflowInstanceDto dto)
  {
    // 检查业务主键是否已存在
    var exists = await _repository.AnyAsync(x => x.BusinessKey == dto.BusinessKey);
    if (exists)
    {
      throw new Exception($"业务主键[{dto.BusinessKey}]已存在");
    }

    var entity = dto.Adapt<LeanWorkflowInstance>();
    return await _repository.CreateAsync(entity);
  }

  /// <inheritdoc/>
  public async Task<LeanApiResult> UpdateAsync(LeanWorkflowInstanceDto dto)
  {
    var entity = await _repository.GetByIdAsync(dto.Id);
    if (entity == null)
    {
      return LeanApiResult.Error("工作流实例不存在");
    }

    dto.Adapt(entity);
    await _repository.UpdateAsync(entity);
    return LeanApiResult.Ok();
  }

  /// <inheritdoc/>
  public async Task<LeanApiResult> DeleteAsync(long id)
  {
    var entity = await _repository.GetByIdAsync(id);
    if (entity == null)
    {
      return LeanApiResult.Error("工作流实例不存在");
    }

    await _repository.DeleteAsync(entity);
    return LeanApiResult.Ok();
  }

  /// <inheritdoc/>
  public async Task<LeanApiResult> StartAsync(long id)
  {
    var entity = await _repository.GetByIdAsync(id);
    if (entity == null)
    {
      return LeanApiResult.Error("流程实例不存在");
    }
    entity.WorkflowStatus = 1; // Running
    entity.StartTime = DateTime.Now;
    await _repository.UpdateAsync(entity);
    return LeanApiResult.Ok();
  }

  /// <inheritdoc/>
  public async Task<LeanApiResult> SuspendAsync(long id)
  {
    var entity = await _repository.GetByIdAsync(id);
    if (entity == null)
    {
      return LeanApiResult.Error("工作流实例不存在");
    }

    entity.WorkflowStatus = 2; // Suspended
    await _repository.UpdateAsync(entity);
    return LeanApiResult.Ok();
  }

  /// <inheritdoc/>
  public async Task<LeanApiResult> ResumeAsync(long id)
  {
    var entity = await _repository.GetByIdAsync(id);
    if (entity == null)
    {
      return LeanApiResult.Error("工作流实例不存在");
    }

    entity.WorkflowStatus = 1; // Running
    await _repository.UpdateAsync(entity);
    return LeanApiResult.Ok();
  }

  /// <inheritdoc/>
  public async Task<LeanApiResult> TerminateAsync(long id)
  {
    var entity = await _repository.GetByIdAsync(id);
    if (entity == null)
    {
      return LeanApiResult.Error("工作流实例不存在");
    }

    entity.WorkflowStatus = 3; // Terminated
    await _repository.UpdateAsync(entity);
    return LeanApiResult.Ok();
  }

  /// <inheritdoc/>
  public async Task<LeanApiResult> ArchiveAsync(long id)
  {
    var entity = await _repository.GetByIdAsync(id);
    if (entity == null)
    {
      return LeanApiResult.Error($"工作流实例[{id}]不存在");
    }

    entity.IsArchived = 1;
    await _repository.UpdateAsync(entity);
    return LeanApiResult.Ok();
  }

  /// <inheritdoc/>
  public async Task<LeanPageResult<LeanWorkflowInstanceDto>> GetPagedListAsync(
      int pageIndex,
      int pageSize,
      long? definitionId = null,
      string? businessKey = null,
      string? businessType = null,
      string? title = null,
      long? initiatorId = null,
      int? workflowStatus = null)
  {
    Expression<Func<LeanWorkflowInstance, bool>> predicate = x => true;

    if (definitionId.HasValue)
    {
      var temp = predicate;
      predicate = x => temp.Compile()(x) && x.DefinitionId == definitionId.Value;
    }

    if (!string.IsNullOrEmpty(businessKey))
    {
      var temp = predicate;
      predicate = x => temp.Compile()(x) && x.BusinessKey.Contains(businessKey);
    }

    if (!string.IsNullOrEmpty(businessType))
    {
      var temp = predicate;
      predicate = x => temp.Compile()(x) && x.BusinessType.Contains(businessType);
    }

    if (!string.IsNullOrEmpty(title))
    {
      var temp = predicate;
      predicate = x => temp.Compile()(x) && x.Title.Contains(title);
    }

    if (initiatorId.HasValue)
    {
      var temp = predicate;
      predicate = x => temp.Compile()(x) && x.InitiatorId == initiatorId.Value;
    }

    if (workflowStatus.HasValue)
    {
      var temp = predicate;
      predicate = x => temp.Compile()(x) && x.WorkflowStatus == workflowStatus.Value;
    }

    var result = await _repository.GetPageListAsync(predicate, pageSize, pageIndex);
    var list = result.Items.Adapt<List<LeanWorkflowInstanceDto>>();

    return new LeanPageResult<LeanWorkflowInstanceDto>
    {
      Total = result.Total,
      Items = list,
      PageIndex = pageIndex,
      PageSize = pageSize
    };
  }
}