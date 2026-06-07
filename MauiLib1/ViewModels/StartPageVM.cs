using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SecurePass.Core.Interfaces;

namespace SecurePass.VM.ViewModels;

public partial class StartPageVM : ObservableObject
{
    private readonly IPasswordManager _passwordManager;
    private readonly IBiometricService _biometricService;
    private readonly IDialogService _dialogService;
    private readonly IShellService _shellService;

    [ObservableProperty]
    private string _masterPassword = string.Empty;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private bool _isBiometricVisible;

    [ObservableProperty]
    private bool _hasError;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    public StartPageVM(
        IPasswordManager passwordManager,
        IBiometricService biometricService,
        IDialogService dialogService,
        IShellService shellService)
    {
        _passwordManager = passwordManager;
        _biometricService = biometricService;
        _dialogService = dialogService;
        _shellService = shellService;
    }

    public async Task InitializeAsync()
    {
        IsLoading = true;

        try
        {
            // Проверяем, установлен ли мастер-пароль в системе
            var isMasterPasswordSet = await _passwordManager.IsMasterPasswordSetAsync();

            if (!isMasterPasswordSet)
            {
                // Первый запуск — создаём мастер-пароль
                IsLoading = false;
                var success = await _dialogService.ShowCreateMasterPasswordDialogAsync();

                if (success)
                {
                    // ПОСЛЕ СОЗДАНИЯ: В вашем диалоге обязательно должно быть реализовано 
                    // сохранение пароля в SecureStorage.Default.SetAsync("secure_master_password", password)
                    // и вызов _passwordManager.SetMasterPasswordAsync(password).
                    _shellService.SwitchToAuthenticatedShell();
                }
                else
                {
                    await _dialogService.ShowAlertAsync("Ошибка", "Не удалось создать мастер-пароль");
                    Application.Current?.Quit();
                }
                return;
            }

            // Проверяем доступность биометрии на устройстве (сканер отпечатков/FaceID)
            IsBiometricVisible = await _biometricService.IsBiometricAvailableAsync();

            // Автоматически предлагаем приложить палец, если биометрия настроена
            if (IsBiometricVisible)
            {
                await BiometricLogin();
            }
        }
        catch (Exception ex)
        {
            await _dialogService.ShowAlertAsync("Ошибка при инициализации", ex.Message);
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task Login()
    {
        if (IsLoading) return;

        IsLoading = true;
        HasError = false;
        ErrorMessage = string.Empty;

        try
        {
            if (string.IsNullOrWhiteSpace(MasterPassword))
            {
                HasError = true;
                ErrorMessage = "Введите мастер-пароль";
                return;
            }

            // Проверяем текстовый пароль, инициализируем AES-ключ и подгружаем аккаунты
            var isValid = await _passwordManager.CheckMasterPasswordAsync(MasterPassword);

            if (isValid)
            {
                // Сохраняем/обновляем пароль в аппаратном хранилище для будущих биометрических входов
                await SecureStorage.Default.SetAsync("secure_master_password", MasterPassword);

                _shellService.SwitchToAuthenticatedShell();
            }
            else
            {
                HasError = true;
                ErrorMessage = "Неверный мастер-пароль";
                MasterPassword = string.Empty;
            }
        }
        catch (Exception ex)
        {
            await _dialogService.ShowAlertAsync("Ошибка авторизации", ex.Message);
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task BiometricLogin()
    {
        if (IsLoading) return;

        IsLoading = true;

        try
        {
            // 1. Запрашиваем у операционной системы сканирование пальца или лица
            var isAuthenticated = await _biometricService.AuthenticateAsync();

            if (isAuthenticated)
            {
                string? savedPassword = await SecureStorage.Default.GetAsync("secure_master_password");

                if (!string.IsNullOrEmpty(savedPassword))
                {
                    // 3. Пропускаем пароль через стандартную цепочку проверки.
                    // Это восстановит сессию шифрования в ProjectService и загрузит данные в ProjectStateManager
                    var isProjectLoaded = await _passwordManager.CheckMasterPasswordAsync(savedPassword);

                    if (isProjectLoaded)
                    {
                        _shellService.SwitchToAuthenticatedShell();
                    }
                    else
                    {
                        await _dialogService.ShowAlertAsync("Ошибка", "Ключ шифрования не подошел. Пожалуйста, введите пароль вручную.");
                    }
                }
                else
                {
                    await _dialogService.ShowAlertAsync("Внимание", "Для первого входа после активации биометрии введите мастер-пароль вручную.");
                }
            }
            else
            {
                // Пользователь отменил биометрию или она не распознана — просто оставляем его на экране ввода пароля
                System.Diagnostics.Debug.WriteLine("Биометрическая аутентификация отклонена или отменена.");
            }
        }
        catch (Exception ex)
        {
            await _dialogService.ShowAlertAsync("Ошибка биометрии", ex.Message);
        }
        finally
        {
            IsLoading = false;
        }
    }
}
