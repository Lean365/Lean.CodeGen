using Lean.CodeGen.Application.Dtos.Workflow;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Domain.Entities.Workflow;
using Lean.CodeGen.Domain.Interfaces.Repositories;
using System.Linq.Expressions;
using Mapster;
using Microsoft.Extensions.Logging;
using Lean.CodeGen.Application.Services.Base;

namespace Lean.CodeGen.Application.Services.Workflow;

/// <summary>
/// 工作流变量服务
/// </summary>
public class LeanWorkflowVariableService : LeanBaseService, ILeanWorkflowVariableService
{
  private readonly ILeanRepository<LeanWorkflowVariable> _repository;
  private readonly ILogger<LeanWorkflowVariableService> _logger;

  public LeanWorkflowVariableService(
      ILeanRepository<LeanWorkflowVariable> repository,
      LeanBaseServiceContext context)
      : base(context)
  {
    _repository = repository;
    _logger = (ILogger<LeanWorkflowVariableService>)context.Logger;
  }

  /// <inheritdoc/>
  public async Task<LeanWorkflowVariableDto?> GetAsync(long id)
  {
    var entity = await _repository.GetByIdAsync(id);
    return entity?.Adapt<LeanWorkflowVariableDto>();
  }

  /// <inheritdoc/>
  public async Task<LeanWorkflowVariableDto?> GetByNameAsync(long definitionId, string variableName)
  {
    var entity = await _repository.FirstOrDefaultAsync(x => x.DefinitionId == definitionId && x.VariableName == variableName);
    return entity?.Adapt<LeanWorkflowVariableDto>();
  }

  /// <inheritdoc/>
  public async Task<long> CreateAsync(LeanWorkflowVariableDto dto)
  {
    // 检查变量名称是否已存在
    var exists = await _repository.AnyAsync(x => x.DefinitionId == dto.DefinitionId && x.VariableName == dto.VariableName);
    if (exists)
    {
      throw new Exception($"变量[{dto.VariableName}]已存在");
    }

    var entity = dto.Adapt<LeanWorkflowVariable>();
    return await _repository.CreateAsync(entity);
  }

  /// <inheritdoc/>
  public async Task<bool> UpdateAsync(LeanWorkflowVariableDto dto)
  {
    // 检查变量名称是否已存在
    var exists = await _repository.AnyAsync(x => x.Id != dto.Id && x.DefinitionId == dto.DefinitionId && x.VariableName == dto.VariableName);
    if (exists)
    {
      throw new Exception($"变量[{dto.VariableName}]已存在");
    }

    var entity = dto.Adapt<LeanWorkflowVariable>();
    return await _repository.UpdateAsync(entity);
  }

  /// <inheritdoc/>
  public async Task<bool> DeleteAsync(long id)
  {
    return await _repository.DeleteAsync(x => x.Id == id);
  }

  /// <inheritdoc/>
  public async Task<LeanPageResult<LeanWorkflowVariableDto>> GetPagedListAsync(
      int pageIndex,
      int pageSize,
      long? definitionId = null,
      string? variableName = null,
      string? variableType = null,
      bool? isRequired = null,
      bool? isReadonly = null,
      bool? status = null)
  {
    Expression<Func<LeanWorkflowVariable, bool>> predicate = x => true;

    if (definitionId.HasValue)
    {
      var temp = predicate;
      predicate = x => temp.Compile()(x) && x.DefinitionId == definitionId.Value;
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

    if (isRequired.HasValue)
    {
      var temp = predicate;
      predicate = x => temp.Compile()(x) && x.IsRequired == (isRequired.Value ? 1 : 0);
    }

    if (isReadonly.HasValue)
    {
      var temp = predicate;
      predicate = x => temp.Compile()(x) && x.IsReadonly == (isReadonly.Value ? 1 : 0);
    }

    if (status.HasValue)
    {
      var temp = predicate;
      predicate = x => temp.Compile()(x) && x.Status == (status.Value ? 1 : 0);
    }

    var result = await _repository.GetPageListAsync(predicate, pageSize, pageIndex);
    var list = result.Items.Adapt<List<LeanWorkflowVariableDto>>();

    return new LeanPageResult<LeanWorkflowVariableDto>
    {
      Total = result.Total,
      Items = list,
      PageIndex = pageIndex,
      PageSize = pageSize
    };
  }
}