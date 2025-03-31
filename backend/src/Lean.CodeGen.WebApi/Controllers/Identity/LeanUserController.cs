using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Application.Services.Identity;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Attributes;
using Microsoft.Extensions.Configuration;
using Lean.CodeGen.Application.Services.Admin;

namespace Lean.CodeGen.WebApi.Controllers.Identity;

/// <summary>
/// 用户管理控制器
/// </summary>
/// <remarks>
/// 提供用户管理相关的API接口，包括：
/// 1. 用户的增删改查
/// 2. 用户状态管理
/// 3. 密码管理
/// 4. 用户导入导出
/// </remarks>
[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "identity")]
[LeanPermission("identity:user", "用户管理")]
public class LeanUserController : LeanBaseController
{
  private readonly ILeanUserService _userService;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="userService">用户服务</param>
  /// <param name="localizationService">本地化服务</param>
  /// <param name="configuration">配置</param>
  /// <param name="userContext">用户上下文</param>
  public LeanUserController(
      ILeanUserService userService,
      ILeanLocalizationService localizationService,
      IConfiguration configuration,
      ILeanUserContext userContext)
      : base(localizationService, configuration, userContext)
  {
    _userService = userService;
  }

  /// <summary>
  /// 分页查询用户
  /// </summary>
  /// <param name="input">查询参数</param>
  /// <returns>分页查询结果</returns>
  [LeanPermission("identity:user:list", "查询用户")]
  [HttpGet("list")]
  public async Task<IActionResult> GetPageAsync([FromQuery] LeanUserQueryDto input)
  {
    var result = await _userService.GetPageAsync(input);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 获取用户信息
  /// </summary>
  /// <param name="id">用户ID</param>
  /// <returns>用户详细信息</returns>
  [LeanPermission("identity:user:query", "查询用户")]
  [HttpGet("{id}")]
  public async Task<IActionResult> GetAsync(long id)
  {
    var result = await _userService.GetAsync(id);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 创建用户
  /// </summary>
  /// <param name="input">用户创建参数</param>
  /// <returns>创建成功的用户信息</returns>
  [LeanPermission("identity:user:create", "新增用户")]
  [HttpPost]
  public async Task<IActionResult> CreateAsync([FromBody] LeanUserCreateDto input)
  {
    var result = await _userService.CreateAsync(input);
    return Success(result, LeanBusinessType.Create);
  }

  /// <summary>
  /// 更新用户
  /// </summary>
  /// <param name="input">用户更新参数</param>
  /// <returns>更新后的用户信息</returns>
  [LeanPermission("identity:user:update", "修改用户")]
  [HttpPut]
  public async Task<IActionResult> UpdateAsync([FromBody] LeanUserUpdateDto input)
  {
    var result = await _userService.UpdateAsync(input);
    return Success(result, LeanBusinessType.Update);
  }

  /// <summary>
  /// 删除用户
  /// </summary>
  /// <param name="input">用户删除参数</param>
  [LeanPermission("identity:user:delete", "删除用户")]
  [HttpDelete]
  public async Task<IActionResult> DeleteAsync([FromBody] LeanUserDeleteDto input)
  {
    await _userService.DeleteAsync(input);
    return Success(LeanBusinessType.Delete);
  }

  /// <summary>
  /// 导出用户
  /// </summary>
  /// <param name="input">导出参数</param>
  /// <returns>导出的文件</returns>
  [LeanPermission("identity:user:export", "导出用户")]
  [HttpGet("export")]
  public async Task<IActionResult> ExportAsync([FromQuery] LeanUserExportDto input)
  {
    var fileBytes = await _userService.ExportAsync(input);
    return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "users.xlsx");
  }

  /// <summary>
  /// 导入用户
  /// </summary>
  /// <param name="file">Excel文件</param>
  /// <returns>导入结果</returns>
  [LeanPermission("identity:user:import", "导入用户")]
  [HttpPost("import")]
  public async Task<IActionResult> ImportAsync([FromForm] LeanFileInfo file)
  {
    var result = await _userService.ImportAsync(file);
    return Success(result, LeanBusinessType.Import);
  }

  /// <summary>
  /// 获取导入模板
  /// </summary>
  /// <returns>模板文件</returns>
  [LeanPermission("identity:user:import", "导入用户")]
  [HttpGet("import-template")]
  public async Task<IActionResult> GetTemplateAsync()
  {
    var fileBytes = await _userService.GetTemplateAsync();
    return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "user-import-template.xlsx");
  }

  /// <summary>
  /// 修改用户密码
  /// </summary>
  /// <param name="input">修改密码参数</param>
  [HttpPut("change-password")]
  public async Task<IActionResult> ChangePasswordAsync([FromBody] LeanUserChangePasswordDto input)
  {
    await _userService.ChangePasswordAsync(input);
    return Success(LeanBusinessType.Update);
  }

  /// <summary>
  /// 重置用户密码
  /// </summary>
  /// <param name="input">重置密码参数</param>
  [LeanPermission("identity:user:update", "修改用户")]
  [HttpPut("reset-password")]
  public async Task<IActionResult> ResetPasswordAsync([FromBody] LeanUserResetPasswordDto input)
  {
    await _userService.ResetPasswordAsync(input);
    return Success(LeanBusinessType.Update);
  }

  /// <summary>
  /// 修改用户状态
  /// </summary>
  /// <param name="input">状态修改参数</param>
  [LeanPermission("identity:user:update", "修改用户")]
  [HttpPut("status")]
  public async Task<IActionResult> ChangeStatusAsync([FromBody] LeanUserChangeStatusDto input)
  {
    await _userService.ChangeStatusAsync(input);
    return Success(LeanBusinessType.Update);
  }
}