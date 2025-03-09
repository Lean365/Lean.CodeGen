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
/// 工作流定义服务
/// </summary>
public class LeanWorkflowDefinitionService : LeanBaseService, ILeanWorkflowDefinitionService
{
  private readonly ILeanRepository<LeanWorkflowDefinition> _repository;
  private readonly ILogger _logger;

  public LeanWorkflowDefinitionService(
      ILeanRepository<LeanWorkflowDefinition> repository,
      LeanBaseServiceContext context)
      : base(context)
  {
    _repository = repository;
    _logger = context.Logger;
  }

  /// <inheritdoc/>
  public async Task<List<LeanWorkflowDefinitionDto>> GetListAsync()
  {
    var list = await _repository.GetListAsync(x => true);
    return list.Adapt<List<LeanWorkflowDefinitionDto>>();
  }

  /// <inheritdoc/>
  public async Task<LeanWorkflowDefinitionDto?> GetAsync(long id)
  {
    var entity = await _repository.GetByIdAsync(id);
    return entity?.Adapt<LeanWorkflowDefinitionDto>();
  }

  /// <inheritdoc/>
  public async Task<LeanWorkflowDefinitionDto?> GetByCodeAsync(string code)
  {
    var entity = await _repository.FirstOrDefaultAsync(x => x.WorkflowCode == code);
    return entity?.Adapt<LeanWorkflowDefinitionDto>();
  }

  /// <inheritdoc/>
  public async Task<long> CreateAsync(LeanWorkflowDefinitionDto dto)
  {
    var entity = dto.Adapt<LeanWorkflowDefinition>();
    return await _repository.CreateAsync(entity);
  }

  /// <inheritdoc/>
  public async Task<LeanApiResult> UpdateAsync(LeanWorkflowDefinitionDto dto)
  {
    var entity = await _repository.GetByIdAsync(dto.Id);
    if (entity == null)
    {
      return LeanApiResult.Error("工作流定义不存在");
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
      return LeanApiResult.Error("工作流定义不存在");
    }

    await _repository.DeleteAsync(entity);
    return LeanApiResult.Ok();
  }

  /// <inheritdoc/>
  public async Task<LeanApiResult> PublishAsync(long id)
  {
    var entity = await _repository.GetByIdAsync(id);
    if (entity == null)
    {
      return LeanApiResult.Error("工作流定义不存在");
    }

    entity.Status = 1; // Enabled
    entity.IsPublished = 1;
    await _repository.UpdateAsync(entity);
    return LeanApiResult.Ok();
  }

  /// <inheritdoc/>
  public async Task<bool> DisableAsync(long id)
  {
    var entity = await _repository.GetByIdAsync(id);
    if (entity == null)
    {
      throw new Exception($"工作流定义[{id}]不存在");
    }

    entity.Status = 0;
    return await _repository.UpdateAsync(entity);
  }

  /// <inheritdoc/>
  public async Task<LeanPageResult<LeanWorkflowDefinitionDto>> GetPagedListAsync(
      int pageIndex,
      int pageSize,
      string? workflowName = null,
      string? workflowCode = null,
      int? status = null)
  {
    Expression<Func<LeanWorkflowDefinition, bool>> predicate = x => true;

    if (!string.IsNullOrEmpty(workflowName))
    {
      var temp = predicate;
      predicate = x => temp.Compile()(x) && x.WorkflowName.Contains(workflowName);
    }

    if (!string.IsNullOrEmpty(workflowCode))
    {
      var temp = predicate;
      predicate = x => temp.Compile()(x) && x.WorkflowCode.Contains(workflowCode);
    }

    if (status.HasValue)
    {
      var temp = predicate;
      predicate = x => temp.Compile()(x) && x.Status == status.Value;
    }

    var result = await _repository.GetPageListAsync(predicate, pageSize, pageIndex);
    var list = result.Items.Adapt<List<LeanWorkflowDefinitionDto>>();

    return new LeanPageResult<LeanWorkflowDefinitionDto>
    {
      Total = result.Total,
      Items = list,
      PageIndex = pageIndex,
      PageSize = pageSize
    };
  }
}