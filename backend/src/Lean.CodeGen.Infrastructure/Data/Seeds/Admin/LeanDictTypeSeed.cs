using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Domain.Entities.Admin;
using SqlSugar;
using System.Threading.Tasks;
using NLog;
using ILogger = NLog.ILogger;
using Lean.CodeGen.Infrastructure.Data.Seeds.Extensions;

namespace Lean.CodeGen.Infrastructure.Data.Seeds.Admin;

/// <summary>
/// 字典类型种子数据
/// </summary>
/// <remarks>
/// 初始化系统所有枚举类型对应的字典类型
/// </remarks>
public class LeanDictTypeSeed
{
  private readonly ISqlSugarClient _db;
  private readonly ILogger _logger;

  public LeanDictTypeSeed(ISqlSugarClient db)
  {
    _db = db;
    _logger = LogManager.GetCurrentClassLogger();
  }

  public async Task InitializeAsync()
  {
    _logger.Info("开始初始化字典类型数据...");

    var defaultDictTypes = new List<LeanDictType>
        {
            // 系统状态
            new()
            {
                DictTypeCode = "sys_status",
                DictTypeName = "系统状态",
                OrderNum = 1,
                DictTypeStatus=1,
                IsBuiltin = 1,
                Remark = "系统通用状态"
            },
            // 用户性别
            new()
            {
                DictTypeCode = "sys_gender",
                DictTypeName = "用户性别",
                OrderNum = 2,
                DictTypeStatus=1,
                IsBuiltin = 1,
                Remark = "用户性别列表"
            },
            // 用户类型
            new()
            {
                DictTypeCode = "sys_user_type",
                DictTypeName = "用户类型",
                OrderNum = 3,
                DictTypeStatus=1,
                IsBuiltin = 1,
                Remark = "用户类型列表"
            },
            // 用户状态
            new()
            {
                DictTypeCode = "sys_user_status",
                DictTypeName = "用户状态",
                OrderNum = 4,
                DictTypeStatus=1,
                IsBuiltin = 1,
                Remark = "用户状态列表"
            },
            // 角色状态
            new()
            {
                DictTypeCode = "sys_role_status",
                DictTypeName = "角色状态",
                OrderNum = 5,
                DictTypeStatus=1,
                IsBuiltin = 1,
                Remark = "角色状态列表"
            },
            // 部门状态
            new()
            {
                DictTypeCode = "sys_dept_status",
                DictTypeName = "部门状态",
                OrderNum = 6,
                DictTypeStatus=1,
                IsBuiltin = 1,
                Remark = "部门状态列表"
            },
            // 菜单状态
            new()
            {
                DictTypeCode = "sys_menu_status",
                DictTypeName = "菜单状态",
                OrderNum = 7,
                DictTypeStatus=1,
                IsBuiltin = 1,
                Remark = "菜单状态列表"
            },
            // 菜单类型
            new()
            {
                DictTypeCode = "sys_menu_type",
                DictTypeName = "菜单类型",
                OrderNum = 8,
                DictTypeStatus=1,
                IsBuiltin = 1,
                Remark = "菜单类型列表"
            },
            // 操作类型
            new()
            {
                DictTypeCode = "sys_oper_type",
                DictTypeName = "操作类型",
                OrderNum = 9,
                DictTypeStatus=1,
                IsBuiltin = 1,
                Remark = "操作类型列表"
            },
            // 操作状态
            new()
            {
                DictTypeCode = "sys_oper_status",
                DictTypeName = "操作状态",
                OrderNum = 10,
                DictTypeStatus=1,
                IsBuiltin = 1,
                Remark = "操作状态列表"
            },
            // 登录类型
            new()
            {
                DictTypeCode = "sys_login_type",
                DictTypeName = "登录类型",
                OrderNum = 11,
                DictTypeStatus=1,
                IsBuiltin = 1,
                Remark = "登录类型列表"
            },
            // 登录状态
            new()
            {
                DictTypeCode = "sys_login_status",
                DictTypeName = "登录状态",
                OrderNum = 12,
                DictTypeStatus=1,
                IsBuiltin = 1,
                Remark = "登录状态列表"
            },
            // 日志级别
            new()
            {
                DictTypeCode = "sys_log_level",
                DictTypeName = "日志级别",
                OrderNum = 13,
                DictTypeStatus=1,
                IsBuiltin = 1,
                Remark = "日志级别列表"
            },
            // 设备状态
            new()
            {
                DictTypeCode = "sys_device_status",
                DictTypeName = "设备状态",
                OrderNum = 14,
                DictTypeStatus=1,
                IsBuiltin = 1,
                Remark = "设备状态列表"
            },
            // 设备类型
            new()
            {
                DictTypeCode = "sys_device_type",
                DictTypeName = "设备类型",
                OrderNum = 15,
                DictTypeStatus=1,
                IsBuiltin = 1,
                Remark = "设备类型列表"
            },
            // 字典类型分类
            new()
            {
                DictTypeCode = "sys_dict_type_category",
                DictTypeName = "字典类型分类",
                OrderNum = 16,
                DictTypeStatus=1,
                IsBuiltin = 1,
                Remark = "字典类型分类列表"
            },
            // 差异类型
            new()
            {
                DictTypeCode = "sys_diff_type",
                DictTypeName = "差异类型",
                OrderNum = 17,
                DictTypeStatus=1,
                IsBuiltin = 1,
                Remark = "差异类型列表"
            },
            // 处理状态
            new()
            {
                DictTypeCode = "sys_handle_status",
                DictTypeName = "处理状态",
                OrderNum = 18,
                DictTypeStatus=1,
                IsBuiltin = 1,
                Remark = "处理状态列表"
            },
            // 业务类型
            new()
            {
                DictTypeCode = "sys_business_type",
                DictTypeName = "业务类型",
                OrderNum = 19,
                DictTypeStatus=1,
                IsBuiltin = 1,
                Remark = "业务类型列表"
            },
            // 配置类型
            new()
            {
                DictTypeCode = "sys_config_type",
                DictTypeName = "配置类型",
                OrderNum = 20,
                DictTypeStatus=1,
                IsBuiltin = 1,
                Remark = "配置类型列表"
            },
            // 数据权限类型
            new()
            {
                DictTypeCode = "sys_data_scope_type",
                DictTypeName = "数据权限类型",
                OrderNum = 21,
                DictTypeStatus=1,
                IsBuiltin = 1,
                Remark = "数据权限类型列表"
            },
            // API状态
            new()
            {
                DictTypeCode = "sys_api_status",
                DictTypeName = "API状态",
                OrderNum = 22,
                DictTypeStatus=1,
                IsBuiltin = 1,
                Remark = "API状态列表"
            },
            // 审计操作类型
            new()
            {
                DictTypeCode = "sys_audit_oper_type",
                DictTypeName = "审计操作类型",
                OrderNum = 23,
                DictTypeStatus=1,
                IsBuiltin = 1,
                Remark = "审计操作类型列表"
            },
            // 审计状态
            new()
            {
                DictTypeCode = "sys_audit_status",
                DictTypeName = "审计状态",
                OrderNum = 24,
                DictTypeStatus=1,
                IsBuiltin = 1,
                Remark = "审计状态列表"
            },
            // 内置状态
            new()
            {
                DictTypeCode = "sys_builtin_status",
                DictTypeName = "内置状态",
                OrderNum = 25,
                DictTypeStatus=1,
                IsBuiltin = 1,
                Remark = "内置状态列表"
            },
            // 工作流任务结果
            new()
            {
                DictTypeCode = "sys_workflow_task_result",
                DictTypeName = "工作流任务结果",
                OrderNum = 26,
                DictTypeStatus=1,
                IsBuiltin = 1,
                Remark = "工作流任务结果列表"
            },
            // 工作流任务状态
            new()
            {
                DictTypeCode = "sys_workflow_task_status",
                DictTypeName = "工作流任务状态",
                OrderNum = 27,
                DictTypeStatus=1,
                IsBuiltin = 1,
                Remark = "工作流任务状态列表"
            },
            // 工作流任务类型
            new()
            {
                DictTypeCode = "sys_workflow_task_type",
                DictTypeName = "工作流任务类型",
                OrderNum = 28,
                DictTypeStatus=1,
                IsBuiltin = 1,
                Remark = "工作流任务类型列表"
            },
            // 工作流活动状态
            new()
            {
                DictTypeCode = "sys_workflow_activity_status",
                DictTypeName = "工作流活动状态",
                OrderNum = 29,
                DictTypeStatus=1,
                IsBuiltin = 1,
                Remark = "工作流活动状态列表"
            },
            // 工作流实例状态
            new()
            {
                DictTypeCode = "sys_workflow_instance_status",
                DictTypeName = "工作流实例状态",
                OrderNum = 30,
                DictTypeStatus=1,
                IsBuiltin = 1,
                Remark = "工作流实例状态列表"
            },
            // 工作流节点类型
            new()
            {
                DictTypeCode = "sys_workflow_node_type",
                DictTypeName = "工作流节点类型",
                OrderNum = 31,
                DictTypeStatus=1,
                IsBuiltin = 1,
                Remark = "工作流节点类型列表"
            },
            // 工作流操作类型
            new()
            {
                DictTypeCode = "sys_workflow_operation_type",
                DictTypeName = "工作流操作类型",
                OrderNum = 32,
                DictTypeStatus=1,
                IsBuiltin = 1,
                Remark = "工作流操作类型列表"
            },
            // 工作流状态
            new()
            {
                DictTypeCode = "sys_workflow_status",
                DictTypeName = "工作流状态",
                OrderNum = 33,
                DictTypeStatus=1,
                IsBuiltin = 1,
                Remark = "工作流状态列表"
            },
            // 主体类型
            new()
            {
                DictTypeCode = "sys_subject_type",
                DictTypeName = "主体类型",
                OrderNum = 34,
                DictTypeStatus=1,
                IsBuiltin = 1,
                Remark = "主体类型列表"
            },
            // 权限类型
            new()
            {
                DictTypeCode = "sys_permission_type",
                DictTypeName = "权限类型",
                OrderNum = 35,
                DictTypeStatus=1,
                IsBuiltin = 1,
                Remark = "权限类型列表"
            },
            // 岗位状态
            new()
            {
                DictTypeCode = "sys_post_status",
                DictTypeName = "岗位状态",
                OrderNum = 36,
                DictTypeStatus=1,
                IsBuiltin = 1,
                Remark = "岗位状态列表"
            },
            // 主要状态
            new()
            {
                DictTypeCode = "sys_primary_status",
                DictTypeName = "主要状态",
                OrderNum = 37,
                DictTypeStatus=1,
                IsBuiltin = 1,
                Remark = "主要状态列表"
            },
            // 资源类型
            new()
            {
                DictTypeCode = "sys_resource_type",
                DictTypeName = "资源类型",
                OrderNum = 38,
                DictTypeStatus=1,
                IsBuiltin = 1,
                Remark = "资源类型列表"
            },
            // 系统是否
            new()
            {
                DictTypeCode = "sys_yes_no",
                DictTypeName = "系统是否",
                OrderNum = 39,
                DictTypeStatus=1,
                IsBuiltin = 1,
                Remark = "系统是否列表"
            }
        };

    foreach (var dictType in defaultDictTypes)
    {
      var exists = await _db.Queryable<LeanDictType>()
          .FirstAsync(x => x.DictTypeCode == dictType.DictTypeCode);

      if (exists != null)
      {
        dictType.Id = exists.Id;
        // 复制原有审计信息并初始化更新信息
        dictType.CopyAuditFields(exists).InitAuditFields(true);
        await _db.Updateable(dictType).ExecuteCommandAsync();
        _logger.Info($"更新字典类型: {dictType.DictTypeName}");
      }
      else
      {
        // 初始化审计字段
        dictType.InitAuditFields();
        await _db.Insertable(dictType).ExecuteCommandAsync();
        _logger.Info($"新增字典类型: {dictType.DictTypeName}");
      }
    }

    _logger.Info("字典类型数据初始化完成");
  }
}