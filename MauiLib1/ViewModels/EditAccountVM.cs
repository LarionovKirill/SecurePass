using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SecurePass.Core.Interfaces;
using SecurePass.Core.Models;
using SecurePass.VM.Messages;

namespace SecurePass.VM.ViewModels;

public partial class EditAccountVM : ObservableObject, IQueryAttributable
{
    private readonly IPasswordGeneratorService _passwordGenerator;
    private readonly IProjectService _projectService;
    private Account _originalAccount;

    [ObservableProperty]
    private Account _account;

    [ObservableProperty]
    private bool _hasError;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    public EditAccountVM(
        IPasswordGeneratorService passwordGenerator,
        IProjectService projectService)
    {
        _passwordGenerator = passwordGenerator;
        _projectService = projectService;
    }

    public void Initialize(Account account)
    {
        _originalAccount = account;
        Account = new Account(account.Name, account.Login, account.Password)
        {
            Id = account.Id,
            Description = account.Description,
            WebsiteUrl = account.WebsiteUrl,
            CreatedAt = account.CreatedAt,
            UpdatedAt = account.UpdatedAt
        };
    }

    [RelayCommand]
    private async Task GeneratePassword()
    {
        var options = await GetPasswordOptions();
        Account.Password = _passwordGenerator.GeneratePassword(options);
    }

    [RelayCommand]
    private async Task Save()
    {
        if (string.IsNullOrWhiteSpace(Account.Name))
        {
            ShowError("Введите название");
            return;
        }

        if (string.IsNullOrWhiteSpace(Account.Login))
        {
            ShowError("Введите логин");
            return;
        }

        if (string.IsNullOrWhiteSpace(Account.Password))
        {
            ShowError("Введите пароль");
            return;
        }

        HasError = false;
        WeakReferenceMessenger.Default.Send(new UpdateAccountMessage(Account));
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task Delete()
    {
        if (Account == null) return;

        bool confirm = await Shell.Current.DisplayAlert("Удаление",
            $"Удалить аккаунт \"{Account.Name}\"?", "Да", "Нет");

        if (confirm)
        {
            WeakReferenceMessenger.Default.Send(new DeleteAccountMessage(Account.Id));
            await Shell.Current.GoToAsync("..");
        }
    }

    private async Task<PasswordGeneratorOptions> GetPasswordOptions()
    {
        var project = await _projectService.LoadProjectAsync();
        return project.PasswordGeneratorOptions ?? new PasswordGeneratorOptions();
    }

    private void ShowError(string message)
    {
        HasError = true;
        ErrorMessage = message;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("account", out var accountObj))
        {
            Initialize((Account)accountObj);
        }
    }
}