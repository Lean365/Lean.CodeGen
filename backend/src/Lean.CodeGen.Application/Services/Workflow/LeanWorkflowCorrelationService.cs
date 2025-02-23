using Lean.CodeGen.Application.Dtos.Workflow;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Domain.Entities.Workflow;
using Lean.CodeGen.Domain.Interfaces.Repositories;
using System.Linq.Expressions;
using Mapster;

namespace Lean.CodeGen.Application.Services.Workflow;

/// <summary>
/// 工作流关联服务
/// </summary>
public class LeanWorkflowCorrelationService : ILeanWorkflowCorrelationService
{
  private readonly ILeanRepository<LeanWorkflowCorrelation> _repository;

  public LeanWorkflowCorrelationService(ILeanRepository<LeanWorkflowCorrelation> repository)
  {
    _repository = repository;
  }

  /// <inheritdoc/>
  public async Task<LeanWorkflowCorrelationDto?> GetAsync(long id)
  {
    var entity = await _repository.GetByIdAsync(id);
    return entity?.Adapt<LeanWorkflowCorrelationDto>();
  }

  /// <inheritdoc/>
  public async Task<LeanWorkflowCorrelationDto?> GetByCorrelationIdAsync(string correlationId)
  {
    var entity = await _repository.FirstOrDefaultAsync(x => x.CorrelationId == correlationId);
    return entity?.Adapt<LeanWorkflowCorrelationDto>();
  }

  /// <inheritdoc/>
  public async Task<long> CreateAsync(LeanWorkflowCorrelationDto dto)
  {
    var entity = dto.Adapt<LeanWorkflowCorrelation>();
    return await _repository.CreateAsync(entity);
  }

  /// <inheritdoc/>
  public async Task<bool> UpdateAsync(LeanWorkflowCorrelationDto dto)
  {
    var entity = dto.Adapt<LeanWorkflowCorrelation>();
    return await _repository.UpdateAsync(entity);
  }

  /// <inheritdoc/>
  public async Task<bool> DeleteAsync(long id)
  {
    return await _repository.DeleteAsync(x => x.Id == id);
  }

  /// <inheritdoc/>
  public async Task<LeanPageResult<LeanWorkflowCorrelationDto>> GetPagedListAsync(
      int pageIndex,
      int pageSize,
      long? instanceId = null,
      string? correlationId = null,
      string? correlationType = null,
      bool? status = null,
      DateTime? startTime = null,
      DateTime? endTime = null)
  {
    Expression<Func<LeanWorkflowCorrelation, bool>> predicate = x => true;

    if (instanceId.HasValue)
    {
      var temp = predicate;
      predicate = x => temp.Compile()(x) && x.InstanceId == instanceId.Value;
    }

    if (!string.IsNullOrEmpty(correlationId))
    {
      var temp = predicate;
      predicate = x => temp.Compile()(x) && x.CorrelationId.Contains(correlationId);
    }

    if (!string.IsNullOrEmpty(correlationType))
    {
      var temp = predicate;
      predicate = x => temp.Compile()(x) && x.CorrelationType == correlationType;
    }

    if (status.HasValue)
    {
      var temp = predicate;
      predicate = x => temp.Compile()(x) && x.Status == status.Value;
    }

    if (startTime.HasValue)
    {
      var temp = predicate;
      predicate = x => temp.Compile()(x) && x.CreateTime >= startTime.Value;
    }

    if (endTime.HasValue)
    {
      var temp = predicate;
      predicate = x => temp.Compile()(x) && x.CreateTime <= endTime.Value;
    }

    var result = await _repository.GetPageListAsync(predicate, pageSize, pageIndex);
    var list = result.Items.Adapt<List<LeanWorkflowCorrelationDto>>();

    return new LeanPageResult<LeanWorkflowCorrelationDto>
    {
      Total = result.Total,
      Items = list,
      PageIndex = pageIndex,
      PageSize = pageSize
    };
  }
}