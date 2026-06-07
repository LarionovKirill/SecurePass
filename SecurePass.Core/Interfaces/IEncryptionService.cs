namespace SecurePass.Core.Interfaces;

public interface IEncryptionService
{
    /// <summary>
    /// Шифрует данные с использованием мастер-пароля
    /// </summary>
    string Encrypt(string plainText, string masterPassword);

    /// <summary>
    /// Расшифровывает данные с использованием мастер-пароля
    /// </summary>
    string Decrypt(string cipherText, string masterPassword);

    /// <summary>
    /// Проверяет, зашифрованы ли данные
    /// </summary>
    bool IsEncrypted(string data);
}