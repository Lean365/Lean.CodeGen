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
/// 工作流输出服务
/// </summary>
public class LeanWorkflowOutputService : LeanBaseService, ILeanWorkflowOutputService
{
    private readonly ILeanRepository<LeanWorkflowOutput> _repository;
    private readonly ILogger _logger;

    /// <summary>
    /// 构造函数
    /// </summary>
    public LeanWorkflowOutputService(
        ILeanRepository<LeanWorkflowOutput> repository,
        LeanBaseServiceContext context)
        : base(context)
    {
        _repository = repository;
        _logger = context.Logger;
    }

    /// <inheritdoc/>
    public async Task<LeanWorkflowOutputDto?> GetAsync(long id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity?.Adapt<LeanWorkflowOutputDto>();
    }

    /// <inheritdoc/>
    public async Task<long> CreateAsync(LeanWorkflowOutputDto dto)
    {
        var entity = dto.Adapt<LeanWorkflowOutput>();
        return await _repository.CreateAsync(entity);
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateAsync(LeanWorkflowOutputDto dto)
    {
        var entity = dto.Adapt<LeanWorkflowOutput>();
        return await _repository.UpdateAsync(entity);
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteAsync(long id)
    {
        return await _repository.DeleteAsync(x => x.Id == id);
    }

    /// <inheritdoc/>
    public async Task<LeanPageResult<LeanWorkflowOutputDto>> GetPagedListAsync(
        int pageIndex,
        int pageSize,
        long? activityInstanceId = null,
        string? outputName = null,
        string? outputType = null)
    {
        Expression<Func<LeanWorkflowOutput, bool>> predicate = x => true;

        if (activityInstanceId.HasValue)
        {
            var temp = predicate;
            predicate = x => temp.Compile()(x) && x.ActivityInstanceId == activityInstanceId.Value;
        }

        if (!string.IsNullOrEmpty(outputName))
        {
            var temp = predicate;
            predicate = x => temp.Compile()(x) && x.OutputName.Contains(outputName);
        }

        if (!string.IsNullOrEmpty(outputType))
        {
            var temp = predicate;
            predicate = x => temp.Compile()(x) && x.OutputType == outputType;
        }

        var result = await _repository.GetPageListAsync(predicate, pageSize, pageIndex);
        var list = result.Items.Adapt<List<LeanWorkflowOutputDto>>();

        return new LeanPageResult<LeanWorkflowOutputDto>
        {
            Total = result.Total,
            Items = list,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }
}