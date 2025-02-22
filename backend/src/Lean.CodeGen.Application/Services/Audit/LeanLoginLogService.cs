//===================================================
// 项目名: Lean.CodeGen.Application
// 文件名: LeanLoginLogService.cs
// 功能描述: 登录日志服务实现
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
  /// 登录日志服务实现
  /// </summary>
  public class LeanLoginLogService : LeanBaseService, ILeanLoginLogService
  {
    private readonly ILeanRepository<LeanLoginLog> _loginLogRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    public LeanLoginLogService(
        ILeanRepository<LeanLoginLog> loginLogRepository,
        ILeanSqlSafeService sqlSafeService,
        IOptions<LeanSecurityOptions> securityOptions)
        : base(sqlSafeService, securityOptions)
    {
      _loginLogRepository = loginLogRepository;
    }

    /// <summary>
    /// 获取登录日志列表（分页）
    /// </summary>
    public async Task<LeanPageResult<LeanLoginLogDto>> GetPageListAsync(LeanLoginLogQueryDto queryDto)
    {
      var predicate = BuildQueryPredicate(queryDto);
      var (total, items) = await _loginLogRepository.GetPageListAsync(predicate, queryDto.PageSize, queryDto.PageIndex);
      var list = items.Select(t => t.Adapt<LeanLoginLogDto>()).ToList();

      return new LeanPageResult<LeanLoginLogDto>
      {
        Total = total,
        Items = list
      };
    }

    /// <summary>
    /// 获取登录日志详情
    /// </summary>
    public async Task<LeanLoginLogDto> GetAsync(long id)
    {
      var loginLog = await _loginLogRepository.FirstOrDefaultAsync(t => t.Id == id);
      if (loginLog == null)
      {
        throw new Exception($"登录日志[{id}]不存在");
      }

      return loginLog.Adapt<LeanLoginLogDto>();
    }

    /// <summary>
    /// 导出登录日志
    /// </summary>
    public async Task<LeanFileResult> ExportAsync(LeanLoginLogQueryDto queryDto)
    {
      var predicate = BuildQueryPredicate(queryDto);
      var items = await _loginLogRepository.GetListAsync(predicate);
      var list = items.Select(t => t.Adapt<LeanLoginLogExportDto>()).ToList();

      var excelBytes = LeanExcelHelper.Export(list);
      return new LeanFileResult
      {
        FileName = $"登录日志_{DateTime.Now:yyyyMMddHHmmss}.xlsx",
        ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        Stream = new MemoryStream(excelBytes)
      };
    }

    /// <summary>
    /// 清空登录日志
    /// </summary>
    public async Task<bool> ClearAsync()
    {
      return await _loginLogRepository.DeleteAsync(t => true);
    }

    /// <summary>
    /// 构建查询条件
    /// </summary>
    private Expression<Func<LeanLoginLog, bool>> BuildQueryPredicate(LeanLoginLogQueryDto queryDto)
    {
      Expression<Func<LeanLoginLog, bool>> predicate = t => true;

      if (queryDto.UserId.HasValue)
      {
        predicate = predicate.And(t => t.UserId == queryDto.UserId);
      }

      if (!string.IsNullOrEmpty(queryDto.UserName))
      {
        var userName = CleanInput(queryDto.UserName);
        predicate = predicate.And(t => t.UserName.Contains(userName));
      }

      if (!string.IsNullOrEmpty(queryDto.DeviceId))
      {
        var deviceId = CleanInput(queryDto.DeviceId);
        predicate = predicate.And(t => t.DeviceId.Contains(deviceId));
      }

      if (queryDto.LoginType.HasValue)
      {
        predicate = predicate.And(t => t.LoginType == queryDto.LoginType);
      }

      if (queryDto.LoginStatus.HasValue)
      {
        predicate = predicate.And(t => t.LoginStatus == queryDto.LoginStatus);
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