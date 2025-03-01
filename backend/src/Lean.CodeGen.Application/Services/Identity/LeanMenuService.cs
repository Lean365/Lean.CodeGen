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
/// 菜单服务实现
/// </summary>
/// <remarks>
/// 提供菜单管理相关的业务功能，包括：
/// 1. 菜单的增删改查
/// 2. 菜单状态管理
/// 3. 菜单树形结构管理
/// 4. 菜单权限管理
/// </remarks>
public class LeanMenuService : LeanBaseService, ILeanMenuService
{
  private readonly ILeanRepository<LeanMenu> _menuRepository;
  private readonly ILeanRepository<LeanRoleMenu> _roleMenuRepository;
  private readonly ILeanRepository<LeanUserRole> _userRoleRepository;
  private readonly LeanUniqueValidator<LeanMenu> _uniqueValidator;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="menuRepository">菜单仓储接口</param>
  /// <param name="roleMenuRepository">角色菜单关联仓储接口</param>
  /// <param name="userRoleRepository">用户角色关联仓储接口</param>
  /// <param name="context">基础服务上下文</param>
  public LeanMenuService(
      ILeanRepository<LeanMenu> menuRepository,
      ILeanRepository<LeanRoleMenu> roleMenuRepository,
      ILeanRepository<LeanUserRole> userRoleRepository,
      LeanBaseServiceContext context)
      : base(context)
  {
    _menuRepository = menuRepository;
    _roleMenuRepository = roleMenuRepository;
    _userRoleRepository = userRoleRepository;
    _uniqueValidator = new LeanUniqueValidator<LeanMenu>(_menuRepository);
  }

  /// <summary>
  /// 创建菜单
  /// </summary>
  /// <param name="input">菜单创建参数</param>
  /// <returns>创建成功的菜单信息</returns>
  public async Task<LeanMenuDto> CreateAsync(LeanCreateMenuDto input)
  {
    return await ExecuteInTransactionAsync(async () =>
    {
      // 验证菜单名称唯一性
      await _uniqueValidator.ValidateAsync(x => x.MenuName, input.MenuName);

      // 验证权限标识唯一性
      if (!string.IsNullOrEmpty(input.Perms))
      {
        await _uniqueValidator.ValidateAsync(x => x.Perms, input.Perms);
      }

      // 创建菜单实体
      var menu = input.Adapt<LeanMenu>();
      await _menuRepository.CreateAsync(menu);

      LogAudit("CreateMenu", $"创建菜单: {menu.MenuName}");
      return await GetAsync(menu.Id);
    }, "创建菜单");
  }

  /// <summary>
  /// 更新菜单
  /// </summary>
  /// <param name="input">菜单更新参数</param>
  /// <returns>更新后的菜单信息</returns>
  public async Task<LeanMenuDto> UpdateAsync(LeanUpdateMenuDto input)
  {
    return await ExecuteInTransactionAsync(async () =>
    {
      var menu = await _menuRepository.GetByIdAsync(input.Id);
      if (menu == null)
      {
        throw new LeanException("菜单不存在");
      }

      if (menu.IsBuiltin == LeanBuiltinStatus.Yes)
      {
        throw new LeanException("内置菜单不能修改");
      }

      // 验证菜单名称唯一性
      await _uniqueValidator.ValidateAsync(x => x.MenuName, input.MenuName, input.Id);

      // 验证权限标识唯一性
      if (!string.IsNullOrEmpty(input.Perms))
      {
        await _uniqueValidator.ValidateAsync(x => x.Perms, input.Perms, input.Id);
      }

      // 更新菜单信息
      input.Adapt(menu);
      await _menuRepository.UpdateAsync(menu);

      LogAudit("UpdateMenu", $"更新菜单: {menu.MenuName}");
      return await GetAsync(menu.Id);
    }, "更新菜单");
  }

  /// <summary>
  /// 删除菜单
  /// </summary>
  /// <param name="input">菜单删除参数</param>
  public async Task DeleteAsync(LeanDeleteMenuDto input)
  {
    await ExecuteInTransactionAsync(async () =>
    {
      var menu = await _menuRepository.GetByIdAsync(input.Id);
      if (menu == null)
      {
        throw new LeanException("菜单不存在");
      }

      if (menu.IsBuiltin == LeanBuiltinStatus.Yes)
      {
        throw new LeanException("内置菜单不能删除");
      }

      // 检查是否存在子菜单
      var hasChildren = await _menuRepository.AnyAsync(x => x.ParentId == input.Id);
      if (hasChildren)
      {
        throw new LeanException("存在子菜单，不能删除");
      }

      // 删除角色菜单关联
      await _roleMenuRepository.DeleteAsync(x => x.MenuId == input.Id);

      // 删除菜单
      await _menuRepository.DeleteAsync(menu);

      LogAudit("DeleteMenu", $"删除菜单: {menu.MenuName}");
    }, "删除菜单");
  }

  /// <summary>
  /// 获取菜单信息
  /// </summary>
  /// <param name="id">菜单ID</param>
  /// <returns>菜单详细信息</returns>
  public async Task<LeanMenuDto> GetAsync(long id)
  {
    var menu = await _menuRepository.GetByIdAsync(id);
    if (menu == null)
    {
      throw new LeanException("菜单不存在");
    }

    return menu.Adapt<LeanMenuDto>();
  }

  /// <summary>
  /// 查询菜单列表
  /// </summary>
  /// <param name="input">查询参数</param>
  /// <returns>菜单列表</returns>
  public async Task<List<LeanMenuDto>> QueryAsync(LeanQueryMenuDto input)
  {
    using (LogPerformance("查询菜单列表"))
    {
      // 构建查询条件
      Expression<Func<LeanMenu, bool>> predicate = x => true;

      if (!string.IsNullOrEmpty(input.MenuName))
      {
        var menuName = CleanInput(input.MenuName);
        predicate = predicate.And(x => x.MenuName.Contains(menuName));
      }

      if (!string.IsNullOrEmpty(input.Perms))
      {
        var perms = CleanInput(input.Perms);
        predicate = predicate.And(x => x.Perms.Contains(perms));
      }

      if (input.MenuType.HasValue)
      {
        predicate = predicate.And(x => x.MenuType == input.MenuType);
      }

      if (input.MenuStatus.HasValue)
      {
        predicate = predicate.And(x => x.MenuStatus == input.MenuStatus);
      }

      if (input.ParentId.HasValue)
      {
        predicate = predicate.And(x => x.ParentId == input.ParentId);
      }

      // 执行查询
      var list = await _menuRepository.GetListAsync(predicate);
      return list.OrderBy(x => x.OrderNum).Adapt<List<LeanMenuDto>>();
    }
  }

  /// <summary>
  /// 获取菜单树形结构
  /// </summary>
  /// <param name="input">查询参数</param>
  /// <returns>菜单树形结构</returns>
  public async Task<List<LeanMenuTreeDto>> GetTreeAsync(LeanQueryMenuDto input)
  {
    // 获取菜单列表
    var menus = await QueryAsync(input);

    // 构建树形结构
    return BuildMenuTree(menus);
  }

  /// <summary>
  /// 修改菜单状态
  /// </summary>
  /// <param name="input">状态修改参数</param>
  public async Task ChangeStatusAsync(LeanChangeMenuStatusDto input)
  {
    await ExecuteInTransactionAsync(async () =>
    {
      var menu = await _menuRepository.GetByIdAsync(input.Id);
      if (menu == null)
      {
        throw new LeanException("菜单不存在");
      }

      if (menu.IsBuiltin == LeanBuiltinStatus.Yes)
      {
        throw new LeanException("内置菜单不能修改状态");
      }

      menu.MenuStatus = input.MenuStatus;
      await _menuRepository.UpdateAsync(menu);

      LogAudit("ChangeMenuStatus", $"修改菜单状态: {menu.MenuName}, 状态: {input.MenuStatus}");
    }, "修改菜单状态");
  }

  /// <summary>
  /// 获取角色菜单树
  /// </summary>
  /// <param name="roleId">角色ID</param>
  /// <returns>菜单树形结构</returns>
  public async Task<List<LeanMenuTreeDto>> GetRoleMenuTreeAsync(long roleId)
  {
    // 获取角色菜单
    var roleMenus = await _roleMenuRepository.GetListAsync(x => x.RoleId == roleId);
    var menuIds = roleMenus.Select(x => x.MenuId).ToList();

    // 获取菜单列表
    var menus = await _menuRepository.GetListAsync(
        x => menuIds.Contains(x.Id) && x.MenuStatus == LeanMenuStatus.Normal);

    // 转换为DTO并构建树形结构
    var menuDtos = menus.OrderBy(x => x.OrderNum)
                       .Select(x => x.Adapt<LeanMenuDto>())
                       .ToList();
    return BuildMenuTree(menuDtos);
  }

  /// <summary>
  /// 获取用户菜单树
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <returns>用户菜单树</returns>
  public async Task<List<LeanMenuTreeDto>> GetUserMenuTreeAsync(long userId)
  {
    // 获取用户角色
    var userRoles = await _userRoleRepository.GetListAsync(x => x.UserId == userId);
    var roleIds = userRoles.Select(x => x.RoleId).ToList();

    // 获取角色菜单
    var roleMenus = await _roleMenuRepository.GetListAsync(x => roleIds.Contains(x.RoleId));
    var menuIds = roleMenus.Select(x => x.MenuId).Distinct().ToList();

    // 获取菜单列表
    var menus = await _menuRepository.GetListAsync(
        x => menuIds.Contains(x.Id) && x.MenuStatus == LeanMenuStatus.Normal);
    var menuDtos = menus.OrderBy(x => x.OrderNum)
                       .Select(x => x.Adapt<LeanMenuDto>())
                       .ToList();
    return BuildMenuTree(menuDtos);
  }

  /// <summary>
  /// 获取用户权限列表
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <returns>用户权限列表</returns>
  public async Task<List<string>> GetUserPermissionsAsync(long userId)
  {
    // 获取用户角色
    var userRoles = await _userRoleRepository.GetListAsync(x => x.UserId == userId);
    var roleIds = userRoles.Select(x => x.RoleId).ToList();

    // 获取角色菜单
    var roleMenus = await _roleMenuRepository.GetListAsync(x => roleIds.Contains(x.RoleId));
    var menuIds = roleMenus.Select(x => x.MenuId).Distinct().ToList();

    // 获取菜单权限
    var menus = await _menuRepository.GetListAsync(
        x => menuIds.Contains(x.Id) &&
             x.MenuStatus == LeanMenuStatus.Normal &&
             !string.IsNullOrEmpty(x.Perms));

    return menus.Select(x => x.Perms)
               .Where(x => !string.IsNullOrEmpty(x))
               .Distinct()
               .ToList();
  }

  /// <summary>
  /// 构建菜单树形结构
  /// </summary>
  /// <param name="menus">菜单列表</param>
  /// <returns>树形结构</returns>
  private List<LeanMenuTreeDto> BuildMenuTree(List<LeanMenuDto> menus)
  {
    var result = new List<LeanMenuTreeDto>();
    var lookup = menus.ToLookup(x => x.ParentId);

    foreach (var menu in menus.Where(x => x.ParentId == null || x.ParentId == 0))
    {
      var node = menu.Adapt<LeanMenuTreeDto>();
      node.Children = BuildMenuTreeNodes(lookup, menu.Id);
      result.Add(node);
    }

    return result;
  }

  /// <summary>
  /// 递归构建菜单树节点
  /// </summary>
  /// <param name="lookup">菜单查找表</param>
  /// <param name="parentId">父级ID</param>
  /// <returns>子节点列表</returns>
  private List<LeanMenuTreeDto> BuildMenuTreeNodes(ILookup<long?, LeanMenuDto> lookup, long parentId)
  {
    var result = new List<LeanMenuTreeDto>();

    foreach (var menu in lookup[parentId])
    {
      var node = menu.Adapt<LeanMenuTreeDto>();
      node.Children = BuildMenuTreeNodes(lookup, menu.Id);
      result.Add(node);
    }

    return result;
  }
}