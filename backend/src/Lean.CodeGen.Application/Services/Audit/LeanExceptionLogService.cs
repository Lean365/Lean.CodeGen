//===================================================
// 项目名: Lean.CodeGen.Application
// 文件名: LeanExceptionLogService.cs
// 功能描述: 异常日志服务实现
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mapster;
using Microsoft.Extensions.Options;
using Lean.CodeGen.Application.Dtos.Audit;
using Lean.CodeGen.Common.Excel;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Domain.Entities.Audit;
using Lean.CodeGen.Domain.Interfaces.Repositories;
using Lean.CodeGen.Application.Services.Base;
using Lean.CodeGen.Common.Options;
using Lean.CodeGen.Application.Services.Security;
using Lean.CodeGen.Common.Extensions;
using NLog;

namespace Lean.CodeGen.Application.Services.Audit
{
  /// <summary>
  /// 异常日志服务实现
  /// </summary>
  public class LeanExceptionLogService : LeanBaseService, ILeanExceptionLogService
  {
    private readonly ILeanRepository<LeanExceptionLog> _exceptionLogRepository;
    private readonly ILogger _logger;

    /// <summary>
    /// 构造函数
    /// </summary>
    public LeanExceptionLogService(
        ILeanRepository<LeanExceptionLog> exceptionLogRepository,
        LeanBaseServiceContext context)
        : base(context)
    {
      _exceptionLogRepository = exceptionLogRepository;
      _logger = context.Logger;
    }

    /// <summary>
    /// 获取异常日志列表（分页）
    /// </summary>
    public async Task<LeanPageResult<LeanExceptionLogDto>> GetPageListAsync(LeanExceptionLogQueryDto queryDto)
    {
      var predicate = BuildQueryPredicate(queryDto);
      var (total, items) = await _exceptionLogRepository.GetPageListAsync(predicate, queryDto.PageSize, queryDto.PageIndex);
      var list = items.Select(t => t.Adapt<LeanExceptionLogDto>()).ToList();

      return new LeanPageResult<LeanExceptionLogDto>
      {
        Total = total,
        Items = list
      };
    }

    /// <summary>
    /// 获取异常日志详情
    /// </summary>
    public async Task<LeanExceptionLogDto> GetAsync(long id)
    {
      var exceptionLog = await _exceptionLogRepository.FirstOrDefaultAsync(t => t.Id == id);
      if (exceptionLog == null)
      {
        throw new Exception($"异常日志[{id}]不存在");
      }

      return exceptionLog.Adapt<LeanExceptionLogDto>();
    }

    /// <summary>
    /// 导出异常日志
    /// </summary>
    public async Task<LeanFileResult> ExportAsync(LeanExceptionLogQueryDto queryDto)
    {
      var predicate = BuildQueryPredicate(queryDto);
      var items = await _exceptionLogRepository.GetListAsync(predicate);
      var list = items.Select(t => t.Adapt<LeanExceptionLogExportDto>()).ToList();

      var excelBytes = LeanExcelHelper.Export(list);
      return new LeanFileResult
      {
        FileName = $"异常日志_{DateTime.Now:yyyyMMddHHmmss}.xlsx",
        ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        Stream = new MemoryStream(excelBytes)
      };
    }

    /// <summary>
    /// 清空异常日志
    /// </summary>
    public async Task<bool> ClearAsync()
    {
      return await _exceptionLogRepository.DeleteAsync(t => true);
    }

    /// <summary>
    /// 处理异常日志
    /// </summary>
    public async Task<bool> HandleAsync(LeanExceptionLogHandleDto handleDto)
    {
      var exceptionLog = await _exceptionLogRepository.FirstOrDefaultAsync(t => t.Id == handleDto.Id);
      if (exceptionLog == null)
      {
        throw new Exception($"异常日志[{handleDto.Id}]不存在");
      }

      exceptionLog.HandleStatus = handleDto.HandleStatus;
      exceptionLog.HandleTime = DateTime.Now;
      exceptionLog.HandlerId = handleDto.HandlerId;
      exceptionLog.HandlerName = handleDto.HandlerName;
      exceptionLog.HandleRemark = handleDto.HandleRemark;

      return await _exceptionLogRepository.UpdateAsync(exceptionLog);
    }

    /// <summary>
    /// 构建查询条件
    /// </summary>
    private Expression<Func<LeanExceptionLog, bool>> BuildQueryPredicate(LeanExceptionLogQueryDto queryDto)
    {
      Expression<Func<LeanExceptionLog, bool>> predicate = t => true;

      if (queryDto.UserId.HasValue)
      {
        predicate = predicate.And(t => t.UserId == queryDto.UserId);
      }

      if (!string.IsNullOrEmpty(queryDto.AppName))
      {
        var appName = CleanInput(queryDto.AppName);
        predicate = predicate.And(t => t.AppName.Contains(appName));
      }

      if (!string.IsNullOrEmpty(queryDto.Environment))
      {
        var environment = CleanInput(queryDto.Environment);
        predicate = predicate.And(t => t.Environment.Contains(environment));
      }

      if (!string.IsNullOrEmpty(queryDto.ExceptionType))
      {
        var exceptionType = CleanInput(queryDto.ExceptionType);
        predicate = predicate.And(t => t.ExceptionType.Contains(exceptionType));
      }

      if (queryDto.LogLevel.HasValue)
      {
        predicate = predicate.And(t => t.LogLevel == queryDto.LogLevel);
      }

      if (queryDto.HandleStatus.HasValue)
      {
        predicate = predicate.And(t => t.HandleStatus == queryDto.HandleStatus);
      }

      if (queryDto.CreateTimeBegin.HasValue)
      {
        predicate = predicate.And(t => t.CreateTime >= queryDto.CreateTimeBegin);
      }

      if (queryDto.CreateTimeEnd.HasValue)
      {
        predicate = predicate.And(t => t.CreateTime <= queryDto.CreateTimeEnd);
      }

      return predicate;
    }
  }
}