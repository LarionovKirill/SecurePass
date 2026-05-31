using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SecurePass.Core.Interfaces;
using SecurePass.Core.Models;
using SecurePass.Core.Services;
using SecurePass.VM.Messages;
using System.Collections.ObjectModel;

namespace SecurePass.VM.ViewModels;

/// <summary>
/// Модель представления главной страницы.
/// </summary>
public partial class MainVM : ObservableObject
{
    private readonly ProjectManager _projectManager;

    private AccountVM _selectedAccount;

    public MainVM(
        INavigationService navigationService,
        IAppInfoService appService)
    {
        _projectManager = Task.Run(ProjectService.LoadProjectAsync).Result;
        UpdateProjectInfo(_projectManager);
        WeakReferenceMessenger.Default.Register<CreateAccountMessage>(this, AddAccount);
    }

    public AccountVM SelectedAccount
    {
        get => _selectedAccount;
    }

    /// <summary>
    /// Список аккаунтов.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<AccountVM> _accounts;

    private void AddAccount(object reciever, CreateAccountMessage message)
    {
        Accounts.Add(new AccountVM(message.NewAccount));
    }

    private void UpdateProjectInfo(ProjectManager projectManager)
    {
        Accounts = new ObservableCollection<AccountVM>();
        foreach (var acc in projectManager.Accounts)
        {
            Accounts.Add(new AccountVM(acc));
        }

    }

    [RelayCommand]
    private async Task GetApps()
    {
        await Shell.Current.GoToAsync("CreateAccountPage");
    }

    [RelayCommand]
    private async Task Test()
    {
        var a = 0;
    }
}
