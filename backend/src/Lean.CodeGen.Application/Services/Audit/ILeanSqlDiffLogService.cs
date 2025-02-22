using System.Threading.Tasks;
using Lean.CodeGen.Application.Dtos.Audit;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Services.Audit
{
  /// <summary>
  /// SQL差异日志服务接口
  /// </summary>
  public interface ILeanSqlDiffLogService
  {
    /// <summary>
    /// 获取SQL差异日志列表（分页）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>SQL差异日志列表</returns>
    Task<LeanPageResult<LeanSqlDiffLogDto>> GetPageListAsync(LeanSqlDiffLogQueryDto queryDto);

    /// <summary>
    /// 获取SQL差异日志详情
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns>SQL差异日志详情</returns>
    Task<LeanSqlDiffLogDto> GetAsync(long id);

    /// <summary>
    /// 导出SQL差异日志
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>文件信息</returns>
    Task<LeanFileResult> ExportAsync(LeanSqlDiffLogQueryDto queryDto);

    /// <summary>
    /// 清空SQL差异日志
    /// </summary>
    /// <returns>是否成功</returns>
    Task<bool> ClearAsync();
  }
}