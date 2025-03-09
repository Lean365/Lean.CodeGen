using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Domain.Entities.Admin;
using SqlSugar;
using System.Threading.Tasks;
using NLog;
using ILogger = NLog.ILogger;
using Lean.CodeGen.Infrastructure.Data.Seeds.Extensions;

namespace Lean.CodeGen.Infrastructure.Data.Seeds.Admin;

/// <summary>
/// 字典翻译种子数据
/// </summary>
public class LeanDictTranslationSeed
{
  private readonly ISqlSugarClient _db;
  private readonly ILogger _logger;

  public LeanDictTranslationSeed(ISqlSugarClient db)
  {
    _db = db;
    _logger = LogManager.GetCurrentClassLogger();
  }

  public async Task InitializeAsync()
  {
    _logger.Info("开始初始化字典翻译数据...");

    // 获取中文和英文语言的ID
    var zhLang = await _db.Queryable<LeanLanguage>()
        .FirstAsync(l => l.LangCode == "zh-CN");
    var enLang = await _db.Queryable<LeanLanguage>()
        .FirstAsync(l => l.LangCode == "en-US");

    if (zhLang == null || enLang == null)
    {
      _logger.Error("未找到必需的中文或英文语言配置");
      return;
    }

    var dictTranslations = new List<LeanTranslation>();

    // 系统状态
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.enable.10", "启用", "Enable");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.disable.11", "禁用", "Disable");

    // 性别
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.male.20", "男", "Male");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.female.21", "女", "Female");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.unknown.22", "未知", "Unknown");

    // 用户类型
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.system_user.30", "系统用户", "System User");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.normal_user.31", "普通用户", "Normal User");

    // 用户状态
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.normal.40", "正常", "Normal");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.disabled.41", "停用", "Disabled");

    // 角色状态
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.normal.50", "正常", "Normal");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.disabled.51", "停用", "Disabled");

    // 部门状态
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.normal.60", "正常", "Normal");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.disabled.61", "停用", "Disabled");

    // 菜单状态
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.show.70", "显示", "Show");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.hide.71", "隐藏", "Hide");

    // 菜单类型
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.directory.8M", "目录", "Directory");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.menu.8C", "菜单", "Menu");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.button.8F", "按钮", "Button");

    // 操作类型
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.other.90", "其他", "Other");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.query.91", "查询", "Query");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.add.92", "新增", "Add");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.edit.93", "修改", "Edit");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.delete.94", "删除", "Delete");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.export.95", "导出", "Export");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.import.96", "导入", "Import");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.force_quit.97", "强退", "Force Quit");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.generate_code.98", "生成代码", "Generate Code");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.clear_data.99", "清空数据", "Clear Data");

    // 操作状态
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.success.100", "成功", "Success");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.fail.101", "失败", "Fail");

    // 登录类型
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.account_password.110", "账号密码", "Account Password");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.sms_code.111", "短信验证码", "SMS Code");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.email_code.112", "邮箱验证码", "Email Code");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.third_party_login.113", "第三方登录", "Third Party Login");

    // 登录状态
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.success.120", "成功", "Success");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.fail.121", "失败", "Fail");

    // 日志级别
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.trace.13Trace", "跟踪", "Trace");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.debug.13Debug", "调试", "Debug");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.info.13Info", "信息", "Info");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.warn.13Warn", "警告", "Warn");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.error.13Error", "错误", "Error");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.fatal.13Fatal", "致命", "Fatal");

    // 设备状态
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.online.140", "在线", "Online");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.offline.141", "离线", "Offline");

    // 设备类型
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.pc.150", "PC", "PC");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.android.151", "Android", "Android");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.ios.152", "iOS", "iOS");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.mini_program.153", "小程序", "Mini Program");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.other.154", "其他", "Other");

    // 字典类型分类
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.traditional_type.160", "传统类型", "Traditional Type");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.sql_type.161", "SQL类型", "SQL Type");

    // 差异类型
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.add.170", "新增", "Add");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.edit.171", "修改", "Edit");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.delete.172", "删除", "Delete");

    // 处理状态
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.unhandled.180", "未处理", "Unhandled");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.processing.181", "处理中", "Processing");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.handled.182", "已处理", "Handled");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.cancelled.183", "已取消", "Cancelled");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.failed.184", "已失败", "Failed");

    // 业务类型
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.other.190", "其他", "Other");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.add.191", "新增", "Add");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.edit.192", "修改", "Edit");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.delete.193", "删除", "Delete");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.authorize.194", "授权", "Authorize");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.export.195", "导出", "Export");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.import.196", "导入", "Import");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.force_quit.197", "强退", "Force Quit");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.generate_code.198", "生成代码", "Generate Code");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.clear_data.199", "清空数据", "Clear Data");

    // 配置类型
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.system_config.200", "系统配置", "System Config");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.business_config.201", "业务配置", "Business Config");

    // 数据权限类型
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.all_data_scope.211", "全部数据权限", "All Data Scope");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.custom_data_scope.212", "自定数据权限", "Custom Data Scope");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.dept_data_scope.213", "部门数据权限", "Department Data Scope");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.dept_and_child_data_scope.214", "部门及以下数据权限", "Department and Child Data Scope");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.self_data_scope.215", "仅本人数据权限", "Self Data Scope");

    // API状态
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.normal.220", "正常", "Normal");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.maintenance.221", "维护", "Maintenance");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.disabled.222", "停用", "Disabled");

    // 审计操作类型
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.other.230", "其他", "Other");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.add.231", "新增", "Add");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.edit.232", "修改", "Edit");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.delete.233", "删除", "Delete");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.authorize.234", "授权", "Authorize");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.export.235", "导出", "Export");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.import.236", "导入", "Import");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.force_quit.237", "强退", "Force Quit");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.generate_code.238", "生成代码", "Generate Code");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.clear_data.239", "清空数据", "Clear Data");

    // 审计状态
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.success.240", "成功", "Success");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.fail.241", "失败", "Fail");

    // 内置状态
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.yes.250", "是", "Yes");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.no.251", "否", "No");

    // 工作流任务结果
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.agree.260", "同意", "Agree");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.reject.261", "拒绝", "Reject");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.return.262", "退回", "Return");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.transfer.263", "转办", "Transfer");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.delegate.264", "委派", "Delegate");

    // 工作流任务状态
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.pending.270", "待处理", "Pending");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.processing.271", "处理中", "Processing");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.completed.272", "已完成", "Completed");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.cancelled.273", "已取消", "Cancelled");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.timeout.274", "已超时", "Timeout");

    // 工作流任务类型
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.approval.280", "审批", "Approval");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.countersign.281", "会签", "Countersign");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.or_sign.282", "或签", "Or Sign");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.circulate.283", "传阅", "Circulate");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.cc.284", "抄送", "CC");

    // 工作流活动状态
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.pending.290", "待处理", "Pending");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.processing.291", "处理中", "Processing");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.completed.292", "已完成", "Completed");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.cancelled.293", "已取消", "Cancelled");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.timeout.294", "已超时", "Timeout");

    // 工作流实例状态
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.draft.300", "草稿", "Draft");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.running.301", "运行中", "Running");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.completed.302", "已完成", "Completed");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.cancelled.303", "已取消", "Cancelled");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.terminated.304", "已终止", "Terminated");

    // 工作流节点类型
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.start_node.310", "开始节点", "Start Node");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.approval_node.311", "审批节点", "Approval Node");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.branch_node.312", "分支节点", "Branch Node");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.converge_node.313", "汇聚节点", "Converge Node");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.end_node.314", "结束节点", "End Node");

    // 工作流操作类型
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.submit.320", "提交", "Submit");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.agree.321", "同意", "Agree");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.reject.322", "拒绝", "Reject");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.return.323", "退回", "Return");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.transfer.324", "转办", "Transfer");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.delegate.325", "委派", "Delegate");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.terminate.326", "终止", "Terminate");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.revoke.327", "撤回", "Revoke");

    // 工作流状态
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.normal.330", "正常", "Normal");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.disabled.331", "停用", "Disabled");

    // 主体类型
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.user.340", "用户", "User");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.role.341", "角色", "Role");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.dept.342", "部门", "Department");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.post.343", "岗位", "Post");

    // 权限类型
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.menu.350", "菜单", "Menu");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.button.351", "按钮", "Button");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.api.352", "接口", "API");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.data.353", "数据", "Data");

    // 岗位状态
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.normal.360", "正常", "Normal");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.disabled.361", "停用", "Disabled");

    // 主要状态
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.yes.370", "是", "Yes");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.no.371", "否", "No");

    // 资源类型
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.image.380", "图片", "Image");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.document.381", "文档", "Document");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.video.382", "视频", "Video");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.audio.383", "音频", "Audio");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.other.384", "其他", "Other");

    // 是否
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.yes.39Y", "是", "Yes");
    AddDictTranslation(dictTranslations, zhLang.Id, enLang.Id, "dict.no.39N", "否", "No");

    // 更新或插入翻译数据
    foreach (var trans in dictTranslations)
    {
      var exists = await _db.Queryable<LeanTranslation>()
          .FirstAsync(t => t.LangId == trans.LangId && t.TransKey == trans.TransKey);

      if (exists != null)
      {
        trans.Id = exists.Id;
        // 复制原有审计信息并初始化更新信息
        trans.CopyAuditFields(exists).InitAuditFields(true);
        await _db.Updateable(trans).ExecuteCommandAsync();
        _logger.Info($"更新翻译: {trans.TransKey} = {trans.TransValue}");
      }
      else
      {
        // 初始化审计字段
        trans.InitAuditFields();
        await _db.Insertable(trans).ExecuteCommandAsync();
        _logger.Info($"新增翻译: {trans.TransKey} = {trans.TransValue}");
      }
    }

    _logger.Info("字典翻译数据初始化完成");
  }

  private void AddDictTranslation(List<LeanTranslation> translations, long zhLangId, long enLangId, string transKey, string zhValue, string enValue)
  {
    // 添加中文翻译
    translations.Add(new LeanTranslation
    {
      LangId = zhLangId,
      TransKey = transKey,
      TransValue = zhValue,
      ModuleName = "dict",
      OrderNum = 0,
      TransStatus = 2,
      IsBuiltin = 1
    });

    // 添加英文翻译
    translations.Add(new LeanTranslation
    {
      LangId = enLangId,
      TransKey = transKey,
      TransValue = enValue,
      ModuleName = "dict",
      OrderNum = 0,
      TransStatus = 2,
      IsBuiltin = 1
    });
  }
}