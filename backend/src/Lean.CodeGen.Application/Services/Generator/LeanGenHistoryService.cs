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
using Lean.CodeGen.Domain.Entities.Generator;
using Lean.CodeGen.Domain.Interfaces.Repositories;
using Lean.CodeGen.Application.Services.Base;
using Lean.CodeGen.Common.Options;
using Lean.CodeGen.Application.Services.Security;
using Lean.CodeGen.Common.Extensions;
using NLog;
using Lean.CodeGen.Domain.Validators;

namespace Lean.CodeGen.Application.Services.Generator
{
    /// <summary>
    /// 代码生成历史服务实现
    /// </summary>
    public class LeanGenHistoryService : LeanBaseService, ILeanGenHistoryService
    {
        private readonly ILogger _logger;
        private readonly ILeanRepository<LeanGenHistory> _historyRepository;
        private readonly ILeanRepository<LeanGenTask> _taskRepository;
        private readonly ILeanRepository<LeanDbTable> _tableRepository;
        private readonly LeanUniqueValidator<LeanGenHistory> _uniqueValidator;

        /// <summary>
        /// 构造函数
        /// </summary>
        public LeanGenHistoryService(
            ILeanRepository<LeanGenHistory> historyRepository,
            ILeanRepository<LeanGenTask> taskRepository,
            ILeanRepository<LeanDbTable> tableRepository,
            LeanBaseServiceContext context)
            : base(context)
        {
            _historyRepository = historyRepository;
            _taskRepository = taskRepository;
            _tableRepository = tableRepository;
            _logger = context.Logger;
            _uniqueValidator = new LeanUniqueValidator<LeanGenHistory>(_historyRepository);
        }

        /// <summary>
        /// 获取代码生成历史列表（分页）
        /// </summary>
        public async Task<LeanPageResult<LeanGenHistoryDto>> GetPageListAsync(LeanGenHistoryQueryDto queryDto)
        {
            var predicate = BuildQueryPredicate(queryDto);
            var (total, items) = await _historyRepository.GetPageListAsync(predicate, queryDto.PageSize, queryDto.PageIndex);
            var list = items.Select(t => t.Adapt<LeanGenHistoryDto>()).ToList();

            await FillHistoryRelationsAsync(list);

            return new LeanPageResult<LeanGenHistoryDto>
            {
                Total = total,
                Items = list
            };
        }

        /// <summary>
        /// 获取代码生成历史详情
        /// </summary>
        public async Task<LeanGenHistoryDto> GetAsync(long id)
        {
            var history = await _historyRepository.FirstOrDefaultAsync(t => t.Id == id);
            if (history == null)
            {
                throw new Exception($"生成历史 {id} 不存在");
            }

            var result = history.Adapt<LeanGenHistoryDto>();
            await FillHistoryRelationsAsync(new List<LeanGenHistoryDto> { result });

            return result;
        }

        /// <summary>
        /// 导出代码生成历史
        /// </summary>
        public async Task<LeanFileResult> ExportAsync(LeanGenHistoryQueryDto queryDto)
        {
            var predicate = BuildQueryPredicate(queryDto);
            var items = await _historyRepository.GetListAsync(predicate);
            var list = items.Select(t => t.Adapt<LeanGenHistoryExportDto>()).ToList();

            var excelBytes = LeanExcelHelper.Export(list);
            return new LeanFileResult
            {
                FileName = $"代码生成历史_{DateTime.Now:yyyyMMddHHmmss}.xlsx",
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                Stream = new MemoryStream(excelBytes)
            };
        }

        /// <summary>
        /// 清空历史记录
        /// </summary>
        public async Task<bool> ClearAsync()
        {
            return await _historyRepository.DeleteAsync(t => true);
        }

        /// <summary>
        /// 构建查询条件
        /// </summary>
        private Expression<Func<LeanGenHistory, bool>> BuildQueryPredicate(LeanGenHistoryQueryDto queryDto)
        {
            Expression<Func<LeanGenHistory, bool>> predicate = t => true;

            if (queryDto.TaskId.HasValue)
            {
                predicate = LeanExpressionExtensions.And(predicate, t => t.TaskId == queryDto.TaskId);
            }

            if (queryDto.TableId.HasValue)
            {
                predicate = LeanExpressionExtensions.And(predicate, t => t.TableId == queryDto.TableId);
            }

            if (queryDto.Status.HasValue)
            {
                predicate = LeanExpressionExtensions.And(predicate, t => t.Status == queryDto.Status);
            }

            if (queryDto.GenerateTimeBegin.HasValue)
            {
                predicate = LeanExpressionExtensions.And(predicate, t => t.GenerateTime >= queryDto.GenerateTimeBegin);
            }

            if (queryDto.GenerateTimeEnd.HasValue)
            {
                predicate = LeanExpressionExtensions.And(predicate, t => t.GenerateTime <= queryDto.GenerateTimeEnd);
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

        /// <summary>
        /// 填充历史关联信息
        /// </summary>
        private async Task FillHistoryRelationsAsync(List<LeanGenHistoryDto> histories)
        {
            if (histories.Count == 0)
            {
                return;
            }

            // 填充任务名称
            var taskIds = histories.Select(t => t.TaskId).Distinct().ToList();
            var tasks = await _taskRepository.GetListAsync(t => taskIds.Contains(t.Id));

            // 填充表名称
            var tableIds = histories.Select(t => t.TableId).Distinct().ToList();
            var tables = await _tableRepository.GetListAsync(t => tableIds.Contains(t.Id));

            foreach (var history in histories)
            {
                var task = tasks.FirstOrDefault(t => t.Id == history.TaskId);
                if (task != null)
                {
                    history.TaskName = task.Name;
                }

                var table = tables.FirstOrDefault(t => t.Id == history.TableId);
                if (table != null)
                {
                    history.TableName = table.TableName;
                }
            }
        }
    }
}