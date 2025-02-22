//===================================================
// 项目名: Lean.CodeGen.Application
// 文件名: ILeanGenHistoryService.cs
// 功能描述: 代码生成历史服务接口
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using System.Threading.Tasks;
using Lean.CodeGen.Application.Dtos.Generator;
using Lean.CodeGen.Common.Excel;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Services.Generator
{
  /// <summary>
  /// 代码生成历史服务接口
  /// </summary>
  public interface ILeanGenHistoryService
  {
    /// <summary>
    /// 获取代码生成历史列表（分页）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>代码生成历史列表</returns>
    Task<LeanPageResult<LeanGenHistoryDto>> GetPageListAsync(LeanGenHistoryQueryDto queryDto);

    /// <summary>
    /// 获取代码生成历史详情
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns>代码生成历史详情</returns>
    Task<LeanGenHistoryDto> GetAsync(long id);

    /// <summary>
    /// 导出代码生成历史
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>文件信息</returns>
    Task<LeanFileResult> ExportAsync(LeanGenHistoryQueryDto queryDto);

    /// <summary>
    /// 清空历史记录
    /// </summary>
    /// <returns>是否成功</returns>
    Task<bool> ClearAsync();
  }
}