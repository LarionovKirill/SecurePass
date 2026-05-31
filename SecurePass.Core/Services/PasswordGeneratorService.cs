using SecurePass.Core.Models;
using System.Security.Cryptography;
using System.Text;

namespace SecurePass.Core.Services;

/// <summary>
/// Сервис генерации паролей.
/// </summary>
public class PasswordGeneratorService
{
    /// <summary>
    /// Создает объект класса <see cref="PasswordGeneratorService"/>.
    /// </summary>
    public PasswordGeneratorService() { }

    /// <summary>
    /// Генерирует пароль на основе настроек.
    /// </summary>
    /// <param name="options">Настройки генерации пароля.</param>
    /// <returns>Пароль.</returns>
    public string GeneratePassword(PasswordGeneratorOptions options)
    {
        var alphabets = GetUsingAlphabets(options);
        var password = new StringBuilder();

        // Точно заполняем по символу из каждого алфавита.
        foreach (var alphabet in alphabets)
        {
            password.Append(alphabet[GetRandomNumber(alphabet.Length)]);
        }

        // Заполняем оставшиеся значения.
        for (int i = alphabets.Count; i < options.Length; i++)
        {
            var alphabet = alphabets[GetRandomNumber(alphabets.Count)];
            password.Append(alphabet[GetRandomNumber(alphabet.Length)]);
        }

        password = ShakePassword(password);

        return password.ToString();
    }

    /// <summary>
    /// Перемешивает символы в пароле.
    /// </summary>
    /// <param name="password">Пароль.</param>
    /// <returns>Перемешанный пароль.</returns>
    private StringBuilder ShakePassword(StringBuilder password)
    {
        for (int i = password.Length - 1; i > 0; i--)
        {
            int j = GetRandomNumber(i);
            (password[i], password[j]) = (password[j], password[i]);
        }

        return password;
    }


    /// <summary>
    /// Проверяет, что пароль имеет символ из алфавита.
    /// </summary>
    /// <param name="password">Пароль.</param>
    /// <param name="alphabet">Алфавит.</param>
    /// <returns>True, если содердит символ из алфавита, иначе false.</returns>
    private bool IsPasswordContainsAlphabet(string password, string alphabet)
    {
        foreach (var symbol in password)
        {
            if (alphabet.Contains(symbol))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Генерирует алфавит по настрокам.
    /// </summary>
    /// <param name="options">Настройки пароля.</param>
    /// <returns>Алфавит.</returns>
    private List<string> GetUsingAlphabets(PasswordGeneratorOptions options)
    {
        var alphabets = new List<string>(4);

        if (options.UseCapitalLetters)
        {
            alphabets.Add(PasswordGeneratorConstants.Uppercase);
        }

        if (options.UseLowercaseLetters)
        {
            alphabets.Add(PasswordGeneratorConstants.Lowercase);
        }

        if (options.UseDigits)
        {
            alphabets.Add(PasswordGeneratorConstants.Digits);
        }

        if (options.UseSpecialCharacters)
        {
            alphabets.Add(PasswordGeneratorConstants.SpecialChars);
        }

        return alphabets;
    }

    /// <summary>
    /// Генерирует случайное число в диапазоне от 0 до maxNumber-1;
    /// </summary>
    /// <param name="maxNumber">Верхняя граница диапазона.</param>
    /// <returns>Случайное число.</returns>
    private int GetRandomNumber(int maxNumber)
    {
        return RandomNumberGenerator.GetInt32(0, maxNumber);
    }
}