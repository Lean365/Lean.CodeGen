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

namespace Lean.CodeGen.Application.Services.Audit;

/// <summary>
/// SQL差异日志服务实现
/// </summary>
public class LeanSqlDiffLogService : LeanBaseService, ILeanSqlDiffLogService
{
  private readonly ILeanRepository<LeanSqlDiffLog> _sqlDiffLogRepository;
  private readonly ILogger _logger;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanSqlDiffLogService(
      ILeanRepository<LeanSqlDiffLog> sqlDiffLogRepository,
      LeanBaseServiceContext context)
      : base(context)
  {
    _sqlDiffLogRepository = sqlDiffLogRepository;
    _logger = context.Logger;
  }

  /// <summary>
  /// 获取SQL差异日志列表（分页）
  /// </summary>
  public async Task<LeanPageResult<LeanSqlDiffLogDto>> GetPageListAsync(LeanSqlDiffLogQueryDto queryDto)
  {
    var predicate = BuildQueryPredicate(queryDto);
    var (total, items) = await _sqlDiffLogRepository.GetPageListAsync(predicate, queryDto.PageSize, queryDto.PageIndex);
    var list = items.Select(t => t.Adapt<LeanSqlDiffLogDto>()).ToList();

    return new LeanPageResult<LeanSqlDiffLogDto>
    {
      Total = total,
      Items = list
    };
  }

  /// <summary>
  /// 获取SQL差异日志详情
  /// </summary>
  public async Task<LeanSqlDiffLogDto> GetAsync(long id)
  {
    var log = await _sqlDiffLogRepository.FirstOrDefaultAsync(t => t.Id == id);
    if (log == null)
    {
      throw new Exception($"SQL差异日志 {id} 不存在");
    }

    return log.Adapt<LeanSqlDiffLogDto>();
  }

  /// <summary>
  /// 导出SQL差异日志
  /// </summary>
  public async Task<LeanFileResult> ExportAsync(LeanSqlDiffLogQueryDto queryDto)
  {
    var predicate = BuildQueryPredicate(queryDto);
    var items = await _sqlDiffLogRepository.GetListAsync(predicate);
    var list = items.Select(t => t.Adapt<LeanSqlDiffLogExportDto>()).ToList();

    var excelBytes = LeanExcelHelper.Export(list);
    return new LeanFileResult
    {
      FileName = $"SQL差异日志_{DateTime.Now:yyyyMMddHHmmss}.xlsx",
      ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
      Stream = new MemoryStream(excelBytes)
    };
  }

  /// <summary>
  /// 清空SQL差异日志
  /// </summary>
  public async Task<bool> ClearAsync()
  {
    return await _sqlDiffLogRepository.DeleteAsync(t => true);
  }

  /// <summary>
  /// 构建查询条件
  /// </summary>
  private Expression<Func<LeanSqlDiffLog, bool>> BuildQueryPredicate(LeanSqlDiffLogQueryDto queryDto)
  {
    Expression<Func<LeanSqlDiffLog, bool>> predicate = t => true;

    if (queryDto.AuditLogId.HasValue)
    {
      predicate = predicate.And(t => t.AuditLogId == queryDto.AuditLogId.Value);
    }

    if (!string.IsNullOrEmpty(queryDto.TableName))
    {
      predicate = predicate.And(t => t.TableName.Contains(queryDto.TableName));
    }

    if (queryDto.DiffType.HasValue)
    {
      predicate = predicate.And(t => t.DiffType == queryDto.DiffType.Value);
    }

    if (queryDto.CreateTimeBegin.HasValue)
    {
      predicate = predicate.And(t => t.CreateTime >= queryDto.CreateTimeBegin.Value);
    }

    if (queryDto.CreateTimeEnd.HasValue)
    {
      predicate = predicate.And(t => t.CreateTime <= queryDto.CreateTimeEnd.Value);
    }

    return predicate;
  }

  /// <summary>
  /// 根据审计日志ID获取SQL差异日志列表
  /// </summary>
  public async Task<List<LeanSqlDiffLogDto>> GetListByAuditLogIdAsync(long auditLogId)
  {
    var logs = await _sqlDiffLogRepository.GetListAsync(t => t.AuditLogId == auditLogId);
    return logs.Select(t => t.Adapt<LeanSqlDiffLogDto>()).ToList();
  }
}