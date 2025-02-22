//===================================================
// 项目名: Lean.CodeGen.Application
// 文件名: ILeanGenTemplateService.cs
// 功能描述: 代码生成模板服务接口
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using System;
using System.Threading.Tasks;
using Lean.CodeGen.Application.Dtos.Generator;
using Lean.CodeGen.Common.Excel;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Services.Generator
{
  /// <summary>
  /// 代码生成模板服务接口
  /// </summary>
  public interface ILeanGenTemplateService
  {
    /// <summary>
    /// 获取代码生成模板列表（分页）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>代码生成模板列表</returns>
    Task<LeanPageResult<LeanGenTemplateDto>> GetPageListAsync(LeanGenTemplateQueryDto queryDto);

    /// <summary>
    /// 获取代码生成模板详情
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns>代码生成模板详情</returns>
    Task<LeanGenTemplateDto> GetAsync(long id);

    /// <summary>
    /// 创建代码生成模板
    /// </summary>
    /// <param name="createDto">创建对象</param>
    /// <returns>代码生成模板详情</returns>
    Task<LeanGenTemplateDto> CreateAsync(LeanCreateGenTemplateDto createDto);

    /// <summary>
    /// 更新代码生成模板
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="updateDto">更新对象</param>
    /// <returns>代码生成模板详情</returns>
    Task<LeanGenTemplateDto> UpdateAsync(long id, LeanUpdateGenTemplateDto updateDto);

    /// <summary>
    /// 删除代码生成模板
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns>是否成功</returns>
    Task<bool> DeleteAsync(long id);

    /// <summary>
    /// 导出代码生成模板
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>文件信息</returns>
    Task<LeanFileResult> ExportAsync(LeanGenTemplateQueryDto queryDto);

    /// <summary>
    /// 导入代码生成模板
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    Task<LeanExcelImportResult<LeanGenTemplateImportDto>> ImportAsync(LeanFileInfo file);

    /// <summary>
    /// 下载导入模板
    /// </summary>
    /// <returns>文件信息</returns>
    Task<LeanFileResult> DownloadTemplateAsync();

    /// <summary>
    /// 复制模板
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns>新模板详情</returns>
    Task<LeanGenTemplateDto> CopyAsync(long id);
  }
}