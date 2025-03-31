using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Application.Services.Base;
using Lean.CodeGen.Common.Excel;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Exceptions;
using Lean.CodeGen.Common.Extensions;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Domain.Entities;
using Lean.CodeGen.Domain.Entities.Identity;
using Lean.CodeGen.Domain.Interfaces.Repositories;
using Lean.CodeGen.Domain.Validators;
using Mapster;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Lean.CodeGen.Common.Security;

namespace Lean.CodeGen.Application.Services.Identity
{
  /// <summary>
  /// 租户服务实现
  /// </summary>
  /// <remarks>
  /// 提供租户管理相关的业务功能，包括：
  /// 1. 租户的增删改查
  /// 2. 租户状态管理
  /// 3. 租户导入导出
  /// </remarks>
  public class LeanTenantService : LeanBaseService, ILeanTenantService
  {
    /// <summary>
    /// 租户仓储接口
    /// </summary>
    /// <remarks>
    /// 用于租户实体的持久化操作
    /// </remarks>
    private readonly ILeanRepository<LeanTenant> _tenantRepository;

    /// <summary>
    /// 租户唯一性验证器
    /// </summary>
    /// <remarks>
    /// 用于验证租户编码等字段的唯一性
    /// </remarks>
    private readonly LeanUniqueValidator<LeanTenant> _uniqueValidator;

    /// <summary>
    /// 日志记录器
    /// </summary>
    private readonly ILogger _logger;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="tenantRepository">租户仓储接口</param>
    /// <param name="logger">日志记录器</param>
    /// <param name="context">基础服务上下文</param>
    public LeanTenantService(
        ILeanRepository<LeanTenant> tenantRepository,
        ILogger logger,
        LeanBaseServiceContext context)
        : base(context)
    {
      _tenantRepository = tenantRepository;
      _logger = logger;
      _uniqueValidator = new LeanUniqueValidator<LeanTenant>(_tenantRepository);
    }

    /// <summary>
    /// 创建租户
    /// </summary>
    /// <param name="input">租户创建参数</param>
    /// <returns>创建成功的租户信息</returns>
    /// <remarks>
    /// 创建新租户时会进行以下操作：
    /// 1. 验证租户编码唯一性
    /// 2. 验证租户名称唯一性
    /// 3. 记录审计日志
    /// </remarks>
    public async Task<LeanTenantDto> CreateAsync(LeanTenantCreateDto input)
    {
      return await ExecuteInTransactionAsync(async () =>
      {
        // 验证租户编码唯一性
        await _uniqueValidator.ValidateAsync(x => x.TenantCode, input.TenantCode);

        // 验证租户名称唯一性
        await _uniqueValidator.ValidateAsync(x => x.TenantName, input.TenantName);

        // 创建租户实体
        var tenant = input.Adapt<LeanTenant>();

        // 插入租户记录
        await _tenantRepository.CreateAsync(tenant);

        LogAudit("CreateTenant", $"创建租户: {tenant.TenantName}");
        return await GetAsync(tenant.Id);
      }, "创建租户");
    }

    /// <summary>
    /// 更新租户
    /// </summary>
    /// <param name="input">租户更新参数</param>
    /// <returns>更新后的租户信息</returns>
    /// <remarks>
    /// 更新租户信息时会进行以下操作：
    /// 1. 检查租户是否存在
    /// 2. 检查是否为内置租户
    /// 3. 验证租户名称唯一性
    /// 4. 记录审计日志
    /// </remarks>
    public async Task<LeanTenantDto> UpdateAsync(LeanTenantUpdateDto input)
    {
      return await ExecuteInTransactionAsync(async () =>
      {
        var tenant = await _tenantRepository.GetByIdAsync(input.Id);
        if (tenant == null)
        {
          throw new LeanException("租户不存在");
        }

        if (tenant.IsBuiltin == 1)
        {
          throw new LeanException("内置租户不能修改");
        }

        // 验证租户名称唯一性
        await _uniqueValidator.ValidateAsync(x => x.TenantName, input.TenantName, input.Id);

        // 更新租户信息
        input.Adapt(tenant);
        await _tenantRepository.UpdateAsync(tenant);

        LogAudit("UpdateTenant", $"更新租户: {tenant.TenantName}");
        return await GetAsync(tenant.Id);
      }, "更新租户");
    }

    /// <summary>
    /// 删除租户
    /// </summary>
    /// <param name="input">租户删除参数</param>
    /// <remarks>
    /// 删除租户时会进行以下操作：
    /// 1. 检查租户是否存在
    /// 2. 检查是否为内置租户
    /// 3. 删除租户记录
    /// 4. 记录审计日志
    /// </remarks>
    public async Task DeleteAsync(LeanTenantDeleteDto input)
    {
      await ExecuteInTransactionAsync(async () =>
      {
        var tenant = await _tenantRepository.GetByIdAsync(input.Id);
        if (tenant == null)
        {
          throw new LeanException("租户不存在");
        }

        if (tenant.IsBuiltin == 1)
        {
          throw new LeanException("内置租户不能删除");
        }

        // 删除租户
        await _tenantRepository.DeleteAsync(tenant);

        LogAudit("DeleteTenant", $"删除租户: {tenant.TenantName}");
      }, "删除租户");
    }

    /// <summary>
    /// 获取租户信息
    /// </summary>
    /// <param name="id">租户ID</param>
    /// <returns>租户详细信息</returns>
    public async Task<LeanTenantDto> GetAsync(long id)
    {
      var tenant = await _tenantRepository.GetByIdAsync(id);
      if (tenant == null)
      {
        throw new LeanException("租户不存在");
      }

      return tenant.Adapt<LeanTenantDto>();
    }

    /// <summary>
    /// 分页查询租户
    /// </summary>
    /// <param name="input">查询参数</param>
    /// <returns>租户列表</returns>
    public async Task<LeanPageResult<LeanTenantDto>> GetPageAsync(LeanTenantQueryDto input)
    {
      // 构建查询条件
      Expression<Func<LeanTenant, bool>> predicate = x => true;

      if (!string.IsNullOrEmpty(input.TenantCode))
      {
        predicate = predicate.And(x => x.TenantCode.Contains(input.TenantCode));
      }

      if (!string.IsNullOrEmpty(input.TenantName))
      {
        predicate = predicate.And(x => x.TenantName.Contains(input.TenantName));
      }

      if (input.TenantType.HasValue)
      {
        predicate = predicate.And(x => x.TenantType == input.TenantType.Value);
      }

      if (!string.IsNullOrEmpty(input.ContactPerson))
      {
        predicate = predicate.And(x => x.TenantContactInfo.Contains(input.ContactPerson));
      }

      if (!string.IsNullOrEmpty(input.ContactPhone))
      {
        predicate = predicate.And(x => x.TenantContactInfo.Contains(input.ContactPhone));
      }

      if (!string.IsNullOrEmpty(input.Email))
      {
        predicate = predicate.And(x => x.TenantContactInfo.Contains(input.Email));
      }

      if (input.TenantStatus.HasValue)
      {
        predicate = predicate.And(x => x.TenantStatus == input.TenantStatus.Value);
      }

      if (input.IsBuiltin.HasValue)
      {
        predicate = predicate.And(x => x.IsBuiltin == input.IsBuiltin.Value);
      }

      // 查询数据
      var (total, items) = await _tenantRepository.GetPageListAsync(predicate, input.PageSize, input.PageIndex);

      // 转换为DTO
      return new LeanPageResult<LeanTenantDto>
      {
        Total = total,
        Items = items.Adapt<List<LeanTenantDto>>()
      };
    }

    /// <summary>
    /// 修改租户状态
    /// </summary>
    /// <param name="input">状态修改参数</param>
    public async Task ChangeStatusAsync(LeanTenantChangeStatusDto input)
    {
      await ExecuteInTransactionAsync(async () =>
      {
        var tenant = await _tenantRepository.GetByIdAsync(input.Id);
        if (tenant == null)
        {
          throw new LeanException("租户不存在");
        }

        if (tenant.IsBuiltin == 1)
        {
          throw new LeanException("内置租户不能修改状态");
        }

        tenant.TenantStatus = input.TenantStatus;
        await _tenantRepository.UpdateAsync(tenant);

        LogAudit("ChangeTenantStatus", $"修改租户状态: {tenant.TenantName}, 状态: {input.TenantStatus}");
      }, "修改租户状态");
    }

    /// <summary>
    /// 导出租户
    /// </summary>
    /// <param name="input">导出查询参数</param>
    /// <returns>导出文件字节数组</returns>
    public async Task<byte[]> ExportAsync(LeanTenantExportQueryDto input)
    {
      // 查询数据
      Expression<Func<LeanTenant, bool>> predicate = x => true;
      if (input.IsExportAll == 0 && input.SelectedIds.Any())
      {
        predicate = predicate.And(x => input.SelectedIds.Contains(x.Id));
      }

      var list = await _tenantRepository.GetListAsync(predicate);
      var data = list.Adapt<List<LeanTenantExportDto>>();

      // 导出Excel
      return LeanExcelHelper.Export(data);
    }

    /// <summary>
    /// 导入租户数据
    /// </summary>
    /// <param name="file">导入文件</param>
    /// <returns>导入结果</returns>
    public async Task<LeanTenantImportResultDto> ImportAsync(LeanFileInfo file)
    {
      var result = new LeanTenantImportResultDto();

      try
      {
        // 导入Excel
        using var memoryStream = new MemoryStream();
        await file.Stream.CopyToAsync(memoryStream);
        var importResult = LeanExcelHelper.Import<LeanTenantImportDto>(memoryStream.ToArray());
        if (!importResult.IsSuccess)
        {
          result.ErrorMessage = importResult.ErrorMessage;
          return result;
        }

        await ExecuteInTransactionAsync(async () =>
        {
          foreach (var item in importResult.Data)
          {
            try
            {
              // 验证租户编码唯一性
              if (await _tenantRepository.AnyAsync(x => x.TenantCode == item.TenantCode))
              {
                result.AddError(item.TenantCode, "租户编码已存在");
                continue;
              }

              // 验证租户名称唯一性
              if (await _tenantRepository.AnyAsync(x => x.TenantName == item.TenantName))
              {
                result.AddError(item.TenantCode, "租户名称已存在");
                continue;
              }

              // 创建租户
              var tenant = item.Adapt<LeanTenant>();
              await _tenantRepository.CreateAsync(tenant);

              result.SuccessCount++;
            }
            catch (Exception ex)
            {
              result.AddError(item.TenantCode, ex.Message);
            }
          }
        }, "导入租户");
      }
      catch (Exception ex)
      {
        _logger.Error(ex, "导入租户失败");
        throw new LeanException("导入租户失败");
      }

      return result;
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <returns>模板文件字节数组</returns>
    public async Task<byte[]> GetTemplateAsync()
    {
      var template = new List<LeanTenantImportDto>
      {
        new LeanTenantImportDto
        {
          TenantCode = "T001",
          TenantName = "示例租户",
          TenantType = 0,
          ContactPerson = "张三",
          ContactPhone = "13800138000",
          Email = "zhangsan@example.com"
        }
      };

      return LeanExcelHelper.Export(template, "租户导入模板");
    }
  }
}