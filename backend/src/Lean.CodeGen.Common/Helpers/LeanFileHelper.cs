using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Lean.CodeGen.Common.Localization;
using Lean.CodeGen.Common.Http;
namespace Lean.CodeGen.Common.Helpers;

/// <summary>
/// 文件操作帮助类
/// </summary>
public class LeanFileHelper
{
  private readonly ILeanLocalizationService _localizationService;
  private readonly ILeanHttpContextAccessor _httpContextAccessor;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanFileHelper(
      ILeanLocalizationService localizationService,
      ILeanHttpContextAccessor httpContextAccessor)
  {
    _localizationService = localizationService;
    _httpContextAccessor = httpContextAccessor;
  }

  /// <summary>
  /// 获取 wwwroot 路径
  /// </summary>
  public string GetWebRootPath()
  {
    return _httpContextAccessor.GetWebRootPath();
  }

  /// <summary>
  /// 删除文件
  /// </summary>
  /// <param name="filePath">文件路径</param>
  /// <returns>(是否成功, 消息)</returns>
  public async Task<(bool Success, string Message)> DeleteFileAsync(string filePath)
  {
    try
    {
      if (!File.Exists(filePath))
      {
        return (false, await _localizationService.GetLocalizedTextAsync("FileNotFound"));
      }

      await Task.Run(() => File.Delete(filePath));
      return (true, await _localizationService.GetLocalizedTextAsync("FileDeletedSuccessfully"));
    }
    catch (IOException ex)
    {
      return (false, await _localizationService.GetLocalizedTextAsync("FileInUse"));
    }
    catch (UnauthorizedAccessException ex)
    {
      return (false, await _localizationService.GetLocalizedTextAsync("FileAccessDenied"));
    }
    catch (Exception ex)
    {
      return (false, await _localizationService.GetLocalizedTextAsync("FileDeleteError"));
    }
  }

  /// <summary>
  /// 检查目录是否为空
  /// </summary>
  /// <param name="directoryPath">目录路径</param>
  /// <returns>(是否为空, 文件数量, 目录数量, 消息)</returns>
  public async Task<(bool IsEmpty, int FileCount, int DirectoryCount, string Message)> CheckDirectoryAsync(string directoryPath)
  {
    try
    {
      if (!Directory.Exists(directoryPath))
      {
        return (true, 0, 0, await _localizationService.GetLocalizedTextAsync("DirectoryNotFound"));
      }

      var files = await Task.Run(() => Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories));
      var directories = await Task.Run(() => Directory.GetDirectories(directoryPath, "*", SearchOption.AllDirectories));

      var isEmpty = files.Length == 0 && directories.Length == 0;
      var message = isEmpty
          ? await _localizationService.GetLocalizedTextAsync("DirectoryEmpty")
          : await _localizationService.GetLocalizedTextAsync("DirectoryNotEmpty");

      return (isEmpty, files.Length, directories.Length, message);
    }
    catch (Exception ex)
    {
      return (true, 0, 0, await _localizationService.GetLocalizedTextAsync("DirectoryCheckError"));
    }
  }

  /// <summary>
  /// 删除目录及其内容
  /// </summary>
  /// <param name="directoryPath">目录路径</param>
  /// <param name="forceDelete">是否强制删除（不检查是否为空）</param>
  /// <returns>(是否成功, 消息)</returns>
  public async Task<(bool Success, string Message)> DeleteDirectoryAsync(string directoryPath, bool forceDelete = false)
  {
    try
    {
      if (!Directory.Exists(directoryPath))
      {
        return (false, await _localizationService.GetLocalizedTextAsync("DirectoryNotFound"));
      }

      if (!forceDelete)
      {
        var (isEmpty, fileCount, directoryCount, message) = await CheckDirectoryAsync(directoryPath);
        if (!isEmpty)
        {
          return (false, message);
        }
      }

      await Task.Run(() => Directory.Delete(directoryPath, true));
      return (true, await _localizationService.GetLocalizedTextAsync("DirectoryDeletedSuccessfully"));
    }
    catch (IOException ex)
    {
      return (false, await _localizationService.GetLocalizedTextAsync("DirectoryInUse"));
    }
    catch (UnauthorizedAccessException ex)
    {
      return (false, await _localizationService.GetLocalizedTextAsync("DirectoryAccessDenied"));
    }
    catch (Exception ex)
    {
      return (false, await _localizationService.GetLocalizedTextAsync("DirectoryDeleteError"));
    }
  }

  /// <summary>
  /// 删除指定目录下的所有文件
  /// </summary>
  /// <param name="directoryPath">目录路径</param>
  /// <param name="recursive">是否递归删除子目录</param>
  /// <returns>(是否成功, 消息)</returns>
  public async Task<(bool Success, string Message)> DeleteDirectoryFilesAsync(string directoryPath, bool recursive = false)
  {
    try
    {
      if (!Directory.Exists(directoryPath))
      {
        return (false, await _localizationService.GetLocalizedTextAsync("DirectoryNotFound"));
      }

      var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
      var files = await Task.Run(() => Directory.GetFiles(directoryPath, "*.*", searchOption));

      foreach (var file in files)
      {
        await Task.Run(() => File.Delete(file));
      }

      return (true, await _localizationService.GetLocalizedTextAsync("DirectoryFilesDeleted"));
    }
    catch (Exception ex)
    {
      return (false, await _localizationService.GetLocalizedTextAsync("DirectoryFilesDeleteError"));
    }
  }

  /// <summary>
  /// 删除指定目录下的指定类型文件
  /// </summary>
  /// <param name="directoryPath">目录路径</param>
  /// <param name="fileExtension">文件扩展名（如：.txt, .log）</param>
  /// <param name="recursive">是否递归删除子目录</param>
  /// <returns>(是否成功, 消息)</returns>
  public async Task<(bool Success, string Message)> DeleteDirectoryFilesByExtensionAsync(string directoryPath, string fileExtension, bool recursive = false)
  {
    try
    {
      if (!Directory.Exists(directoryPath))
      {
        return (false, await _localizationService.GetLocalizedTextAsync("DirectoryNotFound"));
      }

      var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
      var files = await Task.Run(() => Directory.GetFiles(directoryPath, $"*{fileExtension}", searchOption));

      foreach (var file in files)
      {
        await Task.Run(() => File.Delete(file));
      }

      return (true, await _localizationService.GetLocalizedTextAsync("DirectoryExtensionFilesDeleted"));
    }
    catch (Exception ex)
    {
      return (false, await _localizationService.GetLocalizedTextAsync("DirectoryExtensionFilesDeleteError"));
    }
  }
}