using Lean.CodeGen.Application.Dtos.Workflow;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Domain.Entities.Workflow;
using Lean.CodeGen.Domain.Interfaces.Repositories;
using System.Linq.Expressions;
using Mapster;

namespace Lean.CodeGen.Application.Services.Workflow;

/// <summary>
/// 工作流结果服务
/// </summary>
public class LeanWorkflowOutcomeService : ILeanWorkflowOutcomeService
{
  private readonly ILeanRepository<LeanWorkflowOutcome> _repository;

  public LeanWorkflowOutcomeService(ILeanRepository<LeanWorkflowOutcome> repository)
  {
    _repository = repository;
  }

  /// <inheritdoc/>
  public async Task<LeanWorkflowOutcomeDto?> GetAsync(long id)
  {
    var entity = await _repository.GetByIdAsync(id);
    return entity?.Adapt<LeanWorkflowOutcomeDto>();
  }

  /// <inheritdoc/>
  public async Task<long> CreateAsync(LeanWorkflowOutcomeDto dto)
  {
    var entity = dto.Adapt<LeanWorkflowOutcome>();
    return await _repository.CreateAsync(entity);
  }

  /// <inheritdoc/>
  public async Task<bool> UpdateAsync(LeanWorkflowOutcomeDto dto)
  {
    var entity = dto.Adapt<LeanWorkflowOutcome>();
    return await _repository.UpdateAsync(entity);
  }

  /// <inheritdoc/>
  public async Task<bool> DeleteAsync(long id)
  {
    return await _repository.DeleteAsync(x => x.Id == id);
  }

  /// <inheritdoc/>
  public async Task<LeanPageResult<LeanWorkflowOutcomeDto>> GetPagedListAsync(
      int pageIndex,
      int pageSize,
      long? activityInstanceId = null,
      string? outcomeName = null,
      string? outcomeType = null)
  {
    Expression<Func<LeanWorkflowOutcome, bool>> predicate = x => true;

    if (activityInstanceId.HasValue)
    {
      var temp = predicate;
      predicate = x => temp.Compile()(x) && x.ActivityInstanceId == activityInstanceId.Value;
    }

    if (!string.IsNullOrEmpty(outcomeName))
    {
      var temp = predicate;
      predicate = x => temp.Compile()(x) && x.OutcomeName.Contains(outcomeName);
    }

    if (!string.IsNullOrEmpty(outcomeType))
    {
      var temp = predicate;
      predicate = x => temp.Compile()(x) && x.OutcomeType == outcomeType;
    }

    var result = await _repository.GetPageListAsync(predicate, pageSize, pageIndex);
    var list = result.Items.Adapt<List<LeanWorkflowOutcomeDto>>();

    return new LeanPageResult<LeanWorkflowOutcomeDto>
    {
      Total = result.Total,
      Items = list,
      PageIndex = pageIndex,
      PageSize = pageSize
    };
  }
}