using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Application.Services.Identity;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Enums;
using Microsoft.Extensions.Configuration;
using Lean.CodeGen.Application.Services.Admin;

namespace Lean.CodeGen.WebApi.Controllers.Identity;

/// <summary>
/// 岗位控制器
/// </summary>
/// <remarks>
/// 提供岗位管理相关的API接口，包括：
/// 1. 岗位的增删改查
/// 2. 岗位状态管理
/// </remarks>
[Route("api/admin/posts")]
[ApiController]
public class LeanPostController : LeanBaseController
{
  private readonly ILeanPostService _postService;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="postService">岗位服务</param>
  /// <param name="localizationService">本地化服务</param>
  /// <param name="configuration">配置</param>
  public LeanPostController(
      ILeanPostService postService,
      ILeanLocalizationService localizationService,
      IConfiguration configuration)
      : base(localizationService, configuration)
  {
    _postService = postService;
  }

  /// <summary>
  /// 创建岗位
  /// </summary>
  /// <param name="input">岗位创建参数</param>
  /// <returns>创建成功的岗位信息</returns>
  [HttpPost]
  public async Task<IActionResult> CreateAsync([FromBody] LeanCreatePostDto input)
  {
    var result = await _postService.CreateAsync(input);
    return Success(result, LeanBusinessType.Create);
  }

  /// <summary>
  /// 更新岗位
  /// </summary>
  /// <param name="input">岗位更新参数</param>
  /// <returns>更新后的岗位信息</returns>
  [HttpPut]
  public async Task<IActionResult> UpdateAsync([FromBody] LeanUpdatePostDto input)
  {
    var result = await _postService.UpdateAsync(input);
    return Success(result, LeanBusinessType.Update);
  }

  /// <summary>
  /// 删除岗位
  /// </summary>
  /// <param name="id">岗位ID</param>
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteAsync(long id)
  {
    var result = await _postService.DeleteAsync(id);
    return Success(result, LeanBusinessType.Delete);
  }

  /// <summary>
  /// 批量删除岗位
  /// </summary>
  /// <param name="ids">岗位ID列表</param>
  /// <returns>删除结果</returns>
  [HttpDelete]
  public async Task<IActionResult> BatchDeleteAsync([FromBody] List<long> ids)
  {
    var result = await _postService.BatchDeleteAsync(ids);
    return Success(result, LeanBusinessType.Delete);
  }

  /// <summary>
  /// 获取岗位信息
  /// </summary>
  /// <param name="id">岗位ID</param>
  /// <returns>岗位详细信息</returns>
  [HttpGet("{id}")]
  public async Task<IActionResult> GetAsync(long id)
  {
    var result = await _postService.GetAsync(id);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 分页查询岗位
  /// </summary>
  /// <param name="input">查询参数</param>
  /// <returns>岗位列表</returns>
  [HttpGet]
  public async Task<IActionResult> GetPageAsync([FromQuery] LeanQueryPostDto input)
  {
    var result = await _postService.GetPageAsync(input);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 设置岗位状态
  /// </summary>
  /// <param name="input">状态修改参数</param>
  /// <returns>修改后的岗位信息</returns>
  [HttpPut("status")]
  public async Task<IActionResult> SetStatusAsync([FromBody] LeanChangePostStatusDto input)
  {
    var result = await _postService.SetStatusAsync(input);
    return Success(result, LeanBusinessType.Update);
  }

  /// <summary>
  /// 导出岗位数据
  /// </summary>
  /// <param name="input">查询参数</param>
  /// <returns>岗位数据导出结果</returns>
  [HttpGet("export")]
  public async Task<IActionResult> ExportAsync([FromQuery] LeanQueryPostDto input)
  {
    var bytes = await _postService.ExportAsync(input);
    return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "posts.xlsx");
  }

  /// <summary>
  /// 导入岗位数据
  /// </summary>
  /// <param name="file">岗位数据文件</param>
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
    var result = await _postService.ImportAsync(fileInfo);
    return Success(result, LeanBusinessType.Import);
  }

  /// <summary>
  /// 获取导入模板
  /// </summary>
  /// <returns>岗位导入模板文件</returns>
  [HttpGet("template")]
  public async Task<IActionResult> GetImportTemplateAsync()
  {
    var bytes = await _postService.GetImportTemplateAsync();
    return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "post-template.xlsx");
  }
}