using SecurePass.Core.Interfaces;
using SecurePass.Core.Models;
using System.Security.Cryptography;
using System.Text;

namespace SecurePass.Core.Services;

public class PasswordGeneratorService : IPasswordGeneratorService
{
    public string GeneratePassword(PasswordGeneratorOptions options)
    {
        var alphabets = GetUsingAlphabets(options);

        if (alphabets.Count == 0)
            return string.Empty;

        var password = new StringBuilder();

        // Гарантируем хотя бы один символ из каждого выбранного алфавита
        foreach (var alphabet in alphabets)
        {
            password.Append(alphabet[GetRandomNumber(alphabet.Length)]);
        }

        // Заполняем остальные символы
        for (int i = alphabets.Count; i < options.Length; i++)
        {
            var alphabet = alphabets[GetRandomNumber(alphabets.Count)];
            password.Append(alphabet[GetRandomNumber(alphabet.Length)]);
        }

        return ShuffleString(password.ToString());
    }

    private List<string> GetUsingAlphabets(PasswordGeneratorOptions options)
    {
        var alphabets = new List<string>();

        if (options.UseCapitalLetters)
            alphabets.Add(PasswordGeneratorConstants.Uppercase);

        if (options.UseLowercaseLetters)
            alphabets.Add(PasswordGeneratorConstants.Lowercase);

        if (options.UseDigits)
            alphabets.Add(PasswordGeneratorConstants.Digits);

        if (options.UseSpecialCharacters)
            alphabets.Add(PasswordGeneratorConstants.SpecialChars);

        return alphabets;
    }

    private string ShuffleString(string input)
    {
        var array = input.ToCharArray();
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = GetRandomNumber(i + 1);
            (array[i], array[j]) = (array[j], array[i]);
        }
        return new string(array);
    }

    private int GetRandomNumber(int maxNumber)
    {
        return RandomNumberGenerator.GetInt32(0, maxNumber);
    }
}