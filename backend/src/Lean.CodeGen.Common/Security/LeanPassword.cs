using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Lean.CodeGen.Common.Security;

/// <summary>
/// 密码处理工具类
/// </summary>
public static class LeanPassword
{
  private const int SALT_SIZE = 16; // 16 bytes = 128 bits
  private const int HASH_SIZE = 32; // 32 bytes = 256 bits
  private const int ITERATIONS = 10000; // 迭代次数

  /// <summary>
  /// 生成随机盐值
  /// </summary>
  public static string GenerateSalt()
  {
    var salt = new byte[SALT_SIZE];
    using (var rng = new RNGCryptoServiceProvider())
    {
      rng.GetBytes(salt);
    }
    return Convert.ToBase64String(salt);
  }

  /// <summary>
  /// 使用PBKDF2算法哈希密码
  /// </summary>
  public static string HashPassword(string password, string salt)
  {
    if (string.IsNullOrEmpty(password)) throw new ArgumentNullException(nameof(password));
    if (string.IsNullOrEmpty(salt)) throw new ArgumentNullException(nameof(salt));

    var saltBytes = Convert.FromBase64String(salt);
    using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, ITERATIONS, HashAlgorithmName.SHA256))
    {
      var hash = pbkdf2.GetBytes(HASH_SIZE);
      return Convert.ToBase64String(hash);
    }
  }

  /// <summary>
  /// 验证密码强度
  /// </summary>
  public static (bool IsValid, string Message) ValidatePasswordStrength(string password)
  {
    if (string.IsNullOrEmpty(password))
      return (false, "密码不能为空");

    if (password.Length < 8)
      return (false, "密码长度必须至少为8个字符");

    if (password.Length > 32)
      return (false, "密码长度不能超过32个字符");

    var hasNumber = new Regex(@"[0-9]+");
    var hasUpperChar = new Regex(@"[A-Z]+");
    var hasLowerChar = new Regex(@"[a-z]+");
    var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

    if (!hasNumber.IsMatch(password))
      return (false, "密码必须包含至少一个数字");

    if (!hasUpperChar.IsMatch(password))
      return (false, "密码必须包含至少一个大写字母");

    if (!hasLowerChar.IsMatch(password))
      return (false, "密码必须包含至少一个小写字母");

    if (!hasSymbols.IsMatch(password))
      return (false, "密码必须包含至少一个特殊字符");

    return (true, "密码强度符合要求");
  }

  /// <summary>
  /// 验证密码是否匹配
  /// </summary>
  public static bool VerifyPassword(string password, string hash, string salt)
  {
    if (string.IsNullOrEmpty(password)) return false;
    if (string.IsNullOrEmpty(hash)) return false;
    if (string.IsNullOrEmpty(salt)) return false;

    try
    {
      var newHash = HashPassword(password, salt);
      return hash == newHash;
    }
    catch
    {
      return false;
    }
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
}