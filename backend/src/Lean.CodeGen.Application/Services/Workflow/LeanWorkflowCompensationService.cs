using Lean.CodeGen.Application.Dtos.Workflow;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Domain.Entities.Workflow;
using Lean.CodeGen.Domain.Interfaces.Repositories;
using System.Linq.Expressions;
using Mapster;

namespace Lean.CodeGen.Application.Services.Workflow;

/// <summary>
/// 工作流补偿服务
/// </summary>
public class LeanWorkflowCompensationService : ILeanWorkflowCompensationService
{
  private readonly ILeanRepository<LeanWorkflowCompensation> _repository;

  public LeanWorkflowCompensationService(ILeanRepository<LeanWorkflowCompensation> repository)
  {
    _repository = repository;
  }

  /// <inheritdoc/>
  public async Task<LeanWorkflowCompensationDto?> GetAsync(long id)
  {
    var entity = await _repository.GetByIdAsync(id);
    return entity?.Adapt<LeanWorkflowCompensationDto>();
  }

  /// <inheritdoc/>
  public async Task<long> CreateAsync(LeanWorkflowCompensationDto dto)
  {
    var entity = dto.Adapt<LeanWorkflowCompensation>();
    return await _repository.CreateAsync(entity);
  }

  /// <inheritdoc/>
  public async Task<bool> UpdateAsync(LeanWorkflowCompensationDto dto)
  {
    var entity = dto.Adapt<LeanWorkflowCompensation>();
    return await _repository.UpdateAsync(entity);
  }

  /// <inheritdoc/>
  public async Task<bool> DeleteAsync(long id)
  {
    return await _repository.DeleteAsync(x => x.Id == id);
  }

  /// <inheritdoc/>
  public async Task<LeanPageResult<LeanWorkflowCompensationDto>> GetPagedListAsync(
      int pageIndex,
      int pageSize,
      long? activityInstanceId = null,
      DateTime? startTime = null,
      DateTime? endTime = null)
  {
    Expression<Func<LeanWorkflowCompensation, bool>> predicate = x => true;

    if (activityInstanceId.HasValue)
    {
      var temp = predicate;
      predicate = x => temp.Compile()(x) && x.ActivityInstanceId == activityInstanceId.Value;
    }

    if (startTime.HasValue)
    {
      var temp = predicate;
      predicate = x => temp.Compile()(x) && x.CompensationTime >= startTime.Value;
    }

    if (endTime.HasValue)
    {
      var temp = predicate;
      predicate = x => temp.Compile()(x) && x.CompensationTime <= endTime.Value;
    }

    var result = await _repository.GetPageListAsync(predicate, pageSize, pageIndex);
    var list = result.Items.Adapt<List<LeanWorkflowCompensationDto>>();

    return new LeanPageResult<LeanWorkflowCompensationDto>
    {
      Total = result.Total,
      Items = list,
      PageIndex = pageIndex,
      PageSize = pageSize
    };
  }
}