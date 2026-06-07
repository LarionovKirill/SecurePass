using System.Text.Json;
using SecurePass.Core.Interfaces;
using SecurePass.Core.Models;

namespace SecurePass.Core.Services;

public class ProjectService : IProjectService
{
    private const string _dataFileName = "project_data.enc";
    private const string _configFileName = "config.json";
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly IEncryptionService _encryptionService;
    private string _cachedMasterPassword = string.Empty;

    // ВАЖНО: Кэшируем только полностью расшифрованный проект!
    private ProjectManager? _cachedProject;

    private class ConfigModel
    {
        public string MasterPasswordHash { get; set; } = string.Empty;
    }

    public ProjectService(IEncryptionService encryptionService)
    {
        _encryptionService = encryptionService;
        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    private string GetDataFilePath() => Path.Combine(FileSystem.Current.AppDataDirectory, _dataFileName);
    private string GetConfigFilePath() => Path.Combine(FileSystem.Current.AppDataDirectory, _configFileName);

    private async Task SaveMasterPasswordHashAsync(string hash)
    {
        try
        {
            var config = new ConfigModel { MasterPasswordHash = hash };
            string json = JsonSerializer.Serialize(config, _jsonOptions);
            await File.WriteAllTextAsync(GetConfigFilePath(), json);
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при сохранении хеша пароля: {ex.Message}", ex);
        }
    }

    private async Task<string?> LoadMasterPasswordHashAsync()
    {
        try
        {
            string filePath = GetConfigFilePath();
            if (!File.Exists(filePath)) return null;

            string json = await File.ReadAllTextAsync(filePath);
            var config = JsonSerializer.Deserialize<ConfigModel>(json, _jsonOptions);
            return config?.MasterPasswordHash;
        }
        catch
        {
            return null;
        }
    }

    public async Task SaveProjectAsync(ProjectManager project)
    {
        if (string.IsNullOrEmpty(_cachedMasterPassword))
            throw new InvalidOperationException("Master password not set. Call SetMasterPassword first.");

        try
        {
            var hash = await LoadMasterPasswordHashAsync();
            project.MasterPasswordHash = hash ?? string.Empty;

            string json = JsonSerializer.Serialize(project, _jsonOptions);
            string encrypted = _encryptionService.Encrypt(json, _cachedMasterPassword);
            await File.WriteAllTextAsync(GetDataFilePath(), encrypted);

            _cachedProject = project; // Сохраняем в кэш только успешную версию
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при сохранении проекта: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Загружает проект с расшифровкой
    /// </summary>
    public async Task<ProjectManager> LoadProjectAsync()
    {
        // 1. Если проект уже расшифрован ранее и лежит в памяти — отдаем его
        if (_cachedProject != null)
            return _cachedProject;

        try
        {
            string dataFilePath = GetDataFilePath();
            var hash = await LoadMasterPasswordHashAsync();

            // 2. Если файла с данными нет — это самый первый запуск приложения
            if (!File.Exists(dataFilePath))
            {
                return new ProjectManager
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
                    MasterPasswordHash = hash ?? string.Empty
                };
            }

            // 3. ИСПРАВЛЕНИЕ: Если файл ЕСТЬ, но пароль еще НЕ ВВЕДЕН (этап проверки в PasswordManager)
            // Мы возвращаем временную пустышку с хешем, но НЕ сохраняем её в _cachedProject!
            if (string.IsNullOrEmpty(_cachedMasterPassword))
            {
                return new ProjectManager
                {
                    Accounts = new List<Account>(),
                    PasswordGeneratorOptions = new PasswordGeneratorOptions(),
                    MasterPasswordHash = hash ?? string.Empty
                };
            }

            // 4. Если мы здесь — пароль установлен, читаем и расшифровываем реальный файл
            var encrypted = await File.ReadAllTextAsync(dataFilePath);
            if (string.IsNullOrEmpty(encrypted))
            {
                return new ProjectManager { MasterPasswordHash = hash ?? string.Empty };
            }

            string json = _encryptionService.Decrypt(encrypted, _cachedMasterPassword);

            // Записываем в кэш только РЕАЛЬНО расшифрованный из файла проект
            _cachedProject = JsonSerializer.Deserialize<ProjectManager>(json, _jsonOptions) ?? new ProjectManager();
            return _cachedProject;
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при загрузке проекта: {ex.Message}", ex);
        }
    }

    public async Task SaveAccountsAsync(List<Account> accounts)
    {
        var project = await LoadProjectAsync();
        project.Accounts = accounts;
        await SaveProjectAsync(project);
    }

    public async Task<List<Account>> LoadAccountsAsync()
    {
        var project = await LoadProjectAsync();
        return project.Accounts ?? new List<Account>();
    }

    public async Task<bool> SetMasterPasswordAsync(string masterPassword, string? masterPasswordHash = null)
    {
        if (string.IsNullOrEmpty(masterPassword))
            return false;

        _cachedMasterPassword = masterPassword;
        _cachedProject = null; // Обязательно сбрасываем кэш, чтобы принудительно перечитать диск с новым паролем

        if (!string.IsNullOrEmpty(masterPasswordHash))
        {
            await SaveMasterPasswordHashAsync(masterPasswordHash);
        }

        return true;
    }

    public void SetMasterPassword(string masterPassword)
    {
        _cachedMasterPassword = masterPassword;
        _cachedProject = null; // Обязательно сбрасываем кэш! Это заставит следующий вызов LoadProjectAsync прочитать диск
    }

    public async Task SaveMasterPasswordHash(string hash) => await SaveMasterPasswordHashAsync(hash);
    public async Task<string?> LoadMasterPasswordHash() => await LoadMasterPasswordHashAsync();

    public void ClearMasterPassword()
    {
        _cachedMasterPassword = string.Empty;
        _cachedProject = null;
    }

    public bool IsMasterPasswordSet() => !string.IsNullOrEmpty(_cachedMasterPassword);
    public bool DataFileExists() => File.Exists(GetDataFilePath());
}
