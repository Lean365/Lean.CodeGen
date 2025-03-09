using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Services.Identity;

/// <summary>
/// 用户服务接口
/// </summary>
public interface ILeanUserService
{
    /// <summary>
    /// 创建用户
    /// </summary>
    /// <param name="input">用户创建参数</param>
    /// <returns>用户信息</returns>
    Task<LeanUserDto> CreateAsync(LeanUserCreateDto input);

    /// <summary>
    /// 更新用户
    /// </summary>
    /// <param name="input">用户更新参数</param>
    /// <returns>用户信息</returns>
    Task<LeanUserDto> UpdateAsync(LeanUserUpdateDto input);

    /// <summary>
    /// 删除用户
    /// </summary>
    /// <param name="input">用户删除参数</param>
    Task DeleteAsync(LeanUserDeleteDto input);

    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <returns>用户信息</returns>
    Task<LeanUserDto> GetAsync(long id);

    /// <summary>
    /// 分页查询用户
    /// </summary>
    /// <param name="input">查询参数</param>
    /// <returns>用户列表</returns>
    Task<LeanPageResult<LeanUserDto>> QueryAsync(LeanUserQueryDto input);

    /// <summary>
    /// 修改用户状态
    /// </summary>
    /// <param name="input">状态修改参数</param>
    Task ChangeStatusAsync(LeanUserChangeStatusDto input);

    /// <summary>
    /// 重置密码
    /// </summary>
    /// <param name="input">重置密码参数</param>
    Task ResetPasswordAsync(LeanUserResetPasswordDto input);

    /// <summary>
    /// 修改密码
    /// </summary>
    /// <param name="input">修改密码参数</param>
    Task ChangePasswordAsync(LeanUserChangePasswordDto input);

    /// <summary>
    /// 导出用户
    /// </summary>
    /// <param name="input">导出参数</param>
    /// <returns>导出文件字节数组</returns>
    Task<byte[]> ExportAsync(LeanUserExportDto input);

    /// <summary>
    /// 导入用户数据
    /// </summary>
    /// <param name="file">导入文件</param>
    /// <returns>导入结果</returns>
    Task<LeanUserImportResultDto> ImportAsync(LeanFileInfo file);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <returns>模板文件字节数组</returns>
    Task<byte[]> GetImportTemplateAsync();
}