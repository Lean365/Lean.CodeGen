using System.ComponentModel.DataAnnotations;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Domain.Entities;

namespace Lean.CodeGen.Application.Dtos.Identity;

/// <summary>
/// API查询参数
/// </summary>
public class LeanQueryApiDto : LeanPage
{
  /// <summary>
  /// API名称
  /// </summary>
  public string? ApiName { get; set; }

  /// <summary>
  /// API路径
  /// </summary>
  public string? Path { get; set; }

  /// <summary>
  /// 请求方法
  /// </summary>
  public string? Method { get; set; }

  /// <summary>
  /// 所属模块
  /// </summary>
  public string? Module { get; set; }

  /// <summary>
  /// API状态
  /// </summary>
  public LeanApiStatus? ApiStatus { get; set; }

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
/// API创建参数
/// </summary>
public class LeanCreateApiDto
{
  /// <summary>
  /// 父级ID
  /// </summary>
  public long? ParentId { get; set; }

  /// <summary>
  /// API名称
  /// </summary>
  [Required(ErrorMessage = "API名称不能为空")]
  [StringLength(100, MinimumLength = 2, ErrorMessage = "API名称长度必须在2-100个字符之间")]
  public string ApiName { get; set; } = default!;

  /// <summary>
  /// API路径
  /// </summary>
  [Required(ErrorMessage = "API路径不能为空")]
  [StringLength(200, MinimumLength = 2, ErrorMessage = "API路径长度必须在2-200个字符之间")]
  public string Path { get; set; } = default!;

  /// <summary>
  /// 请求方法
  /// </summary>
  [Required(ErrorMessage = "请求方法不能为空")]
  [StringLength(10, MinimumLength = 2, ErrorMessage = "请求方法长度必须在2-10个字符之间")]
  public string Method { get; set; } = default!;

  /// <summary>
  /// 所属模块
  /// </summary>
  [Required(ErrorMessage = "所属模块不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "所属模块长度必须在2-50个字符之间")]
  public string Module { get; set; } = default!;

  /// <summary>
  /// API描述
  /// </summary>
  [StringLength(500, ErrorMessage = "API描述长度不能超过500个字符")]
  public string? ApiDescription { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  public int OrderNum { get; set; }
}

/// <summary>
/// API更新参数
/// </summary>
public class LeanUpdateApiDto : LeanCreateApiDto
{
  /// <summary>
  /// API ID
  /// </summary>
  [Required(ErrorMessage = "API ID不能为空")]
  public long Id { get; set; }
}

/// <summary>
/// API状态变更参数
/// </summary>
public class LeanChangeApiStatusDto
{
  /// <summary>
  /// API ID
  /// </summary>
  [Required(ErrorMessage = "API ID不能为空")]
  public long Id { get; set; }

  /// <summary>
  /// API状态
  /// </summary>
  [Required(ErrorMessage = "API状态不能为空")]
  public LeanApiStatus ApiStatus { get; set; }
}

/// <summary>
/// API DTO
/// </summary>
public class LeanApiDto : LeanBaseEntity
{
  /// <summary>
  /// 父级ID
  /// </summary>
  public long? ParentId { get; set; }

  /// <summary>
  /// API名称
  /// </summary>
  public string ApiName { get; set; } = default!;

  /// <summary>
  /// API路径
  /// </summary>
  public string Path { get; set; } = default!;

  /// <summary>
  /// 请求方法
  /// </summary>
  public string Method { get; set; } = default!;

  /// <summary>
  /// 所属模块
  /// </summary>
  public string Module { get; set; } = default!;

  /// <summary>
  /// API描述
  /// </summary>
  public string? ApiDescription { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  public int OrderNum { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  public LeanApiStatus ApiStatus { get; set; }

  /// <summary>
  /// 是否内置
  /// </summary>
  public LeanBuiltinStatus IsBuiltin { get; set; }
}

/// <summary>
/// API树形结构 DTO
/// </summary>
public class LeanApiTreeDto : LeanApiDto
{
  /// <summary>
  /// 子API列表
  /// </summary>
  public List<LeanApiTreeDto> Children { get; set; } = new();

  /// <summary>
  /// 是否已分配
  /// </summary>
  public bool IsAssigned { get; set; }
}

/// <summary>
/// API访问日志查询参数
/// </summary>
public class LeanQueryApiAccessLogDto : LeanPage
{
  /// <summary>
  /// API路径
  /// </summary>
  public string? Path { get; set; }

  /// <summary>
  /// 请求方法
  /// </summary>
  public string? Method { get; set; }

  /// <summary>
  /// 用户ID
  /// </summary>
  public long? UserId { get; set; }

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
/// API访问日志 DTO
/// </summary>
public class LeanApiAccessLogDto
{
  /// <summary>
  /// 用户ID
  /// </summary>
  public long UserId { get; set; }

  /// <summary>
  /// API路径
  /// </summary>
  public string Path { get; set; } = default!;

  /// <summary>
  /// 请求方法
  /// </summary>
  public string Method { get; set; } = default!;

  /// <summary>
  /// 访问时间
  /// </summary>
  public DateTime AccessTime { get; set; }

  /// <summary>
  /// 响应状态码
  /// </summary>
  public int StatusCode { get; set; }

  /// <summary>
  /// 执行时间(毫秒)
  /// </summary>
  public long ExecutionTime { get; set; }
}

/// <summary>
/// API访问频率限制 DTO
/// </summary>
public class LeanApiRateLimitDto
{
  /// <summary>
  /// 时间窗口(秒)
  /// </summary>
  public int TimeWindow { get; set; }

  /// <summary>
  /// 最大请求次数
  /// </summary>
  public int MaxRequests { get; set; }
}

/// <summary>
/// 设置API访问频率限制参数
/// </summary>
public class LeanSetApiRateLimitDto
{
  /// <summary>
  /// API ID
  /// </summary>
  [Required(ErrorMessage = "API ID不能为空")]
  public long ApiId { get; set; }

  /// <summary>
  /// 时间窗口(秒)
  /// </summary>
  [Required(ErrorMessage = "时间窗口不能为空")]
  public int TimeWindow { get; set; }

  /// <summary>
  /// 最大请求次数
  /// </summary>
  [Required(ErrorMessage = "最大请求次数不能为空")]
  public int MaxRequests { get; set; }
}