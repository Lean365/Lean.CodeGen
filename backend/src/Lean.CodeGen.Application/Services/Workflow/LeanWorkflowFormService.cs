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
/// 工作流表单服务
/// </summary>
public class LeanWorkflowFormService : LeanBaseService, ILeanWorkflowFormService
{
    private readonly ILeanRepository<LeanWorkflowFormDefinition> _formDefinitionRepository;
    private readonly ILeanRepository<LeanWorkflowFormField> _formFieldRepository;
    private readonly ILeanRepository<LeanWorkflowFormData> _formDataRepository;
    private readonly ILogger _logger;

    /// <summary>
    /// 构造函数
    /// </summary>
    public LeanWorkflowFormService(
        ILeanRepository<LeanWorkflowFormDefinition> formDefinitionRepository,
        ILeanRepository<LeanWorkflowFormField> formFieldRepository,
        ILeanRepository<LeanWorkflowFormData> formDataRepository,
        LeanBaseServiceContext context)
        : base(context)
    {
        _formDefinitionRepository = formDefinitionRepository;
        _formFieldRepository = formFieldRepository;
        _formDataRepository = formDataRepository;
        _logger = context.Logger;
    }

    /// <inheritdoc/>
    public async Task<LeanWorkflowFormDefinitionDto?> GetFormDefinitionAsync(long id)
    {
        var entity = await _formDefinitionRepository.GetByIdAsync(id);
        return entity?.Adapt<LeanWorkflowFormDefinitionDto>();
    }

    /// <inheritdoc/>
    public async Task<LeanWorkflowFormDefinitionDto?> GetFormDefinitionByCodeAsync(string formCode)
    {
        var entity = await _formDefinitionRepository.FirstOrDefaultAsync(x => x.FormCode == formCode);
        return entity?.Adapt<LeanWorkflowFormDefinitionDto>();
    }

    /// <inheritdoc/>
    public async Task<long> CreateFormDefinitionAsync(LeanWorkflowFormDefinitionDto dto)
    {
        // 检查表单编码是否已存在
        var exists = await _formDefinitionRepository.AnyAsync(x => x.FormCode == dto.FormCode);
        if (exists)
        {
            throw new Exception($"表单编码[{dto.FormCode}]已存在");
        }

        var entity = dto.Adapt<LeanWorkflowFormDefinition>();
        return await _formDefinitionRepository.CreateAsync(entity);
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateFormDefinitionAsync(LeanWorkflowFormDefinitionDto dto)
    {
        // 检查表单编码是否已存在
        var exists = await _formDefinitionRepository.AnyAsync(x => x.FormCode == dto.FormCode && x.Id != dto.Id);
        if (exists)
        {
            throw new Exception($"表单编码[{dto.FormCode}]已存在");
        }

        var entity = dto.Adapt<LeanWorkflowFormDefinition>();
        return await _formDefinitionRepository.UpdateAsync(entity);
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteFormDefinitionAsync(long id)
    {
        return await _formDefinitionRepository.DeleteAsync(x => x.Id == id);
    }

    /// <inheritdoc/>
    public async Task<LeanWorkflowFormFieldDto?> GetFormFieldAsync(long id)
    {
        var entity = await _formFieldRepository.GetByIdAsync(id);
        return entity?.Adapt<LeanWorkflowFormFieldDto>();
    }

    /// <inheritdoc/>
    public async Task<LeanWorkflowFormFieldDto?> GetFormFieldByCodeAsync(long formId, string fieldCode)
    {
        var entity = await _formFieldRepository.FirstOrDefaultAsync(x => x.FormId == formId && x.FieldCode == fieldCode);
        return entity?.Adapt<LeanWorkflowFormFieldDto>();
    }

    /// <inheritdoc/>
    public async Task<long> CreateFormFieldAsync(LeanWorkflowFormFieldDto dto)
    {
        // 检查字段编码是否已存在
        var exists = await _formFieldRepository.AnyAsync(x => x.FormId == dto.FormId && x.FieldCode == dto.FieldCode);
        if (exists)
        {
            throw new Exception($"字段编码[{dto.FieldCode}]已存在");
        }

        var entity = dto.Adapt<LeanWorkflowFormField>();
        return await _formFieldRepository.CreateAsync(entity);
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateFormFieldAsync(LeanWorkflowFormFieldDto dto)
    {
        // 检查字段编码是否已存在
        var exists = await _formFieldRepository.AnyAsync(x => x.FormId == dto.FormId && x.FieldCode == dto.FieldCode && x.Id != dto.Id);
        if (exists)
        {
            throw new Exception($"字段编码[{dto.FieldCode}]已存在");
        }

        var entity = dto.Adapt<LeanWorkflowFormField>();
        return await _formFieldRepository.UpdateAsync(entity);
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteFormFieldAsync(long id)
    {
        return await _formFieldRepository.DeleteAsync(x => x.Id == id);
    }

    /// <inheritdoc/>
    public async Task<LeanWorkflowFormDataDto?> GetFormDataAsync(long id)
    {
        var entity = await _formDataRepository.GetByIdAsync(id);
        return entity?.Adapt<LeanWorkflowFormDataDto>();
    }

    /// <inheritdoc/>
    public async Task<long> CreateFormDataAsync(LeanWorkflowFormDataDto dto)
    {
        var entity = dto.Adapt<LeanWorkflowFormData>();
        return await _formDataRepository.CreateAsync(entity);
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateFormDataAsync(LeanWorkflowFormDataDto dto)
    {
        var entity = dto.Adapt<LeanWorkflowFormData>();
        return await _formDataRepository.UpdateAsync(entity);
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteFormDataAsync(long id)
    {
        return await _formDataRepository.DeleteAsync(x => x.Id == id);
    }

    /// <inheritdoc/>
    public async Task<LeanPageResult<LeanWorkflowFormDefinitionDto>> GetFormDefinitionPagedListAsync(
        int pageIndex,
        int pageSize,
        string? formName = null,
        string? formCode = null,
        string? formType = null,
        int? status = null)
    {
        Expression<Func<LeanWorkflowFormDefinition, bool>> predicate = x => true;

        if (!string.IsNullOrEmpty(formName))
        {
            var temp = predicate;
            predicate = x => temp.Compile()(x) && x.FormName.Contains(formName);
        }

        if (!string.IsNullOrEmpty(formCode))
        {
            var temp = predicate;
            predicate = x => temp.Compile()(x) && x.FormCode.Contains(formCode);
        }

        if (!string.IsNullOrEmpty(formType))
        {
            var temp = predicate;
            predicate = x => temp.Compile()(x) && x.FormType == formType;
        }

        if (status.HasValue)
        {
            var temp = predicate;
            predicate = x => temp.Compile()(x) && x.Status == status.Value;
        }

        var result = await _formDefinitionRepository.GetPageListAsync(predicate, pageSize, pageIndex);
        var list = result.Items.Adapt<List<LeanWorkflowFormDefinitionDto>>();

        return new LeanPageResult<LeanWorkflowFormDefinitionDto>
        {
            Total = result.Total,
            Items = list,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }

    /// <inheritdoc/>
    public async Task<LeanPageResult<LeanWorkflowFormFieldDto>> GetFormFieldPagedListAsync(
        int pageIndex,
        int pageSize,
        long? formId = null,
        string? fieldName = null,
        string? fieldCode = null,
        string? fieldType = null,
        int? status = null)
    {
        Expression<Func<LeanWorkflowFormField, bool>> predicate = x => true;

        if (formId.HasValue)
        {
            var temp = predicate;
            predicate = x => temp.Compile()(x) && x.FormId == formId.Value;
        }

        if (!string.IsNullOrEmpty(fieldName))
        {
            var temp = predicate;
            predicate = x => temp.Compile()(x) && x.FieldName.Contains(fieldName);
        }

        if (!string.IsNullOrEmpty(fieldCode))
        {
            var temp = predicate;
            predicate = x => temp.Compile()(x) && x.FieldCode.Contains(fieldCode);
        }

        if (!string.IsNullOrEmpty(fieldType))
        {
            var temp = predicate;
            predicate = x => temp.Compile()(x) && x.FieldType == fieldType;
        }

        if (status.HasValue)
        {
            var temp = predicate;
            predicate = x => temp.Compile()(x) && x.Status == status.Value;
        }

        var result = await _formFieldRepository.GetPageListAsync(predicate, pageSize, pageIndex);
        var list = result.Items.Adapt<List<LeanWorkflowFormFieldDto>>();

        return new LeanPageResult<LeanWorkflowFormFieldDto>
        {
            Total = result.Total,
            Items = list,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }

    /// <inheritdoc/>
    public async Task<LeanPageResult<LeanWorkflowFormDataDto>> GetFormDataPagedListAsync(
        int pageIndex,
        int pageSize,
        long? instanceId = null,
        long? taskId = null,
        long? formId = null,
        string? fieldCode = null,
        long? operatorId = null,
        DateTime? startTime = null,
        DateTime? endTime = null)
    {
        Expression<Func<LeanWorkflowFormData, bool>> predicate = x => true;

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

        if (formId.HasValue)
        {
            var temp = predicate;
            predicate = x => temp.Compile()(x) && x.FormId == formId.Value;
        }

        if (!string.IsNullOrEmpty(fieldCode))
        {
            var temp = predicate;
            predicate = x => temp.Compile()(x) && x.FieldCode == fieldCode;
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

        var result = await _formDataRepository.GetPageListAsync(predicate, pageSize, pageIndex);
        var list = result.Items.Adapt<List<LeanWorkflowFormDataDto>>();

        return new LeanPageResult<LeanWorkflowFormDataDto>
        {
            Total = result.Total,
            Items = list,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }
}