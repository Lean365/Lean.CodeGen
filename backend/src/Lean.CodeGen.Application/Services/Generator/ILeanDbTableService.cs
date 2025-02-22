//===================================================
// 项目名: Lean.CodeGen.Application
// 文件名: ILeanDbTableService.cs
// 功能描述: 数据库表管理服务接口
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using System.Collections.Generic;
using System.Threading.Tasks;
using Lean.CodeGen.Application.Dtos.Generator;
using Lean.CodeGen.Common.Excel;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Services.Generator
{
  /// <summary>
  /// 数据库表管理服务接口
  /// </summary>
  public interface ILeanDbTableService
  {
    /// <summary>
    /// 获取数据库表列表（分页）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>数据库表列表</returns>
    Task<LeanPageResult<LeanDbTableDto>> GetPageListAsync(LeanDbTableQueryDto queryDto);

    /// <summary>
    /// 获取数据库表详情
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns>数据库表详情</returns>
    Task<LeanDbTableDto> GetAsync(long id);

    /// <summary>
    /// 创建数据库表
    /// </summary>
    /// <param name="createDto">创建对象</param>
    /// <returns>数据库表详情</returns>
    Task<LeanDbTableDto> CreateAsync(LeanCreateDbTableDto createDto);

    /// <summary>
    /// 更新数据库表
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="updateDto">更新对象</param>
    /// <returns>数据库表详情</returns>
    Task<LeanDbTableDto> UpdateAsync(long id, LeanUpdateDbTableDto updateDto);

    /// <summary>
    /// 删除数据库表
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns>是否成功</returns>
    Task<bool> DeleteAsync(long id);

    /// <summary>
    /// 导出数据库表
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>文件信息</returns>
    Task<LeanFileResult> ExportAsync(LeanDbTableQueryDto queryDto);

    /// <summary>
    /// 导入数据库表
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    Task<LeanExcelImportResult<LeanDbTableImportDto>> ImportAsync(LeanFileInfo file);

    /// <summary>
    /// 下载导入模板
    /// </summary>
    /// <returns>文件信息</returns>
    Task<LeanFileResult> DownloadTemplateAsync();

    /// <summary>
    /// 从数据源导入表
    /// </summary>
    /// <param name="dataSourceId">数据源Id</param>
    /// <returns>导入的表列表</returns>
    Task<List<LeanDbTableDto>> ImportFromDataSourceAsync(long dataSourceId);

    /// <summary>
    /// 同步表结构
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns>是否成功</returns>
    Task<bool> SyncStructureAsync(long id);
  }
}