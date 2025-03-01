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
                DictCode = "sys_status",
                DictName = "系统状态",
                OrderNum = 1,
                Status = LeanStatus.Enable,
                IsBuiltin = LeanBuiltinStatus.Yes,
                Remark = "系统通用状态"
            },
            // 用户性别
            new()
            {
                DictCode = "sys_gender",
                DictName = "用户性别",
                OrderNum = 2,
                Status = LeanStatus.Enable,
                IsBuiltin = LeanBuiltinStatus.Yes,
                Remark = "用户性别列表"
            },
            // 用户类型
            new()
            {
                DictCode = "sys_user_type",
                DictName = "用户类型",
                OrderNum = 3,
                Status = LeanStatus.Enable,
                IsBuiltin = LeanBuiltinStatus.Yes,
                Remark = "用户类型列表"
            },
            // 用户状态
            new()
            {
                DictCode = "sys_user_status",
                DictName = "用户状态",
                OrderNum = 4,
                Status = LeanStatus.Enable,
                IsBuiltin = LeanBuiltinStatus.Yes,
                Remark = "用户状态列表"
            },
            // 角色状态
            new()
            {
                DictCode = "sys_role_status",
                DictName = "角色状态",
                OrderNum = 5,
                Status = LeanStatus.Enable,
                IsBuiltin = LeanBuiltinStatus.Yes,
                Remark = "角色状态列表"
            },
            // 部门状态
            new()
            {
                DictCode = "sys_dept_status",
                DictName = "部门状态",
                OrderNum = 6,
                Status = LeanStatus.Enable,
                IsBuiltin = LeanBuiltinStatus.Yes,
                Remark = "部门状态列表"
            },
            // 菜单状态
            new()
            {
                DictCode = "sys_menu_status",
                DictName = "菜单状态",
                OrderNum = 7,
                Status = LeanStatus.Enable,
                IsBuiltin = LeanBuiltinStatus.Yes,
                Remark = "菜单状态列表"
            },
            // 菜单类型
            new()
            {
                DictCode = "sys_menu_type",
                DictName = "菜单类型",
                OrderNum = 8,
                Status = LeanStatus.Enable,
                IsBuiltin = LeanBuiltinStatus.Yes,
                Remark = "菜单类型列表"
            },
            // 操作类型
            new()
            {
                DictCode = "sys_oper_type",
                DictName = "操作类型",
                OrderNum = 9,
                Status = LeanStatus.Enable,
                IsBuiltin = LeanBuiltinStatus.Yes,
                Remark = "操作类型列表"
            },
            // 操作状态
            new()
            {
                DictCode = "sys_oper_status",
                DictName = "操作状态",
                OrderNum = 10,
                Status = LeanStatus.Enable,
                IsBuiltin = LeanBuiltinStatus.Yes,
                Remark = "操作状态列表"
            },
            // 登录类型
            new()
            {
                DictCode = "sys_login_type",
                DictName = "登录类型",
                OrderNum = 11,
                Status = LeanStatus.Enable,
                IsBuiltin = LeanBuiltinStatus.Yes,
                Remark = "登录类型列表"
            },
            // 登录状态
            new()
            {
                DictCode = "sys_login_status",
                DictName = "登录状态",
                OrderNum = 12,
                Status = LeanStatus.Enable,
                IsBuiltin = LeanBuiltinStatus.Yes,
                Remark = "登录状态列表"
            },
            // 日志级别
            new()
            {
                DictCode = "sys_log_level",
                DictName = "日志级别",
                OrderNum = 13,
                Status = LeanStatus.Enable,
                IsBuiltin = LeanBuiltinStatus.Yes,
                Remark = "日志级别列表"
            },
            // 设备状态
            new()
            {
                DictCode = "sys_device_status",
                DictName = "设备状态",
                OrderNum = 14,
                Status = LeanStatus.Enable,
                IsBuiltin = LeanBuiltinStatus.Yes,
                Remark = "设备状态列表"
            },
            // 设备类型
            new()
            {
                DictCode = "sys_device_type",
                DictName = "设备类型",
                OrderNum = 15,
                Status = LeanStatus.Enable,
                IsBuiltin = LeanBuiltinStatus.Yes,
                Remark = "设备类型列表"
            },
            // 字典类型分类
            new()
            {
                DictCode = "sys_dict_type_category",
                DictName = "字典类型分类",
                OrderNum = 16,
                Status = LeanStatus.Enable,
                IsBuiltin = LeanBuiltinStatus.Yes,
                Remark = "字典类型分类列表"
            },
            // 差异类型
            new()
            {
                DictCode = "sys_diff_type",
                DictName = "差异类型",
                OrderNum = 17,
                Status = LeanStatus.Enable,
                IsBuiltin = LeanBuiltinStatus.Yes,
                Remark = "差异类型列表"
            },
            // 处理状态
            new()
            {
                DictCode = "sys_handle_status",
                DictName = "处理状态",
                OrderNum = 18,
                Status = LeanStatus.Enable,
                IsBuiltin = LeanBuiltinStatus.Yes,
                Remark = "处理状态列表"
            },
            // 业务类型
            new()
            {
                DictCode = "sys_business_type",
                DictName = "业务类型",
                OrderNum = 19,
                Status = LeanStatus.Enable,
                IsBuiltin = LeanBuiltinStatus.Yes,
                Remark = "业务类型列表"
            },
            // 配置类型
            new()
            {
                DictCode = "sys_config_type",
                DictName = "配置类型",
                OrderNum = 20,
                Status = LeanStatus.Enable,
                IsBuiltin = LeanBuiltinStatus.Yes,
                Remark = "配置类型列表"
            },
            // 数据权限类型
            new()
            {
                DictCode = "sys_data_scope_type",
                DictName = "数据权限类型",
                OrderNum = 21,
                Status = LeanStatus.Enable,
                IsBuiltin = LeanBuiltinStatus.Yes,
                Remark = "数据权限类型列表"
            },
            // API状态
            new()
            {
                DictCode = "sys_api_status",
                DictName = "API状态",
                OrderNum = 22,
                Status = LeanStatus.Enable,
                IsBuiltin = LeanBuiltinStatus.Yes,
                Remark = "API状态列表"
            },
            // 审计操作类型
            new()
            {
                DictCode = "sys_audit_oper_type",
                DictName = "审计操作类型",
                OrderNum = 23,
                Status = LeanStatus.Enable,
                IsBuiltin = LeanBuiltinStatus.Yes,
                Remark = "审计操作类型列表"
            },
            // 审计状态
            new()
            {
                DictCode = "sys_audit_status",
                DictName = "审计状态",
                OrderNum = 24,
                Status = LeanStatus.Enable,
                IsBuiltin = LeanBuiltinStatus.Yes,
                Remark = "审计状态列表"
            },
            // 内置状态
            new()
            {
                DictCode = "sys_builtin_status",
                DictName = "内置状态",
                OrderNum = 25,
                Status = LeanStatus.Enable,
                IsBuiltin = LeanBuiltinStatus.Yes,
                Remark = "内置状态列表"
            },
            // 工作流任务结果
            new()
            {
                DictCode = "sys_workflow_task_result",
                DictName = "工作流任务结果",
                OrderNum = 26,
                Status = LeanStatus.Enable,
                IsBuiltin = LeanBuiltinStatus.Yes,
                Remark = "工作流任务结果列表"
            },
            // 工作流任务状态
            new()
            {
                DictCode = "sys_workflow_task_status",
                DictName = "工作流任务状态",
                OrderNum = 27,
                Status = LeanStatus.Enable,
                IsBuiltin = LeanBuiltinStatus.Yes,
                Remark = "工作流任务状态列表"
            },
            // 工作流任务类型
            new()
            {
                DictCode = "sys_workflow_task_type",
                DictName = "工作流任务类型",
                OrderNum = 28,
                Status = LeanStatus.Enable,
                IsBuiltin = LeanBuiltinStatus.Yes,
                Remark = "工作流任务类型列表"
            },
            // 工作流活动状态
            new()
            {
                DictCode = "sys_workflow_activity_status",
                DictName = "工作流活动状态",
                OrderNum = 29,
                Status = LeanStatus.Enable,
                IsBuiltin = LeanBuiltinStatus.Yes,
                Remark = "工作流活动状态列表"
            },
            // 工作流实例状态
            new()
            {
                DictCode = "sys_workflow_instance_status",
                DictName = "工作流实例状态",
                OrderNum = 30,
                Status = LeanStatus.Enable,
                IsBuiltin = LeanBuiltinStatus.Yes,
                Remark = "工作流实例状态列表"
            },
            // 工作流节点类型
            new()
            {
                DictCode = "sys_workflow_node_type",
                DictName = "工作流节点类型",
                OrderNum = 31,
                Status = LeanStatus.Enable,
                IsBuiltin = LeanBuiltinStatus.Yes,
                Remark = "工作流节点类型列表"
            },
            // 工作流操作类型
            new()
            {
                DictCode = "sys_workflow_operation_type",
                DictName = "工作流操作类型",
                OrderNum = 32,
                Status = LeanStatus.Enable,
                IsBuiltin = LeanBuiltinStatus.Yes,
                Remark = "工作流操作类型列表"
            },
            // 工作流状态
            new()
            {
                DictCode = "sys_workflow_status",
                DictName = "工作流状态",
                OrderNum = 33,
                Status = LeanStatus.Enable,
                IsBuiltin = LeanBuiltinStatus.Yes,
                Remark = "工作流状态列表"
            },
            // 主体类型
            new()
            {
                DictCode = "sys_subject_type",
                DictName = "主体类型",
                OrderNum = 34,
                Status = LeanStatus.Enable,
                IsBuiltin = LeanBuiltinStatus.Yes,
                Remark = "主体类型列表"
            },
            // 权限类型
            new()
            {
                DictCode = "sys_permission_type",
                DictName = "权限类型",
                OrderNum = 35,
                Status = LeanStatus.Enable,
                IsBuiltin = LeanBuiltinStatus.Yes,
                Remark = "权限类型列表"
            },
            // 岗位状态
            new()
            {
                DictCode = "sys_post_status",
                DictName = "岗位状态",
                OrderNum = 36,
                Status = LeanStatus.Enable,
                IsBuiltin = LeanBuiltinStatus.Yes,
                Remark = "岗位状态列表"
            },
            // 主要状态
            new()
            {
                DictCode = "sys_primary_status",
                DictName = "主要状态",
                OrderNum = 37,
                Status = LeanStatus.Enable,
                IsBuiltin = LeanBuiltinStatus.Yes,
                Remark = "主要状态列表"
            },
            // 资源类型
            new()
            {
                DictCode = "sys_resource_type",
                DictName = "资源类型",
                OrderNum = 38,
                Status = LeanStatus.Enable,
                IsBuiltin = LeanBuiltinStatus.Yes,
                Remark = "资源类型列表"
            },
            // 系统是否
            new()
            {
                DictCode = "sys_yes_no",
                DictName = "系统是否",
                OrderNum = 39,
                Status = LeanStatus.Enable,
                IsBuiltin = LeanBuiltinStatus.Yes,
                Remark = "系统是否列表"
            }
        };

    foreach (var dictType in defaultDictTypes)
    {
      var exists = await _db.Queryable<LeanDictType>()
          .FirstAsync(x => x.DictCode == dictType.DictCode);

      if (exists != null)
      {
        dictType.Id = exists.Id;
        // 复制原有审计信息并初始化更新信息
        dictType.CopyAuditFields(exists).InitAuditFields(true);
        await _db.Updateable(dictType).ExecuteCommandAsync();
        _logger.Info($"更新字典类型: {dictType.DictName}");
      }
      else
      {
        // 初始化审计字段
        dictType.InitAuditFields();
        await _db.Insertable(dictType).ExecuteCommandAsync();
        _logger.Info($"新增字典类型: {dictType.DictName}");
      }
    }

    _logger.Info("字典类型数据初始化完成");
  }
}