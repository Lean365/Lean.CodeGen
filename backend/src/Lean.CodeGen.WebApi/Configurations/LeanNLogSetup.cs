using NLog;
using NLog.Web;

namespace Lean.CodeGen.WebApi.Configurations;

public static class LeanNLogSetup
{
  public static WebApplicationBuilder AddLeanNLog(this WebApplicationBuilder builder)
  {
    // 设置 NLog 配置文件路径
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // 确保日志目录存在
    var logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
    if (!Directory.Exists(logPath))
    {
      Directory.CreateDirectory(logPath);
    }

    return builder;
  }
}