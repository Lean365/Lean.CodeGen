using Lean.CodeGen.Application.Dtos.Workflow;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Domain.Entities.Workflow;
using Lean.CodeGen.Domain.Interfaces.Repositories;
using System.Linq.Expressions;
using Mapster;

namespace Lean.CodeGen.Application.Services.Workflow;

/// <summary>
/// 工作流变量数据服务
/// </summary>
public class LeanWorkflowVariableDataService : ILeanWorkflowVariableDataService
{
  private readonly ILeanRepository<LeanWorkflowVariableData> _repository;

  public LeanWorkflowVariableDataService(ILeanRepository<LeanWorkflowVariableData> repository)
  {
    _repository = repository;
  }

  /// <inheritdoc/>
  public async Task<LeanWorkflowVariableDataDto?> GetAsync(long id)
  {
    var entity = await _repository.GetByIdAsync(id);
    return entity?.Adapt<LeanWorkflowVariableDataDto>();
  }

  /// <inheritdoc/>
  public async Task<LeanWorkflowVariableDataDto?> GetByNameAsync(long instanceId, string variableName)
  {
    var entity = await _repository.FirstOrDefaultAsync(x => x.InstanceId == instanceId && x.VariableName == variableName);
    return entity?.Adapt<LeanWorkflowVariableDataDto>();
  }

  /// <inheritdoc/>
  public async Task<long> CreateAsync(LeanWorkflowVariableDataDto dto)
  {
    var entity = dto.Adapt<LeanWorkflowVariableData>();
    return await _repository.CreateAsync(entity);
  }

  /// <inheritdoc/>
  public async Task<bool> UpdateAsync(LeanWorkflowVariableDataDto dto)
  {
    var entity = dto.Adapt<LeanWorkflowVariableData>();
    return await _repository.UpdateAsync(entity);
  }

  /// <inheritdoc/>
  public async Task<bool> DeleteAsync(long id)
  {
    return await _repository.DeleteAsync(x => x.Id == id);
  }

  /// <inheritdoc/>
  public async Task<LeanPageResult<LeanWorkflowVariableDataDto>> GetPagedListAsync(
      int pageIndex,
      int pageSize,
      long? instanceId = null,
      long? taskId = null,
      long? variableId = null,
      string? variableName = null,
      string? variableType = null,
      long? operatorId = null,
      DateTime? startTime = null,
      DateTime? endTime = null)
  {
    Expression<Func<LeanWorkflowVariableData, bool>> predicate = x => true;

    if (instanceId.HasValue)
    {
      var temp = predicate;
      predicate = x => temp.Compile()(x) && x.InstanceId == instanceId.Value;
    }

    if (taskId.HasValue)
    {
      var temp = predicate;
      predicate = x => temp.Compile()(x) && x.TaskId == taskId.Value;
    }

    if (variableId.HasValue)
    {
      var temp = predicate;
      predicate = x => temp.Compile()(x) && x.VariableId == variableId.Value;
    }

    if (!string.IsNullOrEmpty(variableName))
    {
      var temp = predicate;
      predicate = x => temp.Compile()(x) && x.VariableName.Contains(variableName);
    }

    if (!string.IsNullOrEmpty(variableType))
    {
      var temp = predicate;
      predicate = x => temp.Compile()(x) && x.VariableType == variableType;
    }

    if (operatorId.HasValue)
    {
      var temp = predicate;
      predicate = x => temp.Compile()(x) && x.OperatorId == operatorId.Value;
    }

    if (startTime.HasValue)
    {
      var temp = predicate;
      predicate = x => temp.Compile()(x) && x.OperationTime >= startTime.Value;
    }

    if (endTime.HasValue)
    {
      var temp = predicate;
      predicate = x => temp.Compile()(x) && x.OperationTime <= endTime.Value;
    }

    var result = await _repository.GetPageListAsync(predicate, pageSize, pageIndex);
    var list = result.Items.Adapt<List<LeanWorkflowVariableDataDto>>();

    return new LeanPageResult<LeanWorkflowVariableDataDto>
    {
      Total = result.Total,
      Items = list,
      PageIndex = pageIndex,
      PageSize = pageSize
    };
  }
}