using Lean.CodeGen.Common.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using NLog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Lean.CodeGen.Common.Helpers;

/// <summary>
/// 邮件帮助类
/// </summary>
public class LeanMailHelper
{
  private readonly ILogger _logger;
  private readonly LeanMailOptions _options;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanMailHelper(ILogger logger, IOptions<LeanMailOptions> options)
  {
    _logger = logger;
    _options = options.Value;
  }

  /// <summary>
  /// 发送邮件
  /// </summary>
  /// <param name="to">收件人</param>
  /// <param name="subject">主题</param>
  /// <param name="body">正文</param>
  /// <param name="isHtml">是否HTML</param>
  /// <returns>发送结果</returns>
  public Task<bool> SendAsync(string to, string subject, string body, bool isHtml = true)
  {
    return SendAsync(_options.DefaultFromAddress, _options.DefaultFromName, to, subject, body, isHtml);
  }

  /// <summary>
  /// 发送邮件
  /// </summary>
  /// <param name="from">发件人</param>
  /// <param name="fromName">发件人名称</param>
  /// <param name="to">收件人</param>
  /// <param name="subject">主题</param>
  /// <param name="body">正文</param>
  /// <param name="isHtml">是否HTML</param>
  /// <returns>发送结果</returns>
  public async Task<bool> SendAsync(string from, string fromName, string to, string subject, string body, bool isHtml = true)
  {
    try
    {
      var message = new MimeMessage();
      message.From.Add(new MailboxAddress(fromName, from));
      message.Subject = subject;

      // 添加收件人
      foreach (var recipient in to.Split(';'))
      {
        message.To.Add(new MailboxAddress("", recipient.Trim()));
      }

      // 设置邮件内容
      var bodyBuilder = new BodyBuilder();
      if (isHtml)
      {
        bodyBuilder.HtmlBody = body;
      }
      else
      {
        bodyBuilder.TextBody = body;
      }

      message.Body = bodyBuilder.ToMessageBody();

      return await SendAsync(message);
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "构建邮件消息失败");
      return false;
    }
  }

  /// <summary>
  /// 发送带附件的邮件
  /// </summary>
  /// <param name="to">收件人</param>
  /// <param name="subject">主题</param>
  /// <param name="body">正文</param>
  /// <param name="attachments">附件列表(文件路径)</param>
  /// <param name="isHtml">是否HTML</param>
  /// <returns>发送结果</returns>
  public Task<bool> SendWithAttachmentsAsync(string to, string subject, string body, string[] attachments, bool isHtml = true)
  {
    return SendWithAttachmentsAsync(_options.DefaultFromAddress, _options.DefaultFromName, to, subject, body, attachments, isHtml);
  }

  /// <summary>
  /// 发送带附件的邮件
  /// </summary>
  /// <param name="from">发件人</param>
  /// <param name="fromName">发件人名称</param>
  /// <param name="to">收件人</param>
  /// <param name="subject">主题</param>
  /// <param name="body">正文</param>
  /// <param name="attachments">附件列表(文件路径)</param>
  /// <param name="isHtml">是否HTML</param>
  /// <returns>发送结果</returns>
  public async Task<bool> SendWithAttachmentsAsync(string from, string fromName, string to, string subject, string body, string[] attachments, bool isHtml = true)
  {
    try
    {
      var message = new MimeMessage();
      message.From.Add(new MailboxAddress(fromName, from));
      message.Subject = subject;

      // 添加收件人
      foreach (var recipient in to.Split(';'))
      {
        message.To.Add(new MailboxAddress("", recipient.Trim()));
      }

      // 设置邮件内容
      var bodyBuilder = new BodyBuilder();
      if (isHtml)
      {
        bodyBuilder.HtmlBody = body;
      }
      else
      {
        bodyBuilder.TextBody = body;
      }

      // 添加附件
      foreach (var attachment in attachments)
      {
        if (File.Exists(attachment))
        {
          bodyBuilder.Attachments.Add(Path.GetFileName(attachment), File.OpenRead(attachment));
        }
      }

      message.Body = bodyBuilder.ToMessageBody();

      return await SendAsync(message);
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "构建邮件消息失败");
      return false;
    }
  }

  /// <summary>
  /// 发送邮件
  /// </summary>
  /// <param name="message">邮件消息</param>
  /// <returns>发送结果</returns>
  private async Task<bool> SendAsync(MimeMessage message)
  {
    try
    {
      using var client = new SmtpClient();
      client.Timeout = _options.Timeout * 1000;

      for (int i = 0; i < _options.RetryCount; i++)
      {
        try
        {
          await client.ConnectAsync(_options.SmtpServer, _options.SmtpPort,
              _options.EnableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None);
          await client.AuthenticateAsync(_options.UserName, _options.Password);
          await client.SendAsync(message);
          await client.DisconnectAsync(true);
          return true;
        }
        catch (Exception ex) when (i < _options.RetryCount - 1)
        {
          _logger.Error(ex, $"发送邮件失败，第{i + 1}次重试");
          await Task.Delay(_options.RetryInterval * 1000);
        }
      }

      return false;
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "发送邮件失败");
      return false;
    }
  }
}