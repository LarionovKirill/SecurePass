using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SecurePass.Core.Interfaces;
using SecurePass.Core.Services;

namespace SecurePass.VM.ViewModels;

/// <summary>
/// View-модель начальной страницы приложения.
/// </summary>
public partial class StartPageVM : ObservableObject
{
    /// <summary>
    /// Сервис для навигации по приложению.
    /// </summary>
    private readonly INavigationService _navigationService;

    /// <summary>
    /// Сервис для навигации по приложению.
    /// </summary>
    private readonly IShellService _shellService;

    /// <summary>
    /// Возвращает и задает значение мастера пароля.
    /// </summary>
    [ObservableProperty]
    private string _masterPassword;

    [ObservableProperty]
    private bool _isBiometricAvaliable;

    public StartPageVM(INavigationService navigationService, IShellService shellService)
    {
        _navigationService = navigationService;
        _shellService = shellService;
        IsBiometricAvaliable = Task.Run(() => BiometricService.IsBiometicAvaliable()).Result;
    }

    [RelayCommand]
    public async Task Login()
    {
        if (PasswordManager.CheckMasterPassword(MasterPassword))
        {
            EnterToApp();
        }
        else
        {
            if (Application.Current?.MainPage is Page mainPage)
            {
                await mainPage.DisplayAlert("Ошибка", "Пароль не совпадает", "OK");
            }

        }
    }

    [RelayCommand]
    public async Task BiometricLogin()
    {
        if (await BiometricService.CheckBiometric())
        {
            EnterToApp();
        }
        else 
        {
            if (Application.Current?.MainPage is Page mainPage)
            {
                await mainPage.DisplayAlert("Ошибка", "Пароль не совпадает", "OK");
            }
        }
    }

    private void EnterToApp()
    {
        _shellService.SwitchToAuthenticatedShell();
    }
}
