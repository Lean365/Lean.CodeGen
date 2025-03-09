//===================================================
// 项目名: Lean.CodeGen.Application
// 文件名: ILeanExceptionLogService.cs
// 功能描述: 异常日志服务接口
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
    /// 异常日志服务接口
    /// </summary>
    public interface ILeanExceptionLogService
    {
        /// <summary>
        /// 获取异常日志列表（分页）
        /// </summary>
        /// <param name="queryDto">查询条件</param>
        /// <returns>异常日志列表</returns>
        Task<LeanPageResult<LeanExceptionLogDto>> GetPageListAsync(LeanExceptionLogQueryDto queryDto);

        /// <summary>
        /// 获取异常日志详情
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>异常日志详情</returns>
        Task<LeanExceptionLogDto> GetAsync(long id);

        /// <summary>
        /// 导出异常日志
        /// </summary>
        /// <param name="queryDto">查询条件</param>
        /// <returns>文件信息</returns>
        Task<LeanFileResult> ExportAsync(LeanExceptionLogQueryDto queryDto);

        /// <summary>
        /// 清空异常日志
        /// </summary>
        /// <returns>是否成功</returns>
        Task<bool> ClearAsync();

        /// <summary>
        /// 处理异常日志
        /// </summary>
        /// <param name="handleDto">处理信息</param>
        /// <returns>是否成功</returns>
        Task<bool> HandleAsync(LeanExceptionLogHandleDto handleDto);
    }
}