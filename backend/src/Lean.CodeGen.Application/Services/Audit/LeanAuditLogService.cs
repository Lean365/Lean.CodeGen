//===================================================
// 项目名: Lean.CodeGen.Application
// 文件名: LeanAuditLogService.cs
// 功能描述: 审计日志服务实现
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

namespace Lean.CodeGen.Application.Services.Audit
{
  /// <summary>
  /// 审计日志服务实现
  /// </summary>
  public class LeanAuditLogService : LeanBaseService, ILeanAuditLogService
  {
    private readonly ILeanRepository<LeanAuditLog> _auditLogRepository;
    private readonly ILeanRepository<LeanSqlDiffLog> _sqlDiffLogRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    public LeanAuditLogService(
        ILeanRepository<LeanAuditLog> auditLogRepository,
        ILeanRepository<LeanSqlDiffLog> sqlDiffLogRepository,
        ILeanSqlSafeService sqlSafeService,
        IOptions<LeanSecurityOptions> securityOptions)
        : base(sqlSafeService, securityOptions)
    {
      _auditLogRepository = auditLogRepository;
      _sqlDiffLogRepository = sqlDiffLogRepository;
    }

    /// <summary>
    /// 获取审计日志列表（分页）
    /// </summary>
    public async Task<LeanPageResult<LeanAuditLogDto>> GetPageListAsync(LeanAuditLogQueryDto queryDto)
    {
      var predicate = BuildQueryPredicate(queryDto);
      var (total, items) = await _auditLogRepository.GetPageListAsync(predicate, queryDto.PageSize, queryDto.PageIndex);
      var list = items.Select(t => t.Adapt<LeanAuditLogDto>()).ToList();

      // 填充数据差异
      foreach (var item in list)
      {
        var sqlDiffLogs = await _sqlDiffLogRepository.GetListAsync(t => t.AuditLogId == item.Id);
        if (sqlDiffLogs.Any())
        {
          item.DataDiffs = sqlDiffLogs.Select(t => t.Adapt<LeanSqlDiffLogDto>()).ToList();
        }
      }

      return new LeanPageResult<LeanAuditLogDto>
      {
        Total = total,
        Items = list
      };
    }

    /// <summary>
    /// 获取审计日志详情
    /// </summary>
    public async Task<LeanAuditLogDto> GetAsync(long id)
    {
      var log = await _auditLogRepository.FirstOrDefaultAsync(t => t.Id == id);
      if (log == null)
      {
        throw new Exception($"审计日志 {id} 不存在");
      }

      var result = log.Adapt<LeanAuditLogDto>();

      // 填充数据差异
      var sqlDiffLogs = await _sqlDiffLogRepository.GetListAsync(t => t.AuditLogId == log.Id);
      if (sqlDiffLogs.Any())
      {
        result.DataDiffs = sqlDiffLogs.Select(t => t.Adapt<LeanSqlDiffLogDto>()).ToList();
      }

      return result;
    }

    /// <summary>
    /// 导出审计日志
    /// </summary>
    public async Task<LeanFileResult> ExportAsync(LeanAuditLogQueryDto queryDto)
    {
      var predicate = BuildQueryPredicate(queryDto);
      var items = await _auditLogRepository.GetListAsync(predicate);
      var list = items.Select(t => t.Adapt<LeanAuditLogExportDto>()).ToList();

      var excelBytes = LeanExcelHelper.Export(list);
      return new LeanFileResult
      {
        FileName = $"审计日志_{DateTime.Now:yyyyMMddHHmmss}.xlsx",
        ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        Stream = new MemoryStream(excelBytes)
      };
    }

    /// <summary>
    /// 清空审计日志
    /// </summary>
    public async Task<bool> ClearAsync()
    {
      // 先删除SQL差异日志
      await _sqlDiffLogRepository.DeleteAsync(t => true);

      // 再删除审计日志
      return await _auditLogRepository.DeleteAsync(t => true);
    }

    /// <summary>
    /// 构建查询条件
    /// </summary>
    private Expression<Func<LeanAuditLog, bool>> BuildQueryPredicate(LeanAuditLogQueryDto queryDto)
    {
      Expression<Func<LeanAuditLog, bool>> predicate = t => true;

      if (queryDto.UserId.HasValue)
      {
        predicate = predicate.And(t => t.UserId == queryDto.UserId.Value);
      }

      if (!string.IsNullOrEmpty(queryDto.EntityType))
      {
        predicate = predicate.And(t => t.EntityType == queryDto.EntityType);
      }

      if (!string.IsNullOrEmpty(queryDto.EntityId))
      {
        predicate = predicate.And(t => t.EntityId == queryDto.EntityId);
      }

      if (queryDto.OperationType.HasValue)
      {
        predicate = predicate.And(t => t.OperationType == queryDto.OperationType.Value);
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
  }
}