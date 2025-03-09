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
    /// 代码生成模板控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "generator")]
    public class LeanGenTemplateController : LeanBaseController
    {
        private readonly ILeanGenTemplateService _genTemplateService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public LeanGenTemplateController(
            ILeanGenTemplateService genTemplateService,
            ILeanLocalizationService localizationService,
            IConfiguration configuration)
            : base(localizationService, configuration)
        {
            _genTemplateService = genTemplateService;
        }

        /// <summary>
        /// 获取代码生成模板列表（分页）
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetPageListAsync([FromQuery] LeanGenTemplateQueryDto queryDto)
        {
            var result = await _genTemplateService.GetPageListAsync(queryDto);
            return Success(result, LeanBusinessType.Query);
        }

        /// <summary>
        /// 获取代码生成模板详情
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(long id)
        {
            var result = await _genTemplateService.GetAsync(id);
            return Success(result, LeanBusinessType.Query);
        }

        /// <summary>
        /// 创建代码生成模板
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] LeanCreateGenTemplateDto createDto)
        {
            var result = await _genTemplateService.CreateAsync(createDto);
            return Success(result, LeanBusinessType.Create);
        }

        /// <summary>
        /// 更新代码生成模板
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(long id, [FromBody] LeanUpdateGenTemplateDto updateDto)
        {
            var result = await _genTemplateService.UpdateAsync(id, updateDto);
            return Success(result, LeanBusinessType.Update);
        }

        /// <summary>
        /// 删除代码生成模板
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            var result = await _genTemplateService.DeleteAsync(id);
            return result ? Success(LeanBusinessType.Delete) : await ErrorAsync("generator.error.delete_failed");
        }

        /// <summary>
        /// 导出代码生成模板
        /// </summary>
        [HttpGet("export")]
        public async Task<IActionResult> ExportAsync([FromQuery] LeanGenTemplateQueryDto queryDto)
        {
            var result = await _genTemplateService.ExportAsync(queryDto);
            var stream = new MemoryStream();
            result.Stream.CopyTo(stream);
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "gen-templates.xlsx");
        }

        /// <summary>
        /// 导入代码生成模板
        /// </summary>
        [HttpPost("import")]
        public async Task<IActionResult> ImportAsync([FromForm] LeanFileInfo file)
        {
            var result = await _genTemplateService.ImportAsync(file);
            return Success(result, LeanBusinessType.Import);
        }

        /// <summary>
        /// 下载导入模板
        /// </summary>
        [HttpGet("template")]
        public async Task<IActionResult> DownloadTemplateAsync()
        {
            var result = await _genTemplateService.DownloadTemplateAsync();
            var stream = new MemoryStream();
            result.Stream.CopyTo(stream);
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "gen-template-template.xlsx");
        }

        /// <summary>
        /// 复制模板
        /// </summary>
        [HttpPost("{id}/copy")]
        public async Task<IActionResult> CopyAsync(long id)
        {
            var result = await _genTemplateService.CopyAsync(id);
            return Success(result, LeanBusinessType.Create);
        }
    }
}