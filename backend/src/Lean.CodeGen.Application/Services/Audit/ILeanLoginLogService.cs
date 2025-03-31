//===================================================
// 项目名: Lean.CodeGen.Application
// 文件名: ILeanLoginLogService.cs
// 功能描述: 登录日志服务接口
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using System.Threading.Tasks;
using Lean.CodeGen.Application.Dtos.Audit;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Services.Audit
{
  /// <summary>
  /// 登录日志服务接口
  /// </summary>
  public interface ILeanLoginLogService
  {
    /// <summary>
    /// 获取登录日志列表（分页）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>登录日志列表</returns>
    Task<LeanPageResult<LeanLoginLogDto>> GetPageListAsync(LeanLoginLogQueryDto queryDto);

    /// <summary>
    /// 获取登录日志详情
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns>登录日志详情</returns>
    Task<LeanLoginLogDto> GetAsync(long id);

    /// <summary>
    /// 导出登录日志
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>文件信息</returns>
    Task<LeanFileResult> ExportAsync(LeanLoginLogQueryDto queryDto);

    /// <summary>
    /// 清空登录日志
    /// </summary>
    /// <returns>是否成功</returns>
    Task<bool> ClearAsync();

    /// <summary>
    /// 添加登录日志
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="userName">用户名</param>
    /// <param name="deviceId">设备ID</param>
    /// <param name="ip">IP地址</param>
    /// <param name="location">登录地点</param>
    /// <param name="browser">浏览器</param>
    /// <param name="os">操作系统</param>
    /// <param name="errorMsg">错误信息</param>
    /// <returns>是否成功</returns>
    Task<bool> AddLoginLogAsync(long userId, string userName, string deviceId, string ip, string location, string browser, string os, string? errorMsg = null);

    /// <summary>
    /// 添加登录错误日志
    /// </summary>
    /// <param name="userName">用户名</param>
    /// <param name="deviceId">设备ID</param>
    /// <param name="ip">IP地址</param>
    /// <param name="location">登录地点</param>
    /// <param name="browser">浏览器</param>
    /// <param name="os">操作系统</param>
    /// <param name="errorMsg">错误信息</param>
    /// <returns>是否成功</returns>
    Task<bool> AddLoginErrorLogAsync(string userName, string deviceId, string ip, string location, string browser, string os, string errorMsg);

    /// <summary>
    /// 添加登出日志
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="userName">用户名</param>
    /// <param name="deviceId">设备ID</param>
    /// <param name="ip">IP地址</param>
    /// <param name="location">登录地点</param>
    /// <param name="browser">浏览器</param>
    /// <param name="os">操作系统</param>
    /// <param name="errorMsg">错误信息</param>
    /// <returns>是否成功</returns>
    Task<bool> AddLogoutLogAsync(long userId, string userName, string deviceId, string ip, string location, string browser, string os, string? errorMsg = null);
  }
}