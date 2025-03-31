// -----------------------------------------------------------------------
// <copyright file="LeanFileService.cs" company="Lean">
// Copyright (c) Lean. All rights reserved.
// </copyright>
// <author>Lean</author>
// <created>2024-03-09</created>
// <summary>文件服务实现</summary>
// -----------------------------------------------------------------------

using Lean.CodeGen.Application.Dtos.Routine;
using Lean.CodeGen.Domain.Entities.Routine;
using Lean.CodeGen.Application.Services.Base;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Domain.Interfaces.Repositories;
using Lean.CodeGen.Domain.Validators;
using Lean.CodeGen.Application.Services.Security;
using Lean.CodeGen.Application.Services.Admin;
using Lean.CodeGen.Common.Excel;
using Lean.CodeGen.Common.Extensions;
using Mapster;
using SqlSugar;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Linq.Expressions;
using System.IO;
using System.Linq.Dynamic.Core;

namespace Lean.CodeGen.Application.Services.Routine;

/// <summary>
/// 文件服务实现
/// </summary>
public class LeanFileService : LeanBaseService, ILeanFileService
{
    private readonly ILeanRepository<LeanFile> _repository;
    private readonly LeanUniqueValidator<LeanFile> _uniqueValidator;
    private readonly ILeanLocalizationService _localizationService;

    /// <summary>
    /// 初始化文件服务实现
    /// </summary>
    /// <param name="repository">文件仓储</param>
    /// <param name="context">服务上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public LeanFileService(
        ILeanRepository<LeanFile> repository,
        LeanBaseServiceContext context,
        ILeanLocalizationService localizationService)
        : base(context)
    {
        _repository = repository;
        _uniqueValidator = new LeanUniqueValidator<LeanFile>(_repository);
        _localizationService = localizationService;
    }

    /// <summary>
    /// 创建文件
    /// </summary>
    public async Task<LeanApiResult<long>> CreateAsync(LeanFileCreateDto input)
    {
        try
        {
            // 检查文件名是否已存在
            await _uniqueValidator.ValidateAsync<string>(x => x.OriginalFileName, input.OriginalFileName);

            // 创建实体
            var entity = input.Adapt<LeanFile>();

            // 保存数据
            var id = await _repository.CreateAsync(entity);
            return LeanApiResult<long>.Ok(id);
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "创建文件失败");
            var message = await _localizationService.GetTranslationAsync("zh-CN", "file.error.create_failed");
            return LeanApiResult<long>.Error(message);
        }
    }

    /// <summary>
    /// 更新文件
    /// </summary>
    public async Task<LeanApiResult> UpdateAsync(LeanFileUpdateDto input)
    {
        try
        {
            // 检查文件是否存在
            var entity = await _repository.GetByIdAsync(input.Id);
            if (entity == null)
            {
                var message = await _localizationService.GetTranslationAsync("zh-CN", "file.error.not_found");
                return LeanApiResult.Error(message);
            }

            // 检查文件名是否已存在
            await _uniqueValidator.ValidateAsync<string>(x => x.OriginalFileName, input.OriginalFileName, input.Id);

            // 更新实体
            input.Adapt(entity);

            // 保存数据
            await _repository.UpdateAsync(entity);
            return LeanApiResult.Ok();
        }
        catch (Exception ex)
        {
            Logger.Error(ex, $"更新文件失败，ID：{input.Id}");
            var message = await _localizationService.GetTranslationAsync("zh-CN", "file.error.update_failed");
            return LeanApiResult.Error(message);
        }
    }

    /// <summary>
    /// 批量删除文件
    /// </summary>
    public async Task<LeanApiResult> BatchDeleteAsync(List<long> ids)
    {
        try
        {
            if (ids == null || !ids.Any())
            {
                var message = await _localizationService.GetTranslationAsync("zh-CN", "common.error.invalid_parameter");
                return LeanApiResult.Error(message);
            }

            // 批量删除文件
            await _repository.DeleteAsync(x => ids.Contains(x.Id));
            return LeanApiResult.Ok();
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "删除文件失败");
            var message = await _localizationService.GetTranslationAsync("zh-CN", "file.error.delete_failed");
            return LeanApiResult.Error(message);
        }
    }

    /// <summary>
    /// 获取文件详情
    /// </summary>
    public async Task<LeanFileDto> GetAsync(long id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new Exception($"文件不存在，ID：{id}");
        }
        return entity.Adapt<LeanFileDto>();
    }

    /// <summary>
    /// 获取文件分页列表
    /// </summary>
    public async Task<LeanApiResult<LeanPageResult<LeanFileDto>>> GetPagedListAsync(LeanFileQueryDto input)
    {
        try
        {
            // 构建查询条件
            Expression<Func<LeanFile, bool>> predicate = x => true;

            if (!string.IsNullOrEmpty(input.FileName))
            {
                predicate = predicate.And(x => x.FileName.Contains(input.FileName));
            }

            if (!string.IsNullOrEmpty(input.ContentType))
            {
                predicate = predicate.And(x => x.ContentType == input.ContentType);
            }

            if (input.StorageType.HasValue)
            {
                predicate = predicate.And(x => x.StorageType == input.StorageType);
            }

            if (input.FileType.HasValue)
            {
                predicate = predicate.And(x => x.FileType == input.FileType);
            }

            // 执行分页查询
            var result = await _repository.GetPageListAsync(
                predicate,
                input.PageSize,
                input.PageIndex,
                x => x.Id,
                false);

            // 转换为DTO
            var pageResult = new LeanPageResult<LeanFileDto>
            {
                Total = result.Total,
                Items = result.Items.Adapt<List<LeanFileDto>>(),
                PageIndex = input.PageIndex,
                PageSize = input.PageSize
            };

            return LeanApiResult<LeanPageResult<LeanFileDto>>.Ok(pageResult);
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "获取文件分页列表失败");
            var message = await _localizationService.GetTranslationAsync("zh-CN", "file.error.query_failed");
            return LeanApiResult<LeanPageResult<LeanFileDto>>.Error(message);
        }
    }

    /// <summary>
    /// 导出文件
    /// </summary>
    public async Task<byte[]> ExportAsync(LeanFileQueryDto input)
    {
        try
        {
            // 构建查询条件
            Expression<Func<LeanFile, bool>> predicate = x => true;

            if (!string.IsNullOrEmpty(input.FileName))
            {
                predicate = predicate.And(x => x.FileName.Contains(input.FileName));
            }

            if (!string.IsNullOrEmpty(input.ContentType))
            {
                predicate = predicate.And(x => x.ContentType == input.ContentType);
            }

            // 查询数据
            var list = await _repository.GetListAsync(predicate);
            var dtos = list.Select(t => t.Adapt<LeanFileExportDto>()).ToList();

            // 导出Excel
            return LeanExcelHelper.Export(dtos);
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "导出文件失败");
            throw;
        }
    }

    /// <summary>
    /// 导入文件
    /// </summary>
    public async Task<LeanFileImportResultDto> ImportAsync(LeanFileInfo file)
    {
        try
        {
            // 导入Excel数据
            var bytes = new byte[file.Stream.Length];
            await file.Stream.ReadAsync(bytes, 0, (int)file.Stream.Length);
            var importResult = LeanExcelHelper.Import<LeanFileImportDto>(bytes);

            var result = new LeanFileImportResultDto();
            result.SuccessCount = 0;
            result.FailCount = 0;

            foreach (var item in importResult.Data)
            {
                try
                {
                    // 检查文件名是否已存在
                    var exists = await _repository.AnyAsync(x => x.OriginalFileName == item.OriginalFileName);
                    if (exists)
                    {
                        result.AddError(item.OriginalFileName, $"文件名 {item.OriginalFileName} 已存在");
                        continue;
                    }

                    // 创建实体
                    var entity = item.Adapt<LeanFile>();
                    entity.CreateTime = DateTime.Now;
                    entity.CreateBy = Context.CurrentUserName;
                    entity.StorageType = 1; // 默认使用本地存储

                    // 保存数据
                    await _repository.CreateAsync(entity);
                    result.SuccessCount++;
                }
                catch (Exception ex)
                {
                    result.AddError(item.OriginalFileName, $"导入失败: {ex.Message}");
                }
            }

            return result;
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "导入文件失败");
            var result = new LeanFileImportResultDto();
            result.ErrorMessage = $"导入失败：{ex.Message}";
            return result;
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    public async Task<byte[]> GetTemplateAsync()
    {
        try
        {
            var template = new List<LeanFileImportDto>
      {
        new LeanFileImportDto
        {
          OriginalFileName = "示例文件.txt",
          ContentType = "text/plain"
        }
      };

            return await Task.FromResult(LeanExcelHelper.Export(template));
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "获取导入模板失败");
            throw;
        }
    }

    /// <summary>
    /// 获取文件列表
    /// </summary>
    public async Task<List<LeanFileDto>> GetListAsync(LeanFileQueryDto input)
    {
        Expression<Func<LeanFile, bool>> predicate = x => true;

        if (!string.IsNullOrEmpty(input.FileName))
        {
            predicate = predicate.And(x => x.FileName.Contains(input.FileName));
        }

        if (!string.IsNullOrEmpty(input.ContentType))
        {
            predicate = predicate.And(x => x.ContentType == input.ContentType);
        }

        var list = await _repository.GetListAsync(predicate);
        return list.Adapt<List<LeanFileDto>>();
    }

    /// <summary>
    /// 上传文件
    /// </summary>
    public async Task<LeanApiResult<LeanFileDto>> UploadAsync(LeanFileInfo file)
    {
        try
        {
            var entity = new LeanFile
            {
                FileName = Path.GetFileName(file.FilePath),
                OriginalFileName = Path.GetFileName(file.FilePath),
                Extension = Path.GetExtension(file.FilePath),
                FileSize = file.Stream.Length,
                ContentType = file.ContentType,
                FilePath = file.FilePath,
                CreateTime = DateTime.Now,
                CreateBy = Context.CurrentUserName
            };

            await _repository.CreateAsync(entity);
            return LeanApiResult<LeanFileDto>.Ok(entity.Adapt<LeanFileDto>());
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "上传文件失败");
            return LeanApiResult<LeanFileDto>.Error("上传文件失败");
        }
    }

    /// <summary>
    /// 下载文件
    /// </summary>
    public async Task<LeanFileResult> DownloadAsync(long id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new Exception($"文件不存在，ID：{id}");
        }

        var fileBytes = await File.ReadAllBytesAsync(entity.FilePath);
        return new LeanFileResult
        {
            Stream = new MemoryStream(fileBytes),
            FileName = entity.OriginalFileName,
            ContentType = entity.ContentType
        };
    }

    /// <summary>
    /// 删除单个文件
    /// </summary>
    public async Task<LeanApiResult> DeleteAsync(long id)
    {
        try
        {
            await _repository.DeleteAsync(x => x.Id == id);
            return LeanApiResult.Ok();
        }
        catch (Exception ex)
        {
            Logger.Error(ex, $"删除文件失败，ID：{id}");
            var message = await _localizationService.GetTranslationAsync("zh-CN", "file.error.delete_failed");
            return LeanApiResult.Error(message);
        }
    }
}