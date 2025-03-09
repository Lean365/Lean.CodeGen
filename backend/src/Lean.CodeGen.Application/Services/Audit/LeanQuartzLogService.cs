//===================================================
// 项目名: Lean.CodeGen.Application
// 文件名: LeanQuartzLogService.cs
// 功能描述: 定时任务日志服务实现
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
using NLog;
using Lean.CodeGen.Application.Dtos.Audit;
using Lean.CodeGen.Common.Excel;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Domain.Entities.Audit;
using Lean.CodeGen.Domain.Interfaces.Repositories;
using Lean.CodeGen.Application.Services.Base;
using Lean.CodeGen.Common.Options;
using Lean.CodeGen.Application.Services.Security;
using Lean.CodeGen.Common.Extensions;

namespace Lean.CodeGen.Application.Services.Audit
{
  /// <summary>
  /// 定时任务日志服务实现
  /// </summary>
  public class LeanQuartzLogService : LeanBaseService, ILeanQuartzLogService
  {
    private readonly ILeanRepository<LeanQuartzLog> _quartzLogRepository;
    private readonly ILogger _logger;

    /// <summary>
    /// 构造函数
    /// </summary>
    public LeanQuartzLogService(
        ILeanRepository<LeanQuartzLog> quartzLogRepository,
        LeanBaseServiceContext context)
        : base(context)
    {
      _quartzLogRepository = quartzLogRepository;
      _logger = context.Logger;
    }

    /// <summary>
    /// 获取定时任务日志列表（分页）
    /// </summary>
    public async Task<LeanPageResult<LeanQuartzLogDto>> GetPageListAsync(LeanQuartzLogQueryDto queryDto)
    {
      var predicate = BuildQueryPredicate(queryDto);
      var (total, items) = await _quartzLogRepository.GetPageListAsync(predicate, queryDto.PageSize, queryDto.PageIndex);
      var list = items.Select(t => t.Adapt<LeanQuartzLogDto>()).ToList();

      return new LeanPageResult<LeanQuartzLogDto>
      {
        Total = total,
        Items = list
      };
    }

    /// <summary>
    /// 获取定时任务日志详情
    /// </summary>
    public async Task<LeanQuartzLogDto> GetAsync(long id)
    {
      var log = await _quartzLogRepository.FirstOrDefaultAsync(t => t.Id == id);
      if (log == null)
      {
        throw new Exception($"定时任务日志 {id} 不存在");
      }

      return log.Adapt<LeanQuartzLogDto>();
    }

    /// <summary>
    /// 导出定时任务日志
    /// </summary>
    public async Task<LeanFileResult> ExportAsync(LeanQuartzLogQueryDto queryDto)
    {
      var predicate = BuildQueryPredicate(queryDto);
      var items = await _quartzLogRepository.GetListAsync(predicate);
      var list = items.Select(t => t.Adapt<LeanQuartzLogExportDto>()).ToList();

      var excelBytes = LeanExcelHelper.Export(list);
      return new LeanFileResult
      {
        FileName = $"定时任务日志_{DateTime.Now:yyyyMMddHHmmss}.xlsx",
        ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        Stream = new MemoryStream(excelBytes)
      };
    }

    /// <summary>
    /// 清空定时任务日志
    /// </summary>
    public async Task<bool> ClearAsync()
    {
      return await _quartzLogRepository.DeleteAsync(t => true);
    }

    /// <summary>
    /// 构建查询条件
    /// </summary>
    private Expression<Func<LeanQuartzLog, bool>> BuildQueryPredicate(LeanQuartzLogQueryDto queryDto)
    {
      Expression<Func<LeanQuartzLog, bool>> predicate = t => true;

      if (queryDto.TaskId.HasValue)
      {
        predicate = predicate.And(t => t.TaskId == queryDto.TaskId.Value);
      }

      if (!string.IsNullOrEmpty(queryDto.TaskName))
      {
        predicate = predicate.And(t => t.TaskName.Contains(queryDto.TaskName));
      }

      if (!string.IsNullOrEmpty(queryDto.GroupName))
      {
        predicate = predicate.And(t => t.GroupName.Contains(queryDto.GroupName));
      }

      if (queryDto.RunResult.HasValue)
      {
        predicate = predicate.And(t => t.RunResult == queryDto.RunResult.Value);
      }

      if (queryDto.StartTimeBegin.HasValue)
      {
        predicate = predicate.And(t => t.StartTime >= queryDto.StartTimeBegin.Value);
      }

      if (queryDto.StartTimeEnd.HasValue)
      {
        predicate = predicate.And(t => t.StartTime <= queryDto.StartTimeEnd.Value);
      }

      return predicate;
    }
  }
}