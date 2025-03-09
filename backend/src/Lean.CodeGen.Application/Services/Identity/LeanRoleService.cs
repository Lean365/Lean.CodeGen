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
    /// 创建角色
    /// </summary>
    /// <param name="input">角色创建参数</param>
    /// <returns>创建成功的角色信息</returns>
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
    /// <param name="input">角色更新参数</param>
    /// <returns>更新后的角色信息</returns>
    public async Task<LeanApiResult> UpdateAsync(LeanRoleUpdateDto input)
    {
        return await ExecuteInTransactionAsync(async () =>
        {
            var role = await _roleRepository.GetByIdAsync(input.Id);
            if (role == null)
            {
                throw new LeanException("角色不存在");
            }

            if (role.IsBuiltin == 1)
            {
                throw new LeanException("内置角色不能修改");
            }

            // 验证角色名称唯一性
            await _uniqueValidator.ValidateAsync(x => x.RoleName, input.RoleName, input.Id);

            // 验证角色编码唯一性
            if (!string.IsNullOrEmpty(input.RoleCode))
            {
                await _uniqueValidator.ValidateAsync(x => x.RoleCode, input.RoleCode, input.Id);
            }

            // 更新角色信息
            input.Adapt(role);
            await _roleRepository.UpdateAsync(role);

            LogAudit("UpdateRole", $"更新角色: {role.RoleName}");
            return LeanApiResult.Ok();
        }, "更新角色");
    }

    /// <summary>
    /// 删除角色
    /// </summary>
    /// <param name="input">角色删除参数</param>
    public async Task<LeanApiResult> DeleteAsync(long id)
    {
        return await ExecuteInTransactionAsync(async () =>
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null)
            {
                throw new LeanException("角色不存在");
            }

            if (role.IsBuiltin == 1)
            {
                throw new LeanException("内置角色不能删除");
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
    /// 获取角色信息
    /// </summary>
    /// <param name="id">角色ID</param>
    /// <returns>角色详细信息</returns>
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
    /// 分页查询角色
    /// </summary>
    /// <param name="input">查询参数</param>
    /// <returns>分页查询结果</returns>
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
    /// 修改角色状态
    /// </summary>
    /// <param name="input">状态修改参数</param>
    public async Task<LeanApiResult> SetStatusAsync(LeanRoleChangeStatusDto input)
    {
        await ChangeStatusAsync(input);
        return LeanApiResult.Ok();
    }

    /// <summary>
    /// 获取角色菜单
    /// </summary>
    /// <param name="id">角色ID</param>
    /// <returns>角色菜单ID列表</returns>
    public async Task<List<long>> GetRoleMenusAsync(long id)
    {
        var roleMenus = await _roleMenuRepository.GetListAsync(x => x.RoleId == id);
        return roleMenus.Select(x => x.MenuId).ToList();
    }

    /// <summary>
    /// 设置角色菜单
    /// </summary>
    /// <param name="input">菜单分配参数</param>
    public async Task<LeanApiResult> SetRoleMenusAsync(LeanRoleSetMenusDto input)
    {
        await AssignMenusAsync(input.Adapt<LeanRoleMenuAssignDto>());
        return LeanApiResult.Ok();
    }

    /// <summary>
    /// 导出角色数据
    /// </summary>
    /// <param name="input">查询参数</param>
    /// <returns>导出的角色数据</returns>
    public async Task<byte[]> ExportAsync(LeanRoleQueryDto input)
    {
        var roles = await QueryAsync(input);
        var exportDtos = roles.Select(x => new LeanRoleExportDto
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
    /// 导入角色数据
    /// </summary>
    /// <param name="file">角色数据文件信息</param>
    /// <returns>导入结果</returns>
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
    /// 获取导入模板
    /// </summary>
    /// <returns>导入模板数据</returns>
    public async Task<byte[]> GetImportTemplateAsync()
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
    /// 修改角色状态
    /// </summary>
    /// <param name="input">状态修改参数</param>
    public async Task ChangeStatusAsync(LeanRoleChangeStatusDto input)
    {
        await ExecuteInTransactionAsync(async () =>
        {
            var role = await _roleRepository.GetByIdAsync(input.Id);
            if (role == null)
            {
                throw new LeanException("角色不存在");
            }

            if (role.IsBuiltin == 1)
            {
                throw new LeanException("内置角色不能修改状态");
            }

            role.RoleStatus = 2;
            role.IsBuiltin = 0;
            await _roleRepository.UpdateAsync(role);

            LogAudit("ChangeRoleStatus", $"修改角色状态: {role.RoleName}, 状态: {input.RoleStatus}");
        }, "修改角色状态");
    }

    /// <summary>
    /// 分配角色菜单
    /// </summary>
    /// <param name="input">菜单分配参数</param>
    public async Task AssignMenusAsync(LeanRoleMenuAssignDto input)
    {
        await ExecuteInTransactionAsync(async () =>
        {
            var role = await _roleRepository.GetByIdAsync(input.Id);
            if (role == null)
            {
                throw new LeanException("角色不存在");
            }

            if (role.IsBuiltin == 1)
            {
                throw new LeanException("内置角色不能修改菜单");
            }

            // 删除原有菜单
            await _roleMenuRepository.DeleteAsync(x => x.RoleId == input.Id);

            // 添加新菜单
            if (input.MenuIds != null && input.MenuIds.Any())
            {
                foreach (var menuId in input.MenuIds)
                {
                    await _roleMenuRepository.CreateAsync(new LeanRoleMenu
                    {
                        RoleId = input.Id,
                        MenuId = menuId
                    });
                }
            }

            LogAudit("AssignRoleMenus", $"分配角色菜单: {role.RoleName}");
        }, "分配角色菜单");
    }

    /// <summary>
    /// 分配角色数据权限
    /// </summary>
    /// <param name="input">数据权限分配参数</param>
    public async Task AssignDataScopeAsync(LeanRoleDataScopeAssignDto input)
    {
        await ExecuteInTransactionAsync(async () =>
        {
            var role = await _roleRepository.GetByIdAsync(input.Id);
            if (role == null)
            {
                throw new LeanException("角色不存在");
            }

            if (role.IsBuiltin == 1)
            {
                throw new LeanException("内置角色不能修改数据权限");
            }

            // 更新数据权限类型
            role.DataScope = input.DataScope;
            await _roleRepository.UpdateAsync(role);

            // 删除原有数据权限
            await _roleDeptRepository.DeleteAsync(x => x.RoleId == input.Id);

            // 添加新数据权限
            if (input.DeptIds != null && input.DeptIds.Any())
            {
                foreach (var deptId in input.DeptIds)
                {
                    await _roleDeptRepository.CreateAsync(new LeanRoleDept
                    {
                        RoleId = input.Id,
                        DeptId = deptId
                    });
                }
            }

            LogAudit("AssignRoleDataScope", $"分配角色数据权限: {role.RoleName}");
        }, "分配角色数据权限");
    }

    /// <summary>
    /// 获取角色数据权限
    /// </summary>
    /// <param name="id">角色ID</param>
    /// <returns>角色数据权限信息</returns>
    public async Task<LeanRoleDataScopeDto> GetDataScopeAsync(long id)
    {
        var role = await _roleRepository.GetByIdAsync(id);
        if (role == null)
        {
            throw new LeanException("角色不存在");
        }

        var roleDepts = await _roleDeptRepository.GetListAsync(x => x.RoleId == id);

        return new LeanRoleDataScopeDto
        {
            Id = role.Id,
            DataScope = role.DataScope,
            DeptIds = roleDepts.Select(x => x.DeptId).ToList()
        };
    }

    /// <summary>
    /// 查询角色列表
    /// </summary>
    public async Task<List<LeanRoleDto>> QueryAsync(LeanRoleQueryDto input)
    {
        var roles = await _roleRepository.GetListAsync(x =>
            (string.IsNullOrEmpty(input.RoleName) || x.RoleName.Contains(input.RoleName)) &&
            (string.IsNullOrEmpty(input.RoleCode) || x.RoleCode.Contains(input.RoleCode)) &&
            (!input.RoleStatus.HasValue || x.RoleStatus == input.RoleStatus) &&
            (!input.StartTime.HasValue || x.CreateTime >= input.StartTime) &&
            (!input.EndTime.HasValue || x.CreateTime <= input.EndTime));

        return roles.OrderBy(x => x.OrderNum)
                   .Select(x => x.Adapt<LeanRoleDto>())
                   .ToList();
    }
}