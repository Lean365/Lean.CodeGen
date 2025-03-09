using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NLog;

namespace Lean.CodeGen.Common.Helpers;

/// <summary>
/// Quartz定时任务帮助类
/// </summary>
public class LeanQuartzHelper
{
  private readonly ILogger _logger;
  private readonly IScheduler _scheduler;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanQuartzHelper(ILogger logger)
  {
    _logger = logger;
    _scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
    _scheduler.Start().Wait();
  }

  /// <summary>
  /// 添加定时任务
  /// </summary>
  /// <typeparam name="T">任务类型</typeparam>
  /// <param name="taskName">任务名称</param>
  /// <param name="cronExpression">Cron表达式</param>
  /// <param name="taskData">任务数据</param>
  public async Task AddTaskAsync<T>(string taskName, string cronExpression, Dictionary<string, object>? taskData = null) where T : IJob
  {
    try
    {
      var jobKey = new JobKey(taskName);
      var triggerKey = new TriggerKey($"{taskName}_trigger");

      // 创建任务
      var job = JobBuilder.Create<T>()
          .WithIdentity(jobKey)
          .Build();

      // 添加任务数据
      if (taskData != null)
      {
        foreach (var item in taskData)
        {
          job.JobDataMap.Put(item.Key, item.Value);
        }
      }

      // 创建触发器
      var trigger = TriggerBuilder.Create()
          .WithIdentity(triggerKey)
          .WithCronSchedule(cronExpression)
          .Build();

      // 添加任务和触发器
      await _scheduler.ScheduleJob(job, trigger);
      _logger.Info($"添加定时任务成功: {taskName}, Cron: {cronExpression}");
    }
    catch (Exception ex)
    {
      _logger.Error(ex, $"添加定时任务失败: {taskName}");
      throw;
    }
  }

  /// <summary>
  /// 移除定时任务
  /// </summary>
  /// <param name="taskName">任务名称</param>
  public async Task RemoveTaskAsync(string taskName)
  {
    try
    {
      var jobKey = new JobKey(taskName);
      if (await _scheduler.CheckExists(jobKey))
      {
        await _scheduler.DeleteJob(jobKey);
        _logger.Info($"移除定时任务成功: {taskName}");
      }
    }
    catch (Exception ex)
    {
      _logger.Error(ex, $"移除定时任务失败: {taskName}");
      throw;
    }
  }

  /// <summary>
  /// 暂停定时任务
  /// </summary>
  /// <param name="taskName">任务名称</param>
  public async Task PauseTaskAsync(string taskName)
  {
    try
    {
      var jobKey = new JobKey(taskName);
      if (await _scheduler.CheckExists(jobKey))
      {
        await _scheduler.PauseJob(jobKey);
        _logger.Info($"暂停定时任务成功: {taskName}");
      }
    }
    catch (Exception ex)
    {
      _logger.Error(ex, $"暂停定时任务失败: {taskName}");
      throw;
    }
  }

  /// <summary>
  /// 恢复定时任务
  /// </summary>
  /// <param name="taskName">任务名称</param>
  public async Task ResumeTaskAsync(string taskName)
  {
    try
    {
      var jobKey = new JobKey(taskName);
      if (await _scheduler.CheckExists(jobKey))
      {
        await _scheduler.ResumeJob(jobKey);
        _logger.Info($"恢复定时任务成功: {taskName}");
      }
    }
    catch (Exception ex)
    {
      _logger.Error(ex, $"恢复定时任务失败: {taskName}");
      throw;
    }
  }

  /// <summary>
  /// 修改定时任务Cron表达式
  /// </summary>
  /// <param name="taskName">任务名称</param>
  /// <param name="cronExpression">新的Cron表达式</param>
  public async Task ModifyTaskCronAsync(string taskName, string cronExpression)
  {
    try
    {
      var jobKey = new JobKey(taskName);
      var triggerKey = new TriggerKey($"{taskName}_trigger");

      if (await _scheduler.CheckExists(jobKey))
      {
        // 创建新的触发器
        var trigger = TriggerBuilder.Create()
            .WithIdentity(triggerKey)
            .WithCronSchedule(cronExpression)
            .Build();

        // 替换旧的触发器
        await _scheduler.RescheduleJob(triggerKey, trigger);
        _logger.Info($"修改定时任务Cron表达式成功: {taskName}, 新Cron: {cronExpression}");
      }
    }
    catch (Exception ex)
    {
      _logger.Error(ex, $"修改定时任务Cron表达式失败: {taskName}");
      throw;
    }
  }

  /// <summary>
  /// 立即执行一次任务
  /// </summary>
  /// <param name="taskName">任务名称</param>
  public async Task TriggerTaskAsync(string taskName)
  {
    try
    {
      var jobKey = new JobKey(taskName);
      if (await _scheduler.CheckExists(jobKey))
      {
        await _scheduler.TriggerJob(jobKey);
        _logger.Info($"触发定时任务执行成功: {taskName}");
      }
    }
    catch (Exception ex)
    {
      _logger.Error(ex, $"触发定时任务执行失败: {taskName}");
      throw;
    }
  }

  /// <summary>
  /// 获取所有任务
  /// </summary>
  public async Task<List<JobKey>> GetAllTasksAsync()
  {
    try
    {
      var jobKeys = new List<JobKey>();
      var groups = await _scheduler.GetJobGroupNames();
      foreach (var group in groups)
      {
        jobKeys.AddRange(await _scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(group)));
      }
      return jobKeys;
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "获取所有任务失败");
      throw;
    }
  }

  /// <summary>
  /// 关闭调度器
  /// </summary>
  public async Task ShutdownAsync()
  {
    try
    {
      if (!_scheduler.IsShutdown)
      {
        await _scheduler.Shutdown();
        _logger.Info("关闭调度器成功");
      }
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "关闭调度器失败");
      throw;
    }
  }
}