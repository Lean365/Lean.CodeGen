using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using SqlSugar;
using System.Collections.Concurrent;
using Lean.CodeGen.Domain.Entities.Admin;

namespace Lean.CodeGen.Infrastructure.Configuration;

/// <summary>
/// 系统配置提供程序
/// </summary>
public class LeanConfigProvider : IConfigurationProvider
{
  private readonly ISqlSugarClient _db;
  private readonly IConfiguration _configuration;
  private readonly ConcurrentDictionary<string, string> _data;
  private IChangeToken _reloadToken;

  public LeanConfigProvider(ISqlSugarClient db, IConfiguration configuration)
  {
    _db = db;
    _configuration = configuration;
    _data = new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    _reloadToken = new CancellationChangeToken(new CancellationTokenSource().Token);
  }

  public IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string parentPath)
  {
    var prefix = parentPath == null ? string.Empty : parentPath + ":";
    var keys = _data.Keys
        .Where(k => k.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
        .Select(k => k.Substring(prefix.Length))
        .Where(k => !k.Contains(":"))
        .Concat(earlierKeys)
        .OrderBy(k => k, StringComparer.OrdinalIgnoreCase);

    return keys;
  }

  public bool TryGet(string key, out string value)
  {
    return _data.TryGetValue(key, out value);
  }

  public IChangeToken GetReloadToken()
  {
    return _reloadToken;
  }

  public void Set(string key, string value)
  {
    _data[key] = value;
    var tokenSource = new CancellationTokenSource();
    var oldToken = Interlocked.Exchange(ref _reloadToken, new CancellationChangeToken(tokenSource.Token));
    tokenSource.Cancel();
  }

  public void Load()
  {
    // 1. 从数据库加载配置
    var dbConfigs = _db.Queryable<LeanConfig>()
        .Where(c => c.Status == Common.Enums.LeanStatus.Normal)
        .ToList();

    foreach (var config in dbConfigs)
    {
      _data[config.ConfigKey] = config.ConfigValue;
    }

    // 2. 对于数据库中不存在的配置，从 appsettings.json 加载
    foreach (var config in GetAllSettings(_configuration.GetChildren()))
    {
      if (!_data.ContainsKey(config.Key))
      {
        _data[config.Key] = config.Value;
      }
    }
  }

  private IEnumerable<KeyValuePair<string, string>> GetAllSettings(IEnumerable<IConfigurationSection> sections)
  {
    foreach (var section in sections)
    {
      if (!section.GetChildren().Any())
      {
        yield return new KeyValuePair<string, string>(section.Path, section.Value);
      }
      else
      {
        foreach (var child in GetAllSettings(section.GetChildren()))
        {
          yield return child;
        }
      }
    }
  }
}