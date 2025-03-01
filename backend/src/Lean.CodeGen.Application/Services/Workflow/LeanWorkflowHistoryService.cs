using Lean.CodeGen.Application.Dtos.Workflow;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Domain.Entities.Workflow;
using Lean.CodeGen.Domain.Interfaces.Repositories;
using System.Linq.Expressions;
using Mapster;
using Microsoft.Extensions.Logging;
using Lean.CodeGen.Application.Services.Base;

namespace Lean.CodeGen.Application.Services.Workflow;

/// <summary>
/// 工作流历史服务
/// </summary>
public class LeanWorkflowHistoryService : LeanBaseService, ILeanWorkflowHistoryService
{
  private readonly ILeanRepository<LeanWorkflowHistory> _repository;
  private readonly ILogger<LeanWorkflowHistoryService> _logger;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanWorkflowHistoryService(
      ILeanRepository<LeanWorkflowHistory> repository,
      LeanBaseServiceContext context)
      : base(context)
  {
    _repository = repository;
    _logger = (ILogger<LeanWorkflowHistoryService>)context.Logger;
  }

  /// <inheritdoc/>
  public async Task<LeanWorkflowHistoryDto> GetAsync(long id)
  {
    var entity = await _repository.GetByIdAsync(id);
    if (entity == null)
    {
      throw new Exception($"工作流历史[{id}]不存在");
    }
    return entity.Adapt<LeanWorkflowHistoryDto>();
  }

  /// <inheritdoc/>
  public async Task<long> CreateAsync(LeanWorkflowHistoryDto dto)
  {
    var entity = dto.Adapt<LeanWorkflowHistory>();
    return await _repository.CreateAsync(entity);
  }

  /// <inheritdoc/>
  public async Task<bool> UpdateAsync(LeanWorkflowHistoryDto dto)
  {
    var entity = dto.Adapt<LeanWorkflowHistory>();
    return await _repository.UpdateAsync(entity);
  }

  /// <inheritdoc/>
  public async Task<bool> DeleteAsync(long id)
  {
    return await _repository.DeleteAsync(x => x.Id == id);
  }

  /// <inheritdoc/>
  public async Task<LeanPageResult<LeanWorkflowHistoryDto>> GetPagedListAsync(
      int pageIndex,
      int pageSize,
      long? instanceId = null,
      long? taskId = null,
      string? operationType = null,
      long? operatorId = null,
      DateTime? startTime = null,
      DateTime? endTime = null)
  {
    Expression<Func<LeanWorkflowHistory, bool>> predicate = x => true;

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

    if (!string.IsNullOrEmpty(operationType))
    {
      var temp = predicate;
      predicate = x => temp.Compile()(x) && x.OperationType.ToString() == operationType;
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
    var list = result.Items.Adapt<List<LeanWorkflowHistoryDto>>();

    return new LeanPageResult<LeanWorkflowHistoryDto>
    {
      Total = result.Total,
      Items = list,
      PageIndex = pageIndex,
      PageSize = pageSize
    };
  }
}