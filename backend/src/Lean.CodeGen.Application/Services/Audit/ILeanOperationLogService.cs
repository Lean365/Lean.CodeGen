//===================================================
// 项目名: Lean.CodeGen.Application
// 文件名: ILeanOperationLogService.cs
// 功能描述: 操作日志服务接口
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
  /// 操作日志服务接口
  /// </summary>
  public interface ILeanOperationLogService
  {
    /// <summary>
    /// 获取操作日志列表（分页）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>操作日志列表</returns>
    Task<LeanPageResult<LeanOperationLogDto>> GetPageListAsync(LeanOperationLogQueryDto queryDto);

    /// <summary>
    /// 获取操作日志详情
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns>操作日志详情</returns>
    Task<LeanOperationLogDto> GetAsync(long id);

    /// <summary>
    /// 导出操作日志
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>文件信息</returns>
    Task<LeanFileResult> ExportAsync(LeanOperationLogQueryDto queryDto);

    /// <summary>
    /// 清空操作日志
    /// </summary>
    /// <returns>是否成功</returns>
    Task<bool> ClearAsync();
  }
}