using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Services.Identity;

/// <summary>
/// 部门服务接口
/// </summary>
/// <remarks>
/// 提供部门管理相关的业务功能，包括：
/// 1. 部门的增删改查
/// 2. 部门状态管理
/// 3. 部门树形结构管理
/// 4. 部门数据权限管理
/// </remarks>
public interface ILeanDeptService
{
  /// <summary>
  /// 创建部门
  /// </summary>
  /// <param name="input">部门创建参数</param>
  /// <returns>创建成功的部门信息</returns>
  Task<LeanDeptDto> CreateAsync(LeanCreateDeptDto input);

  /// <summary>
  /// 更新部门
  /// </summary>
  /// <param name="input">部门更新参数</param>
  /// <returns>更新后的部门信息</returns>
  Task<LeanDeptDto> UpdateAsync(LeanUpdateDeptDto input);

  /// <summary>
  /// 删除部门
  /// </summary>
  /// <param name="input">部门删除参数</param>
  Task DeleteAsync(LeanDeleteDeptDto input);

  /// <summary>
  /// 获取部门信息
  /// </summary>
  /// <param name="id">部门ID</param>
  /// <returns>部门详细信息</returns>
  Task<LeanDeptDto> GetAsync(long id);

  /// <summary>
  /// 查询部门列表
  /// </summary>
  /// <param name="input">查询参数</param>
  /// <returns>部门列表</returns>
  Task<List<LeanDeptDto>> QueryAsync(LeanQueryDeptDto input);

  /// <summary>
  /// 获取部门树形结构
  /// </summary>
  /// <param name="input">查询参数</param>
  /// <returns>部门树形结构</returns>
  Task<List<LeanDeptTreeDto>> GetTreeAsync(LeanQueryDeptDto input);

  /// <summary>
  /// 修改部门状态
  /// </summary>
  /// <param name="input">状态修改参数</param>
  Task ChangeStatusAsync(LeanChangeDeptStatusDto input);

  /// <summary>
  /// 获取角色部门树
  /// </summary>
  /// <param name="roleId">角色ID</param>
  /// <returns>角色部门树</returns>
  Task<List<LeanDeptTreeDto>> GetRoleDeptTreeAsync(long roleId);

  /// <summary>
  /// 获取用户部门树
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <returns>用户部门树</returns>
  Task<List<LeanDeptTreeDto>> GetUserDeptTreeAsync(long userId);
}