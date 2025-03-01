using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mapster;
using Microsoft.Extensions.Options;
using Lean.CodeGen.Application.Dtos.Generator;
using Lean.CodeGen.Common.Excel;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Options;
using Lean.CodeGen.Domain.Entities.Generator;
using Lean.CodeGen.Domain.Interfaces.Repositories;
using Lean.CodeGen.Application.Services.Base;
using Lean.CodeGen.Application.Services.Security;
using Lean.CodeGen.Common.Extensions;
using Microsoft.Extensions.Logging;
using Lean.CodeGen.Domain.Validators;

namespace Lean.CodeGen.Application.Services.Generator
{
  /// <summary>
  /// 代码生成任务服务实现
  /// </summary>
  public class LeanGenTaskService : LeanBaseService, ILeanGenTaskService
  {
    private readonly ILogger<LeanGenTaskService> _logger;
    private readonly ILeanRepository<LeanGenTask> _repository;
    private readonly ILeanRepository<LeanGenConfig> _configRepository;
    private readonly ILeanRepository<LeanDbTable> _tableRepository;
    private readonly ILeanRepository<LeanGenHistory> _historyRepository;
    private readonly LeanUniqueValidator<LeanGenTask> _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    public LeanGenTaskService(
        ILeanRepository<LeanGenTask> repository,
        ILeanRepository<LeanGenConfig> configRepository,
        ILeanRepository<LeanDbTable> tableRepository,
        ILeanRepository<LeanGenHistory> historyRepository,
        LeanBaseServiceContext context)
        : base(context)
    {
      _repository = repository;
      _configRepository = configRepository;
      _tableRepository = tableRepository;
      _historyRepository = historyRepository;
      _logger = (ILogger<LeanGenTaskService>)context.Logger;
      _uniqueValidator = new LeanUniqueValidator<LeanGenTask>(_repository);
    }

    /// <summary>
    /// 获取代码生成任务列表（分页）
    /// </summary>
    public async Task<LeanPageResult<LeanGenTaskDto>> GetPageListAsync(LeanGenTaskQueryDto queryDto)
    {
      var predicate = BuildQueryPredicate(queryDto);
      var (total, items) = await _repository.GetPageListAsync(predicate, queryDto.PageSize, queryDto.PageIndex);
      var list = items.Select(t => t.Adapt<LeanGenTaskDto>()).ToList();

      return new LeanPageResult<LeanGenTaskDto>
      {
        Total = total,
        Items = list
      };
    }

    /// <summary>
    /// 获取代码生成任务详情
    /// </summary>
    public async Task<LeanGenTaskDto> GetAsync(long id)
    {
      var task = await _repository.GetByIdAsync(id);
      if (task == null)
      {
        throw new Exception($"任务 {id} 不存在");
      }

      return task.Adapt<LeanGenTaskDto>();
    }

    /// <summary>
    /// 创建代码生成任务
    /// </summary>
    public async Task<LeanGenTaskDto> CreateAsync(LeanCreateGenTaskDto createDto)
    {
      var entity = createDto.Adapt<LeanGenTask>();
      entity.Status = 0; // 等待执行
      entity.CreateTime = DateTime.Now;

      var id = await _repository.CreateAsync(entity);
      return await GetAsync(id);
    }

    /// <summary>
    /// 更新代码生成任务
    /// </summary>
    public async Task<LeanGenTaskDto> UpdateAsync(long id, LeanUpdateGenTaskDto updateDto)
    {
      var entity = await _repository.GetByIdAsync(id);
      if (entity == null)
      {
        throw new Exception($"任务 {id} 不存在");
      }

      updateDto.Adapt(entity);
      entity.UpdateTime = DateTime.Now;

      await _repository.UpdateAsync(entity);
      return await GetAsync(id);
    }

    /// <summary>
    /// 删除代码生成任务
    /// </summary>
    public async Task<bool> DeleteAsync(long id)
    {
      var task = await _repository.GetByIdAsync(id);
      if (task == null)
      {
        throw new Exception($"任务 {id} 不存在");
      }

      return await _repository.DeleteAsync(t => t.Id == id);
    }

    /// <summary>
    /// 导出代码生成任务
    /// </summary>
    public async Task<LeanFileResult> ExportAsync(LeanGenTaskQueryDto queryDto)
    {
      var predicate = BuildQueryPredicate(queryDto);
      var items = await _repository.GetListAsync(predicate);
      var list = items.Select(t => t.Adapt<LeanGenTaskExportDto>()).ToList();

      var excelBytes = LeanExcelHelper.Export(list);
      return new LeanFileResult
      {
        FileName = $"代码生成任务_{DateTime.Now:yyyyMMddHHmmss}.xlsx",
        ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        Stream = new MemoryStream(excelBytes)
      };
    }

    /// <summary>
    /// 导入代码生成任务
    /// </summary>
    public async Task<LeanExcelImportResult<LeanGenTaskImportDto>> ImportAsync(LeanFileInfo file)
    {
      var result = new LeanExcelImportResult<LeanGenTaskImportDto>();

      try
      {
        var importResult = LeanExcelHelper.Import<LeanGenTaskImportDto>(File.ReadAllBytes(file.FilePath));
        foreach (var item in importResult.Data)
        {
          try
          {
            var entity = item.Adapt<LeanGenTask>();
            entity.Status = 0; // 等待执行
            entity.CreateTime = DateTime.Now;

            await _repository.CreateAsync(entity);
            result.Data.Add(item);
          }
          catch (Exception ex)
          {
            result.Errors.Add(new LeanExcelImportError
            {
              RowIndex = importResult.Data.IndexOf(item) + 2,
              ErrorMessage = ex.Message
            });
          }
        }
      }
      catch (Exception ex)
      {
        result.ErrorMessage = ex.Message;
      }

      return result;
    }

    /// <summary>
    /// 下载导入模板
    /// </summary>
    public Task<LeanFileResult> DownloadTemplateAsync()
    {
      var template = new List<LeanGenTaskImportTemplateDto>
      {
        new LeanGenTaskImportTemplateDto()
      };

      var excelBytes = LeanExcelHelper.Export(template);
      var result = new LeanFileResult
      {
        FileName = "代码生成任务导入模板.xlsx",
        ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        Stream = new MemoryStream(excelBytes)
      };

      return Task.FromResult(result);
    }

    /// <summary>
    /// 启动任务
    /// </summary>
    public async Task<bool> StartAsync(long id)
    {
      var task = await _repository.GetByIdAsync(id);
      if (task == null)
      {
        throw new Exception($"任务 {id} 不存在");
      }

      if (task.Status != 0)
      {
        throw new Exception("只有等待执行的任务才能启动");
      }

      task.Status = 1; // 执行中
      task.StartTime = DateTime.Now;
      task.UpdateTime = DateTime.Now;

      return await _repository.UpdateAsync(task);
    }

    /// <summary>
    /// 停止任务
    /// </summary>
    public async Task<bool> StopAsync(long id)
    {
      var task = await _repository.GetByIdAsync(id);
      if (task == null)
      {
        throw new Exception($"任务 {id} 不存在");
      }

      if (task.Status != 1)
      {
        throw new Exception("只有执行中的任务才能停止");
      }

      task.Status = 3; // 执行失败
      task.EndTime = DateTime.Now;
      task.UpdateTime = DateTime.Now;
      task.ErrorMessage = "任务被手动停止";

      return await _repository.UpdateAsync(task);
    }

    /// <summary>
    /// 重试任务
    /// </summary>
    public async Task<bool> RetryAsync(long id)
    {
      var task = await _repository.GetByIdAsync(id);
      if (task == null)
      {
        throw new Exception($"任务 {id} 不存在");
      }

      if (task.Status != 3)
      {
        throw new Exception("只有执行失败的任务才能重试");
      }

      task.Status = 0; // 等待执行
      task.StartTime = null;
      task.EndTime = null;
      task.ErrorMessage = null;
      task.UpdateTime = DateTime.Now;

      return await _repository.UpdateAsync(task);
    }

    /// <summary>
    /// 预览代码
    /// </summary>
    public Task<LeanGenPreviewResultDto> PreviewAsync(LeanGenPreviewRequestDto requestDto)
    {
      // TODO: 实现代码预览逻辑
      throw new NotImplementedException();
    }

    /// <summary>
    /// 下载代码
    /// </summary>
    public Task<LeanGenDownloadResultDto> DownloadAsync(LeanGenDownloadRequestDto requestDto)
    {
      // TODO: 实现代码下载逻辑
      throw new NotImplementedException();
    }

    /// <summary>
    /// 构建查询条件
    /// </summary>
    private Expression<Func<LeanGenTask, bool>> BuildQueryPredicate(LeanGenTaskQueryDto queryDto)
    {
      Expression<Func<LeanGenTask, bool>> predicate = t => true;

      if (!string.IsNullOrEmpty(queryDto.Name))
      {
        var name = CleanInput(queryDto.Name);
        predicate = LeanExpressionExtensions.And(predicate, t => t.Name.Contains(name));
      }

      if (queryDto.Status.HasValue)
      {
        predicate = LeanExpressionExtensions.And(predicate, t => t.Status == queryDto.Status);
      }

      if (queryDto.ConfigId.HasValue)
      {
        predicate = LeanExpressionExtensions.And(predicate, t => t.ConfigId == queryDto.ConfigId.Value);
      }

      if (queryDto.StartTimeBegin.HasValue)
      {
        predicate = LeanExpressionExtensions.And(predicate, t => t.StartTime >= queryDto.StartTimeBegin);
      }

      if (queryDto.StartTimeEnd.HasValue)
      {
        predicate = LeanExpressionExtensions.And(predicate, t => t.StartTime <= queryDto.StartTimeEnd);
      }

      if (queryDto.CreateTimeBegin.HasValue)
      {
        predicate = LeanExpressionExtensions.And(predicate, t => t.CreateTime >= queryDto.CreateTimeBegin);
      }

      if (queryDto.CreateTimeEnd.HasValue)
      {
        predicate = LeanExpressionExtensions.And(predicate, t => t.CreateTime <= queryDto.CreateTimeEnd);
      }

      return predicate;
    }
  }
}