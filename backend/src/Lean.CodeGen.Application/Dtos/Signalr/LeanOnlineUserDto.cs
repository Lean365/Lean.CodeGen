using System.ComponentModel.DataAnnotations;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Dtos.Signalr;

/// <summary>
/// 在线用户查询参数
/// </summary>
public class LeanOnlineUserQueryDto : LeanPage
{
  /// <summary>
  /// 用户ID
  /// </summary>
  public long? UserId { get; set; }

  /// <summary>
  /// 用户名
  /// </summary>
  public string? UserName { get; set; }

  /// <summary>
  /// 是否在线
  /// 0-离线
  /// 1-在线
  /// </summary>
  public int? IsOnline { get; set; }

  /// <summary>
  /// 开始时间
  /// </summary>
  public DateTime? StartTime { get; set; }

  /// <summary>
  /// 结束时间
  /// </summary>
  public DateTime? EndTime { get; set; }
}

/// <summary>
/// 在线用户DTO
/// </summary>
public class LeanOnlineUserDto
{
  /// <summary>
  /// 用户ID
  /// </summary>
  [Required(ErrorMessage = "用户ID不能为空")]
  public long UserId { get; set; }

  /// <summary>
  /// 用户名
  /// </summary>
  public string UserName { get; set; } = null!;

  /// <summary>
  /// 头像
  /// </summary>
  public string? Avatar { get; set; }

  /// <summary>
  /// 连接ID
  /// </summary>
  public string ConnectionId { get; set; } = null!;

  /// <summary>
  /// 设备ID
  /// </summary>
  public string DeviceId { get; set; } = null!;

  /// <summary>
  /// 设备名称
  /// </summary>
  public string? DeviceName { get; set; }

  /// <summary>
  /// 设备类型
  /// 0-PC
  /// 1-Mobile
  /// 2-Tablet
  /// 3-Other
  /// </summary>
  public int? DeviceType { get; set; }

  /// <summary>
  /// 是否在线
  /// 0-离线
  /// 1-在线
  /// </summary>
  public int IsOnline { get; set; }

  /// <summary>
  /// 最后活动时间
  /// </summary>
  public DateTime LastActiveTime { get; set; }

  /// <summary>
  /// 创建时间
  /// </summary>
  public DateTime CreateTime { get; set; }

  /// <summary>
  /// 更新时间
  /// </summary>
  public DateTime? UpdateTime { get; set; }
}

/// <summary>
/// 在线用户更新参数
/// </summary>
public class LeanOnlineUserUpdateDto
{
  /// <summary>
  /// 用户ID
  /// </summary>
  [Required(ErrorMessage = "用户ID不能为空")]
  public long UserId { get; set; }

  /// <summary>
  /// 用户名
  /// </summary>
  [Required(ErrorMessage = "用户名不能为空")]
  public string UserName { get; set; } = null!;

  /// <summary>
  /// 头像
  /// </summary>
  public string? Avatar { get; set; }
}