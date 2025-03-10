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
        private readonly ILogger _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        public LeanAuditLogService(
            ILeanRepository<LeanAuditLog> auditLogRepository,
            LeanBaseServiceContext context)
            : base(context)
        {
            _auditLogRepository = auditLogRepository;
            _logger = context.Logger;
        }

        /// <summary>
        /// 获取审计日志列表（分页）
        /// </summary>
        public async Task<LeanPageResult<LeanAuditLogDto>> GetPageListAsync(LeanAuditLogQueryDto queryDto)
        {
            var predicate = BuildQueryPredicate(queryDto);
            var (total, items) = await _auditLogRepository.GetPageListAsync(predicate, queryDto.PageSize, queryDto.PageIndex);
            var list = items.Select(t => t.Adapt<LeanAuditLogDto>()).ToList();

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

            return log.Adapt<LeanAuditLogDto>();
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