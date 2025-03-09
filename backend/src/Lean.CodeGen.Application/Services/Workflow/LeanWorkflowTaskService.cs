using Lean.CodeGen.Application.Dtos.Workflow;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Domain.Entities.Workflow;
using Lean.CodeGen.Domain.Interfaces.Repositories;
using System.Linq.Expressions;
using Mapster;
using NLog;
using Lean.CodeGen.Application.Services.Base;

namespace Lean.CodeGen.Application.Services.Workflow;

/// <summary>
/// 工作流任务服务
/// </summary>
public class LeanWorkflowTaskService : LeanBaseService, ILeanWorkflowTaskService
{
  private readonly ILeanRepository<LeanWorkflowTask> _repository;
  private readonly ILogger _logger;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanWorkflowTaskService(
      ILeanRepository<LeanWorkflowTask> repository,
      LeanBaseServiceContext context)
      : base(context)
  {
    _repository = repository;
    _logger = context.Logger;
  }

  /// <inheritdoc/>
  public async Task<LeanWorkflowTaskDto> GetAsync(long id)
  {
    var entity = await _repository.GetByIdAsync(id);
    if (entity == null)
    {
      throw new Exception($"工作流任务[{id}]不存在");
    }
    return entity.Adapt<LeanWorkflowTaskDto>();
  }

  /// <inheritdoc/>
  public async Task<long> CreateAsync(LeanWorkflowTaskDto dto)
  {
    var entity = dto.Adapt<LeanWorkflowTask>();
    return await _repository.CreateAsync(entity);
  }

  /// <inheritdoc/>
  public async Task<bool> UpdateAsync(LeanWorkflowTaskDto dto)
  {
    var entity = dto.Adapt<LeanWorkflowTask>();
    return await _repository.UpdateAsync(entity);
  }

  /// <inheritdoc/>
  public async Task<bool> DeleteAsync(long id)
  {
    return await _repository.DeleteAsync(x => x.Id == id);
  }

  /// <inheritdoc/>
  public async Task<bool> CompleteAsync(long id, string? comment = null)
  {
    var entity = await _repository.GetByIdAsync(id);
    if (entity == null)
    {
      throw new Exception($"工作流任务[{id}]不存在");
    }

    entity.EndTime = DateTime.Now;
    entity.TaskStatus = 2;
    entity.Comment = comment;
    return await _repository.UpdateAsync(entity);
  }

  /// <inheritdoc/>
  public async Task<bool> RejectAsync(long id, string? comment = null)
  {
    var entity = await _repository.GetByIdAsync(id);
    if (entity == null)
    {
      throw new Exception($"工作流任务[{id}]不存在");
    }

    entity.EndTime = DateTime.Now;
    entity.TaskStatus = 4;
    entity.Comment = comment;
    return await _repository.UpdateAsync(entity);
  }

  /// <inheritdoc/>
  public async Task<bool> TransferAsync(long id, long assigneeId, string? comment = null)
  {
    var entity = await _repository.GetByIdAsync(id);
    if (entity == null)
    {
      throw new Exception($"工作流任务[{id}]不存在");
    }

    // 保存原处理人信息
    entity.OriginalAssigneeId = entity.AssigneeId;
    entity.OriginalAssigneeName = entity.AssigneeName;

    // 更新新处理人信息
    entity.AssigneeId = assigneeId;
    entity.Comment = comment;
    entity.TaskStatus = 6;
    return await _repository.UpdateAsync(entity);
  }

  /// <inheritdoc/>
  public async Task<bool> DelegateAsync(long id, long assigneeId, string? comment = null)
  {
    var entity = await _repository.GetByIdAsync(id);
    if (entity == null)
    {
      throw new Exception($"工作流任务[{id}]不存在");
    }

    // 保存原处理人信息
    entity.OriginalAssigneeId = entity.AssigneeId;
    entity.OriginalAssigneeName = entity.AssigneeName;

    // 更新新处理人信息
    entity.AssigneeId = assigneeId;
    entity.Comment = comment;
    return await _repository.UpdateAsync(entity);
  }

  /// <inheritdoc/>
  public async Task<LeanPageResult<LeanWorkflowTaskDto>> GetPagedListAsync(
      int pageIndex,
      int pageSize,
      long? instanceId = null,
      string? taskType = null,
      string? taskNode = null,
      int? priority = null,
      long? assigneeId = null,
      int? taskStatus = null,
      DateTime? startTime = null,
      DateTime? endTime = null)
  {
    Expression<Func<LeanWorkflowTask, bool>> predicate = x => true;

    if (instanceId.HasValue)
    {
      var temp = predicate;
      predicate = x => temp.Compile()(x) && x.InstanceId == instanceId.Value;
    }

    if (!string.IsNullOrEmpty(taskType))
    {
      var temp = predicate;
      predicate = x => temp.Compile()(x) && x.TaskType.ToString() == taskType;
    }

    if (!string.IsNullOrEmpty(taskNode))
    {
      var temp = predicate;
      predicate = x => temp.Compile()(x) && x.TaskNode == taskNode;
    }

    if (priority.HasValue)
    {
      var temp = predicate;
      predicate = x => temp.Compile()(x) && x.Priority == priority.Value;
    }

    if (assigneeId.HasValue)
    {
      var temp = predicate;
      predicate = x => temp.Compile()(x) && x.AssigneeId == assigneeId.Value;
    }

    if (taskStatus.HasValue)
    {
      var temp = predicate;
      predicate = x => temp.Compile()(x) && (int)x.TaskStatus == taskStatus.Value;
    }

    if (startTime.HasValue)
    {
      var temp = predicate;
      predicate = x => temp.Compile()(x) && x.StartTime >= startTime.Value;
    }

    if (endTime.HasValue)
    {
      var temp = predicate;
      predicate = x => temp.Compile()(x) && x.EndTime <= endTime.Value;
    }

    var result = await _repository.GetPageListAsync(predicate, pageSize, pageIndex);
    var list = result.Items.Adapt<List<LeanWorkflowTaskDto>>();

    return new LeanPageResult<LeanWorkflowTaskDto>
    {
      Total = result.Total,
      Items = list,
      PageIndex = pageIndex,
      PageSize = pageSize
    };
  }
}