using Microsoft.Win32;
using SecurePass.Core.Interfaces;
using SecurePass.Core.Models;

[assembly: Dependency(typeof(SecurePass.Core.AppInfoServiceWindows))]
namespace SecurePass.Core;

public class AppInfoServiceWindows : IAppInfoService
{
    public Task<List<InstalledApp>> GetInstalledAppsAsync()
    {
        return Task.Run(() =>
        {
            var apps = new List<InstalledApp>();

            // Проверяем 64-битные и 32-битные приложения
            apps.AddRange(GetAppsFromRegistry(RegistryView.Registry64));
            apps.AddRange(GetAppsFromRegistry(RegistryView.Registry32));

            return apps
                .GroupBy(x => x.AppName) // Убираем дубликаты
                .Select(g => g.First())
                .OrderBy(x => x.AppName)
                .ToList();
        });
    }

    private List<InstalledApp> GetAppsFromRegistry(RegistryView view)
    {
        var apps = new List<InstalledApp>();
        const string uninstallKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

        // Открываем нужную ветку реестра (64 или 32 бита)[citation:2]
        using var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, view);
        using var uninstallKey = baseKey.OpenSubKey(uninstallKeyPath);

        if (uninstallKey == null) return apps;

        foreach (string subKeyName in uninstallKey.GetSubKeyNames())
        {
            using var subKey = uninstallKey.OpenSubKey(subKeyName);
            if (subKey == null) continue;

            // DisplayName — это основной ключ с названием программы[citation:9]
            var displayName = subKey.GetValue("DisplayName") as string;

            // Пропускаем пустые записи, обновления системы (KB...)
            if (string.IsNullOrEmpty(displayName)) continue;
            if (displayName.StartsWith("KB") && displayName.Length < 15) continue;

            var app = new InstalledApp
            {
                AppName = displayName,
                // Для Win32 программ иконку придется искать по пути exe файла
                // Путь лежит в ключе DisplayIcon или InstallLocation[citation:6]
                AppIcon = GetIconFromLocation(subKey.GetValue("DisplayIcon") as string),
                // Сохраним доп. информацию, чтобы потом, например, открыть программу
                PackageName = subKey.GetValue("InstallLocation") as string ??
                              subKey.GetValue("DisplayIcon") as string
            };

            apps.Add(app);
        }

        return apps;
    }

    private ImageSource? GetIconFromLocation(string? location)
    {
        if (string.IsNullOrEmpty(location)) return null;

        var exePath = location.Split(',')[0];
        if (File.Exists(exePath))
        {
            return null;
        }

        return null;
    }

}
