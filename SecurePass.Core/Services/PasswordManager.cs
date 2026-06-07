using SecurePass.Core.Interfaces;
using SecurePass.Core.Models;
using System.Security.Cryptography;
using System.Text;

namespace SecurePass.Core.Services;

public class PasswordManager : IPasswordManager
{
    private readonly IProjectService _projectService;

    public PasswordManager(IProjectService projectService)
    {
        _projectService = projectService;
    }

    public async Task<bool> CheckMasterPasswordAsync(string enteredPassword)
    {
        if (string.IsNullOrEmpty(enteredPassword)) return false;

        // Читаем хэш напрямую через сервис (без инициализации менеджера состояний!)
        var savedHash = await _projectService.LoadMasterPasswordHash();
        if (string.IsNullOrEmpty(savedHash)) return false;

        var enteredHash = ComputeHash(enteredPassword);
        if (enteredHash == savedHash)
        {
            _projectService.SetMasterPassword(enteredPassword);
            await _projectService.LoadProjectAsync();
            return true;
        }
        return false;
    }


    public async Task<bool> SetMasterPasswordAsync(string newPassword)
    {
        if (string.IsNullOrEmpty(newPassword))
            return false;

        // 1. Вычисляем хэш
        var hash = ComputeHash(newPassword);

        // 2. Сохраняем хеш в отдельный файл конфигурации через ваш сервис
        await _projectService.SaveMasterPasswordHash(hash);

        // 3. Устанавливаем пароль для шифрования, чтобы сервис знал ключ
        await _projectService.SetMasterPasswordAsync(newPassword, hash);

        // 4. Вместо вызова LoadProjectAsync (который вернет пустой шаблон без привязки хэша),
        // мы ОСОЗНАННО создаем правильный, чистый экземпляр проекта для нового пользователя
        var project = new ProjectManager
        {
            Accounts = new List<Account>(),
            PasswordGeneratorOptions = new PasswordGeneratorOptions
            {
                Length = 12,
                UseCapitalLetters = true,
                UseLowercaseLetters = true,
                UseDigits = true,
                UseSpecialCharacters = true
            },
            UseBiometric = false,
            MasterPasswordHash = hash
        };

        // 5. Записываем свежесозданный зашифрованный проект на диск
        await _projectService.SaveProjectAsync(project);

        // 6. Если вы используете ProjectStateManager для хранения текущего состояния приложения,
        // теперь, когда файл на диске существует и пароль установлен в сервисе,
        // мы можем безопасно вызвать его инициализацию (этот метод у вас есть):
        await _projectService.LoadProjectAsync();

        return true;
    }

    public async Task<bool> IsMasterPasswordSetAsync()
    {
        var hash = await _projectService.LoadMasterPasswordHash();
        return !string.IsNullOrEmpty(hash);
    }

    public void ClearMasterPassword()
    {
        _projectService.ClearMasterPassword();
    }

    private string ComputeHash(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}