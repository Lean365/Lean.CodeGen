using Lean.CodeGen.Application.Dtos.Workflow;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Services.Workflow;

/// <summary>
/// 工作流输出服务接口
/// </summary>
public interface ILeanWorkflowOutputService
{
  /// <summary>
  /// 获取输出记录
  /// </summary>
  /// <param name="id">输出ID</param>
  /// <returns>输出记录</returns>
  Task<LeanWorkflowOutputDto?> GetAsync(long id);

  /// <summary>
  /// 创建输出记录
  /// </summary>
  /// <param name="dto">输出记录</param>
  /// <returns>输出ID</returns>
  Task<long> CreateAsync(LeanWorkflowOutputDto dto);

  /// <summary>
  /// 更新输出记录
  /// </summary>
  /// <param name="dto">输出记录</param>
  /// <returns>是否成功</returns>
  Task<bool> UpdateAsync(LeanWorkflowOutputDto dto);

  /// <summary>
  /// 删除输出记录
  /// </summary>
  /// <param name="id">输出ID</param>
  /// <returns>是否成功</returns>
  Task<bool> DeleteAsync(long id);

  /// <summary>
  /// 分页查询输出记录
  /// </summary>
  /// <param name="pageIndex">页码</param>
  /// <param name="pageSize">每页大小</param>
  /// <param name="activityInstanceId">活动实例ID</param>
  /// <param name="outputName">输出名称</param>
  /// <param name="outputType">输出类型</param>
  /// <returns>分页结果</returns>
  Task<LeanPageResult<LeanWorkflowOutputDto>> GetPagedListAsync(
      int pageIndex,
      int pageSize,
      long? activityInstanceId = null,
      string? outputName = null,
      string? outputType = null);
}