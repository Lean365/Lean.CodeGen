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
    /// <returns>创建成功的部门ID</returns>
    public async Task<LeanApiResult<long>> CreateAsync(LeanDeptCreateDto input)
    {
        return await ExecuteInTransactionAsync(async () =>
        {
            // 验证部门名称唯一性
            await _uniqueValidator.ValidateAsync(x => x.DeptName, input.DeptName);

            // 创建部门实体
            var dept = input.Adapt<LeanDept>();
            await _deptRepository.CreateAsync(dept);

            LogAudit("CreateDept", $"创建部门: {dept.DeptName}");
            return LeanApiResult<long>.Ok(dept.Id);
        }, "创建部门");
    }

    /// <summary>
    /// 更新部门
    /// </summary>
    /// <param name="input">部门更新参数</param>
    /// <returns>更新后的部门信息</returns>
    public async Task<LeanApiResult> UpdateAsync(LeanDeptUpdateDto input)
    {
        return await ExecuteInTransactionAsync(async () =>
        {
            var dept = await _deptRepository.GetByIdAsync(input.Id);
            if (dept == null)
            {
                throw new LeanException("部门不存在");
            }

            if (dept.IsBuiltin == 1)
            {
                throw new LeanException("内置部门不能修改");
            }

            // 验证部门名称唯一性
            if (dept.DeptName != input.DeptName)
            {
                await _uniqueValidator.ValidateAsync(x => x.DeptName, input.DeptName, dept.Id);
            }

            // 更新部门实体
            input.Adapt(dept);
            await _deptRepository.UpdateAsync(dept);

            LogAudit("UpdateDept", $"更新部门: {dept.DeptName}");
            return new LeanApiResult();
        }, "更新部门");
    }

    /// <summary>
    /// 删除部门
    /// </summary>
    /// <param name="id">部门ID</param>
    public async Task<LeanApiResult> DeleteAsync(long id)
    {
        return await ExecuteInTransactionAsync(async () =>
        {
            var dept = await _deptRepository.GetByIdAsync(id);
            if (dept == null)
            {
                throw new LeanException("部门不存在");
            }

            if (dept.IsBuiltin == 1)
            {
                throw new LeanException("内置部门不能删除");
            }

            // 检查是否有子部门
            var hasChildren = await _deptRepository.AnyAsync(x => x.ParentId == id);
            if (hasChildren)
            {
                throw new LeanException("存在子部门，不能删除");
            }

            // 检查是否有关联的角色
            var hasRoles = await _roleDeptRepository.AnyAsync(x => x.DeptId == id);
            if (hasRoles)
            {
                throw new LeanException("部门已被角色使用，不能删除");
            }

            // 检查是否有关联的用户
            var hasUsers = await _userDeptRepository.AnyAsync(x => x.DeptId == id);
            if (hasUsers)
            {
                throw new LeanException("部门已被用户使用，不能删除");
            }

            // 删除部门
            await _deptRepository.DeleteAsync(dept);

            LogAudit("DeleteDept", $"删除部门: {dept.DeptName}");
            return new LeanApiResult();
        }, "删除部门");
    }

    /// <summary>
    /// 获取部门信息
    /// </summary>
    /// <param name="id">部门ID</param>
    /// <returns>部门详细信息</returns>
    public async Task<LeanApiResult<LeanDeptDto>> GetAsync(long id)
    {
        var dept = await _deptRepository.GetByIdAsync(id);
        if (dept == null)
        {
            throw new LeanException("部门不存在");
        }

        return LeanApiResult<LeanDeptDto>.Ok(dept.Adapt<LeanDeptDto>());
    }

    /// <summary>
    /// 查询部门列表
    /// </summary>
    /// <param name="input">查询参数</param>
    /// <returns>部门列表</returns>
    public async Task<List<LeanDeptDto>> QueryAsync(LeanDeptQueryDto input)
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
    public async Task<List<LeanDeptTreeDto>> GetTreeAsync(LeanDeptQueryDto input)
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
    public async Task ChangeStatusAsync(LeanDeptChangeStatusDto input)
    {
        await ExecuteInTransactionAsync(async () =>
        {
            var dept = await _deptRepository.GetByIdAsync(input.Id);
            if (dept == null)
            {
                throw new LeanException("部门不存在");
            }

            if (dept.IsBuiltin == 1)
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
            x => deptIds.Contains(x.Id) && x.DeptStatus == 2);

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
            x => deptIds.Contains(x.Id) && x.DeptStatus == 2);

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

    /// <summary>
    /// 批量删除部门
    /// </summary>
    public async Task<LeanApiResult> BatchDeleteAsync(List<long> ids)
    {
        return await ExecuteInTransactionAsync(async () =>
        {
            foreach (var id in ids)
            {
                await DeleteAsync(id);
            }
            return LeanApiResult.Ok();
        }, "批量删除部门");
    }

    /// <summary>
    /// 分页查询部门
    /// </summary>
    public async Task<LeanApiResult<LeanPageResult<LeanDeptDto>>> GetPageAsync(LeanDeptQueryDto input)
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

        // 执行分页查询
        var (total, items) = await _deptRepository.GetPageListAsync(predicate, input.PageSize, input.PageIndex);
        var list = items.Adapt<List<LeanDeptDto>>();

        var result = new LeanPageResult<LeanDeptDto>
        {
            Total = total,
            Items = list,
            PageIndex = input.PageIndex,
            PageSize = input.PageSize
        };

        return LeanApiResult<LeanPageResult<LeanDeptDto>>.Ok(result);
    }

    /// <summary>
    /// 设置部门状态
    /// </summary>
    public async Task<LeanApiResult> SetStatusAsync(LeanDeptChangeStatusDto input)
    {
        return await ExecuteInTransactionAsync(async () =>
        {
            var dept = await _deptRepository.GetByIdAsync(input.Id);
            if (dept == null)
            {
                throw new LeanException("部门不存在");
            }

            if (dept.IsBuiltin == 1)
            {
                throw new LeanException("内置部门不能修改状态");
            }

            dept.DeptStatus = input.DeptStatus;
            await _deptRepository.UpdateAsync(dept);

            LogAudit("ChangeDeptStatus", $"修改部门状态: {dept.DeptName}, 状态: {input.DeptStatus}");
            return LeanApiResult.Ok();
        }, "修改部门状态");
    }

    /// <summary>
    /// 导出部门数据
    /// </summary>
    public async Task<byte[]> ExportAsync(LeanDeptQueryDto input)
    {
        // 获取部门列表
        var depts = await QueryAsync(input);

        // 转换为导出DTO
        var exportDtos = depts.Select(x => new LeanDeptExportDto
        {
            DeptCode = x.DeptCode,
            DeptName = x.DeptName,
            ParentId = x.ParentId,
            DeptDescription = x.DeptDescription,
            Leader = x.Leader,
            Phone = x.Phone,
            Email = x.Email,
            OrderNum = x.OrderNum,
            DeptStatus = x.DeptStatus,
            IsBuiltin = x.IsBuiltin,
            //CreateTime = x.CreateTime
        }).ToList();

        // 导出Excel
        return LeanExcelHelper.Export(exportDtos);
    }

    /// <summary>
    /// 导入部门数据
    /// </summary>
    public async Task<LeanDeptImportResultDto> ImportAsync(LeanFileInfo file)
    {
        var result = new LeanDeptImportResultDto();
        try
        {
            // 读取Excel文件
            var bytes = new byte[file.Stream.Length];
            await file.Stream.ReadAsync(bytes, 0, (int)file.Stream.Length);
            var importResult = LeanExcelHelper.Import<LeanDeptImportDto>(bytes);

            foreach (var item in importResult.Data)
            {
                try
                {
                    // 检查部门编码是否存在
                    var exists = await _deptRepository.AnyAsync(x => x.DeptCode == item.DeptCode);
                    if (exists)
                    {
                        result.AddError(item.DeptCode, $"部门编码 {item.DeptCode} 已存在");
                        continue;
                    }

                    // 创建部门实体
                    var dept = new LeanDept
                    {
                        DeptCode = item.DeptCode,
                        DeptName = item.DeptName,
                        ParentId = item.ParentId,
                        OrderNum = item.OrderNum,
                        Leader = item.Leader,
                        Phone = item.Phone,
                        Email = item.Email,
                        DeptStatus = 2, // Normal = 2
                        IsBuiltin = 0, // No = 0
                        DataScope = 3 // Self = 3
                    };

                    // 保存部门
                    await _deptRepository.CreateAsync(dept);
                    LogAudit("ImportDept", $"导入部门: {dept.DeptName}");
                    result.SuccessCount++;
                }
                catch (Exception ex)
                {
                    result.AddError(item.DeptCode, ex.Message);
                }
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
    public async Task<byte[]> GetImportTemplateAsync()
    {
        var template = new List<LeanDeptImportDto>
    {
      new LeanDeptImportDto
      {
        DeptCode = "dept001",
        DeptName = "示例部门",
        ParentId = 0,
        OrderNum = 1,
        Leader = "张三",
        Phone = "13800138000",
        Email = "zhangsan@example.com"
      }
    };

        return await Task.FromResult(LeanExcelHelper.Export(template));
    }
}