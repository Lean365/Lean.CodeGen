using Microsoft.Extensions.Configuration;
using SqlSugar;

namespace Lean.CodeGen.Infrastructure.Configuration;

/// <summary>
/// 系统配置源
/// </summary>
public class LeanConfigSource : IConfigurationSource
{
  private readonly ISqlSugarClient _db;
  private readonly IConfiguration _configuration;

  public LeanConfigSource(ISqlSugarClient db, IConfiguration configuration)
  {
    _db = db;
    _configuration = configuration;
  }

  public IConfigurationProvider Build(IConfigurationBuilder builder)
  {
    return new LeanConfigProvider(_db, _configuration);
  }
}