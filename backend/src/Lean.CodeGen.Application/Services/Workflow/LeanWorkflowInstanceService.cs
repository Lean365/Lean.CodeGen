using Lean.CodeGen.Application.Dtos.Workflow;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Domain.Entities.Workflow;
using Lean.CodeGen.Domain.Interfaces.Repositories;
using System.Linq.Expressions;
using Mapster;

namespace Lean.CodeGen.Application.Services.Workflow;

/// <summary>
/// 工作流实例服务
/// </summary>
public class LeanWorkflowInstanceService : ILeanWorkflowInstanceService
{
  private readonly ILeanRepository<LeanWorkflowInstance> _repository;

  public LeanWorkflowInstanceService(ILeanRepository<LeanWorkflowInstance> repository)
  {
    _repository = repository;
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
  public async Task<bool> UpdateAsync(LeanWorkflowInstanceDto dto)
  {
    // 检查业务主键是否已存在
    var exists = await _repository.AnyAsync(x => x.BusinessKey == dto.BusinessKey && x.Id != dto.Id);
    if (exists)
    {
      throw new Exception($"业务主键[{dto.BusinessKey}]已存在");
    }

    var entity = dto.Adapt<LeanWorkflowInstance>();
    return await _repository.UpdateAsync(entity);
  }

  /// <inheritdoc/>
  public async Task<bool> DeleteAsync(long id)
  {
    return await _repository.DeleteAsync(x => x.Id == id);
  }

  /// <inheritdoc/>
  public async Task<bool> StartAsync(long id)
  {
    var entity = await _repository.GetByIdAsync(id);
    if (entity == null)
    {
      throw new Exception($"工作流实例[{id}]不存在");
    }

    entity.StartTime = DateTime.Now;
    entity.WorkflowStatus = LeanWorkflowInstanceStatus.Running;
    return await _repository.UpdateAsync(entity);
  }

  /// <inheritdoc/>
  public async Task<bool> SuspendAsync(long id)
  {
    var entity = await _repository.GetByIdAsync(id);
    if (entity == null)
    {
      throw new Exception($"工作流实例[{id}]不存在");
    }

    entity.IsSuspended = 1;
    entity.WorkflowStatus = LeanWorkflowInstanceStatus.Suspended;
    return await _repository.UpdateAsync(entity);
  }

  /// <inheritdoc/>
  public async Task<bool> ResumeAsync(long id)
  {
    var entity = await _repository.GetByIdAsync(id);
    if (entity == null)
    {
      throw new Exception($"工作流实例[{id}]不存在");
    }

    entity.IsSuspended = 0;
    entity.WorkflowStatus = LeanWorkflowInstanceStatus.Running;
    return await _repository.UpdateAsync(entity);
  }

  /// <inheritdoc/>
  public async Task<bool> TerminateAsync(long id)
  {
    var entity = await _repository.GetByIdAsync(id);
    if (entity == null)
    {
      throw new Exception($"工作流实例[{id}]不存在");
    }

    entity.EndTime = DateTime.Now;
    entity.WorkflowStatus = LeanWorkflowInstanceStatus.Cancelled;
    return await _repository.UpdateAsync(entity);
  }

  /// <inheritdoc/>
  public async Task<bool> ArchiveAsync(long id)
  {
    var entity = await _repository.GetByIdAsync(id);
    if (entity == null)
    {
      throw new Exception($"工作流实例[{id}]不存在");
    }

    entity.IsArchived = 1;
    return await _repository.UpdateAsync(entity);
  }

  /// <inheritdoc/>
  public async Task<LeanPageResult<LeanWorkflowInstanceDto>> GetPagedListAsync(int pageIndex, int pageSize, long? definitionId = null, string? businessKey = null, string? businessType = null, string? title = null, long? initiatorId = null, LeanWorkflowInstanceStatus? workflowStatus = null)
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