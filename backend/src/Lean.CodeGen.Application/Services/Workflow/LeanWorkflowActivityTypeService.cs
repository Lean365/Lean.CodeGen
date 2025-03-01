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
/// 工作流活动类型服务
/// </summary>
public class LeanWorkflowActivityTypeService : LeanBaseService, ILeanWorkflowActivityTypeService
{
    private readonly ILeanRepository<LeanWorkflowActivityType> _repository;
    private readonly ILogger _logger;

    public LeanWorkflowActivityTypeService(
        ILeanRepository<LeanWorkflowActivityType> repository,
        LeanBaseServiceContext context)
        : base(context)
    {
        _repository = repository;
        _logger = context.Logger;
    }

    /// <inheritdoc/>
    public async Task<List<LeanWorkflowActivityTypeDto>> GetListAsync()
    {
        var list = await _repository.GetListAsync(x => true);
        return list.Adapt<List<LeanWorkflowActivityTypeDto>>();
    }

    /// <inheritdoc/>
    public async Task<LeanWorkflowActivityTypeDto?> GetAsync(string typeName)
    {
        var entity = await _repository.FirstOrDefaultAsync(x => x.TypeName == typeName);
        return entity?.Adapt<LeanWorkflowActivityTypeDto>();
    }

    /// <inheritdoc/>
    public async Task<bool> CreateAsync(LeanWorkflowActivityTypeDto dto)
    {
        // 检查活动类型名称是否已存在
        var exists = await _repository.AnyAsync(x => x.TypeName == dto.TypeName);
        if (exists)
        {
            throw new Exception($"活动类型[{dto.TypeName}]已存在");
        }

        var entity = dto.Adapt<LeanWorkflowActivityType>();
        var id = await _repository.CreateAsync(entity);
        return id > 0;
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateAsync(LeanWorkflowActivityTypeDto dto)
    {
        var entity = dto.Adapt<LeanWorkflowActivityType>();
        return await _repository.UpdateAsync(entity);
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteAsync(string typeName)
    {
        return await _repository.DeleteAsync(x => x.TypeName == typeName);
    }

    /// <inheritdoc/>
    public async Task<LeanPageResult<LeanWorkflowActivityTypeDto>> GetPagedListAsync(
        int pageIndex,
        int pageSize,
        string? typeName = null,
        string? category = null,
        bool? isBlocking = null,
        bool? isTrigger = null,
        bool? isContainer = null,
        bool? isSystem = null,
        bool? status = null)
    {
        Expression<Func<LeanWorkflowActivityType, bool>> predicate = x => true;

        if (!string.IsNullOrEmpty(typeName))
        {
            var temp = predicate;
            predicate = x => temp.Compile()(x) && x.TypeName.Contains(typeName);
        }

        if (!string.IsNullOrEmpty(category))
        {
            var temp = predicate;
            predicate = x => temp.Compile()(x) && x.Category == category;
        }

        if (isBlocking.HasValue)
        {
            var temp = predicate;
            predicate = x => temp.Compile()(x) && x.IsBlocking == isBlocking.Value;
        }

        if (isTrigger.HasValue)
        {
            var temp = predicate;
            predicate = x => temp.Compile()(x) && x.IsTrigger == isTrigger.Value;
        }

        if (isContainer.HasValue)
        {
            var temp = predicate;
            predicate = x => temp.Compile()(x) && x.IsContainer == isContainer.Value;
        }

        if (isSystem.HasValue)
        {
            var temp = predicate;
            predicate = x => temp.Compile()(x) && x.IsSystem == isSystem.Value;
        }

        if (status.HasValue)
        {
            var temp = predicate;
            predicate = x => temp.Compile()(x) && x.Status == status.Value;
        }

        var result = await _repository.GetPageListAsync(predicate, pageSize, pageIndex);
        var list = result.Items.Adapt<List<LeanWorkflowActivityTypeDto>>();

        return new LeanPageResult<LeanWorkflowActivityTypeDto>
        {
            Total = result.Total,
            Items = list,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }
}