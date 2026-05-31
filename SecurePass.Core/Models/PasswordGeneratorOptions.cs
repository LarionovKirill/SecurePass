namespace SecurePass.Core.Models;

/// <summary>
/// Настройки для генерации пароля.
/// </summary>
public class PasswordGeneratorOptions
{
    /// <summary>
    /// Длина пароля.
    /// </summary>
    public int Length { get; set; }

    /// <summary>
    /// Использовать заглавные буквы.
    /// </summary>
    public bool UseCapitalLetters { get; set; }

    /// <summary>
    /// Использовать строчные буквы.
    /// </summary>
    public bool UseLowercaseLetters { get; set; }

    /// <summary>
    /// Использовать цифры.
    /// </summary>
    public bool UseDigits { get; set; }

    /// <summary>
    /// Использовать спецсимволы.
    /// </summary>
    public bool UseSpecialCharacters { get; set; }
}
