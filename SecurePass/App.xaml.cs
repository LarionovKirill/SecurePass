using SecurePass.Core.Services;

namespace SecurePass;

public partial class App : Application
{
    private readonly ProjectStateManager _projectStateManager;
    private bool _isSaving;

    public App(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _projectStateManager = serviceProvider.GetRequiredService<ProjectStateManager>();
        MainPage = new AppShell();

        if (Current?.MainPage?.Handler?.MauiContext != null)
        {
            var window = Current.Windows.FirstOrDefault();
            if (window != null)
            {
                window.Destroying += OnWindowDestroying;
            }
        }
    }

    protected override void OnSleep()
    {
        _ = SaveProjectAsync();
        base.OnSleep();
    }

    private async void OnWindowDestroying(object? sender, EventArgs e)
    {
        await SaveProjectAsync();
    }

    private async Task SaveProjectAsync()
    {
        if (_isSaving) return;
        _isSaving = true;

        try
        {
            // Благодаря изменению в IsLoaded, триггер сработает только если пользователь авторизован
            if (_projectStateManager.IsLoaded)
            {
                await _projectStateManager.SaveProjectAsync();
                System.Diagnostics.Debug.WriteLine("Проект успешно сохранен в фоне");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Ошибка при автоматическом сохранении: {ex.Message}");
        }
        finally
        {
            _isSaving = false;
        }
    }
}
