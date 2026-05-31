using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SecurePass.Core.Models;
using SecurePass.Core.Services;
using SecurePass.VM.Messages;

namespace SecurePass.VM.ViewModels;

public partial class CreateAccountVM : ObservableObject
{
    public CreateAccountVM() 
    {
    }

    [ObservableProperty]
    private string _appName;

    [ObservableProperty]
    private string _login;

    [ObservableProperty]
    private string _password;

    [ObservableProperty]
    private string _description;

    [RelayCommand]
    private void GeneratePassword()
    {
        var generator = new PasswordGeneratorService();
        var options = new PasswordGeneratorOptions()
        {
            Length = 10,
            UseCapitalLetters = true,
            UseDigits = true,
            UseSpecialCharacters = true,
            UseLowercaseLetters = true,
        };

        Password = generator.GeneratePassword(options);
    }

    [RelayCommand]
    private async Task CreateAccount()
    {
        var newAccount = new Account(AppName, Login, Password)
        {
            Description = Description,
        };
        WeakReferenceMessenger.Default.Send(new CreateAccountMessage(newAccount));
        await Shell.Current.GoToAsync("..");
    }

    public bool HasChanged()
    {
        return (
            !string.IsNullOrEmpty(Login) || 
            !string.IsNullOrEmpty(Password) || 
            !string.IsNullOrEmpty(Description)||
            !string.IsNullOrEmpty(AppName));
    }

    [RelayCommand]
    private async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }
}
