using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Application.Services.Identity;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Enums;
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
[Route("api/[controller]")]
public class LeanUserController : LeanBaseController
{
  private readonly ILeanUserService _userService;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="userService">用户服务</param>
  /// <param name="localizationService">本地化服务</param>
  /// <param name="configuration">配置</param>
  public LeanUserController(
      ILeanUserService userService,
      ILeanLocalizationService localizationService,
      IConfiguration configuration)
      : base(localizationService, configuration)
  {
    _userService = userService;
  }

  /// <summary>
  /// 创建用户
  /// </summary>
  /// <param name="input">用户创建参数</param>
  /// <returns>创建成功的用户信息</returns>
  [HttpPost]
  public async Task<IActionResult> CreateAsync([FromBody] LeanCreateUserDto input)
  {
    var result = await _userService.CreateAsync(input);
    return Success(result, LeanBusinessType.Create);
  }

  /// <summary>
  /// 更新用户
  /// </summary>
  /// <param name="input">用户更新参数</param>
  /// <returns>更新后的用户信息</returns>
  [HttpPut]
  public async Task<IActionResult> UpdateAsync([FromBody] LeanUpdateUserDto input)
  {
    var result = await _userService.UpdateAsync(input);
    return Success(result, LeanBusinessType.Update);
  }

  /// <summary>
  /// 删除用户
  /// </summary>
  /// <param name="input">用户删除参数</param>
  [HttpDelete]
  public async Task<IActionResult> DeleteAsync([FromBody] LeanDeleteUserDto input)
  {
    await _userService.DeleteAsync(input);
    return Success(LeanBusinessType.Delete);
  }

  /// <summary>
  /// 获取用户信息
  /// </summary>
  /// <param name="id">用户ID</param>
  /// <returns>用户详细信息</returns>
  [HttpGet("{id}")]
  public async Task<IActionResult> GetAsync(long id)
  {
    var result = await _userService.GetAsync(id);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 分页查询用户
  /// </summary>
  /// <param name="input">查询参数</param>
  /// <returns>分页查询结果</returns>
  [HttpGet]
  public async Task<IActionResult> QueryAsync([FromQuery] LeanQueryUserDto input)
  {
    var result = await _userService.QueryAsync(input);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 修改用户状态
  /// </summary>
  /// <param name="input">状态修改参数</param>
  [HttpPut("status")]
  public async Task<IActionResult> ChangeStatusAsync([FromBody] LeanChangeUserStatusDto input)
  {
    await _userService.ChangeStatusAsync(input);
    return Success(LeanBusinessType.Update);
  }

  /// <summary>
  /// 重置用户密码
  /// </summary>
  /// <param name="input">重置密码参数</param>
  [HttpPut("reset-password")]
  public async Task<IActionResult> ResetPasswordAsync([FromBody] LeanResetUserPasswordDto input)
  {
    await _userService.ResetPasswordAsync(input);
    return Success(LeanBusinessType.Update);
  }

  /// <summary>
  /// 修改用户密码
  /// </summary>
  /// <param name="input">修改密码参数</param>
  [HttpPut("change-password")]
  public async Task<IActionResult> ChangePasswordAsync([FromBody] LeanChangeUserPasswordDto input)
  {
    await _userService.ChangePasswordAsync(input);
    return Success(LeanBusinessType.Update);
  }

  /// <summary>
  /// 导出用户
  /// </summary>
  /// <param name="input">导出参数</param>
  /// <returns>导出的文件</returns>
  [HttpGet("export")]
  public async Task<IActionResult> ExportAsync([FromQuery] LeanExportUserDto input)
  {
    var fileBytes = await _userService.ExportAsync(input);
    return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "users.xlsx");
  }

  /// <summary>
  /// 导入用户
  /// </summary>
  /// <returns>导入结果</returns>
  [HttpPost("import")]
  public async Task<IActionResult> ImportAsync([FromForm] IFormFile file)
  {
    using var ms = new MemoryStream();
    await file.CopyToAsync(ms);
    ms.Position = 0;
    var fileInfo = new LeanFileInfo
    {
      Stream = ms,
      FileName = file.FileName,
      ContentType = file.ContentType
    };
    var result = await _userService.ImportAsync(fileInfo);
    return Success(result, LeanBusinessType.Import);
  }

  /// <summary>
  /// 获取导入模板
  /// </summary>
  /// <returns>模板文件</returns>
  [HttpGet("import-template")]
  public async Task<IActionResult> GetImportTemplateAsync()
  {
    var fileBytes = await _userService.GetImportTemplateAsync();
    return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "user-import-template.xlsx");
  }
}