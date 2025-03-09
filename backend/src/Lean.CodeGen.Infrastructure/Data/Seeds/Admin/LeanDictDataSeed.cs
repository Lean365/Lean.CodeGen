using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Domain.Entities.Admin;
using SqlSugar;
using System.Threading.Tasks;
using NLog;
using ILogger = NLog.ILogger;
using Lean.CodeGen.Infrastructure.Data.Seeds.Extensions;

namespace Lean.CodeGen.Infrastructure.Data.Seeds.Admin;

/// <summary>
/// 字典数据种子数据
/// </summary>
/// <remarks>
/// 初始化系统所有字典类型对应的字典数据
/// </remarks>
public class LeanDictDataSeed
{
  private readonly ISqlSugarClient _db;
  private readonly ILogger _logger;

  public LeanDictDataSeed(ISqlSugarClient db)
  {
    _db = db;
    _logger = LogManager.GetCurrentClassLogger();
  }

  public async Task InitializeAsync()
  {
    _logger.Info("开始初始化字典数据...");

    // 获取所有字典类型
    var dictTypes = await _db.Queryable<LeanDictType>().ToListAsync();

    foreach (var dictType in dictTypes)
    {
      await InitializeDictDataAsync(dictType);
    }

    _logger.Info("字典数据初始化完成");
  }

  private async Task InitializeDictDataAsync(LeanDictType dictType)
  {
    _logger.Info($"开始初始化字典类型[{dictType.DictTypeName}]的字典数据...");

    var defaultDictData = new List<LeanDictData>();

    switch (dictType.DictTypeCode)
    {
      case "sys_status":
        defaultDictData.AddRange(new[]
        {
                    CreateDictData(dictType.Id, "启用", "0", "success", 1),
                    CreateDictData(dictType.Id, "禁用", "1", "danger", 2)
                });
        break;

      case "sys_gender":
        defaultDictData.AddRange(new[]
        {
                    CreateDictData(dictType.Id, "男", "0", "primary", 1),
                    CreateDictData(dictType.Id, "女", "1", "warning", 2),
                    CreateDictData(dictType.Id, "未知", "2", "info", 3)
                });
        break;

      case "sys_user_type":
        defaultDictData.AddRange(new[]
        {
                    CreateDictData(dictType.Id, "系统用户", "0", "primary", 1),
                    CreateDictData(dictType.Id, "普通用户", "1", "success", 2)
                });
        break;

      case "sys_user_status":
        defaultDictData.AddRange(new[]
        {
                    CreateDictData(dictType.Id, "正常", "0", "success", 1),
                    CreateDictData(dictType.Id, "停用", "1", "danger", 2)
                });
        break;

      case "sys_role_status":
        defaultDictData.AddRange(new[]
        {
                    CreateDictData(dictType.Id, "正常", "0", "success", 1),
                    CreateDictData(dictType.Id, "停用", "1", "danger", 2)
                });
        break;

      case "sys_dept_status":
        defaultDictData.AddRange(new[]
        {
                    CreateDictData(dictType.Id, "正常", "0", "success", 1),
                    CreateDictData(dictType.Id, "停用", "1", "danger", 2)
                });
        break;

      case "sys_menu_status":
        defaultDictData.AddRange(new[]
        {
                    CreateDictData(dictType.Id, "显示", "0", "success", 1),
                    CreateDictData(dictType.Id, "隐藏", "1", "danger", 2)
                });
        break;

      case "sys_menu_type":
        defaultDictData.AddRange(new[]
        {
                    CreateDictData(dictType.Id, "目录", "M", "primary", 1),
                    CreateDictData(dictType.Id, "菜单", "C", "success", 2),
                    CreateDictData(dictType.Id, "按钮", "F", "warning", 3)
                });
        break;

      case "sys_oper_type":
        defaultDictData.AddRange(new[]
        {
                    CreateDictData(dictType.Id, "其他", "0", "info", 1),
                    CreateDictData(dictType.Id, "查询", "1", "primary", 2),
                    CreateDictData(dictType.Id, "新增", "2", "success", 3),
                    CreateDictData(dictType.Id, "修改", "3", "warning", 4),
                    CreateDictData(dictType.Id, "删除", "4", "danger", 5),
                    CreateDictData(dictType.Id, "导出", "5", "primary", 6),
                    CreateDictData(dictType.Id, "导入", "6", "warning", 7),
                    CreateDictData(dictType.Id, "强退", "7", "danger", 8),
                    CreateDictData(dictType.Id, "生成代码", "8", "success", 9),
                    CreateDictData(dictType.Id, "清空数据", "9", "danger", 10)
                });
        break;

      case "sys_oper_status":
        defaultDictData.AddRange(new[]
        {
                    CreateDictData(dictType.Id, "成功", "0", "success", 1),
                    CreateDictData(dictType.Id, "失败", "1", "danger", 2)
                });
        break;

      case "sys_login_type":
        defaultDictData.AddRange(new[]
        {
                    CreateDictData(dictType.Id, "账号密码", "0", "primary", 1),
                    CreateDictData(dictType.Id, "短信验证码", "1", "success", 2),
                    CreateDictData(dictType.Id, "邮箱验证码", "2", "info", 3),
                    CreateDictData(dictType.Id, "第三方登录", "3", "warning", 4)
                });
        break;

      case "sys_login_status":
        defaultDictData.AddRange(new[]
        {
                    CreateDictData(dictType.Id, "成功", "0", "success", 1),
                    CreateDictData(dictType.Id, "失败", "1", "danger", 2)
                });
        break;

      case "sys_log_level":
        defaultDictData.AddRange(new[]
        {
                    CreateDictData(dictType.Id, "跟踪", "Trace", "info", 1),
                    CreateDictData(dictType.Id, "调试", "Debug", "primary", 2),
                    CreateDictData(dictType.Id, "信息", "Info", "success", 3),
                    CreateDictData(dictType.Id, "警告", "Warn", "warning", 4),
                    CreateDictData(dictType.Id, "错误", "Error", "danger", 5),
                    CreateDictData(dictType.Id, "致命", "Fatal", "dark", 6)
                });
        break;

      case "sys_device_status":
        defaultDictData.AddRange(new[]
        {
                    CreateDictData(dictType.Id, "在线", "0", "success", 1),
                    CreateDictData(dictType.Id, "离线", "1", "danger", 2)
                });
        break;

      case "sys_device_type":
        defaultDictData.AddRange(new[]
        {
                    CreateDictData(dictType.Id, "PC", "0", "primary", 1),
                    CreateDictData(dictType.Id, "Android", "1", "success", 2),
                    CreateDictData(dictType.Id, "iOS", "2", "info", 3),
                    CreateDictData(dictType.Id, "小程序", "3", "warning", 4),
                    CreateDictData(dictType.Id, "其他", "4", "danger", 5)
                });
        break;

      case "sys_dict_type_category":
        defaultDictData.AddRange(new[]
        {
                    CreateDictData(dictType.Id, "传统类型", "0", "primary", 1),
                    CreateDictData(dictType.Id, "SQL类型", "1", "success", 2)
                });
        break;

      case "sys_diff_type":
        defaultDictData.AddRange(new[]
        {
                    CreateDictData(dictType.Id, "新增", "0", "success", 1),
                    CreateDictData(dictType.Id, "修改", "1", "warning", 2),
                    CreateDictData(dictType.Id, "删除", "2", "danger", 3)
                });
        break;

      case "sys_handle_status":
        defaultDictData.AddRange(new[]
        {
                    CreateDictData(dictType.Id, "未处理", "0", "info", 1),
                    CreateDictData(dictType.Id, "处理中", "1", "primary", 2),
                    CreateDictData(dictType.Id, "已处理", "2", "success", 3),
                    CreateDictData(dictType.Id, "已取消", "3", "warning", 4),
                    CreateDictData(dictType.Id, "已失败", "4", "danger", 5)
                });
        break;

      case "sys_business_type":
        defaultDictData.AddRange(new[]
        {
                    CreateDictData(dictType.Id, "其他", "0", "info", 1),
                    CreateDictData(dictType.Id, "新增", "1", "success", 2),
                    CreateDictData(dictType.Id, "修改", "2", "warning", 3),
                    CreateDictData(dictType.Id, "删除", "3", "danger", 4),
                    CreateDictData(dictType.Id, "授权", "4", "primary", 5),
                    CreateDictData(dictType.Id, "导出", "5", "warning", 6),
                    CreateDictData(dictType.Id, "导入", "6", "primary", 7),
                    CreateDictData(dictType.Id, "强退", "7", "danger", 8),
                    CreateDictData(dictType.Id, "生成代码", "8", "success", 9),
                    CreateDictData(dictType.Id, "清空数据", "9", "danger", 10)
                });
        break;

      case "sys_config_type":
        defaultDictData.AddRange(new[]
        {
                    CreateDictData(dictType.Id, "系统配置", "0", "primary", 1),
                    CreateDictData(dictType.Id, "业务配置", "1", "success", 2)
                });
        break;

      case "sys_data_scope_type":
        defaultDictData.AddRange(new[]
        {
                    CreateDictData(dictType.Id, "全部数据权限", "1", "primary", 1),
                    CreateDictData(dictType.Id, "自定数据权限", "2", "success", 2),
                    CreateDictData(dictType.Id, "部门数据权限", "3", "warning", 3),
                    CreateDictData(dictType.Id, "部门及以下数据权限", "4", "info", 4),
                    CreateDictData(dictType.Id, "仅本人数据权限", "5", "danger", 5)
                });
        break;

      case "sys_api_status":
        defaultDictData.AddRange(new[]
        {
                    CreateDictData(dictType.Id, "正常", "0", "success", 1),
                    CreateDictData(dictType.Id, "维护", "1", "warning", 2),
                    CreateDictData(dictType.Id, "停用", "2", "danger", 3)
                });
        break;

      case "sys_audit_oper_type":
        defaultDictData.AddRange(new[]
        {
                    CreateDictData(dictType.Id, "其他", "0", "info", 1),
                    CreateDictData(dictType.Id, "新增", "1", "success", 2),
                    CreateDictData(dictType.Id, "修改", "2", "warning", 3),
                    CreateDictData(dictType.Id, "删除", "3", "danger", 4),
                    CreateDictData(dictType.Id, "授权", "4", "primary", 5),
                    CreateDictData(dictType.Id, "导出", "5", "warning", 6),
                    CreateDictData(dictType.Id, "导入", "6", "primary", 7),
                    CreateDictData(dictType.Id, "强退", "7", "danger", 8),
                    CreateDictData(dictType.Id, "生成代码", "8", "success", 9),
                    CreateDictData(dictType.Id, "清空数据", "9", "danger", 10)
                });
        break;

      case "sys_audit_status":
        defaultDictData.AddRange(new[]
        {
                    CreateDictData(dictType.Id, "成功", "0", "success", 1),
                    CreateDictData(dictType.Id, "失败", "1", "danger", 2)
                });
        break;

      case "sys_builtin_status":
        defaultDictData.AddRange(new[]
        {
                    CreateDictData(dictType.Id, "是", "0", "primary", 1),
                    CreateDictData(dictType.Id, "否", "1", "danger", 2)
                });
        break;

      case "sys_workflow_task_result":
        defaultDictData.AddRange(new[]
        {
                    CreateDictData(dictType.Id, "同意", "0", "success", 1),
                    CreateDictData(dictType.Id, "拒绝", "1", "danger", 2),
                    CreateDictData(dictType.Id, "退回", "2", "warning", 3),
                    CreateDictData(dictType.Id, "转办", "3", "primary", 4),
                    CreateDictData(dictType.Id, "委派", "4", "info", 5)
                });
        break;

      case "sys_workflow_task_status":
        defaultDictData.AddRange(new[]
        {
                    CreateDictData(dictType.Id, "待处理", "0", "primary", 1),
                    CreateDictData(dictType.Id, "处理中", "1", "warning", 2),
                    CreateDictData(dictType.Id, "已完成", "2", "success", 3),
                    CreateDictData(dictType.Id, "已取消", "3", "info", 4),
                    CreateDictData(dictType.Id, "已超时", "4", "danger", 5)
                });
        break;

      case "sys_workflow_task_type":
        defaultDictData.AddRange(new[]
        {
                    CreateDictData(dictType.Id, "审批", "0", "primary", 1),
                    CreateDictData(dictType.Id, "会签", "1", "success", 2),
                    CreateDictData(dictType.Id, "或签", "2", "warning", 3),
                    CreateDictData(dictType.Id, "传阅", "3", "info", 4),
                    CreateDictData(dictType.Id, "抄送", "4", "danger", 5)
                });
        break;

      case "sys_workflow_activity_status":
        defaultDictData.AddRange(new[]
        {
                    CreateDictData(dictType.Id, "待处理", "0", "primary", 1),
                    CreateDictData(dictType.Id, "处理中", "1", "warning", 2),
                    CreateDictData(dictType.Id, "已完成", "2", "success", 3),
                    CreateDictData(dictType.Id, "已取消", "3", "info", 4),
                    CreateDictData(dictType.Id, "已超时", "4", "danger", 5)
                });
        break;

      case "sys_workflow_instance_status":
        defaultDictData.AddRange(new[]
        {
                    CreateDictData(dictType.Id, "草稿", "0", "info", 1),
                    CreateDictData(dictType.Id, "运行中", "1", "primary", 2),
                    CreateDictData(dictType.Id, "已完成", "2", "success", 3),
                    CreateDictData(dictType.Id, "已取消", "3", "warning", 4),
                    CreateDictData(dictType.Id, "已终止", "4", "danger", 5)
                });
        break;

      case "sys_workflow_node_type":
        defaultDictData.AddRange(new[]
        {
                    CreateDictData(dictType.Id, "开始节点", "0", "primary", 1),
                    CreateDictData(dictType.Id, "审批节点", "1", "success", 2),
                    CreateDictData(dictType.Id, "分支节点", "2", "warning", 3),
                    CreateDictData(dictType.Id, "汇聚节点", "3", "info", 4),
                    CreateDictData(dictType.Id, "结束节点", "4", "danger", 5)
                });
        break;

      case "sys_workflow_operation_type":
        defaultDictData.AddRange(new[]
        {
                    CreateDictData(dictType.Id, "提交", "0", "primary", 1),
                    CreateDictData(dictType.Id, "同意", "1", "success", 2),
                    CreateDictData(dictType.Id, "拒绝", "2", "danger", 3),
                    CreateDictData(dictType.Id, "退回", "3", "warning", 4),
                    CreateDictData(dictType.Id, "转办", "4", "info", 5),
                    CreateDictData(dictType.Id, "委派", "5", "primary", 6),
                    CreateDictData(dictType.Id, "终止", "6", "danger", 7),
                    CreateDictData(dictType.Id, "撤回", "7", "warning", 8)
                });
        break;

      case "sys_workflow_status":
        defaultDictData.AddRange(new[]
        {
                    CreateDictData(dictType.Id, "正常", "0", "success", 1),
                    CreateDictData(dictType.Id, "停用", "1", "danger", 2)
                });
        break;

      case "sys_subject_type":
        defaultDictData.AddRange(new[]
        {
                    CreateDictData(dictType.Id, "用户", "0", "primary", 1),
                    CreateDictData(dictType.Id, "角色", "1", "success", 2),
                    CreateDictData(dictType.Id, "部门", "2", "warning", 3),
                    CreateDictData(dictType.Id, "岗位", "3", "info", 4)
                });
        break;

      case "sys_permission_type":
        defaultDictData.AddRange(new[]
        {
                    CreateDictData(dictType.Id, "菜单", "0", "primary", 1),
                    CreateDictData(dictType.Id, "按钮", "1", "success", 2),
                    CreateDictData(dictType.Id, "接口", "2", "warning", 3),
                    CreateDictData(dictType.Id, "数据", "3", "info", 4)
                });
        break;

      case "sys_post_status":
        defaultDictData.AddRange(new[]
        {
                    CreateDictData(dictType.Id, "正常", "0", "success", 1),
                    CreateDictData(dictType.Id, "停用", "1", "danger", 2)
                });
        break;

      case "sys_primary_status":
        defaultDictData.AddRange(new[]
        {
                    CreateDictData(dictType.Id, "是", "0", "primary", 1),
                    CreateDictData(dictType.Id, "否", "1", "danger", 2)
                });
        break;

      case "sys_resource_type":
        defaultDictData.AddRange(new[]
        {
                    CreateDictData(dictType.Id, "图片", "0", "primary", 1),
                    CreateDictData(dictType.Id, "文档", "1", "success", 2),
                    CreateDictData(dictType.Id, "视频", "2", "warning", 3),
                    CreateDictData(dictType.Id, "音频", "3", "info", 4),
                    CreateDictData(dictType.Id, "其他", "4", "danger", 5)
                });
        break;

      case "sys_yes_no":
        defaultDictData.AddRange(new[]
        {
                    CreateDictData(dictType.Id, "是", "Y", "primary", 1),
                    CreateDictData(dictType.Id, "否", "N", "danger", 2)
                });
        break;
    }

    if (defaultDictData.Count > 0)
    {
      foreach (var dictData in defaultDictData)
      {
        var exists = await _db.Queryable<LeanDictData>()
            .FirstAsync(x => x.TypeId == dictData.TypeId && x.DictDataValue == dictData.DictDataValue);

        if (exists != null)
        {
          dictData.Id = exists.Id;
          // 复制原有审计信息并初始化更新信息
          dictData.CopyAuditFields(exists).InitAuditFields(true);
          await _db.Updateable(dictData).ExecuteCommandAsync();
          _logger.Info($"更新字典数据: {dictData.DictDataLabel}");
        }
        else
        {
          // 初始化审计字段
          dictData.InitAuditFields();
          await _db.Insertable(dictData).ExecuteCommandAsync();
          _logger.Info($"新增字典数据: {dictData.DictDataLabel}");
        }
      }
    }
  }

  private static LeanDictData CreateDictData(long typeId, string label, string value, string cssClass, int orderNum)
  {
    return new LeanDictData
    {
      TypeId = typeId,
      DictDataLabel = label,
      DictDataValue = value,
      CssClass = cssClass,
      ListClass = cssClass,
      TransKey = $"dict.{GetEnglishLabel(label)}.{typeId}{value}",
      OrderNum = orderNum,
      DictDataStatus = 2,
      IsBuiltin = 1,
      Remark = $"{label}({value})"
    };
  }

  private static string GetEnglishLabel(string label)
  {
    return label switch
    {
      "启用" => "enable",
      "禁用" => "disable",
      "男" => "male",
      "女" => "female",
      "未知" => "unknown",
      "系统用户" => "system_user",
      "普通用户" => "normal_user",
      "正常" => "normal",
      "停用" => "disabled",
      "显示" => "show",
      "隐藏" => "hide",
      "目录" => "directory",
      "菜单" => "menu",
      "按钮" => "button",
      "其他" => "other",
      "查询" => "query",
      "新增" => "add",
      "修改" => "edit",
      "删除" => "delete",
      "导出" => "export",
      "导入" => "import",
      "强退" => "force_quit",
      "生成代码" => "generate_code",
      "清空数据" => "clear_data",
      "成功" => "success",
      "失败" => "fail",
      "账号密码" => "account_password",
      "短信验证码" => "sms_code",
      "邮箱验证码" => "email_code",
      "第三方登录" => "third_party_login",
      "跟踪" => "trace",
      "调试" => "debug",
      "信息" => "info",
      "警告" => "warn",
      "错误" => "error",
      "致命" => "fatal",
      "在线" => "online",
      "离线" => "offline",
      "PC" => "pc",
      "小程序" => "mini_program",
      "传统类型" => "traditional_type",
      "SQL类型" => "sql_type",
      "未处理" => "unhandled",
      "处理中" => "processing",
      "已处理" => "handled",
      "已取消" => "cancelled",
      "已失败" => "failed",
      "授权" => "authorize",
      "系统配置" => "system_config",
      "业务配置" => "business_config",
      "全部数据权限" => "all_data_scope",
      "自定数据权限" => "custom_data_scope",
      "部门数据权限" => "dept_data_scope",
      "部门及以下数据权限" => "dept_and_child_data_scope",
      "仅本人数据权限" => "self_data_scope",
      "维护" => "maintenance",
      "同意" => "agree",
      "拒绝" => "reject",
      "退回" => "return",
      "转办" => "transfer",
      "委派" => "delegate",
      "待处理" => "pending",
      "已完成" => "completed",
      "已超时" => "timeout",
      "草稿" => "draft",
      "运行中" => "running",
      "已终止" => "terminated",
      "开始节点" => "start_node",
      "审批节点" => "approval_node",
      "分支节点" => "branch_node",
      "汇聚节点" => "converge_node",
      "结束节点" => "end_node",
      "提交" => "submit",
      "终止" => "terminate",
      "撤回" => "revoke",
      "用户" => "user",
      "角色" => "role",
      "部门" => "dept",
      "岗位" => "post",
      "接口" => "api",
      "数据" => "data",
      "是" => "yes",
      "否" => "no",
      "图片" => "image",
      "文档" => "document",
      "视频" => "video",
      "音频" => "audio",
      "审批" => "approval",
      "会签" => "countersign",
      "或签" => "or_sign",
      "传阅" => "circulate",
      "抄送" => "cc",
      _ => label.ToLower().Replace(" ", "_")
    };
  }
}