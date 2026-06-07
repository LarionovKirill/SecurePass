using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SecurePass.Core.Interfaces;
using SecurePass.Core.Models;
using SecurePass.Core.Services;
using SecurePass.VM.Messages;

namespace SecurePass.VM.ViewModels;

public partial class CreateAccountVM : ObservableObject
{
    private readonly IPasswordGeneratorService _passwordGenerator;
    private readonly ProjectManager _projectManager;

    [ObservableProperty]
    private string _appName = string.Empty;

    [ObservableProperty]
    private string _login = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private string _description = string.Empty;

    [ObservableProperty]
    private bool _hasError;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    public CreateAccountVM(
        IPasswordGeneratorService passwordGenerator,
        ProjectStateManager projectStateManager)
    {
        _passwordGenerator = passwordGenerator;
        _projectManager = projectStateManager.CurrentProject;
    }

    [RelayCommand]
    private async Task GeneratePassword()
    {
        Password = _passwordGenerator.GeneratePassword(_projectManager.PasswordGeneratorOptions);
    }

    [RelayCommand]
    private async Task CreateAccount()
    {
        if (string.IsNullOrWhiteSpace(AppName))
        {
            ShowError("Введите название сайта или приложения");
            return;
        }

        if (string.IsNullOrWhiteSpace(Login))
        {
            ShowError("Введите логин или email");
            return;
        }

        if (string.IsNullOrWhiteSpace(Password))
        {
            ShowError("Введите пароль");
            return;
        }

        HasError = false;
        ErrorMessage = string.Empty;

        var newAccount = new Account(AppName.Trim(), Login.Trim(), Password)
        {
            Description = Description?.Trim() ?? string.Empty
        };

        WeakReferenceMessenger.Default.Send(new CreateAccountMessage(newAccount));
        await Shell.Current.GoToAsync("..");
    }

    private void ShowError(string message)
    {
        HasError = true;
        ErrorMessage = message;
    }

    public bool HasChanged()
    {
        return !string.IsNullOrWhiteSpace(Login) ||
               !string.IsNullOrWhiteSpace(Password) ||
               !string.IsNullOrWhiteSpace(Description) ||
               !string.IsNullOrWhiteSpace(AppName);
    }


    [RelayCommand]
    private async Task PickApp()
    {
        await Shell.Current.DisplayAlert("Выбор приложения",
            "Эта функция будет доступна в следующей версии", "OK");
    }
}