using SecurePass.Core.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace SecurePass.Core.Services;

public class EncryptionService : IEncryptionService
{
    private const int KeySize = 256;
    private const int Iterations = 100000; // 100k итераций для защиты от брутфорса
    private static readonly byte[] Salt = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

    public string Encrypt(string plainText, string masterPassword)
    {
        if (string.IsNullOrEmpty(plainText))
            return string.Empty;

        using var aes = Aes.Create();
        var key = DeriveKey(masterPassword);
        aes.Key = key;
        aes.GenerateIV();

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream();

        // Сначала пишем IV
        ms.Write(aes.IV, 0, aes.IV.Length);

        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        using (var sw = new StreamWriter(cs))
        {
            sw.Write(plainText);
        }

        return Convert.ToBase64String(ms.ToArray());
    }

    public string Decrypt(string cipherText, string masterPassword)
    {
        if (string.IsNullOrEmpty(cipherText))
            return string.Empty;

        try
        {
            var fullCipher = Convert.FromBase64String(cipherText);

            using var aes = Aes.Create();
            var key = DeriveKey(masterPassword);
            aes.Key = key;

            // Извлекаем IV из первых 16 байт
            var iv = new byte[aes.BlockSize / 8];
            if (fullCipher.Length < iv.Length)
                return string.Empty; // Защита от слишком короткой строки

            Array.Copy(fullCipher, 0, iv, 0, iv.Length);
            aes.IV = iv;

            // Остальные данные - зашифрованный текст
            var cipherBytes = new byte[fullCipher.Length - iv.Length];
            Array.Copy(fullCipher, iv.Length, cipherBytes, 0, cipherBytes.Length);

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream(cipherBytes);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);

            return sr.ReadToEnd();
        }
        catch (CryptographicException)
        {
            throw new UnauthorizedAccessException("Не удалось расшифровать данные. Возможно, указан неверный пароль.");
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }

    public bool IsEncrypted(string data)
    {
        if (string.IsNullOrEmpty(data))
            return false;

        // Проверяем, похоже ли на Base64 и достаточно ли длинное
        try
        {
            var bytes = Convert.FromBase64String(data);
            return bytes.Length > 16; // Минимум IV + немного данных
        }
        catch
        {
            return false;
        }
    }

    private byte[] DeriveKey(string password)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(password);

        return Rfc2898DeriveBytes.Pbkdf2(
            passwordBytes,
            Salt,
            Iterations,
            HashAlgorithmName.SHA256,
            KeySize / 8);
    }
}