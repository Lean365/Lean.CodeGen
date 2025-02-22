//===================================================
// 项目名: Lean.CodeGen.Application
// 文件名: ILeanTableConfigService.cs
// 功能描述: 表配置关联服务接口
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
  /// 表配置关联服务接口
  /// </summary>
  public interface ILeanTableConfigService
  {
    /// <summary>
    /// 获取表配置关联列表（分页）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>表配置关联列表</returns>
    Task<LeanPageResult<LeanTableConfigDto>> GetPageListAsync(LeanTableConfigQueryDto queryDto);

    /// <summary>
    /// 获取表配置关联详情
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns>表配置关联详情</returns>
    Task<LeanTableConfigDto> GetAsync(long id);

    /// <summary>
    /// 创建表配置关联
    /// </summary>
    /// <param name="createDto">创建对象</param>
    /// <returns>表配置关联详情</returns>
    Task<LeanTableConfigDto> CreateAsync(LeanCreateTableConfigDto createDto);

    /// <summary>
    /// 更新表配置关联
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="updateDto">更新对象</param>
    /// <returns>表配置关联详情</returns>
    Task<LeanTableConfigDto> UpdateAsync(long id, LeanUpdateTableConfigDto updateDto);

    /// <summary>
    /// 删除表配置关联
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns>是否成功</returns>
    Task<bool> DeleteAsync(long id);

    /// <summary>
    /// 导出表配置关联
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>文件信息</returns>
    Task<LeanFileResult> ExportAsync(LeanTableConfigQueryDto queryDto);

    /// <summary>
    /// 导入表配置关联
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    Task<LeanExcelImportResult<LeanTableConfigImportDto>> ImportAsync(LeanFileInfo file);

    /// <summary>
    /// 下载导入模板
    /// </summary>
    /// <returns>文件信息</returns>
    Task<LeanFileResult> DownloadTemplateAsync();
  }
}