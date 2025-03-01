//===================================================
// 项目名: Lean.CodeGen.Application
// 文件名: LeanOperationLogService.cs
// 功能描述: 操作日志服务实现
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
  /// 操作日志服务实现
  /// </summary>
  public class LeanOperationLogService : LeanBaseService, ILeanOperationLogService
  {
    private readonly ILeanRepository<LeanOperationLog> _operationLogRepository;
    private readonly ILogger _logger;

    /// <summary>
    /// 构造函数
    /// </summary>
    public LeanOperationLogService(
        ILeanRepository<LeanOperationLog> operationLogRepository,
        LeanBaseServiceContext context)
        : base(context)
    {
      _operationLogRepository = operationLogRepository;
      _logger = context.Logger;
    }

    /// <summary>
    /// 获取操作日志列表（分页）
    /// </summary>
    public async Task<LeanPageResult<LeanOperationLogDto>> GetPageListAsync(LeanOperationLogQueryDto queryDto)
    {
      var predicate = BuildQueryPredicate(queryDto);
      var (total, items) = await _operationLogRepository.GetPageListAsync(predicate, queryDto.PageSize, queryDto.PageIndex);
      var list = items.Select(t => t.Adapt<LeanOperationLogDto>()).ToList();

      return new LeanPageResult<LeanOperationLogDto>
      {
        Total = total,
        Items = list
      };
    }

    /// <summary>
    /// 获取操作日志详情
    /// </summary>
    public async Task<LeanOperationLogDto> GetAsync(long id)
    {
      var operationLog = await _operationLogRepository.FirstOrDefaultAsync(t => t.Id == id);
      if (operationLog == null)
      {
        throw new Exception($"操作日志[{id}]不存在");
      }

      return operationLog.Adapt<LeanOperationLogDto>();
    }

    /// <summary>
    /// 导出操作日志
    /// </summary>
    public async Task<LeanFileResult> ExportAsync(LeanOperationLogQueryDto queryDto)
    {
      var predicate = BuildQueryPredicate(queryDto);
      var items = await _operationLogRepository.GetListAsync(predicate);
      var list = items.Select(t => t.Adapt<LeanOperationLogExportDto>()).ToList();

      var excelBytes = LeanExcelHelper.Export(list);
      return new LeanFileResult
      {
        FileName = $"操作日志_{DateTime.Now:yyyyMMddHHmmss}.xlsx",
        ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        Stream = new MemoryStream(excelBytes)
      };
    }

    /// <summary>
    /// 清空操作日志
    /// </summary>
    public async Task<bool> ClearAsync()
    {
      return await _operationLogRepository.DeleteAsync(t => true);
    }

    /// <summary>
    /// 构建查询条件
    /// </summary>
    private Expression<Func<LeanOperationLog, bool>> BuildQueryPredicate(LeanOperationLogQueryDto queryDto)
    {
      Expression<Func<LeanOperationLog, bool>> predicate = t => true;

      if (queryDto.UserId.HasValue)
      {
        predicate = predicate.And(t => t.UserId == queryDto.UserId);
      }

      if (!string.IsNullOrEmpty(queryDto.Module))
      {
        var module = CleanInput(queryDto.Module);
        predicate = predicate.And(t => t.Module.Contains(module));
      }

      if (!string.IsNullOrEmpty(queryDto.Operation))
      {
        var operation = CleanInput(queryDto.Operation);
        predicate = predicate.And(t => t.Operation.Contains(operation));
      }

      if (queryDto.OperationStatus.HasValue)
      {
        predicate = predicate.And(t => t.OperationStatus == queryDto.OperationStatus);
      }

      if (!string.IsNullOrEmpty(queryDto.ClientIp))
      {
        var clientIp = CleanInput(queryDto.ClientIp);
        predicate = predicate.And(t => t.ClientIp.Contains(clientIp));
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