using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Application.Dtos.Generator;
using Lean.CodeGen.Application.Services.Generator;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Excel;
using Lean.CodeGen.Common.Enums;
using System.IO;
using Microsoft.Extensions.Configuration;
using Lean.CodeGen.Application.Services.Admin;

namespace Lean.CodeGen.WebApi.Controllers.Generator
{
    /// <summary>
    /// 代码生成历史控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "generator")]
    public class LeanGenHistoryController : LeanBaseController
    {
        private readonly ILeanGenHistoryService _genHistoryService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public LeanGenHistoryController(
            ILeanGenHistoryService genHistoryService,
            ILeanLocalizationService localizationService,
            IConfiguration configuration)
            : base(localizationService, configuration)
        {
            _genHistoryService = genHistoryService;
        }

        /// <summary>
        /// 获取代码生成历史列表（分页）
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetPageListAsync([FromQuery] LeanGenHistoryQueryDto queryDto)
        {
            var result = await _genHistoryService.GetPageListAsync(queryDto);
            return Success(result, LeanBusinessType.Query);
        }

        /// <summary>
        /// 获取代码生成历史详情
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(long id)
        {
            var result = await _genHistoryService.GetAsync(id);
            return Success(result, LeanBusinessType.Query);
        }

        /// <summary>
        /// 删除代码生成历史
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            // 由于接口中没有提供单个删除的方法，这里暂时返回操作被禁止错误
            return await ErrorAsync("generator.error.not_implemented", LeanErrorCode.OperationForbidden);
        }

        /// <summary>
        /// 导出代码生成历史
        /// </summary>
        [HttpGet("export")]
        public async Task<IActionResult> ExportAsync([FromQuery] LeanGenHistoryQueryDto queryDto)
        {
            var result = await _genHistoryService.ExportAsync(queryDto);
            var stream = new MemoryStream();
            result.Stream.CopyTo(stream);
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "gen-histories.xlsx");
        }

        /// <summary>
        /// 清空代码生成历史
        /// </summary>
        [HttpDelete("clear")]
        public async Task<IActionResult> ClearAsync()
        {
            var result = await _genHistoryService.ClearAsync();
            return result ? Success(LeanBusinessType.Delete) : await ErrorAsync("generator.error.clear_failed");
        }
    }
}