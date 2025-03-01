using Lean.CodeGen.Application.Dtos.Workflow;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Domain.Entities.Workflow;
using Lean.CodeGen.Domain.Interfaces.Repositories;
using System.Linq.Expressions;
using Mapster;
using NLog;
using Lean.CodeGen.Application.Services.Base;

namespace Lean.CodeGen.Application.Services.Workflow;

/// <summary>
/// 工作流活动实例服务
/// </summary>
public class LeanWorkflowActivityInstanceService : LeanBaseService, ILeanWorkflowActivityInstanceService
{
    private readonly ILeanRepository<LeanWorkflowActivityInstance> _repository;
    private readonly ILogger _logger;

    public LeanWorkflowActivityInstanceService(
        ILeanRepository<LeanWorkflowActivityInstance> repository,
        LeanBaseServiceContext context)
        : base(context)
    {
        _repository = repository;
        _logger = context.Logger;
    }

    /// <inheritdoc/>
    public async Task<LeanWorkflowActivityInstanceDto?> GetAsync(long id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity?.Adapt<LeanWorkflowActivityInstanceDto>();
    }

    /// <inheritdoc/>
    public async Task<long> CreateAsync(LeanWorkflowActivityInstanceDto dto)
    {
        var entity = dto.Adapt<LeanWorkflowActivityInstance>();
        return await _repository.CreateAsync(entity);
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateAsync(LeanWorkflowActivityInstanceDto dto)
    {
        var entity = dto.Adapt<LeanWorkflowActivityInstance>();
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
            throw new Exception($"活动实例[{id}]不存在");
        }

        entity.StartTime = DateTime.Now;
        entity.ActivityStatus = LeanWorkflowActivityStatus.Running;
        return await _repository.UpdateAsync(entity);
    }

    /// <inheritdoc/>
    public async Task<bool> CompleteAsync(long id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new Exception($"活动实例[{id}]不存在");
        }

        entity.EndTime = DateTime.Now;
        entity.ActivityStatus = LeanWorkflowActivityStatus.Completed;
        return await _repository.UpdateAsync(entity);
    }

    /// <inheritdoc/>
    public async Task<bool> CancelAsync(long id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new Exception($"活动实例[{id}]不存在");
        }

        entity.EndTime = DateTime.Now;
        entity.ActivityStatus = LeanWorkflowActivityStatus.Cancelled;
        return await _repository.UpdateAsync(entity);
    }

    /// <inheritdoc/>
    public async Task<bool> CompensateAsync(long id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new Exception($"活动实例[{id}]不存在");
        }

        entity.EndTime = DateTime.Now;
        entity.ActivityStatus = LeanWorkflowActivityStatus.Compensated;
        return await _repository.UpdateAsync(entity);
    }

    /// <inheritdoc/>
    public async Task<LeanPageResult<LeanWorkflowActivityInstanceDto>> GetPagedListAsync(
        int pageIndex,
        int pageSize,
        long? workflowInstanceId = null,
        string? activityType = null,
        int? activityStatus = null,
        DateTime? startTime = null,
        DateTime? endTime = null)
    {
        Expression<Func<LeanWorkflowActivityInstance, bool>> predicate = x => true;

        if (workflowInstanceId.HasValue)
        {
            var temp = predicate;
            predicate = x => temp.Compile()(x) && x.WorkflowInstanceId == workflowInstanceId.Value;
        }

        if (!string.IsNullOrEmpty(activityType))
        {
            var temp = predicate;
            predicate = x => temp.Compile()(x) && x.ActivityType == activityType;
        }

        if (activityStatus.HasValue)
        {
            var temp = predicate;
            predicate = x => temp.Compile()(x) && (int)x.ActivityStatus == activityStatus.Value;
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
        var list = result.Items.Adapt<List<LeanWorkflowActivityInstanceDto>>();

        return new LeanPageResult<LeanWorkflowActivityInstanceDto>
        {
            Total = result.Total,
            Items = list,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }
}