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
using Microsoft.Extensions.Logging;

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
  private readonly ILeanRepository<LeanRole> _roleRepository;
  private readonly LeanUniqueValidator<LeanMenu> _uniqueValidator;
  private readonly ILogger<LeanMenuService> _logger;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="menuRepository">菜单仓储接口</param>
  /// <param name="roleMenuRepository">角色菜单关联仓储接口</param>
  /// <param name="userRoleRepository">用户角色关联仓储接口</param>
  /// <param name="roleRepository">角色仓储接口</param>
  /// <param name="context">基础服务上下文</param>
  /// <param name="logger">日志记录器</param>
  public LeanMenuService(
      ILeanRepository<LeanMenu> menuRepository,
      ILeanRepository<LeanRoleMenu> roleMenuRepository,
      ILeanRepository<LeanUserRole> userRoleRepository,
      ILeanRepository<LeanRole> roleRepository,
      LeanBaseServiceContext context,
      ILogger<LeanMenuService> logger)
      : base(context)
  {
    _menuRepository = menuRepository;
    _roleMenuRepository = roleMenuRepository;
    _userRoleRepository = userRoleRepository;
    _roleRepository = roleRepository;
    _uniqueValidator = new LeanUniqueValidator<LeanMenu>(_menuRepository);
    _logger = logger;
  }

  /// <summary>
  /// 分页查询菜单
  /// </summary>
  public async Task<LeanApiResult<LeanPageResult<LeanMenuDto>>> GetPageAsync(LeanMenuQueryDto input)
  {
    Expression<Func<LeanMenu, bool>> predicate = x => true;

    if (!string.IsNullOrEmpty(input.MenuName))
    {
      var menuName = CleanInput(input.MenuName);
      predicate = predicate.And(x => x.MenuName.Contains(menuName));
    }

    if (input.MenuType.HasValue)
    {
      predicate = predicate.And(x => x.MenuType == input.MenuType);
    }

    if (input.MenuStatus.HasValue)
    {
      predicate = predicate.And(x => x.MenuStatus == input.MenuStatus);
    }

    var (total, items) = await _menuRepository.GetPageListAsync(predicate, input.PageSize, input.PageIndex);
    var list = items.Adapt<List<LeanMenuDto>>();

    var result = new LeanPageResult<LeanMenuDto>
    {
      Total = total,
      Items = list,
      PageIndex = input.PageIndex,
      PageSize = input.PageSize
    };

    return LeanApiResult<LeanPageResult<LeanMenuDto>>.Ok(result);
  }

  /// <summary>
  /// 获取菜单信息
  /// </summary>
  /// <param name="id">菜单ID</param>
  /// <returns>菜单详细信息</returns>
  public async Task<LeanApiResult<LeanMenuDto>> GetAsync(long id)
  {
    var menu = await _menuRepository.GetByIdAsync(id);
    if (menu == null)
    {
      throw new LeanException("菜单不存在");
    }

    return LeanApiResult<LeanMenuDto>.Ok(menu.Adapt<LeanMenuDto>());
  }

  /// <summary>
  /// 创建菜单
  /// </summary>
  /// <param name="input">菜单创建参数</param>
  /// <returns>创建成功的菜单信息</returns>
  public async Task<LeanApiResult<long>> CreateAsync(LeanMenuCreateDto input)
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
      return LeanApiResult<long>.Ok(menu.Id);
    }, "创建菜单");
  }

  /// <summary>
  /// 更新菜单
  /// </summary>
  /// <param name="input">菜单更新参数</param>
  /// <returns>更新后的菜单信息</returns>
  public async Task<LeanApiResult> UpdateAsync(LeanMenuUpdateDto input)
  {
    return await ExecuteInTransactionAsync(async () =>
    {
      var menu = await _menuRepository.GetByIdAsync(input.Id);
      if (menu == null)
      {
        throw new LeanException("菜单不存在");
      }

      if (menu.IsBuiltin == 1)
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
      return LeanApiResult.Ok();
    }, "更新菜单");
  }

  /// <summary>
  /// 排序菜单
  /// </summary>
  public async Task<LeanApiResult> SortAsync(List<LeanMenuSortDto> input)
  {
    return await ExecuteInTransactionAsync(async () =>
    {
      foreach (var item in input)
      {
        var menu = await _menuRepository.GetByIdAsync(item.Id);
        if (menu == null)
        {
          throw new LeanException($"菜单ID {item.Id} 不存在");
        }

        menu.OrderNum = item.OrderNum;
        menu.ParentId = item.ParentId;
        await _menuRepository.UpdateAsync(menu);
      }

      LogAudit("SortMenu", $"批量更新菜单排序，数量: {input.Count}");
      return LeanApiResult.Ok();
    }, "排序菜单");
  }

  /// <summary>
  /// 删除菜单
  /// </summary>
  /// <param name="id">菜单ID</param>
  /// <returns>操作结果</returns>
  public async Task<LeanApiResult> DeleteAsync(long id)
  {
    return await ExecuteInTransactionAsync(async () =>
    {
      var menu = await _menuRepository.GetByIdAsync(id);
      if (menu == null)
      {
        throw new LeanException("菜单不存在");
      }

      if (menu.IsBuiltin == 1)
      {
        throw new LeanException("内置菜单不能删除");
      }

      // 检查是否存在子菜单
      var hasChildren = await _menuRepository.AnyAsync(x => x.ParentId == id);
      if (hasChildren)
      {
        throw new LeanException("存在子菜单，不能删除");
      }

      // 删除角色菜单关联
      await _roleMenuRepository.DeleteAsync(x => x.MenuId == id);

      // 删除菜单
      await _menuRepository.DeleteAsync(menu);

      LogAudit("DeleteMenu", $"删除菜单: {menu.MenuName}");
      return LeanApiResult.Ok();
    }, "删除菜单");
  }

  /// <summary>
  /// 批量删除菜单
  /// </summary>
  /// <param name="ids">菜单ID列表</param>
  /// <returns>操作结果</returns>
  public async Task<LeanApiResult> BatchDeleteAsync(List<long> ids)
  {
    return await ExecuteInTransactionAsync(async () =>
    {
      foreach (var id in ids)
      {
        await DeleteAsync(id);
      }
      return LeanApiResult.Ok();
    }, "批量删除菜单");
  }

  /// <summary>
  /// 导出菜单数据
  /// </summary>
  public async Task<byte[]> ExportAsync(LeanMenuQueryDto input)
  {
    var menuResult = await GetPageAsync(input);
    if (!menuResult.Success)
    {
      throw new LeanException(menuResult.Message);
    }

    var exportDtos = menuResult.Data.Items.Select(x => new LeanMenuExportDto
    {
      MenuName = x.MenuName,
      MenuType = x.MenuType,
      MenuStatus = x.MenuStatus,
      ParentId = x.ParentId,
      OrderNum = x.OrderNum,
      Perms = x.Perms,
      Path = x.Path,
      Component = x.Component,
      Icon = x.Icon,
      IsBuiltin = x.IsBuiltin,
      CreateTime = x.CreateTime
    }).ToList();

    return LeanExcelHelper.Export(exportDtos);
  }

  /// <summary>
  /// 获取导入模板
  /// </summary>
  public async Task<byte[]> GetTemplateAsync()
  {
    var template = new List<LeanMenuImportDto>
    {
      new LeanMenuImportDto
      {
        MenuName = "示例菜单",
        MenuType = 1,
        MenuStatus = 2,
        ParentId = 0,
        OrderNum = 1,
        Perms = "system:menu:list",
        Path = "/system/menu",
        Component = "system/menu/index",
        Icon = "menu"
      }
    };

    return await Task.FromResult(LeanExcelHelper.Export(template));
  }

  /// <summary>
  /// 导入菜单数据
  /// </summary>
  public async Task<LeanMenuImportResultDto> ImportAsync(LeanFileInfo file)
  {
    var result = new LeanMenuImportResultDto();
    try
    {
      var bytes = new byte[file.Stream.Length];
      await file.Stream.ReadAsync(bytes, 0, (int)file.Stream.Length);
      var importResult = LeanExcelHelper.Import<LeanMenuImportDto>(bytes);

      foreach (var item in importResult.Data)
      {
        if (await _menuRepository.AnyAsync(x => x.MenuName == item.MenuName))
        {
          result.AddError(item.Perms, $"菜单名称 {item.MenuName} 已存在");
          continue;
        }
        var menu = item.Adapt<LeanMenu>();
        menu.MenuStatus = 2;
        menu.IsBuiltin = 0;
        await _menuRepository.CreateAsync(menu);
        LogAudit("ImportMenu", $"导入菜单: {menu.MenuName}");
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
  /// 设置菜单状态
  /// </summary>
  public async Task<LeanApiResult> SetStatusAsync(LeanMenuChangeStatusDto input)
  {
    await ChangeStatusAsync(input);
    return LeanApiResult.Ok();
  }

  /// <summary>
  /// 获取菜单树形结构
  /// </summary>
  /// <param name="input">查询参数</param>
  /// <returns>菜单树形结构</returns>
  public async Task<List<LeanMenuTreeDto>> GetTreeAsync(LeanMenuQueryDto input)
  {
    // 获取菜单列表
    var menuResult = await GetPageAsync(input);
    if (!menuResult.Success)
    {
      throw new LeanException(menuResult.Message);
    }

    // 构建树形结构
    return BuildMenuTree(menuResult.Data.Items);
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
        x => menuIds.Contains(x.Id) && x.MenuStatus == 2);

    // 转换为DTO并构建树形结构
    var menuDtos = menus.OrderBy(x => x.OrderNum)
                       .Select(x => x.Adapt<LeanMenuDto>())
                       .ToList();
    return BuildMenuTree(menuDtos);
  }

  /// <summary>
  /// 获取用户菜单树
  /// </summary>
  public async Task<List<LeanMenuTreeDto>> GetUserMenuTreeAsync(long userId)
  {
    // 获取用户角色
    var userRoles = await _userRoleRepository.GetListAsync(x => x.UserId == userId);
    if (!userRoles.Any())
    {
      return new List<LeanMenuTreeDto>();
    }

    var roleIds = userRoles.Select(x => x.RoleId).ToList();
    _logger.LogInformation($"用户 {userId} 的角色列表: {string.Join(",", roleIds)}");

    // 获取角色菜单
    var roleMenus = await _roleMenuRepository.GetListAsync(x => roleIds.Contains(x.RoleId));
    var menuIds = roleMenus.Select(x => x.MenuId).Distinct().ToList();
    _logger.LogInformation($"用户 {userId} 的菜单权限列表: {string.Join(",", menuIds)}");

    // 获取菜单列表
    var menus = await _menuRepository.GetListAsync(
      x => x.MenuStatus == 0 && // 菜单状态：0-正常
          x.Visible == 0 && // 显示状态：0-显示
          (x.MenuType == 0 || x.MenuType == 1) && // 菜单类型：0-目录, 1-菜单
          (x.ParentId == 0 || menuIds.Contains(x.Id))); // 根目录或有权限的菜单

    // 转换为DTO
    var menuDtos = menus.Select(x => x.Adapt<LeanMenuDto>()).ToList();

    // 构建菜单树
    return BuildMenuTree(menuDtos);
  }

  /// <summary>
  /// 获取用户权限清单
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <returns>权限清单</returns>
  public async Task<List<string>> GetUserPermissionsAsync(long userId)
  {
    var userRoles = await _userRoleRepository.GetListAsync(x => x.UserId == userId);
    if (!userRoles.Any())
    {
      return new List<string>();
    }

    var roleIds = userRoles.Select(x => x.RoleId).ToList();
    var roleMenus = await _roleMenuRepository.GetListAsync(x => roleIds.Contains(x.RoleId));
    if (!roleMenus.Any())
    {
      return new List<string>();
    }

    var menuIds = roleMenus.Select(x => x.MenuId).ToList();
    var menus = await _menuRepository.GetListAsync(x => menuIds.Contains(x.Id));
    return menus.Select(x => x.Perms).Where(x => !string.IsNullOrEmpty(x)).ToList();
  }

  /// <summary>
  /// 获取用户角色列表
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <returns>角色列表</returns>
  public async Task<List<string>> GetUserRolesAsync(long userId)
  {
    var userRoles = await _userRoleRepository.GetListAsync(x => x.UserId == userId);
    if (!userRoles.Any())
    {
      return new List<string>();
    }

    var roleIds = userRoles.Select(x => x.RoleId).ToList();
    var roles = await _roleRepository.GetListAsync(x => roleIds.Contains(x.Id));
    return roles.Select(x => x.RoleName).ToList();
  }

  /// <summary>
  /// 获取菜单权限列表
  /// </summary>
  /// <param name="menuIds">菜单ID列表</param>
  /// <returns>权限列表</returns>
  public async Task<List<string>> GetMenuPermissionsAsync(List<long> menuIds)
  {
    if (!menuIds.Any())
    {
      return new List<string>();
    }

    var menus = await _menuRepository.GetListAsync(x => menuIds.Contains(x.Id));
    return menus.Select(x => x.Perms).Where(x => !string.IsNullOrEmpty(x)).ToList();
  }

  /// <summary>
  /// 构建菜单树形结构
  /// </summary>
  /// <param name="menus">菜单列表</param>
  /// <returns>树形结构</returns>
  private List<LeanMenuTreeDto> BuildMenuTree(List<LeanMenuDto> menus)
  {
    LogAudit("BuildMenuTree", $"开始构建树形结构，菜单数量: {menus.Count}");

    // 打印所有菜单的ID和ParentId，用于调试
    foreach (var menu in menus)
    {
      LogAudit("BuildMenuTree", $"菜单ID: {menu.Id}, 菜单名称: {menu.MenuName}, ParentId: {menu.ParentId}");
    }

    // 获取ParentId为0的根节点
    var rootMenus = menus.Where(x => x.ParentId == 0).OrderBy(x => x.OrderNum).ToList();
    LogAudit("BuildMenuTree", $"根节点数量: {rootMenus.Count}");

    var result = new List<LeanMenuTreeDto>();
    foreach (var menu in rootMenus)
    {
      var node = menu.Adapt<LeanMenuTreeDto>();
      // 递归获取子节点
      node.Children = GetChildren(menus, menu.Id);
      result.Add(node);
    }

    LogAudit("BuildMenuTree", $"树形结构构建完成，根节点数量: {result.Count}");
    return result;
  }

  /// <summary>
  /// 递归获取子节点
  /// </summary>
  private List<LeanMenuTreeDto> GetChildren(List<LeanMenuDto> menus, long parentId)
  {
    // 查找所有ParentId等于当前parentId的菜单
    var children = menus.Where(x => x.ParentId == parentId)
                       .OrderBy(x => x.OrderNum)
                       .Select(x =>
                       {
                         var node = x.Adapt<LeanMenuTreeDto>();
                         // 递归查找子节点的子节点
                         node.Children = GetChildren(menus, x.Id);
                         return node;
                       })
                       .ToList();
    return children;
  }

  /// <summary>
  /// 修改菜单状态
  /// </summary>
  private async Task ChangeStatusAsync(LeanMenuChangeStatusDto input)
  {
    await ExecuteInTransactionAsync(async () =>
    {
      var menu = await _menuRepository.GetByIdAsync(input.Id);
      if (menu == null)
      {
        throw new LeanException("菜单不存在");
      }

      if (menu.IsBuiltin == 1)
      {
        throw new LeanException("内置菜单不能修改状态");
      }

      menu.MenuStatus = 2;
      await _menuRepository.UpdateAsync(menu);

      LogAudit("ChangeMenuStatus", $"修改菜单状态: {menu.MenuName}, 状态: {input.MenuStatus}");
    }, "修改菜单状态");
  }
}