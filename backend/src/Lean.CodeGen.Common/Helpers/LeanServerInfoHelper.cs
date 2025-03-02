using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Management;
using System.Text.RegularExpressions;

namespace Lean.CodeGen.Common.Helpers;

/// <summary>
/// 服务器系统信息帮助类
/// </summary>
public class LeanServerInfoHelper
{
  /// <summary>
  /// 获取服务器系统信息
  /// </summary>
  public ServerSystemInfo GetSystemInfo()
  {
    return new ServerSystemInfo
    {
      OsVersion = Environment.OSVersion.ToString(),
      OsPlatform = GetOsPlatform(),
      ProcessorCount = Environment.ProcessorCount,
      MachineName = Environment.MachineName,
      UserName = Environment.UserName,
      SystemDirectory = Environment.SystemDirectory,
      Is64BitOperatingSystem = Environment.Is64BitOperatingSystem,
      Is64BitProcess = Environment.Is64BitProcess,
      ProcessorArchitecture = RuntimeInformation.ProcessArchitecture.ToString(),
      FrameworkDescription = RuntimeInformation.FrameworkDescription,
      MemoryInfo = GetMemoryInfo(),
      DiskInfo = GetDiskInfo()
    };
  }

  /// <summary>
  /// 获取操作系统平台
  /// </summary>
  private string GetOsPlatform()
  {
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
      return "Windows";
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
      return "Linux";
    if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
      return "macOS";
    return "Unknown";
  }

  /// <summary>
  /// 获取内存信息
  /// </summary>
  private MemoryInfo GetMemoryInfo()
  {
    var memoryInfo = new MemoryInfo();

    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    {
      try
      {
        using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
        foreach (ManagementObject obj in searcher.Get())
        {
          memoryInfo.TotalPhysicalMemory = Convert.ToInt64(obj["TotalVisibleMemorySize"]) * 1024; // KB to Bytes
          memoryInfo.FreePhysicalMemory = Convert.ToInt64(obj["FreePhysicalMemory"]) * 1024; // KB to Bytes
          memoryInfo.TotalVirtualMemory = Convert.ToInt64(obj["TotalVirtualMemorySize"]) * 1024; // KB to Bytes
          memoryInfo.FreeVirtualMemory = Convert.ToInt64(obj["FreeVirtualMemory"]) * 1024; // KB to Bytes
        }
      }
      catch
      {
        // 如果无法获取WMI信息，使用Process类获取基本内存信息
        using var process = Process.GetCurrentProcess();
        memoryInfo.WorkingSet = process.WorkingSet64;
        memoryInfo.PeakWorkingSet = process.PeakWorkingSet64;
        memoryInfo.PrivateMemorySize = process.PrivateMemorySize64;
      }
    }
    else
    {
      // 在Linux/macOS上尝试读取/proc/meminfo
      try
      {
        if (File.Exists("/proc/meminfo"))
        {
          var lines = File.ReadAllLines("/proc/meminfo");
          foreach (var line in lines)
          {
            var match = Regex.Match(line, @"(\w+):\s+(\d+)");
            if (match.Success)
            {
              var key = match.Groups[1].Value;
              var value = long.Parse(match.Groups[2].Value) * 1024; // KB to Bytes
              switch (key)
              {
                case "MemTotal":
                  memoryInfo.TotalPhysicalMemory = value;
                  break;
                case "MemFree":
                  memoryInfo.FreePhysicalMemory = value;
                  break;
                case "SwapTotal":
                  memoryInfo.TotalVirtualMemory = value;
                  break;
                case "SwapFree":
                  memoryInfo.FreeVirtualMemory = value;
                  break;
              }
            }
          }
        }
      }
      catch
      {
        // 如果无法读取/proc/meminfo，使用Process类获取基本内存信息
        using var process = Process.GetCurrentProcess();
        memoryInfo.WorkingSet = process.WorkingSet64;
        memoryInfo.PeakWorkingSet = process.PeakWorkingSet64;
        memoryInfo.PrivateMemorySize = process.PrivateMemorySize64;
      }
    }

    return memoryInfo;
  }

  /// <summary>
  /// 获取磁盘信息
  /// </summary>
  private List<DiskInfo> GetDiskInfo()
  {
    var diskInfoList = new List<DiskInfo>();

    try
    {
      var drives = DriveInfo.GetDrives();
      foreach (var drive in drives)
      {
        if (drive.IsReady)
        {
          diskInfoList.Add(new DiskInfo
          {
            Name = drive.Name,
            DriveType = drive.DriveType.ToString(),
            DriveFormat = drive.DriveFormat,
            TotalSize = drive.TotalSize,
            AvailableFreeSpace = drive.AvailableFreeSpace,
            VolumeLabel = drive.VolumeLabel
          });
        }
      }
    }
    catch
    {
      // 如果无法获取磁盘信息，返回空列表
    }

    return diskInfoList;
  }
}

/// <summary>
/// 服务器系统信息
/// </summary>
public class ServerSystemInfo
{
  /// <summary>
  /// 操作系统版本
  /// </summary>
  public string OsVersion { get; set; } = "";

  /// <summary>
  /// 操作系统平台
  /// </summary>
  public string OsPlatform { get; set; } = "";

  /// <summary>
  /// 处理器数量
  /// </summary>
  public int ProcessorCount { get; set; }

  /// <summary>
  /// 机器名称
  /// </summary>
  public string MachineName { get; set; } = "";

  /// <summary>
  /// 用户名
  /// </summary>
  public string UserName { get; set; } = "";

  /// <summary>
  /// 系统目录
  /// </summary>
  public string SystemDirectory { get; set; } = "";

  /// <summary>
  /// 是否64位操作系统
  /// </summary>
  public bool Is64BitOperatingSystem { get; set; }

  /// <summary>
  /// 是否64位进程
  /// </summary>
  public bool Is64BitProcess { get; set; }

  /// <summary>
  /// 处理器架构
  /// </summary>
  public string ProcessorArchitecture { get; set; } = "";

  /// <summary>
  /// 框架描述
  /// </summary>
  public string FrameworkDescription { get; set; } = "";

  /// <summary>
  /// 内存信息
  /// </summary>
  public MemoryInfo MemoryInfo { get; set; } = new();

  /// <summary>
  /// 磁盘信息
  /// </summary>
  public List<DiskInfo> DiskInfo { get; set; } = new();

  public override string ToString()
  {
    return JsonConvert.SerializeObject(this, Formatting.Indented);
  }
}

/// <summary>
/// 内存信息
/// </summary>
public class MemoryInfo
{
  /// <summary>
  /// 物理内存总量
  /// </summary>
  public long TotalPhysicalMemory { get; set; }

  /// <summary>
  /// 可用物理内存
  /// </summary>
  public long FreePhysicalMemory { get; set; }

  /// <summary>
  /// 虚拟内存总量
  /// </summary>
  public long TotalVirtualMemory { get; set; }

  /// <summary>
  /// 可用虚拟内存
  /// </summary>
  public long FreeVirtualMemory { get; set; }

  /// <summary>
  /// 工作集
  /// </summary>
  public long WorkingSet { get; set; }

  /// <summary>
  /// 峰值工作集
  /// </summary>
  public long PeakWorkingSet { get; set; }

  /// <summary>
  /// 私有内存大小
  /// </summary>
  public long PrivateMemorySize { get; set; }
}

/// <summary>
/// 磁盘信息
/// </summary>
public class DiskInfo
{
  /// <summary>
  /// 驱动器名称
  /// </summary>
  public string Name { get; set; } = "";

  /// <summary>
  /// 驱动器类型
  /// </summary>
  public string DriveType { get; set; } = "";

  /// <summary>
  /// 文件系统格式
  /// </summary>
  public string DriveFormat { get; set; } = "";

  /// <summary>
  /// 总大小
  /// </summary>
  public long TotalSize { get; set; }

  /// <summary>
  /// 可用空间
  /// </summary>
  public long AvailableFreeSpace { get; set; }

  /// <summary>
  /// 卷标
  /// </summary>
  public string VolumeLabel { get; set; } = "";
}