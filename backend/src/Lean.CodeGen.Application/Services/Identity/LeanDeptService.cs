using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using Mapster;
using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Application.Services.Base;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Exceptions;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Domain.Entities.Identity;
using Lean.CodeGen.Domain.Interfaces.Repositories;
using Lean.CodeGen.Domain.Validators;
using Lean.CodeGen.Common.Extensions;

namespace Lean.CodeGen.Application.Services.Identity;

/// <summary>
/// 部门服务实现
/// </summary>
/// <remarks>
/// 提供部门管理相关的业务功能，包括：
/// 1. 部门的增删改查
/// 2. 部门状态管理
/// 3. 部门树形结构管理
/// 4. 部门数据权限管理
/// </remarks>
public class LeanDeptService : LeanBaseService, ILeanDeptService
{
  private readonly ILeanRepository<LeanDept> _deptRepository;
  private readonly ILeanRepository<LeanRoleDept> _roleDeptRepository;
  private readonly ILeanRepository<LeanUserDept> _userDeptRepository;
  private readonly LeanUniqueValidator<LeanDept> _uniqueValidator;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="deptRepository">部门仓储接口</param>
  /// <param name="roleDeptRepository">角色部门关联仓储接口</param>
  /// <param name="userDeptRepository">用户部门关联仓储接口</param>
  /// <param name="context">基础服务上下文</param>
  public LeanDeptService(
      ILeanRepository<LeanDept> deptRepository,
      ILeanRepository<LeanRoleDept> roleDeptRepository,
      ILeanRepository<LeanUserDept> userDeptRepository,
      LeanBaseServiceContext context)
      : base(context)
  {
    _deptRepository = deptRepository;
    _roleDeptRepository = roleDeptRepository;
    _userDeptRepository = userDeptRepository;
    _uniqueValidator = new LeanUniqueValidator<LeanDept>(_deptRepository);
  }

  /// <summary>
  /// 创建部门
  /// </summary>
  /// <param name="input">部门创建参数</param>
  /// <returns>创建成功的部门信息</returns>
  public async Task<LeanDeptDto> CreateAsync(LeanCreateDeptDto input)
  {
    return await ExecuteInTransactionAsync(async () =>
    {
      // 验证部门名称唯一性
      await _uniqueValidator.ValidateAsync(x => x.DeptName, input.DeptName);

      // 创建部门实体
      var dept = input.Adapt<LeanDept>();
      await _deptRepository.CreateAsync(dept);

      LogAudit("CreateDept", $"创建部门: {dept.DeptName}");
      return await GetAsync(dept.Id);
    }, "创建部门");
  }

  /// <summary>
  /// 更新部门
  /// </summary>
  /// <param name="input">部门更新参数</param>
  /// <returns>更新后的部门信息</returns>
  public async Task<LeanDeptDto> UpdateAsync(LeanUpdateDeptDto input)
  {
    return await ExecuteInTransactionAsync(async () =>
    {
      var dept = await _deptRepository.GetByIdAsync(input.Id);
      if (dept == null)
      {
        throw new LeanException("部门不存在");
      }

      if (dept.IsBuiltin == LeanBuiltinStatus.Yes)
      {
        throw new LeanException("内置部门不能修改");
      }

      // 验证部门名称唯一性
      await _uniqueValidator.ValidateAsync(x => x.DeptName, input.DeptName, input.Id);

      // 更新部门信息
      input.Adapt(dept);
      await _deptRepository.UpdateAsync(dept);

      LogAudit("UpdateDept", $"更新部门: {dept.DeptName}");
      return await GetAsync(dept.Id);
    }, "更新部门");
  }

  /// <summary>
  /// 删除部门
  /// </summary>
  /// <param name="input">部门删除参数</param>
  public async Task DeleteAsync(LeanDeleteDeptDto input)
  {
    await ExecuteInTransactionAsync(async () =>
    {
      var dept = await _deptRepository.GetByIdAsync(input.Id);
      if (dept == null)
      {
        throw new LeanException("部门不存在");
      }

      if (dept.IsBuiltin == LeanBuiltinStatus.Yes)
      {
        throw new LeanException("内置部门不能删除");
      }

      // 检查是否存在子部门
      var hasChildren = await _deptRepository.AnyAsync(x => x.ParentId == input.Id);
      if (hasChildren)
      {
        throw new LeanException("存在子部门，不能删除");
      }

      // 删除角色部门关联
      await _roleDeptRepository.DeleteAsync(x => x.DeptId == input.Id);

      // 删除用户部门关联
      await _userDeptRepository.DeleteAsync(x => x.DeptId == input.Id);

      // 删除部门
      await _deptRepository.DeleteAsync(dept);

      LogAudit("DeleteDept", $"删除部门: {dept.DeptName}");
    }, "删除部门");
  }

  /// <summary>
  /// 获取部门信息
  /// </summary>
  /// <param name="id">部门ID</param>
  /// <returns>部门详细信息</returns>
  public async Task<LeanDeptDto> GetAsync(long id)
  {
    var dept = await _deptRepository.GetByIdAsync(id);
    if (dept == null)
    {
      throw new LeanException("部门不存在");
    }

    return dept.Adapt<LeanDeptDto>();
  }

  /// <summary>
  /// 查询部门列表
  /// </summary>
  /// <param name="input">查询参数</param>
  /// <returns>部门列表</returns>
  public async Task<List<LeanDeptDto>> QueryAsync(LeanQueryDeptDto input)
  {
    using (LogPerformance("查询部门列表"))
    {
      // 构建查询条件
      Expression<Func<LeanDept, bool>> predicate = x => true;

      if (!string.IsNullOrEmpty(input.DeptName))
      {
        var deptName = CleanInput(input.DeptName);
        predicate = predicate.And(x => x.DeptName.Contains(deptName));
      }

      if (input.DeptStatus.HasValue)
      {
        predicate = predicate.And(x => x.DeptStatus == input.DeptStatus);
      }

      if (input.ParentId.HasValue)
      {
        predicate = predicate.And(x => x.ParentId == input.ParentId);
      }

      // 执行查询
      var list = await _deptRepository.GetListAsync(predicate);
      return list.OrderBy(x => x.OrderNum).Adapt<List<LeanDeptDto>>();
    }
  }

  /// <summary>
  /// 获取部门树形结构
  /// </summary>
  /// <param name="input">查询参数</param>
  /// <returns>部门树形结构</returns>
  public async Task<List<LeanDeptTreeDto>> GetTreeAsync(LeanQueryDeptDto input)
  {
    // 获取部门列表
    var depts = await QueryAsync(input);

    // 构建树形结构
    return BuildDeptTree(depts);
  }

  /// <summary>
  /// 修改部门状态
  /// </summary>
  /// <param name="input">状态修改参数</param>
  public async Task ChangeStatusAsync(LeanChangeDeptStatusDto input)
  {
    await ExecuteInTransactionAsync(async () =>
    {
      var dept = await _deptRepository.GetByIdAsync(input.Id);
      if (dept == null)
      {
        throw new LeanException("部门不存在");
      }

      if (dept.IsBuiltin == LeanBuiltinStatus.Yes)
      {
        throw new LeanException("内置部门不能修改状态");
      }

      dept.DeptStatus = input.DeptStatus;
      await _deptRepository.UpdateAsync(dept);

      LogAudit("ChangeDeptStatus", $"修改部门状态: {dept.DeptName}, 状态: {input.DeptStatus}");
    }, "修改部门状态");
  }

  /// <summary>
  /// 获取角色部门树
  /// </summary>
  /// <param name="roleId">角色ID</param>
  /// <returns>角色部门树</returns>
  public async Task<List<LeanDeptTreeDto>> GetRoleDeptTreeAsync(long roleId)
  {
    // 获取角色部门
    var roleDepts = await _roleDeptRepository.GetListAsync(x => x.RoleId == roleId);
    var deptIds = roleDepts.Select(x => x.DeptId).ToList();

    // 获取部门列表
    var depts = await _deptRepository.GetListAsync(
        x => deptIds.Contains(x.Id) && x.DeptStatus == LeanDeptStatus.Normal);

    // 转换为DTO并构建树形结构
    var deptDtos = depts.OrderBy(x => x.OrderNum)
                       .Select(x => x.Adapt<LeanDeptDto>())
                       .ToList();
    return BuildDeptTree(deptDtos);
  }

  /// <summary>
  /// 获取用户部门树
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <returns>用户部门树</returns>
  public async Task<List<LeanDeptTreeDto>> GetUserDeptTreeAsync(long userId)
  {
    // 获取用户部门
    var userDepts = await _userDeptRepository.GetListAsync(x => x.UserId == userId);
    var deptIds = userDepts.Select(x => x.DeptId).ToList();

    // 获取部门列表
    var depts = await _deptRepository.GetListAsync(
        x => deptIds.Contains(x.Id) && x.DeptStatus == LeanDeptStatus.Normal);

    // 转换为DTO并构建树形结构
    var deptDtos = depts.OrderBy(x => x.OrderNum)
                       .Select(x => x.Adapt<LeanDeptDto>())
                       .ToList();
    return BuildDeptTree(deptDtos);
  }

  /// <summary>
  /// 构建部门树形结构
  /// </summary>
  /// <param name="depts">部门列表</param>
  /// <returns>树形结构</returns>
  private List<LeanDeptTreeDto> BuildDeptTree(List<LeanDeptDto> depts)
  {
    var result = new List<LeanDeptTreeDto>();
    var lookup = depts.ToLookup(x => x.ParentId);

    foreach (var dept in depts.Where(x => x.ParentId == null || x.ParentId == 0))
    {
      var node = dept.Adapt<LeanDeptTreeDto>();
      node.Children = BuildDeptTreeNodes(lookup, dept.Id);
      result.Add(node);
    }

    return result;
  }

  /// <summary>
  /// 递归构建部门树节点
  /// </summary>
  /// <param name="lookup">部门查找表</param>
  /// <param name="parentId">父级ID</param>
  /// <returns>子节点列表</returns>
  private List<LeanDeptTreeDto> BuildDeptTreeNodes(ILookup<long?, LeanDeptDto> lookup, long parentId)
  {
    var result = new List<LeanDeptTreeDto>();

    foreach (var dept in lookup[parentId])
    {
      var node = dept.Adapt<LeanDeptTreeDto>();
      node.Children = BuildDeptTreeNodes(lookup, dept.Id);
      result.Add(node);
    }

    return result;
  }
}