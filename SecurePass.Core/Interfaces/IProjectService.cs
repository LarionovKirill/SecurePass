using SecurePass.Core.Models;

namespace SecurePass.Core.Interfaces;

public interface IProjectService
{
    Task SaveProjectAsync(ProjectManager project);
    Task<ProjectManager> LoadProjectAsync();
    Task SaveAccountsAsync(List<Account> accounts);
    Task<List<Account>> LoadAccountsAsync();

    void SetMasterPassword(string masterPassword);
    Task<bool> SetMasterPasswordAsync(string masterPassword, string? masterPasswordHash = null);
    Task SaveMasterPasswordHash(string hash);
    Task<string?> LoadMasterPasswordHash();
    void ClearMasterPassword();
    bool IsMasterPasswordSet();
    bool DataFileExists();
}