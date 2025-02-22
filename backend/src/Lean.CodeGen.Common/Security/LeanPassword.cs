using System.Security.Cryptography;
using System.Text;

namespace Lean.CodeGen.Common.Security;

/// <summary>
/// 密码处理工具类
/// </summary>
public static class LeanPassword
{
  /// <summary>
  /// 默认盐值长度
  /// </summary>
  private const int DefaultSaltLength = 32;

  /// <summary>
  /// 默认哈希迭代次数
  /// </summary>
  private const int DefaultIterations = 100000;

  /// <summary>
  /// 生成随机盐值
  /// </summary>
  /// <param name="length">盐值长度，默认16字节</param>
  /// <returns>Base64编码的盐值字符串</returns>
  public static string GenerateSalt(int length = DefaultSaltLength)
  {
    byte[] salt = new byte[length];
    using (var rng = RandomNumberGenerator.Create())
    {
      rng.GetBytes(salt);
    }
    return Convert.ToBase64String(salt);
  }

  /// <summary>
  /// 使用PBKDF2算法加密密码
  /// </summary>
  /// <param name="password">原始密码</param>
  /// <param name="salt">盐值</param>
  /// <param name="iterations">迭代次数，默认10000次</param>
  /// <returns>Base64编码的加密密码</returns>
  public static string HashPassword(string password, string salt, int iterations = DefaultIterations)
  {
    byte[] saltBytes = Convert.FromBase64String(salt);
    byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

    using (var pbkdf2 = new Rfc2898DeriveBytes(passwordBytes, saltBytes, iterations, HashAlgorithmName.SHA256))
    {
      byte[] hash = pbkdf2.GetBytes(32); // 256位哈希
      return Convert.ToBase64String(hash);
    }
  }

  /// <summary>
  /// 验证密码
  /// </summary>
  /// <param name="password">待验证的密码</param>
  /// <param name="salt">盐值</param>
  /// <param name="hashedPassword">已加密的密码</param>
  /// <param name="iterations">迭代次数，默认10000次</param>
  /// <returns>密码是否匹配</returns>
  public static bool VerifyPassword(string password, string salt, string hashedPassword, int iterations = DefaultIterations)
  {
    string computedHash = HashPassword(password, salt, iterations);
    return computedHash == hashedPassword;
  }

  /// <summary>
  /// 生成安全的随机密码
  /// </summary>
  /// <param name="length">密码长度，默认12位</param>
  /// <returns>随机生成的密码</returns>
  public static string GenerateRandomPassword(int length = 12)
  {
    const string lowerChars = "abcdefghijklmnopqrstuvwxyz";
    const string upperChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    const string numberChars = "0123456789";
    const string specialChars = "!@#$%^&*()_+-=[]{}|;:,.<>?";

    var allChars = lowerChars + upperChars + numberChars + specialChars;
    var result = new StringBuilder();

    // 确保至少包含一个小写字母、大写字母、数字和特殊字符
    result.Append(lowerChars[RandomNumberGenerator.GetInt32(lowerChars.Length)]);
    result.Append(upperChars[RandomNumberGenerator.GetInt32(upperChars.Length)]);
    result.Append(numberChars[RandomNumberGenerator.GetInt32(numberChars.Length)]);
    result.Append(specialChars[RandomNumberGenerator.GetInt32(specialChars.Length)]);

    // 生成剩余的随机字符
    for (int i = 4; i < length; i++)
    {
      result.Append(allChars[RandomNumberGenerator.GetInt32(allChars.Length)]);
    }

    // 打乱字符顺序
    return new string(result.ToString().ToCharArray().OrderBy(x => RandomNumberGenerator.GetInt32(length)).ToArray());
  }

  /// <summary>
  /// 验证密码强度
  /// </summary>
  /// <param name="password">待验证的密码</param>
  /// <returns>密码是否符合强度要求</returns>
  public static (bool IsValid, string Message) ValidatePasswordStrength(string password)
  {
    if (string.IsNullOrEmpty(password))
    {
      return (false, "密码不能为空");
    }

    if (password.Length < 8)
    {
      return (false, "密码长度至少为8个字符");
    }

    if (!password.Any(char.IsUpper))
    {
      return (false, "密码必须包含至少一个大写字母");
    }

    if (!password.Any(char.IsLower))
    {
      return (false, "密码必须包含至少一个小写字母");
    }

    if (!password.Any(char.IsDigit))
    {
      return (false, "密码必须包含至少一个数字");
    }

    if (!password.Any(c => !char.IsLetterOrDigit(c)))
    {
      return (false, "密码必须包含至少一个特殊字符");
    }

    return (true, "密码符合强度要求");
  }
}