using System.Text.Json;
using SecurePass.Core.Models;

namespace SecurePass.Core.Services;

/// <summary>
/// Сервис для работы с проектом (сохранение/загрузка).
/// </summary>
public static class ProjectService
{
    private static readonly string _fileName = "project_data.json";
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    /// <summary>
    /// Возвращает полный путь к файлу проекта.
    /// </summary>
    private static string GetFilePath()
    {
        string appDataDirectory = FileSystem.Current.AppDataDirectory;
        return Path.Combine(appDataDirectory, _fileName);
    }

    /// <summary>
    /// Сохраняет данные проекта в файл.
    /// </summary>
    public static async Task SaveProjectAsync(ProjectManager project)
    {
        try
        {
            string filePath = GetFilePath();
            string json = JsonSerializer.Serialize(project, _jsonOptions);
            await File.WriteAllTextAsync(filePath, json);
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при сохранении проекта: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Загружает данные проекта из файла.
    /// </summary>
    public static async Task<ProjectManager> LoadProjectAsync()
    {
        try
        {
            string filePath = GetFilePath();

            if (!File.Exists(filePath))
            {
                return new ProjectManager
                {
                    Accounts = new List<Account>(),
                    PasswordGeneratorOptions = new PasswordGeneratorOptions(),
                    UseBiometric = false
                };
            }

            string json = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<ProjectManager>(json, _jsonOptions);
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при загрузке проекта: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Сохраняет только список аккаунтов.
    /// </summary>
    public static async Task SaveAccountsAsync(List<Account> accounts)
    {
        var project = await LoadProjectAsync();
        project.Accounts = accounts;
        await SaveProjectAsync(project);
    }

    /// <summary>
    /// Загружает только список аккаунтов.
    /// </summary>
    public static async Task<List<Account>> LoadAccountsAsync()
    {
        var project = await LoadProjectAsync();
        return project.Accounts ?? new List<Account>();
    }
}