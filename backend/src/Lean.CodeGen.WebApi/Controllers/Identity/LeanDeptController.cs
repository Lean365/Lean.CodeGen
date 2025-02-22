using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Application.Services.Identity;
using Lean.CodeGen.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.WebApi.Controllers;

namespace Lean.CodeGen.WebApi.Controllers.Identity;

/// <summary>
/// 部门管理
/// </summary>
[Route("api/identity/[controller]")]
[ApiController]
public class LeanDeptController : LeanBaseController
{
  private readonly ILeanDeptService _deptService;

  public LeanDeptController(ILeanDeptService deptService)
  {
    _deptService = deptService;
  }

  /// <summary>
  /// 创建部门
  /// </summary>
  [HttpPost]
  public Task<LeanApiResult<long>> CreateAsync([FromBody] LeanCreateDeptDto input)
  {
    return _deptService.CreateAsync(input);
  }

  /// <summary>
  /// 更新部门
  /// </summary>
  [HttpPut]
  public Task<LeanApiResult> UpdateAsync([FromBody] LeanUpdateDeptDto input)
  {
    return _deptService.UpdateAsync(input);
  }

  /// <summary>
  /// 删除部门
  /// </summary>
  [HttpDelete("{id}")]
  public Task<LeanApiResult> DeleteAsync([FromRoute] long id)
  {
    return _deptService.DeleteAsync(id);
  }

  /// <summary>
  /// 批量删除部门
  /// </summary>
  [HttpDelete("batch")]
  public Task<LeanApiResult> BatchDeleteAsync([FromBody] List<long> ids)
  {
    return _deptService.BatchDeleteAsync(ids);
  }

  /// <summary>
  /// 获取部门信息
  /// </summary>
  [HttpGet("{id}")]
  public Task<LeanApiResult<LeanDeptDto>> GetAsync([FromRoute] long id)
  {
    return _deptService.GetAsync(id);
  }

  /// <summary>
  /// 分页查询部门
  /// </summary>
  [HttpGet("page")]
  public Task<LeanApiResult<LeanPageResult<LeanDeptDto>>> GetPageAsync([FromQuery] LeanQueryDeptDto input)
  {
    return _deptService.GetPageAsync(input);
  }

  /// <summary>
  /// 修改部门状态
  /// </summary>
  [HttpPut("status")]
  public Task<LeanApiResult> SetStatusAsync([FromBody] LeanChangeDeptStatusDto input)
  {
    return _deptService.SetStatusAsync(input);
  }

  /// <summary>
  /// 获取部门树形结构
  /// </summary>
  [HttpGet("tree")]
  public Task<LeanApiResult<List<LeanDeptTreeDto>>> GetTreeAsync()
  {
    return _deptService.GetTreeAsync();
  }

  /// <summary>
  /// 获取用户部门树
  /// </summary>
  [HttpGet("user/{userId}/tree")]
  public Task<LeanApiResult<List<LeanDeptTreeDto>>> GetUserDeptTreeAsync([FromRoute] long userId)
  {
    return _deptService.GetUserDeptTreeAsync(userId);
  }

  /// <summary>
  /// 获取角色部门树
  /// </summary>
  [HttpGet("role/{roleId}/tree")]
  public Task<LeanApiResult<List<LeanDeptTreeDto>>> GetRoleDeptTreeAsync([FromRoute] long roleId)
  {
    return _deptService.GetRoleDeptTreeAsync(roleId);
  }

  /// <summary>
  /// 获取部门的所有下级部门ID列表
  /// </summary>
  [HttpGet("{deptId}/children")]
  public Task<LeanApiResult<List<long>>> GetDeptChildrenIdsAsync([FromRoute] long deptId)
  {
    return _deptService.GetDeptChildrenIdsAsync(deptId);
  }

  /// <summary>
  /// 获取部门的所有上级部门ID列表
  /// </summary>
  [HttpGet("{deptId}/parents")]
  public Task<LeanApiResult<List<long>>> GetDeptParentIdsAsync([FromRoute] long deptId)
  {
    return _deptService.GetDeptParentIdsAsync(deptId);
  }

  /// <summary>
  /// 获取用户的所有部门ID列表
  /// </summary>
  [HttpGet("user/{userId}/depts")]
  public Task<LeanApiResult<List<long>>> GetUserDeptIdsAsync([FromRoute] long userId)
  {
    return _deptService.GetUserDeptIdsAsync(userId);
  }

  /// <summary>
  /// 获取角色的所有部门ID列表
  /// </summary>
  [HttpGet("role/{roleId}/depts")]
  public Task<LeanApiResult<List<long>>> GetRoleDeptIdsAsync([FromRoute] long roleId)
  {
    return _deptService.GetRoleDeptIdsAsync(roleId);
  }

  /// <summary>
  /// 获取用户的主部门
  /// </summary>
  [HttpGet("user/{userId}/primary")]
  public Task<LeanApiResult<LeanDeptDto>> GetUserPrimaryDeptAsync([FromRoute] long userId)
  {
    return _deptService.GetUserPrimaryDeptAsync(userId);
  }

  /// <summary>
  /// 验证部门编码是否唯一
  /// </summary>
  [HttpGet("validate-code")]
  public Task<LeanApiResult<bool>> ValidateDeptCodeUniqueAsync([FromQuery] string deptCode, [FromQuery] long? id = null)
  {
    return _deptService.ValidateDeptCodeUniqueAsync(deptCode, id);
  }

  /// <summary>
  /// 获取部门的所有用户ID列表
  /// </summary>
  [HttpGet("{deptId}/users")]
  public Task<LeanApiResult<List<long>>> GetDeptUserIdsAsync([FromRoute] long deptId, [FromQuery] bool includeChildDepts = false)
  {
    return _deptService.GetDeptUserIdsAsync(deptId, includeChildDepts);
  }

  /// <summary>
  /// 获取部门的所有角色ID列表
  /// </summary>
  [HttpGet("{deptId}/roles")]
  public Task<LeanApiResult<List<long>>> GetDeptRoleIdsAsync([FromRoute] long deptId, [FromQuery] bool includeChildDepts = false)
  {
    return _deptService.GetDeptRoleIdsAsync(deptId, includeChildDepts);
  }

  /// <summary>
  /// 导入部门数据
  /// </summary>
  [HttpPost("import")]
  public Task<LeanApiResult<LeanImportDeptResultDto>> ImportAsync([FromBody] List<LeanImportTemplateDeptDto> depts)
  {
    return _deptService.ImportAsync(depts);
  }

  /// <summary>
  /// 导出部门数据
  /// </summary>
  [HttpGet("export")]
  public Task<LeanApiResult<byte[]>> ExportAsync([FromQuery] LeanExportDeptDto input)
  {
    return _deptService.ExportAsync(input);
  }

  /// <summary>
  /// 获取用户可访问的部门数据范围
  /// </summary>
  [HttpGet("user/{userId}/data-scope")]
  public Task<LeanApiResult<List<long>>> GetUserDataScopeDeptIdsAsync([FromRoute] long userId)
  {
    return _deptService.GetUserDataScopeDeptIdsAsync(userId);
  }

  /// <summary>
  /// 设置部门数据访问权限
  /// </summary>
  [HttpPut("data-permission")]
  public Task<LeanApiResult> SetDeptDataPermissionAsync([FromBody] LeanSetDeptDataPermissionDto input)
  {
    return _deptService.SetDeptDataPermissionAsync(input);
  }

  /// <summary>
  /// 验证用户是否有权限访问指定部门的数据
  /// </summary>
  [HttpGet("user/{userId}/validate-permission/{deptId}")]
  public Task<LeanApiResult<bool>> ValidateUserDeptDataPermissionAsync([FromRoute] long userId, [FromRoute] long deptId)
  {
    return _deptService.ValidateUserDeptDataPermissionAsync(userId, deptId);
  }

  /// <summary>
  /// 获取部门数据访问策略
  /// </summary>
  [HttpGet("{deptId}/data-permission")]
  public Task<LeanApiResult<LeanDeptDataPermissionDto>> GetDeptDataPermissionAsync([FromRoute] long deptId)
  {
    return _deptService.GetDeptDataPermissionAsync(deptId);
  }
}