using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Application.Services.Identity;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Enums;

namespace Lean.CodeGen.WebApi.Controllers.Identity;

/// <summary>
/// 部门管理控制器
/// </summary>
/// <remarks>
/// 提供部门管理相关的API接口，包括：
/// 1. 部门的增删改查
/// 2. 部门状态管理
/// 3. 部门树形结构管理
/// 4. 部门数据权限管理
/// </remarks>
[Route("api/[controller]")]
public class LeanDeptController : LeanBaseController
{
  private readonly ILeanDeptService _deptService;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanDeptController(ILeanDeptService deptService)
  {
    _deptService = deptService;
  }

  /// <summary>
  /// 创建部门
  /// </summary>
  /// <param name="input">部门创建参数</param>
  /// <returns>创建成功的部门信息</returns>
  [HttpPost]
  public async Task<IActionResult> CreateAsync([FromBody] LeanCreateDeptDto input)
  {
    var result = await _deptService.CreateAsync(input);
    return Success(result, LeanBusinessType.Create);
  }

  /// <summary>
  /// 更新部门
  /// </summary>
  /// <param name="input">部门更新参数</param>
  /// <returns>更新后的部门信息</returns>
  [HttpPut]
  public async Task<IActionResult> UpdateAsync([FromBody] LeanUpdateDeptDto input)
  {
    var result = await _deptService.UpdateAsync(input);
    return Success(result, LeanBusinessType.Update);
  }

  /// <summary>
  /// 删除部门
  /// </summary>
  /// <param name="input">部门删除参数</param>
  [HttpDelete]
  public async Task<IActionResult> DeleteAsync([FromBody] LeanDeleteDeptDto input)
  {
    await _deptService.DeleteAsync(input);
    return Success(LeanBusinessType.Delete);
  }

  /// <summary>
  /// 获取部门信息
  /// </summary>
  /// <param name="id">部门ID</param>
  /// <returns>部门详细信息</returns>
  [HttpGet("{id}")]
  public async Task<IActionResult> GetAsync(long id)
  {
    var result = await _deptService.GetAsync(id);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 查询部门列表
  /// </summary>
  /// <param name="input">查询参数</param>
  /// <returns>部门列表</returns>
  [HttpGet]
  public async Task<IActionResult> QueryAsync([FromQuery] LeanQueryDeptDto input)
  {
    var result = await _deptService.QueryAsync(input);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 获取部门树形结构
  /// </summary>
  /// <param name="input">查询参数</param>
  /// <returns>部门树形结构</returns>
  [HttpGet("tree")]
  public async Task<IActionResult> GetTreeAsync([FromQuery] LeanQueryDeptDto input)
  {
    var result = await _deptService.GetTreeAsync(input);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 修改部门状态
  /// </summary>
  /// <param name="input">状态修改参数</param>
  [HttpPut("status")]
  public async Task<IActionResult> ChangeStatusAsync([FromBody] LeanChangeDeptStatusDto input)
  {
    await _deptService.ChangeStatusAsync(input);
    return Success(LeanBusinessType.Update);
  }

  /// <summary>
  /// 获取角色部门树
  /// </summary>
  /// <param name="roleId">角色ID</param>
  /// <returns>角色部门树</returns>
  [HttpGet("role/{roleId}")]
  public async Task<IActionResult> GetRoleDeptTreeAsync(long roleId)
  {
    var result = await _deptService.GetRoleDeptTreeAsync(roleId);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 获取用户部门树
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <returns>用户部门树</returns>
  [HttpGet("user/{userId}")]
  public async Task<IActionResult> GetUserDeptTreeAsync(long userId)
  {
    var result = await _deptService.GetUserDeptTreeAsync(userId);
    return Success(result, LeanBusinessType.Query);
  }
}