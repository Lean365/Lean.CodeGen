using Lean.CodeGen.Application.Dtos.Workflow;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Domain.Entities.Workflow;
using Lean.CodeGen.Domain.Interfaces.Repositories;
using System.Linq.Expressions;
using Mapster;

namespace Lean.CodeGen.Application.Services.Workflow;

/// <summary>
/// 工作流定义服务
/// </summary>
public class LeanWorkflowDefinitionService : ILeanWorkflowDefinitionService
{
  private readonly ILeanRepository<LeanWorkflowDefinition> _repository;

  public LeanWorkflowDefinitionService(ILeanRepository<LeanWorkflowDefinition> repository)
  {
    _repository = repository;
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
    // 检查编码是否已存在
    var exists = await _repository.AnyAsync(x => x.WorkflowCode == dto.WorkflowCode);
    if (exists)
    {
      throw new Exception($"工作流编码[{dto.WorkflowCode}]已存在");
    }

    var entity = dto.Adapt<LeanWorkflowDefinition>();
    return await _repository.CreateAsync(entity);
  }

  /// <inheritdoc/>
  public async Task<bool> UpdateAsync(LeanWorkflowDefinitionDto dto)
  {
    // 检查编码是否已存在
    var exists = await _repository.AnyAsync(x => x.WorkflowCode == dto.WorkflowCode && x.Id != dto.Id);
    if (exists)
    {
      throw new Exception($"工作流编码[{dto.WorkflowCode}]已存在");
    }

    var entity = dto.Adapt<LeanWorkflowDefinition>();
    return await _repository.UpdateAsync(entity);
  }

  /// <inheritdoc/>
  public async Task<bool> DeleteAsync(long id)
  {
    return await _repository.DeleteAsync(x => x.Id == id);
  }

  /// <inheritdoc/>
  public async Task<bool> PublishAsync(long id)
  {
    var entity = await _repository.GetByIdAsync(id);
    if (entity == null)
    {
      throw new Exception($"工作流定义[{id}]不存在");
    }

    entity.IsPublished = 1;
    entity.WorkflowStatus = Common.Enums.LeanWorkflowStatus.Published;
    return await _repository.UpdateAsync(entity);
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
  public async Task<LeanPageResult<LeanWorkflowDefinitionDto>> GetPagedListAsync(int pageIndex, int pageSize, string? workflowName = null, string? workflowCode = null, int? status = null)
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