using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Services.Identity;

/// <summary>
/// 部门服务接口
/// </summary>
public interface ILeanDeptService
{
  /// <summary>
  /// 创建部门
  /// </summary>
  Task<LeanApiResult<long>> CreateAsync(LeanCreateDeptDto input);

  /// <summary>
  /// 更新部门
  /// </summary>
  Task<LeanApiResult> UpdateAsync(LeanUpdateDeptDto input);

  /// <summary>
  /// 删除部门
  /// </summary>
  Task<LeanApiResult> DeleteAsync(long id);

  /// <summary>
  /// 批量删除部门
  /// </summary>
  Task<LeanApiResult> BatchDeleteAsync(List<long> ids);

  /// <summary>
  /// 获取部门信息
  /// </summary>
  Task<LeanApiResult<LeanDeptDto>> GetAsync(long id);

  /// <summary>
  /// 分页查询部门
  /// </summary>
  Task<LeanApiResult<LeanPageResult<LeanDeptDto>>> GetPageAsync(LeanQueryDeptDto input);

  /// <summary>
  /// 修改部门状态
  /// </summary>
  Task<LeanApiResult> SetStatusAsync(LeanChangeDeptStatusDto input);

  /// <summary>
  /// 获取部门树形结构
  /// </summary>
  Task<LeanApiResult<List<LeanDeptTreeDto>>> GetTreeAsync();

  /// <summary>
  /// 获取用户部门树
  /// </summary>
  Task<LeanApiResult<List<LeanDeptTreeDto>>> GetUserDeptTreeAsync(long userId);

  /// <summary>
  /// 获取角色部门树
  /// </summary>
  Task<LeanApiResult<List<LeanDeptTreeDto>>> GetRoleDeptTreeAsync(long roleId);

  /// <summary>
  /// 获取部门的所有下级部门ID列表
  /// </summary>
  Task<LeanApiResult<List<long>>> GetDeptChildrenIdsAsync(long deptId);

  /// <summary>
  /// 获取部门的所有上级部门ID列表
  /// </summary>
  Task<LeanApiResult<List<long>>> GetDeptParentIdsAsync(long deptId);

  /// <summary>
  /// 获取用户的所有部门ID列表
  /// </summary>
  Task<LeanApiResult<List<long>>> GetUserDeptIdsAsync(long userId);

  /// <summary>
  /// 获取角色的所有部门ID列表
  /// </summary>
  Task<LeanApiResult<List<long>>> GetRoleDeptIdsAsync(long roleId);

  /// <summary>
  /// 获取用户的主部门
  /// </summary>
  Task<LeanApiResult<LeanDeptDto>> GetUserPrimaryDeptAsync(long userId);

  /// <summary>
  /// 验证部门编码是否唯一
  /// </summary>
  Task<LeanApiResult<bool>> ValidateDeptCodeUniqueAsync(string deptCode, long? id = null);

  /// <summary>
  /// 获取部门的所有用户ID列表
  /// </summary>
  Task<LeanApiResult<List<long>>> GetDeptUserIdsAsync(long deptId, bool includeChildDepts = false);

  /// <summary>
  /// 获取部门的所有角色ID列表
  /// </summary>
  Task<LeanApiResult<List<long>>> GetDeptRoleIdsAsync(long deptId, bool includeChildDepts = false);

  /// <summary>
  /// 导入部门数据
  /// </summary>
  Task<LeanApiResult<LeanImportDeptResultDto>> ImportAsync(List<LeanImportTemplateDeptDto> depts);

  /// <summary>
  /// 导出部门数据
  /// </summary>
  Task<LeanApiResult<byte[]>> ExportAsync(LeanExportDeptDto input);

  /// <summary>
  /// 获取用户可访问的部门数据范围
  /// </summary>
  Task<LeanApiResult<List<long>>> GetUserDataScopeDeptIdsAsync(long userId);

  /// <summary>
  /// 设置部门数据访问权限
  /// </summary>
  Task<LeanApiResult> SetDeptDataPermissionAsync(LeanSetDeptDataPermissionDto input);

  /// <summary>
  /// 验证用户是否有权限访问指定部门的数据
  /// </summary>
  Task<LeanApiResult<bool>> ValidateUserDeptDataPermissionAsync(long userId, long deptId);

  /// <summary>
  /// 获取部门数据访问策略
  /// </summary>
  Task<LeanApiResult<LeanDeptDataPermissionDto>> GetDeptDataPermissionAsync(long deptId);
}