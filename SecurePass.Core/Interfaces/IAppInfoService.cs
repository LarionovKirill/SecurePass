using SecurePass.Core.Models;

namespace SecurePass.Core.Interfaces;

public interface IAppInfoService
{
    Task<List<InstalledApp>> GetInstalledAppsAsync();
}
