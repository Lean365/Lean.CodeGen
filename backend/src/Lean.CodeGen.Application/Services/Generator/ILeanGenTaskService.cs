using System.Threading.Tasks;
using Lean.CodeGen.Application.Dtos.Generator;
using Lean.CodeGen.Common.Excel;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Services.Generator
{
  /// <summary>
  /// 代码生成任务服务接口
  /// </summary>
  public interface ILeanGenTaskService
  {
    /// <summary>
    /// 获取代码生成任务列表（分页）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>代码生成任务列表</returns>
    Task<LeanPageResult<LeanGenTaskDto>> GetPageListAsync(LeanGenTaskQueryDto queryDto);

    /// <summary>
    /// 获取代码生成任务详情
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns>代码生成任务详情</returns>
    Task<LeanGenTaskDto> GetAsync(long id);

    /// <summary>
    /// 创建代码生成任务
    /// </summary>
    /// <param name="createDto">创建对象</param>
    /// <returns>代码生成任务详情</returns>
    Task<LeanGenTaskDto> CreateAsync(LeanCreateGenTaskDto createDto);

    /// <summary>
    /// 更新代码生成任务
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="updateDto">更新对象</param>
    /// <returns>代码生成任务详情</returns>
    Task<LeanGenTaskDto> UpdateAsync(long id, LeanUpdateGenTaskDto updateDto);

    /// <summary>
    /// 删除代码生成任务
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns>是否成功</returns>
    Task<bool> DeleteAsync(long id);

    /// <summary>
    /// 导出代码生成任务
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>文件信息</returns>
    Task<LeanFileResult> ExportAsync(LeanGenTaskQueryDto queryDto);

    /// <summary>
    /// 导入代码生成任务
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    Task<LeanExcelImportResult<LeanGenTaskImportDto>> ImportAsync(LeanFileInfo file);

    /// <summary>
    /// 下载导入模板
    /// </summary>
    /// <returns>文件信息</returns>
    Task<LeanFileResult> DownloadTemplateAsync();

    /// <summary>
    /// 启动任务
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns>是否成功</returns>
    Task<bool> StartAsync(long id);

    /// <summary>
    /// 停止任务
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns>是否成功</returns>
    Task<bool> StopAsync(long id);

    /// <summary>
    /// 重试任务
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns>是否成功</returns>
    Task<bool> RetryAsync(long id);

    /// <summary>
    /// 预览代码
    /// </summary>
    /// <param name="requestDto">预览请求</param>
    /// <returns>预览结果</returns>
    Task<LeanGenPreviewResultDto> PreviewAsync(LeanGenPreviewRequestDto requestDto);

    /// <summary>
    /// 下载代码
    /// </summary>
    /// <param name="requestDto">下载请求</param>
    /// <returns>下载结果</returns>
    Task<LeanGenDownloadResultDto> DownloadAsync(LeanGenDownloadRequestDto requestDto);
  }
}