using SecurePass.Core.Interfaces;
using SecurePass.Core.Models;

namespace SecurePass.Core.Services;

public class ProjectStateManager
{
    private readonly IProjectService _projectService;
    private ProjectManager? _project;

    public ProjectStateManager(IProjectService projectService)
    {
        _projectService = projectService;
    }

    /// <summary>
    /// Загружает реальный проект ИЗНАЧАЛЬНО. 
    /// Вызывается СТРОГО после успешного ввода мастер-пароля!
    /// </summary>
    public async Task<ProjectManager> LoadProjectAsync()
    {
        if (_project != null 
            && !string.IsNullOrEmpty(_project.MasterPasswordHash) 
            && _project.Accounts != null)
        {
            return _project;
        }

        if (!_projectService.IsMasterPasswordSet())
        {
            throw new InvalidOperationException("Попытка загрузить проект без установленного мастер-пароля.");
        }

        _project = await _projectService.LoadProjectAsync();
        return _project;
    }

    /// <summary>
    /// Синхронно устанавливает пустой проект при ПЕРВОЙ регистрации пользователя
    /// </summary>
    public void InitializeNewProject(string hash)
    {
        _project = new ProjectManager
        {
            Accounts = new List<Account>(),
            PasswordGeneratorOptions = new PasswordGeneratorOptions(),
            MasterPasswordHash = hash
        };
    }

    public ProjectManager CurrentProject => _project ?? throw new InvalidOperationException("Проект еще не загружен.");

    /// <summary>
    /// Сохраняет текущий проект в файл (вызывается при изменении или закрытии)
    /// </summary>
    public async Task SaveProjectAsync()
    {
        // Сохраняем ТОЛЬКО если проект реально существует в памяти и пароль установлен
        if (_project != null && _projectService.IsMasterPasswordSet())
        {
            await _projectService.SaveProjectAsync(_project);
        }
    }

    // Теперь IsLoaded возвращает true ТОЛЬКО если загружены реальные данные пользователя
    public bool IsLoaded => _project != null && _projectService.IsMasterPasswordSet();

    public void AddAccount(Account account)
    {
        _project?.Accounts.Add(account);
        _ = SaveProjectAsync(); // Рекомендуется сохранять сразу при изменении!
    }

    public bool RemoveAccount(string accountId)
    {
        var account = _project?.Accounts.FirstOrDefault(a => a.Id == accountId);
        if (account != null && _project!.Accounts.Remove(account))
        {
            _ = SaveProjectAsync();
            return true;
        }
        return false;
    }

    public bool UpdateAccount(Account updatedAccount)
    {
        var existing = _project?.Accounts.FirstOrDefault(a => a.Id == updatedAccount.Id);
        if (existing != null)
        {
            existing.UpdateFrom(updatedAccount);
            _ = SaveProjectAsync();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Сброс состояния при логауте
    /// </summary>
    public void Clear()
    {
        _project = null;
    }
}
