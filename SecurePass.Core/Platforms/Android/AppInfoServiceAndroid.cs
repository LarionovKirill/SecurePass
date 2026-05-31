using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using SecurePass.Core.Interfaces;
using SecurePass.Core.Models;

[assembly: Dependency(typeof(SecurePass.Core.AppInfoServiceAndroid))]

namespace SecurePass.Core;

public class AppInfoServiceAndroid : IAppInfoService
{
    public Task<List<InstalledApp>> GetInstalledAppsAsync()
    {
        return Task.Run(() =>
        {
            var appsList = new List<InstalledApp>();
            var context = Android.App.Application.Context;
            var packageManager = context.PackageManager;
            var currentPackageName = context.PackageName;

            // Пробуем получить через GetInstalledApplications
            List<ApplicationInfo> allApps = new List<ApplicationInfo>();

            try
            {
                // Способ 1
                allApps = packageManager.GetInstalledApplications(PackageInfoFlags.MatchAll).ToList();

                // Если список пуст, пробуем способ 2
                if (allApps.Count == 0)
                {
                    var packages = packageManager.GetInstalledPackages(PackageInfoFlags.MatchAll);
                    allApps = packages.Select(p => p.ApplicationInfo).Where(a => a != null).ToList();
                }

                // Если всё ещё пусто, пробуем получить только приложения с лаунчером
                if (allApps.Count == 0)
                { 
                    var intent = new Intent(Intent.ActionMain);
                    intent.AddCategory(Intent.CategoryLauncher);
                    var resolveInfos = packageManager.QueryIntentActivities(intent, 0);
                    allApps = resolveInfos.Select(r => r.ActivityInfo.ApplicationInfo).ToList();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting apps: {ex.Message}");
                return appsList;
            }

            foreach (var app in allApps)
            {
                if (app?.PackageName == currentPackageName)
                    continue;

                if (app == null) continue;

                // Проверка на системное приложение (опционально)
                bool isSystemApp = (app.Flags & ApplicationInfoFlags.System) != 0;
                bool isUpdatedSystemApp = (app.Flags & ApplicationInfoFlags.UpdatedSystemApp) != 0;
                bool isUserApp = !isSystemApp && !isUpdatedSystemApp;

                // Решите, нужны ли вам системные приложения
                // Если нужны только пользовательские, раскомментируйте следующую строку:
                // if (!isUserApp) continue;

                try
                {
                    var installedApp = new InstalledApp
                    {
                        AppName = app.LoadLabel(packageManager)?.ToString() ?? app.PackageName ?? "Unknown",
                        PackageName = app.PackageName ?? "Unknown",
                        AppIcon = GetImageSourceFromDrawable(app.LoadIcon(packageManager))
                    };
                    appsList.Add(installedApp);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error loading icon for {app.PackageName}: {ex.Message}");
                }
            }

            return appsList.OrderBy(a => a.AppName).ToList();
        });
    }

    private ImageSource GetImageSourceFromDrawable(Drawable drawable)
    {
        if (drawable == null)
            return null;

        // Получаем размеры drawable, с запасным вариантом
        int width = drawable.IntrinsicWidth > 0 ? drawable.IntrinsicWidth : 96;
        int height = drawable.IntrinsicHeight > 0 ? drawable.IntrinsicHeight : 96;

        using (var bitmap = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888))
        {
            var canvas = new Canvas(bitmap);
            drawable.SetBounds(0, 0, canvas.Width, canvas.Height);
            drawable.Draw(canvas);

            using (var stream = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
                stream.Seek(0, SeekOrigin.Begin);
                return ImageSource.FromStream(() => stream);
            }
        }
    }
}