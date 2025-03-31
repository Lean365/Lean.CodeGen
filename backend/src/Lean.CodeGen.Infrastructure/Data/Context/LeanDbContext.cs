using System.Linq.Expressions;
using Microsoft.Extensions.Options;
using Lean.CodeGen.Common.Options;
using Lean.CodeGen.Domain.Interfaces.Entities;
using SqlSugar;
using System.Reflection;
using Lean.CodeGen.Domain.Entities.Identity;

namespace Lean.CodeGen.Infrastructure.Data.Context;

/// <summary>
/// 数据库上下文
/// </summary>
/// <remarks>
/// 该类负责：
/// 1. 管理数据库连接和配置
/// 2. 提供统一的数据访问接口
/// 3. 配置实体映射和过滤器
/// 4. 处理软删除等通用逻辑
/// </remarks>
public class LeanDbContext
{
  /// <summary>
  /// SqlSugar 数据库访问对象
  /// </summary>
  private readonly SqlSugarScope _db;
  private readonly NLog.ILogger _logger = NLog.LogManager.GetCurrentClassLogger();

  /// <summary>
  /// 初始化数据库上下文
  /// </summary>
  /// <param name="options">数据库配置选项</param>
  public LeanDbContext(IOptions<LeanDatabaseOptions> options)
  {
    var config = new ConnectionConfig
    {
      DbType = options.Value.DbType,
      ConnectionString = options.Value.ConnectionString,
      IsAutoCloseConnection = true,
      ConfigureExternalServices = new ConfigureExternalServices
      {
        EntityNameService = (type, entity) =>
        {
          entity.IsDisabledUpdateAll = true;
          if (options.Value.EnableUnderLine)
          {
            entity.DbTableName = Common.Utils.LeanNameConvert.ToUnderline(entity.DbTableName);
          }
        }
      }
    };

    _db = new SqlSugarScope(config);

    if (options.Value.EnableSqlLog)
    {
      _db.Aop.OnLogExecuting = (sql, parameters) =>
      {
        var sqlUpper = sql.ToUpper().TrimStart();
        if (sqlUpper.StartsWith("CREATE TABLE"))
        {
          var match = System.Text.RegularExpressions.Regex.Match(sql, @"CREATE TABLE [\[`""]?([^\[`""\s\(]+)");
          if (match.Success)
          {
            _logger.Info($"\u001b[32m数据库操作 >>\u001b[0m \u001b[36m创建表:\u001b[0m {match.Groups[1].Value}");
          }
        }
        else if (sqlUpper.StartsWith("ALTER TABLE"))
        {
          var match = System.Text.RegularExpressions.Regex.Match(sql, @"ALTER TABLE [\[`""]?([^\[`""\s\(]+)");
          if (match.Success)
          {
            _logger.Info($"\u001b[32m数据库操作 >>\u001b[0m \u001b[33m更新表:\u001b[0m {match.Groups[1].Value}");
          }
        }
        else if (sqlUpper.StartsWith("INSERT"))
        {
          var match = System.Text.RegularExpressions.Regex.Match(sql, @"INSERT INTO [\[`""]?([^\[`""\s\(]+)");
          if (match.Success)
          {
            _logger.Info($"\u001b[32m数据库操作 >>\u001b[0m \u001b[34m插入数据:\u001b[0m {match.Groups[1].Value} \u001b[90m|\u001b[0m 记录数: {parameters?.Length ?? 0}");
          }
        }
      };
    }

    ConfigureFilters();
  }

  /// <summary>
  /// 获取数据库访问对象
  /// </summary>
  /// <returns>SqlSugar数据库访问对象</returns>
  public ISqlSugarClient GetDatabase()
  {
    return _db;
  }

  /// <summary>
  /// 获取实体仓储
  /// </summary>
  /// <typeparam name="T">实体类型</typeparam>
  /// <returns>实体仓储对象</returns>
  public SimpleClient<T> GetRepository<T>() where T : class, new()
  {
    return _db.GetSimpleClient<T>();
  }

  /// <summary>
  /// 配置实体过滤器
  /// </summary>
  /// <remarks>
  /// 配置全局查询过滤器，主要用于：
  /// 1. 软删除过滤
  /// 2. 租户过滤
  /// 3. 数据权限过滤等
  /// </remarks>
  private void ConfigureFilters()
  {
    // 配置软删除过滤器
    _db.QueryFilter.AddTableFilter<ILeanSoftDelete>(it => it.IsDeleted == 0);
  }

  /// <summary>
  /// 配置实体映射
  /// </summary>
  /// <remarks>
  /// 用于：
  /// 1. 创建或更新数据库表结构
  /// 2. 配置实体与表的映射关系
  /// 3. 配置索引、主键等数据库对象
  /// </remarks>
  public void ConfigureEntities()
  {
    try
    {
      // 1. 配置全局过滤器
      ConfigureFilters();

      // 2. 获取 Domain 程序集
      var domainAssembly = Assembly.Load("Lean.CodeGen.Domain");
      _logger.Info($"加载 Domain 程序集成功");

      // 3. 获取所有实体类型
      var allTypes = domainAssembly.GetTypes()
          .Where(t => t.Namespace?.StartsWith("Lean.CodeGen.Domain.Entities") == true)
          .ToList();

      // 按目录分组统计实体数量
      var groupedTypes = allTypes
          .GroupBy(t => t.Namespace ?? "Unknown")
          .ToDictionary(g => g.Key, g => g.Count());

      foreach (var group in groupedTypes)
      {
        _logger.Info($"\u001b[35m实体扫描 >>\u001b[0m 目录: {group.Key} \u001b[90m|\u001b[0m 实体数量: {group.Value}");
      }

      // 获取有效的实体类型
      var entityTypes = allTypes
          .Where(t =>
          {
            // 排除基类和抽象类
            if (t.IsAbstract || t.Name == "LeanBaseEntity")
              return false;

            // 检查是否有 SugarTable 特性
            var attr = t.GetCustomAttribute<SugarTable>();
            if (attr != null)
            {
              _logger.Info($"\u001b[36m实体映射 >>\u001b[0m 实体: {t.Name} \u001b[90m|\u001b[0m 表名: {attr.TableName}");
              return true;
            }
            return false;
          })
          .ToList();

      _logger.Info($"\u001b[35m实体扫描 >>\u001b[0m \u001b[32m有效实体总数: {entityTypes.Count}\u001b[0m");

      // 4. 遍历实体类型并创建表
      foreach (var entityType in entityTypes)
      {
        try
        {
          // 配置表名和列名
          _db.CodeFirst.SetStringDefaultLength(200).InitTables(entityType);
          _logger.Info($"\u001b[32m初始化表成功 >>\u001b[0m {entityType.Name}");
        }
        catch (Exception ex)
        {
          _logger.Error($"\u001b[31m初始化表失败 >>\u001b[0m {entityType.Name} \u001b[90m|\u001b[0m {ex.Message}");
        }
      }
    }
    catch (Exception ex)
    {
      _logger.Error($"配置实体时发生错误: {ex.Message}");
      throw;
    }
  }

  /// <summary>
  /// 开始事务
  /// </summary>
  public async Task BeginTransactionAsync()
  {
    await _db.Ado.BeginTranAsync();
  }

  /// <summary>
  /// 提交事务
  /// </summary>
  public async Task CommitTransactionAsync()
  {
    await _db.Ado.CommitTranAsync();
  }

  /// <summary>
  /// 回滚事务
  /// </summary>
  public async Task RollbackTransactionAsync()
  {
    await _db.Ado.RollbackTranAsync();
  }

  /// <summary>
  /// 插入实体并返回自增主键
  /// </summary>
  public async Task<long> InsertReturnIdentityAsync<TEntity>(TEntity entity) where TEntity : class, new()
  {
    return await _db.Insertable(entity).ExecuteReturnIdentityAsync();
  }

  /// <summary>
  /// 批量插入实体
  /// </summary>
  public async Task<bool> InsertRangeAsync<TEntity>(List<TEntity> entities) where TEntity : class, new()
  {
    return await _db.Insertable(entities).ExecuteCommandAsync() > 0;
  }

  /// <summary>
  /// 更新实体
  /// </summary>
  public async Task<bool> UpdateAsync<TEntity>(TEntity entity) where TEntity : class, new()
  {
    return await _db.Updateable(entity).ExecuteCommandAsync() > 0;
  }

  /// <summary>
  /// 批量更新实体
  /// </summary>
  public async Task<bool> UpdateRangeAsync<TEntity>(List<TEntity> entities) where TEntity : class, new()
  {
    return await _db.Updateable(entities).ExecuteCommandAsync() > 0;
  }

  /// <summary>
  /// 删除实体
  /// </summary>
  public async Task<bool> DeleteAsync<TEntity>(TEntity entity) where TEntity : class, new()
  {
    return await _db.Deleteable(entity).ExecuteCommandAsync() > 0;
  }

  /// <summary>
  /// 根据条件删除实体
  /// </summary>
  public async Task<bool> DeleteAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class, new()
  {
    return await _db.Deleteable<TEntity>().Where(predicate).ExecuteCommandAsync() > 0;
  }

  /// <summary>
  /// 批量删除实体
  /// </summary>
  public async Task<bool> DeleteRangeAsync<TEntity>(List<TEntity> entities) where TEntity : class, new()
  {
    return await _db.Deleteable(entities).ExecuteCommandAsync() > 0;
  }

  /// <summary>
  /// 根据ID获取实体
  /// </summary>
  public async Task<TEntity?> GetByIdAsync<TEntity>(object id) where TEntity : class, new()
  {
    return await _db.Queryable<TEntity>().InSingleAsync(id);
  }

  /// <summary>
  /// 根据条件获取第一个实体
  /// </summary>
  public async Task<TEntity?> FirstOrDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class, new()
  {
    return await _db.Queryable<TEntity>().FirstAsync(predicate);
  }

  /// <summary>
  /// 根据条件获取实体列表
  /// </summary>
  public async Task<List<TEntity>> GetListAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class, new()
  {
    return await _db.Queryable<TEntity>().Where(predicate).ToListAsync();
  }

  /// <summary>
  /// 分页查询
  /// </summary>
  public async Task<(long Total, List<TEntity> Items)> GetPageListAsync<TEntity>(
    Expression<Func<TEntity, bool>> predicate,
    int pageSize,
    int pageIndex,
    Expression<Func<TEntity, object>>? orderByExpression = null,
    bool isAsc = true) where TEntity : class, new()
  {
    var query = _db.Queryable<TEntity>().Where(predicate);

    if (orderByExpression != null)
    {
      query = isAsc ? query.OrderBy(orderByExpression) : query.OrderByDescending(orderByExpression);
    }

    RefAsync<int> total = 0;
    var items = await query.ToPageListAsync(pageIndex, pageSize, total);

    return (total, items);
  }

  /// <summary>
  /// 检查是否存在
  /// </summary>
  public async Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class, new()
  {
    return await _db.Queryable<TEntity>().AnyAsync(predicate);
  }

  /// <summary>
  /// 根据条件获取数量
  /// </summary>
  public async Task<long> CountAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class, new()
  {
    return await _db.Queryable<TEntity>().CountAsync(predicate);
  }

  /// <summary>
  /// 批量查询
  /// </summary>
  public async Task<List<TEntity>> GetBatchAsync<TEntity>(Expression<Func<TEntity, bool>> predicate, int batchSize = 1000) where TEntity : class, new()
  {
    var result = new List<TEntity>();
    var skip = 0;

    while (true)
    {
      var batch = await _db.Queryable<TEntity>()
        .Where(predicate)
        .Skip(skip)
        .Take(batchSize)
        .ToListAsync();

      if (!batch.Any())
        break;

      result.AddRange(batch);
      skip += batchSize;

      if (batch.Count < batchSize)
        break;
    }

    return result;
  }
}