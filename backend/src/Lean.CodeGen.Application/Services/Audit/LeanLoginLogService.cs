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
using NLog;

namespace Lean.CodeGen.Application.Services.Audit
{
  /// <summary>
  /// 登录日志服务实现
  /// </summary>
  public class LeanLoginLogService : LeanBaseService, ILeanLoginLogService
  {
    private readonly ILeanRepository<LeanLoginLog> _loginLogRepository;
    private readonly ILogger _logger;

    /// <summary>
    /// 构造函数
    /// </summary>
    public LeanLoginLogService(
        ILeanRepository<LeanLoginLog> loginLogRepository,
        LeanBaseServiceContext context)
        : base(context)
    {
      _loginLogRepository = loginLogRepository;
      _logger = context.Logger;
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
        Items = list,
        PageIndex = queryDto.PageIndex,
        PageSize = queryDto.PageSize
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

    /// <summary>
    /// 添加登录日志
    /// </summary>
    public async Task<bool> AddLoginLogAsync(
        long userId,
        string userName,
        string deviceId,
        string ip,
        string location,
        string browser,
        string os,
        string? errorMsg = null)
    {
      try
      {
        var log = new LeanLoginLog
        {
          UserId = userId,
          UserName = userName,
          DeviceId = deviceId,
          LoginIp = ip,
          LoginLocation = location,
          Browser = browser,
          Os = os,
          LoginType = (int)LeanLoginType.Password,
          LoginStatus = string.IsNullOrEmpty(errorMsg) ? 0 : 1,
          ErrorMsg = errorMsg
        };

        await _loginLogRepository.CreateAsync(log);
        return true;
      }
      catch (Exception ex)
      {
        _logger.Error(ex, "记录登录日志失败");
        return false;
      }
    }

    /// <summary>
    /// 添加登录错误日志
    /// </summary>
    public async Task<bool> AddLoginErrorLogAsync(
        string userName,
        string deviceId,
        string ip,
        string location,
        string browser,
        string os,
        string errorMsg)
    {
      try
      {
        var log = new LeanLoginLog
        {
          UserId = 0, // 未知用户ID
          UserName = userName,
          DeviceId = deviceId,
          LoginIp = ip,
          LoginLocation = location,
          Browser = browser,
          Os = os,
          LoginType = (int)LeanLoginType.Password,
          LoginStatus = 1, // 登录失败
          ErrorMsg = errorMsg
        };

        await _loginLogRepository.CreateAsync(log);
        return true;
      }
      catch (Exception ex)
      {
        _logger.Error(ex, "记录登录错误日志失败");
        return false;
      }
    }

    /// <summary>
    /// 记录登出日志
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="userName">用户名</param>
    /// <param name="deviceId">设备ID</param>
    /// <param name="loginIp">登录IP</param>
    /// <param name="loginLocation">登录地点</param>
    /// <param name="browser">浏览器</param>
    /// <param name="os">操作系统</param>
    /// <param name="errorMsg">错误消息</param>
    /// <returns>记录结果</returns>
    public async Task<bool> AddLogoutLogAsync(
        long userId,
        string userName,
        string deviceId,
        string loginIp,
        string? loginLocation,
        string? browser,
        string? os,
        string? errorMsg = null)
    {
      try
      {
        var log = new LeanLoginLog
        {
          UserId = userId,
          UserName = userName,
          DeviceId = deviceId,
          LoginIp = loginIp,
          LoginLocation = loginLocation,
          Browser = browser,
          Os = os,
          LoginType = (int)LeanLoginType.Token,
          LoginStatus = string.IsNullOrEmpty(errorMsg) ? 0 : 1,
          ErrorMsg = errorMsg
        };

        await _loginLogRepository.CreateAsync(log);
        return true;
      }
      catch (Exception ex)
      {
        _logger.Error(ex, "记录登出日志失败");
        return false;
      }
    }
  }
}