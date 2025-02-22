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
using Microsoft.Extensions.Logging;

namespace Lean.CodeGen.Application.Services.Identity;

/// <summary>
/// 菜单服务实现
/// </summary>
public class LeanMenuService : LeanBaseService, ILeanMenuService
{
  private readonly ILeanRepository<LeanMenu> _menuRepository;
  private readonly ILeanRepository<LeanUserMenu> _userMenuRepository;
  private readonly ILeanRepository<LeanRoleMenu> _roleMenuRepository;
  private readonly ILeanRepository<LeanMenuOperation> _menuOperationRepository;
  private readonly LeanUniqueValidator<LeanMenu> _uniqueValidator;
  private readonly ILogger<LeanMenuService> _logger;

  public LeanMenuService(
      ILeanRepository<LeanMenu> menuRepository,
      ILeanRepository<LeanUserMenu> userMenuRepository,
      ILeanRepository<LeanRoleMenu> roleMenuRepository,
      ILeanRepository<LeanMenuOperation> menuOperationRepository,
      ILeanSqlSafeService sqlSafeService,
      IOptions<LeanSecurityOptions> securityOptions,
      ILogger<LeanMenuService> logger)
      : base(sqlSafeService, securityOptions, logger)
  {
    _menuRepository = menuRepository;
    _userMenuRepository = userMenuRepository;
    _roleMenuRepository = roleMenuRepository;
    _menuOperationRepository = menuOperationRepository;
    _uniqueValidator = new LeanUniqueValidator<LeanMenu>(menuRepository);
    _logger = logger;
  }

  /// <summary>
  /// 创建菜单
  /// </summary>
  public async Task<LeanApiResult<long>> CreateAsync(LeanCreateMenuDto input)
  {
    try
    {
      if (!await ValidateMenuInputAsync(input.MenuName, input.MenuCode))
      {
        return LeanApiResult<long>.Error("输入参数验证失败");
      }

      var menu = input.Adapt<LeanMenu>();
      menu.CreateTime = DateTime.Now;
      menu.Perms = input.MenuCode;

      await _menuRepository.CreateAsync(menu);

      return LeanApiResult<long>.Ok(menu.Id);
    }
    catch (Exception ex)
    {
      return LeanApiResult<long>.Error($"创建菜单失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 更新菜单
  /// </summary>
  public async Task<LeanApiResult> UpdateAsync(LeanUpdateMenuDto input)
  {
    try
    {
      var menu = await GetMenuByIdAsync(input.Id);
      if (menu == null)
      {
        return LeanApiResult.Error($"菜单 {input.Id} 不存在");
      }

      if (!await ValidateMenuInputAsync(input.MenuName, input.MenuCode, input.Id))
      {
        return LeanApiResult.Error("输入参数验证失败");
      }

      input.Adapt(menu);
      menu.Perms = input.MenuCode;
      menu.UpdateTime = DateTime.Now;

      await _menuRepository.UpdateAsync(menu);

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"更新菜单失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 删除菜单
  /// </summary>
  public async Task<LeanApiResult> DeleteAsync(long id)
  {
    try
    {
      var menu = await _menuRepository.GetByIdAsync(id);
      if (menu == null)
      {
        return LeanApiResult.Error($"菜单 {id} 不存在");
      }

      if (menu.IsBuiltin == LeanBuiltinStatus.Yes)
      {
        return LeanApiResult.Error($"菜单 {menu.MenuName} 是内置菜单，不能删除");
      }

      // 检查是否有子菜单
      var hasChildren = await _menuRepository.AnyAsync(m => m.ParentId == id);
      if (hasChildren)
      {
        return LeanApiResult.Error($"菜单 {menu.MenuName} 存在子菜单，不能删除");
      }

      await DeleteMenuRelationsAsync(id);
      await _menuRepository.DeleteAsync(m => m.Id == id);

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"删除菜单失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 批量删除菜单
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
      return LeanApiResult.Error($"批量删除菜单失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取菜单信息
  /// </summary>
  public async Task<LeanApiResult<LeanMenuDto>> GetAsync(long id)
  {
    try
    {
      var menu = await GetMenuByIdAsync(id);
      if (menu == null)
      {
        return LeanApiResult<LeanMenuDto>.Error($"菜单 {id} 不存在");
      }

      var result = menu.Adapt<LeanMenuDto>();
      result.MenuCode = menu.Perms;

      return LeanApiResult<LeanMenuDto>.Ok(result);
    }
    catch (Exception ex)
    {
      return LeanApiResult<LeanMenuDto>.Error($"获取菜单信息失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 分页查询菜单
  /// </summary>
  public async Task<LeanApiResult<LeanPageResult<LeanMenuDto>>> GetPageAsync(LeanQueryMenuDto input)
  {
    try
    {
      var predicate = BuildMenuQueryPredicate(input);
      var page = await _menuRepository.GetPageListAsync(predicate, input.PageIndex, input.PageSize);

      var result = new LeanPageResult<LeanMenuDto>
      {
        Total = page.Total,
        Items = page.Items.Select(m =>
        {
          var dto = m.Adapt<LeanMenuDto>();
          dto.MenuCode = m.Perms;
          return dto;
        }).ToList()
      };

      return LeanApiResult<LeanPageResult<LeanMenuDto>>.Ok(result);
    }
    catch (Exception ex)
    {
      return LeanApiResult<LeanPageResult<LeanMenuDto>>.Error($"查询菜单失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 修改菜单状态
  /// </summary>
  public async Task<LeanApiResult> SetStatusAsync(LeanChangeMenuStatusDto input)
  {
    try
    {
      var menu = await GetMenuByIdAsync(input.Id);
      if (menu == null)
      {
        return LeanApiResult.Error($"菜单 {input.Id} 不存在");
      }

      menu.MenuStatus = input.MenuStatus;
      menu.UpdateTime = DateTime.Now;

      await _menuRepository.UpdateAsync(menu);

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"修改菜单状态失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取菜单树形结构
  /// </summary>
  public async Task<LeanApiResult<List<LeanMenuTreeDto>>> GetTreeAsync()
  {
    try
    {
      var menus = await _menuRepository.GetListAsync(m => true);
      var result = BuildMenuTree(menus);

      return LeanApiResult<List<LeanMenuTreeDto>>.Ok(result);
    }
    catch (Exception ex)
    {
      return LeanApiResult<List<LeanMenuTreeDto>>.Error($"获取菜单树失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取用户菜单树
  /// </summary>
  public async Task<LeanApiResult<List<LeanMenuTreeDto>>> GetUserMenuTreeAsync(long userId)
  {
    try
    {
      // 获取用户直接分配的菜单
      var userMenus = await _userMenuRepository.GetListAsync(um => um.UserId == userId);
      var userMenuIds = userMenus.Select(um => um.MenuId).ToList();

      // 获取用户角色分配的菜单
      var roleMenus = await _roleMenuRepository.GetListAsync(rm => rm.Role.UserRoles.Any(ur => ur.UserId == userId));
      var roleMenuIds = roleMenus.Select(rm => rm.MenuId).ToList();

      // 合并菜单ID
      var menuIds = userMenuIds.Union(roleMenuIds).Distinct().ToList();

      // 获取菜单信息
      var menus = await _menuRepository.GetListAsync(m => menuIds.Contains(m.Id));
      var result = BuildMenuTree(menus);

      return LeanApiResult<List<LeanMenuTreeDto>>.Ok(result);
    }
    catch (Exception ex)
    {
      return LeanApiResult<List<LeanMenuTreeDto>>.Error($"获取用户菜单树失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取用户权限列表
  /// </summary>
  public async Task<LeanApiResult<List<string>>> GetUserPermissionsAsync(long userId)
  {
    try
    {
      // 获取用户直接分配的菜单
      var userMenus = await _userMenuRepository.GetListAsync(um => um.UserId == userId);
      var userMenuIds = userMenus.Select(um => um.MenuId).ToList();

      // 获取用户角色分配的菜单
      var roleMenus = await _roleMenuRepository.GetListAsync(rm => rm.Role.UserRoles.Any(ur => ur.UserId == userId));
      var roleMenuIds = roleMenus.Select(rm => rm.MenuId).ToList();

      // 合并菜单ID
      var menuIds = userMenuIds.Union(roleMenuIds).Distinct().ToList();

      // 获取菜单权限标识
      var menus = await _menuRepository.GetListAsync(m => menuIds.Contains(m.Id));
      var permissions = menus.Where(m => !string.IsNullOrEmpty(m.Perms))
                            .Select(m => m.Perms)
                            .Distinct()
                            .ToList();

      return LeanApiResult<List<string>>.Ok(permissions);
    }
    catch (Exception ex)
    {
      return LeanApiResult<List<string>>.Error($"获取用户权限列表失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取角色菜单树
  /// </summary>
  public async Task<LeanApiResult<List<LeanMenuTreeDto>>> GetRoleMenuTreeAsync(long roleId)
  {
    try
    {
      var roleMenus = await _roleMenuRepository.GetListAsync(rm => rm.RoleId == roleId);
      var menuIds = roleMenus.Select(rm => rm.MenuId).ToList();
      var menus = await _menuRepository.GetListAsync(m => menuIds.Contains(m.Id));
      var result = BuildMenuTree(menus);

      return LeanApiResult<List<LeanMenuTreeDto>>.Ok(result);
    }
    catch (Exception ex)
    {
      return LeanApiResult<List<LeanMenuTreeDto>>.Error($"获取角色菜单树失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取菜单的操作权限列表
  /// </summary>
  public async Task<LeanApiResult<List<LeanMenuOperationDto>>> GetMenuOperationsAsync(long menuId)
  {
    try
    {
      var menu = await GetMenuByIdAsync(menuId);
      if (menu == null)
      {
        return LeanApiResult<List<LeanMenuOperationDto>>.Error($"菜单 {menuId} 不存在");
      }

      var operations = await _menuOperationRepository.GetListAsync(mo => mo.MenuId == menuId);
      var result = operations.Select(o => o.Adapt<LeanMenuOperationDto>()).ToList();

      return LeanApiResult<List<LeanMenuOperationDto>>.Ok(result);
    }
    catch (Exception ex)
    {
      return LeanApiResult<List<LeanMenuOperationDto>>.Error($"获取菜单操作权限失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 设置菜单的操作权限
  /// </summary>
  public async Task<LeanApiResult> SetMenuOperationsAsync(LeanSetMenuOperationDto input)
  {
    try
    {
      var menu = await GetMenuByIdAsync(input.MenuId);
      if (menu == null)
      {
        return LeanApiResult.Error($"菜单 {input.MenuId} 不存在");
      }

      await _menuOperationRepository.DeleteAsync(mo => mo.MenuId == input.MenuId);

      if (input.Operations?.Any() == true)
      {
        var operations = input.Operations.Select(o => new LeanMenuOperation
        {
          MenuId = input.MenuId,
          Code = o.Code,
          Name = o.Name,
          CreateTime = DateTime.Now
        }).ToList();

        await _menuOperationRepository.CreateRangeAsync(operations);
      }

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"设置菜单操作权限失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取用户在指定菜单的操作权限
  /// </summary>
  public async Task<LeanApiResult<List<string>>> GetUserMenuOperationsAsync(long userId, long menuId)
  {
    try
    {
      // 检查用户是否有菜单权限
      var hasMenuPermission = await ValidateUserMenuOperationAsync(userId, menuId, null);
      if (!hasMenuPermission.Success || !hasMenuPermission.Data)
      {
        return LeanApiResult<List<string>>.Ok(new List<string>());
      }

      // 获取菜单的所有操作权限
      var operations = await _menuOperationRepository.GetListAsync(mo => mo.MenuId == menuId);
      var result = operations.Select(o => o.Code).ToList();

      return LeanApiResult<List<string>>.Ok(result);
    }
    catch (Exception ex)
    {
      return LeanApiResult<List<string>>.Error($"获取用户菜单操作权限失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 验证用户是否有指定菜单的操作权限
  /// </summary>
  public async Task<LeanApiResult<bool>> ValidateUserMenuOperationAsync(long userId, long menuId, string operation)
  {
    try
    {
      // 检查菜单是否存在
      var menu = await GetMenuByIdAsync(menuId);
      if (menu == null)
      {
        return LeanApiResult<bool>.Error($"菜单 {menuId} 不存在");
      }

      // 检查菜单状态
      if (menu.MenuStatus != LeanMenuStatus.Normal)
      {
        return LeanApiResult<bool>.Ok(false);
      }

      // 检查用户是否有菜单权限
      var userMenus = await _userMenuRepository.GetListAsync(um => um.UserId == userId && um.MenuId == menuId);
      var roleMenus = await _roleMenuRepository.GetListAsync(rm => rm.MenuId == menuId && rm.Role.UserRoles.Any(ur => ur.UserId == userId));

      if (!userMenus.Any() && !roleMenus.Any())
      {
        return LeanApiResult<bool>.Ok(false);
      }

      // 如果不需要检查具体操作权限，直接返回true
      if (string.IsNullOrEmpty(operation))
      {
        return LeanApiResult<bool>.Ok(true);
      }

      // 检查操作权限是否存在
      var hasOperation = await _menuOperationRepository.AnyAsync(mo => mo.MenuId == menuId && mo.Code == operation);
      return LeanApiResult<bool>.Ok(hasOperation);
    }
    catch (Exception ex)
    {
      return LeanApiResult<bool>.Error($"验证用户菜单操作权限失败: {ex.Message}");
    }
  }

  #region 私有方法

  /// <summary>
  /// 获取菜单
  /// </summary>
  private async Task<LeanMenu?> GetMenuByIdAsync(long id)
  {
    return await _menuRepository.GetByIdAsync(id);
  }

  /// <summary>
  /// 验证菜单输入
  /// </summary>
  private async Task<bool> ValidateMenuInputAsync(string menuName, string perms, long? id = null)
  {
    if (string.IsNullOrEmpty(menuName) || string.IsNullOrEmpty(perms))
    {
      return false;
    }

    if (!ValidateInput(new[] { menuName, perms }))
    {
      return false;
    }

    try
    {
      await _uniqueValidator.ValidateAsync(
          (m => m.MenuName, menuName, id, $"菜单名称 {menuName} 已存在"),
          (m => m.Perms, perms, id, $"菜单编码 {perms} 已存在")
      );
      return true;
    }
    catch
    {
      return false;
    }
  }

  /// <summary>
  /// 删除菜单关联信息
  /// </summary>
  private async Task DeleteMenuRelationsAsync(long menuId)
  {
    await _userMenuRepository.DeleteAsync(um => um.MenuId == menuId);
    await _roleMenuRepository.DeleteAsync(rm => rm.MenuId == menuId);
    await _menuOperationRepository.DeleteAsync(mo => mo.MenuId == menuId);
  }

  /// <summary>
  /// 构建菜单查询条件
  /// </summary>
  private Expression<Func<LeanMenu, bool>> BuildMenuQueryPredicate(LeanQueryMenuDto input)
  {
    Expression<Func<LeanMenu, bool>> predicate = m => true;

    if (!string.IsNullOrEmpty(input.MenuName))
    {
      var menuName = CleanInput(input.MenuName);
      predicate = LeanExpressionExtensions.And(predicate, m => m.MenuName.Contains(menuName));
    }

    if (!string.IsNullOrEmpty(input.MenuCode))
    {
      var menuCode = CleanInput(input.MenuCode);
      predicate = LeanExpressionExtensions.And(predicate, m => m.Perms.Contains(menuCode));
    }

    if (input.MenuStatus.HasValue)
    {
      predicate = LeanExpressionExtensions.And(predicate, m => m.MenuStatus == input.MenuStatus.Value);
    }

    if (input.StartTime.HasValue)
    {
      predicate = LeanExpressionExtensions.And(predicate, m => m.CreateTime >= input.StartTime.Value);
    }

    if (input.EndTime.HasValue)
    {
      predicate = LeanExpressionExtensions.And(predicate, m => m.CreateTime <= input.EndTime.Value);
    }

    return predicate;
  }

  /// <summary>
  /// 构建菜单树
  /// </summary>
  private List<LeanMenuTreeDto> BuildMenuTree(List<LeanMenu> menus)
  {
    var menuDict = menus.ToDictionary(m => m.Id, m =>
    {
      var dto = m.Adapt<LeanMenuTreeDto>();
      dto.MenuCode = m.Perms;
      return dto;
    });

    foreach (var menu in menus.Where(m => m.ParentId.HasValue))
    {
      if (menuDict.TryGetValue(menu.ParentId.Value, out var parentMenu))
      {
        parentMenu.Children.Add(menuDict[menu.Id]);
      }
    }

    return menuDict.Values.Where(m => !m.ParentId.HasValue).ToList();
  }

  #endregion
}