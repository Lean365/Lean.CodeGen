using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Excel;

namespace Lean.CodeGen.Application.Services.Identity;

/// <summary>
/// 岗位服务接口
/// </summary>
/// <remarks>
/// 提供岗位管理相关的业务功能，包括：
/// 1. 岗位的增删改查
/// 2. 岗位状态管理
/// </remarks>
public interface ILeanPostService
{
    /// <summary>
    /// 创建岗位
    /// </summary>
    /// <param name="input">岗位创建参数</param>
    /// <returns>创建成功的岗位信息</returns>
    Task<LeanApiResult<long>> CreateAsync(LeanPostCreateDto input);

    /// <summary>
    /// 更新岗位
    /// </summary>
    /// <param name="input">岗位更新参数</param>
    /// <returns>更新后的岗位信息</returns>
    Task<LeanApiResult> UpdateAsync(LeanPostUpdateDto input);

    /// <summary>
    /// 删除岗位
    /// </summary>
    /// <param name="input">岗位删除参数</param>
    Task<LeanApiResult> DeleteAsync(long id);

    /// <summary>
    /// 批量删除岗位
    /// </summary>
    Task<LeanApiResult> BatchDeleteAsync(List<long> ids);

    /// <summary>
    /// 获取岗位信息
    /// </summary>
    /// <param name="id">岗位ID</param>
    /// <returns>岗位详细信息</returns>
    Task<LeanApiResult<LeanPostDto>> GetAsync(long id);

    /// <summary>
    /// 查询岗位列表
    /// </summary>
    /// <param name="input">查询参数</param>
    /// <returns>岗位列表</returns>
    Task<List<LeanPostDto>> QueryAsync(LeanPostQueryDto input);

    /// <summary>
    /// 分页查询岗位
    /// </summary>
    Task<LeanApiResult<LeanPageResult<LeanPostDto>>> GetPageAsync(LeanPostQueryDto input);

    /// <summary>
    /// 修改岗位状态
    /// </summary>
    /// <param name="input">状态修改参数</param>
    Task<LeanApiResult> SetStatusAsync(LeanPostChangeStatusDto input);

    /// <summary>
    /// 导出岗位数据
    /// </summary>
    Task<byte[]> ExportAsync(LeanPostQueryDto input);

    /// <summary>
    /// 导入岗位数据
    /// </summary>
    Task<LeanPostImportResultDto> ImportAsync(LeanFileInfo file);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    Task<byte[]> GetImportTemplateAsync();
}