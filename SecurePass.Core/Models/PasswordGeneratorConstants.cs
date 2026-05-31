namespace SecurePass.Core.Models;

internal class PasswordGeneratorConstants
{
    /// <summary>
    /// Длина пароля по умолчанию.
    /// </summary>
    public const int DefaultLength = 12;
    
    /// <summary>
    /// Максимальная длина пароля.
    /// </summary>
    public const int MaxLength = 32;

    /// <summary>
    /// Минимальная длина пароля.
    /// </summary>
    public const int MinLength = 8;

    /// <summary>
    /// Буквы в верхнем регистре.
    /// </summary>
    public const string Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    /// <summary>
    /// Буквы в нижнем регистре.
    /// </summary>
    public const string Lowercase = "abcdefghijklmnopqrstuvwxyz";

    /// <summary>
    /// Числа.
    /// </summary>
    public const string Digits = "0123456789";

    /// <summary>
    /// Спецсимволы.
    /// </summary>
    public const string SpecialChars = "!@#$%^&*";
}
