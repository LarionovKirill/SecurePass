namespace SecurePass.Core.Models;

/// <summary>
/// Настройки для генерации пароля.
/// </summary>
public class PasswordGeneratorOptions
{
    /// <summary>
    /// Длина пароля.
    /// </summary>
    public int Length { get; set; } = 10;

    /// <summary>
    /// Использовать заглавные буквы.
    /// </summary>
    public bool UseCapitalLetters { get; set; } = true;

    /// <summary>
    /// Использовать строчные буквы.
    /// </summary>
    public bool UseLowercaseLetters { get; set; } = true;

    /// <summary>
    /// Использовать цифры.
    /// </summary>
    public bool UseDigits { get; set; } = true;

    /// <summary>
    /// Использовать спецсимволы.
    /// </summary>
    public bool UseSpecialCharacters { get; set; } = true;
}
