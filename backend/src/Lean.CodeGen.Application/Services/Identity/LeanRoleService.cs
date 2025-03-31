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
using Lean.CodeGen.Common.Excel;

namespace Lean.CodeGen.Application.Services.Identity;

/// <summary>
/// 角色服务实现
/// </summary>
/// <remarks>
/// 提供角色管理相关的业务功能，包括：
/// 1. 角色的增删改查
/// 2. 角色状态管理
/// 3. 角色菜单管理
/// 4. 角色数据权限管理
/// </remarks>
public class LeanRoleService : LeanBaseService, ILeanRoleService
{
  private readonly ILeanRepository<LeanRole> _roleRepository;
  private readonly ILeanRepository<LeanRoleMenu> _roleMenuRepository;
  private readonly ILeanRepository<LeanRoleDept> _roleDeptRepository;
  private readonly LeanUniqueValidator<LeanRole> _uniqueValidator;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="roleRepository">角色仓储接口</param>
  /// <param name="roleMenuRepository">角色菜单关联仓储接口</param>
  /// <param name="roleDeptRepository">角色部门关联仓储接口</param>
  /// <param name="context">基础服务上下文</param>
  public LeanRoleService(
      ILeanRepository<LeanRole> roleRepository,
      ILeanRepository<LeanRoleMenu> roleMenuRepository,
      ILeanRepository<LeanRoleDept> roleDeptRepository,
      LeanBaseServiceContext context)
      : base(context)
  {
    _roleRepository = roleRepository;
    _roleMenuRepository = roleMenuRepository;
    _roleDeptRepository = roleDeptRepository;
    _uniqueValidator = new LeanUniqueValidator<LeanRole>(_roleRepository);
  }

  /// <summary>
  /// 分页查询角色
  /// </summary>
  public async Task<LeanApiResult<LeanPageResult<LeanRoleDto>>> GetPageAsync(LeanRoleQueryDto input)
  {
    using (LogPerformance("查询角色列表"))
    {
      // 构建查询条件
      Expression<Func<LeanRole, bool>> predicate = x => true;

      if (!string.IsNullOrEmpty(input.RoleName))
      {
        var roleName = CleanInput(input.RoleName);
        predicate = predicate.And(x => x.RoleName.Contains(roleName));
      }

      if (!string.IsNullOrEmpty(input.RoleCode))
      {
        var roleCode = CleanInput(input.RoleCode);
        predicate = predicate.And(x => x.RoleCode.Contains(roleCode));
      }

      if (input.RoleStatus.HasValue)
      {
        predicate = predicate.And(x => x.RoleStatus == input.RoleStatus);
      }

      if (input.StartTime.HasValue)
      {
        predicate = predicate.And(x => x.CreateTime >= input.StartTime);
      }

      if (input.EndTime.HasValue)
      {
        predicate = predicate.And(x => x.CreateTime <= input.EndTime);
      }

      // 执行分页查询
      var result = await _roleRepository.GetPageListAsync(
          predicate,
          input.PageSize,
          input.PageIndex,
          x => x.CreateTime,
          false);

      var list = result.Items.Adapt<List<LeanRoleDto>>();
      var pageResult = new LeanPageResult<LeanRoleDto>
      {
        Total = result.Total,
        Items = list,
        PageIndex = input.PageIndex,
        PageSize = input.PageSize
      };

      return LeanApiResult<LeanPageResult<LeanRoleDto>>.Ok(pageResult);
    }
  }

  /// <summary>
  /// 获取角色信息
  /// </summary>
  public async Task<LeanApiResult<LeanRoleDto>> GetAsync(long id)
  {
    var role = await _roleRepository.GetByIdAsync(id);
    if (role == null)
    {
      throw new LeanException("角色不存在");
    }

    var result = role.Adapt<LeanRoleDto>();

    // 获取角色菜单
    var roleMenus = await _roleMenuRepository.GetListAsync(x => x.RoleId == id);
    result.MenuIds = roleMenus.Select(x => x.MenuId).ToList();

    // 获取角色部门
    var roleDepts = await _roleDeptRepository.GetListAsync(x => x.RoleId == id);
    result.DataScopeDeptIds = roleDepts.Select(x => x.DeptId).ToList();

    return LeanApiResult<LeanRoleDto>.Ok(result);
  }

  /// <summary>
  /// 创建角色
  /// </summary>
  public async Task<LeanApiResult<long>> CreateAsync(LeanRoleCreateDto input)
  {
    return await ExecuteInTransactionAsync(async () =>
    {
      // 验证角色名称唯一性
      await _uniqueValidator.ValidateAsync(x => x.RoleName, input.RoleName);

      // 验证角色编码唯一性
      if (!string.IsNullOrEmpty(input.RoleCode))
      {
        await _uniqueValidator.ValidateAsync(x => x.RoleCode, input.RoleCode);
      }

      // 创建角色实体
      var role = input.Adapt<LeanRole>();
      await _roleRepository.CreateAsync(role);

      LogAudit("CreateRole", $"创建角色: {role.RoleName}");
      return LeanApiResult<long>.Ok(role.Id);
    }, "创建角色");
  }

  /// <summary>
  /// 更新角色
  /// </summary>
  public async Task<LeanApiResult> UpdateAsync(LeanRoleUpdateDto input)
  {
    return await ExecuteInTransactionAsync(async () =>
    {
      // 获取角色
      var role = await _roleRepository.GetByIdAsync(input.Id);
      if (role == null)
      {
        throw new LeanException("角色不存在");
      }

      // 验证内置角色
      if (role.IsBuiltin == 1)
      {
        throw new LeanException("内置角色不允许修改");
      }

      // 更新角色
      role = input.Adapt(role);
      await _roleRepository.UpdateAsync(role);

      LogAudit("UpdateRole", $"更新角色: {role.RoleName}");
      return LeanApiResult.Ok();
    }, "更新角色");
  }

  /// <summary>
  /// 删除角色
  /// </summary>
  public async Task<LeanApiResult> DeleteAsync(long id)
  {
    return await ExecuteInTransactionAsync(async () =>
    {
      // 获取角色
      var role = await _roleRepository.GetByIdAsync(id);
      if (role == null)
      {
        throw new LeanException("角色不存在");
      }

      // 验证内置角色
      if (role.IsBuiltin == 1)
      {
        throw new LeanException("内置角色不允许删除");
      }

      // 删除角色菜单关联
      await _roleMenuRepository.DeleteAsync(x => x.RoleId == id);

      // 删除角色部门关联
      await _roleDeptRepository.DeleteAsync(x => x.RoleId == id);

      // 删除角色
      await _roleRepository.DeleteAsync(role);

      LogAudit("DeleteRole", $"删除角色: {role.RoleName}");
      return LeanApiResult.Ok();
    }, "删除角色");
  }

  /// <summary>
  /// 批量删除角色
  /// </summary>
  public async Task<LeanApiResult> BatchDeleteAsync(List<long> ids)
  {
    foreach (var id in ids)
    {
      await DeleteAsync(id);
    }
    return LeanApiResult.Ok();
  }

  /// <summary>
  /// 导出角色数据
  /// </summary>
  public async Task<byte[]> ExportAsync(LeanRoleQueryDto input)
  {
    var roles = await GetPageAsync(input);
    var exportDtos = roles.Data.Items.Select(x => new LeanRoleExportDto
    {
      RoleName = x.RoleName,
      RoleCode = x.RoleCode,
      RoleStatus = x.RoleStatus,
      OrderNum = x.OrderNum,
      IsBuiltin = x.IsBuiltin,
      CreateTime = x.CreateTime,
      DataScope = x.DataScope,
      RoleDescription = x.Remark
    }).ToList();
    return LeanExcelHelper.Export(exportDtos);
  }

  /// <summary>
  /// 获取导入模板
  /// </summary>
  public async Task<byte[]> GetTemplateAsync()
  {
    var template = new List<LeanRoleImportDto>
        {
            new LeanRoleImportDto
            {
                RoleName = "示例角色",
                RoleCode = "ROLE001",
                OrderNum = 1
            }
        };

    return await Task.FromResult(LeanExcelHelper.Export(template));
  }

  /// <summary>
  /// 导入角色数据
  /// </summary>
  public async Task<LeanRoleImportResultDto> ImportAsync(LeanFileInfo file)
  {
    var result = new LeanRoleImportResultDto();
    try
    {
      var bytes = new byte[file.Stream.Length];
      await file.Stream.ReadAsync(bytes, 0, (int)file.Stream.Length);
      var importResult = LeanExcelHelper.Import<LeanRoleImportDto>(bytes);

      foreach (var item in importResult.Data)
      {
        if (await _roleRepository.AnyAsync(x => x.RoleCode == item.RoleCode))
        {
          result.AddError(item.RoleCode, $"角色编码 {item.RoleCode} 已存在");
          continue;
        }
        var role = item.Adapt<LeanRole>();
        role.RoleStatus = 2;
        role.IsBuiltin = 0;
        await _roleRepository.CreateAsync(role);
        result.SuccessCount++;
      }
      return result;
    }
    catch (Exception ex)
    {
      result.ErrorMessage = $"导入失败：{ex.Message}";
      return result;
    }
  }

  /// <summary>
  /// 获取角色的菜单权限
  /// </summary>
  public async Task<List<long>> GetRoleMenusAsync(long roleId)
  {
    var roleMenus = await _roleMenuRepository.GetListAsync(x => x.RoleId == roleId);
    return roleMenus.Select(x => x.MenuId).ToList();
  }

  /// <summary>
  /// 设置角色的菜单权限
  /// </summary>
  public async Task<LeanApiResult> SetRoleMenusAsync(LeanRoleSetMenusDto input)
  {
    await AssignMenusAsync(input.Adapt<LeanRoleMenuAssignDto>());
    return LeanApiResult.Ok();
  }

  /// <summary>
  /// 分配菜单权限
  /// </summary>
  private async Task AssignMenusAsync(LeanRoleMenuAssignDto input)
  {
    // 删除原有菜单关联
    await _roleMenuRepository.DeleteAsync(x => x.RoleId == input.Id);

    // 添加新的菜单关联
    if (input.MenuIds?.Any() == true)
    {
      var roleMenus = input.MenuIds.Select(menuId => new LeanRoleMenu
      {
        RoleId = input.Id,
        MenuId = menuId
      }).ToList();

      await _roleMenuRepository.CreateRangeAsync(roleMenus);
    }
  }
}