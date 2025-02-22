using System.Linq.Expressions;
using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Domain.Entities.Identity;
using Lean.CodeGen.Domain.Interfaces.Repositories;
using Lean.CodeGen.Domain.Validators;
using Mapster;
using Lean.CodeGen.Common.Options;
using Lean.CodeGen.Application.Services.Base;
using Lean.CodeGen.Application.Services.Security;
using Microsoft.Extensions.Options;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Extensions;

namespace Lean.CodeGen.Application.Services.Identity;

/// <summary>
/// 部门服务实现
/// </summary>
public class LeanDeptService : LeanBaseService, ILeanDeptService
{
  private readonly ILeanRepository<LeanDept> _deptRepository;
  private readonly ILeanRepository<LeanUserDept> _userDeptRepository;
  private readonly ILeanRepository<LeanRoleDept> _roleDeptRepository;
  private readonly ILeanRepository<LeanDeptDataPermission> _deptDataPermissionRepository;
  private readonly LeanUniqueValidator<LeanDept> _uniqueValidator;

  public LeanDeptService(
      ILeanRepository<LeanDept> deptRepository,
      ILeanRepository<LeanUserDept> userDeptRepository,
      ILeanRepository<LeanRoleDept> roleDeptRepository,
      ILeanRepository<LeanDeptDataPermission> deptDataPermissionRepository,
      ILeanSqlSafeService sqlSafeService,
      IOptions<LeanSecurityOptions> securityOptions)
      : base(sqlSafeService, securityOptions)
  {
    _deptRepository = deptRepository;
    _userDeptRepository = userDeptRepository;
    _roleDeptRepository = roleDeptRepository;
    _deptDataPermissionRepository = deptDataPermissionRepository;
    _uniqueValidator = new LeanUniqueValidator<LeanDept>(deptRepository);
  }

  /// <summary>
  /// 创建部门
  /// </summary>
  public async Task<LeanApiResult<long>> CreateAsync(LeanCreateDeptDto input)
  {
    try
    {
      if (!await ValidateDeptInputAsync(input.DeptName, input.DeptCode))
      {
        return LeanApiResult<long>.Error("输入参数验证失败");
      }

      var dept = input.Adapt<LeanDept>();
      dept.CreateTime = DateTime.Now;

      await _deptRepository.CreateAsync(dept);

      return LeanApiResult<long>.Ok(dept.Id);
    }
    catch (Exception ex)
    {
      return LeanApiResult<long>.Error($"创建部门失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 更新部门
  /// </summary>
  public async Task<LeanApiResult> UpdateAsync(LeanUpdateDeptDto input)
  {
    try
    {
      var dept = await GetDeptByIdAsync(input.Id);
      if (dept == null)
      {
        return LeanApiResult.Error($"部门 {input.Id} 不存在");
      }

      if (!await ValidateDeptInputAsync(input.DeptName, input.DeptCode, input.Id))
      {
        return LeanApiResult.Error("输入参数验证失败");
      }

      input.Adapt(dept);
      dept.UpdateTime = DateTime.Now;

      await _deptRepository.UpdateAsync(dept);

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"更新部门失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 删除部门
  /// </summary>
  public async Task<LeanApiResult> DeleteAsync(long id)
  {
    try
    {
      var dept = await _deptRepository.GetByIdAsync(id);
      if (dept == null)
      {
        return LeanApiResult.Error($"部门 {id} 不存在");
      }

      if (dept.IsBuiltin == LeanBuiltinStatus.Yes)
      {
        return LeanApiResult.Error($"部门 {dept.DeptName} 是内置部门，不能删除");
      }

      // 检查是否有子部门
      var hasChildren = await _deptRepository.AnyAsync(d => d.ParentId == id);
      if (hasChildren)
      {
        return LeanApiResult.Error($"部门 {dept.DeptName} 存在子部门，不能删除");
      }

      await DeleteDeptRelationsAsync(id);
      await _deptRepository.DeleteAsync(d => d.Id == id);

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"删除部门失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 批量删除部门
  /// </summary>
  public async Task<LeanApiResult> BatchDeleteAsync(List<long> ids)
  {
    try
    {
      foreach (var id in ids)
      {
        var result = await DeleteAsync(id);
        if (!result.Success)
        {
          return result;
        }
      }

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"批量删除部门失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取部门信息
  /// </summary>
  public async Task<LeanApiResult<LeanDeptDto>> GetAsync(long id)
  {
    try
    {
      var dept = await GetDeptByIdAsync(id);
      if (dept == null)
      {
        return LeanApiResult<LeanDeptDto>.Error($"部门 {id} 不存在");
      }

      var result = dept.Adapt<LeanDeptDto>();

      return LeanApiResult<LeanDeptDto>.Ok(result);
    }
    catch (Exception ex)
    {
      return LeanApiResult<LeanDeptDto>.Error($"获取部门信息失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 分页查询部门
  /// </summary>
  public async Task<LeanApiResult<LeanPageResult<LeanDeptDto>>> GetPageAsync(LeanQueryDeptDto input)
  {
    try
    {
      var predicate = BuildDeptQueryPredicate(input);
      var (total, items) = await _deptRepository.GetPageListAsync(predicate, input.PageSize, input.PageIndex);

      var result = new LeanPageResult<LeanDeptDto>
      {
        Total = total,
        Items = items.Select(d => d.Adapt<LeanDeptDto>()).ToList()
      };

      return LeanApiResult<LeanPageResult<LeanDeptDto>>.Ok(result);
    }
    catch (Exception ex)
    {
      return LeanApiResult<LeanPageResult<LeanDeptDto>>.Error($"查询部门失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 修改部门状态
  /// </summary>
  public async Task<LeanApiResult> SetStatusAsync(LeanChangeDeptStatusDto input)
  {
    try
    {
      var dept = await GetDeptByIdAsync(input.Id);
      if (dept == null)
      {
        return LeanApiResult.Error($"部门 {input.Id} 不存在");
      }

      dept.DeptStatus = input.DeptStatus;
      dept.UpdateTime = DateTime.Now;

      await _deptRepository.UpdateAsync(dept);

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"修改部门状态失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取部门树形结构
  /// </summary>
  public async Task<LeanApiResult<List<LeanDeptTreeDto>>> GetTreeAsync()
  {
    try
    {
      var depts = await _deptRepository.GetListAsync(d => true);
      var result = BuildDeptTree(depts);

      return LeanApiResult<List<LeanDeptTreeDto>>.Ok(result);
    }
    catch (Exception ex)
    {
      return LeanApiResult<List<LeanDeptTreeDto>>.Error($"获取部门树失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取用户部门树
  /// </summary>
  public async Task<LeanApiResult<List<LeanDeptTreeDto>>> GetUserDeptTreeAsync(long userId)
  {
    try
    {
      var userDepts = await _userDeptRepository.GetListAsync(ud => ud.UserId == userId);
      var deptIds = userDepts.Select(ud => ud.DeptId).ToList();
      var depts = await _deptRepository.GetListAsync(d => deptIds.Contains(d.Id));
      var result = BuildDeptTree(depts);

      return LeanApiResult<List<LeanDeptTreeDto>>.Ok(result);
    }
    catch (Exception ex)
    {
      return LeanApiResult<List<LeanDeptTreeDto>>.Error($"获取用户部门树失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取角色部门树
  /// </summary>
  public async Task<LeanApiResult<List<LeanDeptTreeDto>>> GetRoleDeptTreeAsync(long roleId)
  {
    try
    {
      var roleDepts = await _roleDeptRepository.GetListAsync(rd => rd.RoleId == roleId);
      var deptIds = roleDepts.Select(rd => rd.DeptId).ToList();
      var depts = await _deptRepository.GetListAsync(d => deptIds.Contains(d.Id));
      var result = BuildDeptTree(depts);

      return LeanApiResult<List<LeanDeptTreeDto>>.Ok(result);
    }
    catch (Exception ex)
    {
      return LeanApiResult<List<LeanDeptTreeDto>>.Error($"获取角色部门树失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取部门的所有下级部门ID列表
  /// </summary>
  public async Task<LeanApiResult<List<long>>> GetDeptChildrenIdsAsync(long deptId)
  {
    try
    {
      var childrenIds = new List<long>();
      await GetChildrenIdsRecursiveAsync(deptId, childrenIds);
      return LeanApiResult<List<long>>.Ok(childrenIds);
    }
    catch (Exception ex)
    {
      return LeanApiResult<List<long>>.Error($"获取部门下级ID列表失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取部门的所有上级部门ID列表
  /// </summary>
  public async Task<LeanApiResult<List<long>>> GetDeptParentIdsAsync(long deptId)
  {
    try
    {
      var parentIds = new List<long>();
      await GetParentIdsRecursiveAsync(deptId, parentIds);
      return LeanApiResult<List<long>>.Ok(parentIds);
    }
    catch (Exception ex)
    {
      return LeanApiResult<List<long>>.Error($"获取部门上级ID列表失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取用户的所有部门ID列表
  /// </summary>
  public async Task<LeanApiResult<List<long>>> GetUserDeptIdsAsync(long userId)
  {
    try
    {
      var userDepts = await _userDeptRepository.GetListAsync(ud => ud.UserId == userId);
      var deptIds = userDepts.Select(ud => ud.DeptId).ToList();
      return LeanApiResult<List<long>>.Ok(deptIds);
    }
    catch (Exception ex)
    {
      return LeanApiResult<List<long>>.Error($"获取用户部门ID列表失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取角色的所有部门ID列表
  /// </summary>
  public async Task<LeanApiResult<List<long>>> GetRoleDeptIdsAsync(long roleId)
  {
    try
    {
      var roleDepts = await _roleDeptRepository.GetListAsync(rd => rd.RoleId == roleId);
      var deptIds = roleDepts.Select(rd => rd.DeptId).ToList();
      return LeanApiResult<List<long>>.Ok(deptIds);
    }
    catch (Exception ex)
    {
      return LeanApiResult<List<long>>.Error($"获取角色部门ID列表失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取用户的主部门
  /// </summary>
  public async Task<LeanApiResult<LeanDeptDto>> GetUserPrimaryDeptAsync(long userId)
  {
    try
    {
      var userDept = await _userDeptRepository.FirstOrDefaultAsync(ud => ud.UserId == userId && ud.IsPrimary == LeanPrimaryStatus.Yes);
      if (userDept == null)
      {
        return LeanApiResult<LeanDeptDto>.Error($"用户 {userId} 没有主部门");
      }

      var dept = await GetDeptByIdAsync(userDept.DeptId);
      if (dept == null)
      {
        return LeanApiResult<LeanDeptDto>.Error($"部门 {userDept.DeptId} 不存在");
      }

      var result = dept.Adapt<LeanDeptDto>();
      return LeanApiResult<LeanDeptDto>.Ok(result);
    }
    catch (Exception ex)
    {
      return LeanApiResult<LeanDeptDto>.Error($"获取用户主部门失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 验证部门编码是否唯一
  /// </summary>
  public async Task<LeanApiResult<bool>> ValidateDeptCodeUniqueAsync(string deptCode, long? id = null)
  {
    try
    {
      var exists = await _deptRepository.AnyAsync(d => d.DeptCode == deptCode && (id == null || d.Id != id));
      return LeanApiResult<bool>.Ok(!exists);
    }
    catch (Exception ex)
    {
      return LeanApiResult<bool>.Error($"验证部门编码唯一性失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取部门的所有用户ID列表
  /// </summary>
  public async Task<LeanApiResult<List<long>>> GetDeptUserIdsAsync(long deptId, bool includeChildDepts = false)
  {
    try
    {
      var deptIds = new List<long> { deptId };
      if (includeChildDepts)
      {
        var childrenIds = new List<long>();
        await GetChildrenIdsRecursiveAsync(deptId, childrenIds);
        deptIds.AddRange(childrenIds);
      }

      var userDepts = await _userDeptRepository.GetListAsync(ud => deptIds.Contains(ud.DeptId));
      var userIds = userDepts.Select(ud => ud.UserId).Distinct().ToList();
      return LeanApiResult<List<long>>.Ok(userIds);
    }
    catch (Exception ex)
    {
      return LeanApiResult<List<long>>.Error($"获取部门用户ID列表失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取部门的所有角色ID列表
  /// </summary>
  public async Task<LeanApiResult<List<long>>> GetDeptRoleIdsAsync(long deptId, bool includeChildDepts = false)
  {
    try
    {
      var deptIds = new List<long> { deptId };
      if (includeChildDepts)
      {
        var childrenIds = new List<long>();
        await GetChildrenIdsRecursiveAsync(deptId, childrenIds);
        deptIds.AddRange(childrenIds);
      }

      var roleDepts = await _roleDeptRepository.GetListAsync(rd => deptIds.Contains(rd.DeptId));
      var roleIds = roleDepts.Select(rd => rd.RoleId).Distinct().ToList();
      return LeanApiResult<List<long>>.Ok(roleIds);
    }
    catch (Exception ex)
    {
      return LeanApiResult<List<long>>.Error($"获取部门角色ID列表失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 导入部门数据
  /// </summary>
  public async Task<LeanApiResult<LeanImportDeptResultDto>> ImportAsync(List<LeanImportTemplateDeptDto> depts)
  {
    try
    {
      var result = new LeanImportDeptResultDto();

      foreach (var dept in depts)
      {
        try
        {
          if (!await ValidateDeptInputAsync(dept.DeptName, dept.DeptCode))
          {
            result.ErrorMessages.Add($"部门 {dept.DeptName} 验证失败");
            continue;
          }

          var parentDept = string.IsNullOrEmpty(dept.ParentDeptCode)
              ? null
              : await _deptRepository.FirstOrDefaultAsync(d => d.DeptCode == dept.ParentDeptCode);

          var newDept = new LeanDept
          {
            ParentId = parentDept?.Id,
            DeptName = dept.DeptName,
            DeptCode = dept.DeptCode,
            CreateTime = DateTime.Now
          };

          await _deptRepository.CreateAsync(newDept);
          result.SuccessCount++;
        }
        catch (Exception ex)
        {
          result.ErrorMessages.Add($"部门 {dept.DeptName} 导入失败: {ex.Message}");
          result.FailCount++;
        }
      }

      return LeanApiResult<LeanImportDeptResultDto>.Ok(result);
    }
    catch (Exception ex)
    {
      return LeanApiResult<LeanImportDeptResultDto>.Error($"导入部门数据失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 导出部门数据
  /// </summary>
  public async Task<LeanApiResult<byte[]>> ExportAsync(LeanExportDeptDto input)
  {
    try
    {
      var predicate = BuildDeptQueryPredicate(input);
      var depts = await _deptRepository.GetListAsync(predicate);
      var deptDtos = depts.Select(d => d.Adapt<LeanDeptDto>()).ToList();

      // TODO: 实现导出逻辑，生成Excel文件
      var excelBytes = new byte[] { };

      return LeanApiResult<byte[]>.Ok(excelBytes);
    }
    catch (Exception ex)
    {
      return LeanApiResult<byte[]>.Error($"导出部门数据失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取用户可访问的部门数据范围
  /// </summary>
  public async Task<LeanApiResult<List<long>>> GetUserDataScopeDeptIdsAsync(long userId)
  {
    try
    {
      // 获取用户所属部门
      var userDepts = await _userDeptRepository.GetListAsync(ud => ud.UserId == userId);
      var userDeptIds = userDepts.Select(ud => ud.DeptId).ToList();

      // 获取用户角色的数据权限
      var roleDepts = await _roleDeptRepository.GetListAsync(rd => rd.Role.UserRoles.Any(ur => ur.UserId == userId));
      var roleDeptIds = roleDepts.Select(rd => rd.DeptId).ToList();

      // 合并部门ID
      var deptIds = userDeptIds.Union(roleDeptIds).Distinct().ToList();

      return LeanApiResult<List<long>>.Ok(deptIds);
    }
    catch (Exception ex)
    {
      return LeanApiResult<List<long>>.Error($"获取用户数据范围失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 设置部门数据访问权限
  /// </summary>
  public async Task<LeanApiResult> SetDeptDataPermissionAsync(LeanSetDeptDataPermissionDto input)
  {
    try
    {
      var dept = await _deptRepository.GetByIdAsync(input.DeptId);
      if (dept == null)
      {
        return LeanApiResult.Error($"部门 {input.DeptId} 不存在");
      }

      await _deptDataPermissionRepository.DeleteAsync(dp => dp.DeptId == input.DeptId);

      if (input.AccessibleDeptIds?.Any() == true)
      {
        var permissions = input.AccessibleDeptIds.Select(accessibleDeptId => new LeanDeptDataPermission
        {
          DeptId = input.DeptId,
          AccessibleDeptId = accessibleDeptId,
          DataScope = input.DataScope,
          CreateTime = DateTime.Now
        }).ToList();

        await _deptDataPermissionRepository.CreateRangeAsync(permissions);
      }

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"设置部门数据权限失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 验证用户是否有权限访问指定部门的数据
  /// </summary>
  public async Task<LeanApiResult<bool>> ValidateUserDeptDataPermissionAsync(long userId, long deptId)
  {
    try
    {
      // 获取用户可访问的部门ID列表
      var result = await GetUserDataScopeDeptIdsAsync(userId);
      if (!result.Success)
      {
        return LeanApiResult<bool>.Error(result.Message);
      }

      return LeanApiResult<bool>.Ok(result.Data.Contains(deptId));
    }
    catch (Exception ex)
    {
      return LeanApiResult<bool>.Error($"验证用户部门数据权限失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取部门数据访问策略
  /// </summary>
  public async Task<LeanApiResult<LeanDeptDataPermissionDto>> GetDeptDataPermissionAsync(long deptId)
  {
    try
    {
      var dept = await _deptRepository.GetByIdAsync(deptId);
      if (dept == null)
      {
        return LeanApiResult<LeanDeptDataPermissionDto>.Error($"部门 {deptId} 不存在");
      }

      var permissions = await _deptDataPermissionRepository.GetListAsync(dp => dp.DeptId == deptId);
      var result = new LeanDeptDataPermissionDto
      {
        DataScope = dept.DataScope,
        AccessibleDeptIds = permissions.Select(dp => dp.AccessibleDeptId).ToList()
      };

      return LeanApiResult<LeanDeptDataPermissionDto>.Ok(result);
    }
    catch (Exception ex)
    {
      return LeanApiResult<LeanDeptDataPermissionDto>.Error($"获取部门数据权限失败: {ex.Message}");
    }
  }

  #region 私有方法

  /// <summary>
  /// 获取部门
  /// </summary>
  private async Task<LeanDept?> GetDeptByIdAsync(long id)
  {
    return await _deptRepository.GetByIdAsync(id);
  }

  /// <summary>
  /// 验证部门输入
  /// </summary>
  private async Task<bool> ValidateDeptInputAsync(string deptName, string deptCode, long? id = null)
  {
    // 验证部门名称是否重复
    var exists = await _deptRepository.AnyAsync(d => d.DeptName == deptName && d.Id != id);
    if (exists)
    {
      return false;
    }

    // 验证部门编码是否重复
    exists = await _deptRepository.AnyAsync(d => d.DeptCode == deptCode && d.Id != id);
    if (exists)
    {
      return false;
    }

    return true;
  }

  /// <summary>
  /// 删除部门关联信息
  /// </summary>
  private async Task DeleteDeptRelationsAsync(long deptId)
  {
    await _userDeptRepository.DeleteAsync(ud => ud.DeptId == deptId);
    await _roleDeptRepository.DeleteAsync(rd => rd.DeptId == deptId);
    await _deptDataPermissionRepository.DeleteAsync(dp => dp.DeptId == deptId || dp.AccessibleDeptId == deptId);
  }

  /// <summary>
  /// 递归获取子部门ID列表
  /// </summary>
  private async Task GetChildrenIdsRecursiveAsync(long deptId, List<long> childrenIds)
  {
    var children = await _deptRepository.GetListAsync(d => d.ParentId == deptId);
    foreach (var child in children)
    {
      childrenIds.Add(child.Id);
      await GetChildrenIdsRecursiveAsync(child.Id, childrenIds);
    }
  }

  /// <summary>
  /// 递归获取父部门ID列表
  /// </summary>
  private async Task GetParentIdsRecursiveAsync(long deptId, List<long> parentIds)
  {
    var dept = await GetDeptByIdAsync(deptId);
    if (dept?.ParentId != null)
    {
      parentIds.Add(dept.ParentId.Value);
      await GetParentIdsRecursiveAsync(dept.ParentId.Value, parentIds);
    }
  }

  /// <summary>
  /// 构建部门查询条件
  /// </summary>
  private Expression<Func<LeanDept, bool>> BuildDeptQueryPredicate(LeanQueryDeptDto input)
  {
    Expression<Func<LeanDept, bool>> predicate = d => true;

    if (!string.IsNullOrEmpty(input.DeptName))
    {
      var deptName = CleanInput(input.DeptName);
      predicate = LeanExpressionExtensions.And(predicate, d => d.DeptName.Contains(deptName));
    }

    if (!string.IsNullOrEmpty(input.DeptCode))
    {
      var deptCode = CleanInput(input.DeptCode);
      predicate = LeanExpressionExtensions.And(predicate, d => d.DeptCode.Contains(deptCode));
    }

    if (input.DeptStatus.HasValue)
    {
      predicate = LeanExpressionExtensions.And(predicate, d => d.DeptStatus == input.DeptStatus.Value);
    }

    if (input.StartTime.HasValue)
    {
      predicate = LeanExpressionExtensions.And(predicate, d => d.CreateTime >= input.StartTime.Value);
    }

    if (input.EndTime.HasValue)
    {
      predicate = LeanExpressionExtensions.And(predicate, d => d.CreateTime <= input.EndTime.Value);
    }

    return predicate;
  }

  /// <summary>
  /// 构建部门树
  /// </summary>
  private List<LeanDeptTreeDto> BuildDeptTree(List<LeanDept> depts)
  {
    var deptDict = depts.ToDictionary(d => d.Id, d => d.Adapt<LeanDeptTreeDto>());

    foreach (var dept in depts.Where(d => d.ParentId.HasValue))
    {
      if (deptDict.TryGetValue(dept.ParentId.Value, out var parentDept))
      {
        parentDept.Children.Add(deptDict[dept.Id]);
      }
    }

    return deptDict.Values.Where(d => !d.ParentId.HasValue).ToList();
  }

  private async Task<LeanUserDept?> GetUserDeptAsync(long userId, long deptId)
  {
    return await _userDeptRepository.FirstOrDefaultAsync(ud => ud.UserId == userId && ud.DeptId == deptId);
  }

  private async Task<bool> ValidateDeptExistsAsync(long deptId)
  {
    return await _deptRepository.AnyAsync(d => d.Id == deptId);
  }

  private async Task<LeanDept?> GetFirstDeptAsync(Expression<Func<LeanDept, bool>> predicate)
  {
    return await _deptRepository.FirstOrDefaultAsync(predicate);
  }

  #endregion
}