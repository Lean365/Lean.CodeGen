//===================================================
// 项目名: Lean.CodeGen.Application
// 文件名: ILeanDataSourceService.cs
// 功能描述: 数据源管理服务接口
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
  /// 数据源管理服务接口
  /// </summary>
  public interface ILeanDataSourceService
  {
    /// <summary>
    /// 获取数据源列表（分页）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>数据源列表</returns>
    Task<LeanPageResult<LeanDataSourceDto>> GetPageListAsync(LeanDataSourceQueryDto queryDto);

    /// <summary>
    /// 获取数据源详情
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns>数据源详情</returns>
    Task<LeanDataSourceDto> GetAsync(long id);

    /// <summary>
    /// 创建数据源
    /// </summary>
    /// <param name="createDto">创建对象</param>
    /// <returns>数据源详情</returns>
    Task<LeanDataSourceDto> CreateAsync(LeanCreateDataSourceDto createDto);

    /// <summary>
    /// 更新数据源
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="updateDto">更新对象</param>
    /// <returns>数据源详情</returns>
    Task<LeanDataSourceDto> UpdateAsync(long id, LeanUpdateDataSourceDto updateDto);

    /// <summary>
    /// 删除数据源
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns>是否成功</returns>
    Task<bool> DeleteAsync(long id);

    /// <summary>
    /// 导出数据源
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>文件信息</returns>
    Task<LeanFileResult> ExportAsync(LeanDataSourceQueryDto queryDto);

    /// <summary>
    /// 导入数据源
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    Task<LeanExcelImportResult<LeanDataSourceImportDto>> ImportAsync(LeanFileInfo file);

    /// <summary>
    /// 下载导入模板
    /// </summary>
    /// <returns>文件信息</returns>
    Task<LeanFileResult> DownloadTemplateAsync();

    /// <summary>
    /// 测试连接
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns>是否成功</returns>
    Task<bool> TestConnectionAsync(long id);
  }
}