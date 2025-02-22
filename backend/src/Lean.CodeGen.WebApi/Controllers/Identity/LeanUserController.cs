using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Application.Services.Identity;
using Lean.CodeGen.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lean.CodeGen.WebApi.Controllers.Identity;

/// <summary>
/// 用户管理
/// </summary>
[ApiController]
[Route("api/identity/[controller]")]
public class LeanUserController : LeanBaseController
{
  private readonly ILeanUserService _userService;

  public LeanUserController(ILeanUserService userService)
  {
    _userService = userService;
  }

  /// <summary>
  /// 创建用户
  /// </summary>
  /// <param name="input">用户创建参数</param>
  /// <returns>用户ID</returns>
  [HttpPost]
  public async Task<IActionResult> CreateAsync([FromBody] LeanCreateUserDto input)
  {
    var result = await _userService.CreateAsync(input);
    return ApiResult(result);
  }

  /// <summary>
  /// 更新用户
  /// </summary>
  /// <param name="input">用户更新参数</param>
  [HttpPut]
  public async Task<IActionResult> UpdateAsync([FromBody] LeanUpdateUserDto input)
  {
    var result = await _userService.UpdateAsync(input);
    return ApiResult(result);
  }

  /// <summary>
  /// 删除用户
  /// </summary>
  /// <param name="ids">用户ID列表</param>
  /// <returns>删除结果</returns>
  [HttpDelete]
  public async Task<IActionResult> DeleteAsync([FromBody] List<long> ids)
  {
    var result = await _userService.BatchDeleteAsync(ids);
    return ApiResult(result);
  }

  /// <summary>
  /// 获取用户详情
  /// </summary>
  /// <param name="id">用户ID</param>
  /// <returns>用户详情</returns>
  [HttpGet("{id}")]
  public async Task<IActionResult> GetAsync(long id)
  {
    var result = await _userService.GetAsync(id);
    return ApiResult(result);
  }

  /// <summary>
  /// 分页查询用户
  /// </summary>
  /// <param name="input">查询参数</param>
  /// <returns>分页结果</returns>
  [HttpGet]
  public async Task<IActionResult> GetPageAsync([FromQuery] LeanQueryUserDto input)
  {
    var result = await _userService.GetPageAsync(input);
    return ApiResult(result);
  }

  /// <summary>
  /// 修改用户状态
  /// </summary>
  /// <param name="input">状态修改参数</param>
  [HttpPut("status")]
  public async Task<IActionResult> SetStatusAsync([FromBody] LeanChangeUserStatusDto input)
  {
    var result = await _userService.SetStatusAsync(input);
    return ApiResult(result);
  }

  /// <summary>
  /// 重置密码
  /// </summary>
  /// <param name="input">重置密码参数</param>
  [HttpPut("reset-password")]
  public async Task<IActionResult> ResetPasswordAsync([FromBody] LeanResetUserPasswordDto input)
  {
    var result = await _userService.ResetPasswordAsync(input);
    return ApiResult(result);
  }

  /// <summary>
  /// 修改密码
  /// </summary>
  /// <param name="input">修改密码参数</param>
  [HttpPut("change-password")]
  public async Task<IActionResult> ChangePasswordAsync([FromBody] LeanChangeUserPasswordDto input)
  {
    var result = await _userService.ChangePasswordAsync(input);
    return ApiResult(result);
  }

  /// <summary>
  /// 导入用户
  /// </summary>
  /// <param name="users">用户列表</param>
  /// <returns>导入结果</returns>
  [HttpPost("import")]
  public async Task<IActionResult> ImportAsync([FromBody] List<LeanImportTemplateUserDto> users)
  {
    var result = await _userService.ImportAsync(users);
    return ApiResult(result);
  }

  /// <summary>
  /// 导出用户
  /// </summary>
  /// <param name="input">导出参数</param>
  /// <returns>文件流</returns>
  [HttpGet("export")]
  public async Task<IActionResult> ExportAsync([FromQuery] LeanExportUserDto input)
  {
    var result = await _userService.ExportAsync(input);
    return FileResult(result, $"users.{input.FileFormat}");
  }

  /// <summary>
  /// 批量导出用户
  /// </summary>
  /// <param name="input">批量导出参数</param>
  /// <returns>文件流</returns>
  [HttpPost("batch-export")]
  public async Task<IActionResult> BatchExportAsync([FromBody] LeanBatchExportUserDto input)
  {
    var exportInput = new LeanExportUserDto
    {
      SelectedIds = input.UserIds,
      ExportFields = input.ExportFields,
      FileFormat = input.FileFormat,
      IsExportAll = false
    };

    var result = await _userService.ExportAsync(exportInput);
    if (!result.Success || result.Data == null)
    {
      return BadRequest(result.Message);
    }
    return File(result.Data, "application/octet-stream", $"users.{input.FileFormat}", true);
  }

  /// <summary>
  /// 处理API结果
  /// </summary>
  private new IActionResult ApiResult<T>(LeanApiResult<T> result)
  {
    if (!result.Success)
    {
      return BadRequest(result.Message);
    }
    return Ok(result.Data);
  }

  /// <summary>
  /// 处理API结果（无数据）
  /// </summary>
  private new IActionResult ApiResult(LeanApiResult result)
  {
    if (!result.Success)
    {
      return BadRequest(result.Message);
    }
    return Ok();
  }
}