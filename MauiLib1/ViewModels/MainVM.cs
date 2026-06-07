using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SecurePass.Core.Services;
using SecurePass.VM.Messages;
using System.Collections.ObjectModel;

namespace SecurePass.VM.ViewModels;

public partial class MainVM : ObservableObject
{
    private readonly ProjectStateManager _projectStateManager;

    [ObservableProperty]
    private ObservableCollection<AccountVM> _accounts = new();

    [ObservableProperty]
    private ObservableCollection<AccountVM> _filteredAccounts = new();

    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private AccountVM? _selectedAccount;

    public MainVM(ProjectStateManager projectStateManager)
    {
        _projectStateManager = projectStateManager;

        WeakReferenceMessenger.Default.Register<CreateAccountMessage>(this, OnAccountCreated);
        WeakReferenceMessenger.Default.Register<UpdateAccountMessage>(this, OnAccountUpdated);
        WeakReferenceMessenger.Default.Register<DeleteAccountMessage>(this, OnAccountDeleted);

        PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(SearchText))
                ApplyFilter();
        };
    }

    /// <summary>
    /// Команда, которая будет автоматически вызываться при открытии главного экрана
    /// </summary>
    [RelayCommand]
    public void OnAppearing()
    {
        LoadAccounts();
    }

    private async void LoadAccounts()
    {
        try
        {
            // Если проект по какой-то причине еще не подгружен в этот экземпляр стейт-менеджера,
            // но авторизация уже успешно пройдена (мастер-пароль в памяти ядра есть) — 
            // принудительно пинаем менеджер состояний загрузить данные.
            if (!_projectStateManager.IsLoaded)
            {
                // Этот вызов обратится к ProjectService, который возьмет уже готовый
                // в оперативной памяти AES-ключ, расшифрует файл и заполнит стейт.
                await _projectStateManager.LoadProjectAsync();
            }

            // Теперь, когда мы гарантировали загрузку, спокойно наполняем UI-коллекцию
            if (_projectStateManager.IsLoaded && _projectStateManager.CurrentProject?.Accounts != null)
            {
                Accounts.Clear();
                foreach (var acc in _projectStateManager.CurrentProject.Accounts)
                {
                    Accounts.Add(new AccountVM(acc));
                }
                ApplyFilter();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[MainVM] Критическая ошибка при наполнении списка: {ex.Message}");
        }
    }


    private void ApplyFilter()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            FilteredAccounts = new ObservableCollection<AccountVM>(Accounts);
        }
        else
        {
            var filtered = Accounts.Where(a =>
                a.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                a.Login.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            FilteredAccounts = new ObservableCollection<AccountVM>(filtered);
        }
    }

    [RelayCommand]
    private async Task EditAccount()
    {
        if (SelectedAccount == null) return;

        var navigationParams = new Dictionary<string, object>
        {
            { "account", SelectedAccount.GetAccount() }
        };
        await Shell.Current.GoToAsync("EditAccountPage", navigationParams);

        SelectedAccount = null;
    }

    private void OnAccountCreated(object recipient, CreateAccountMessage message)
    {
        _projectStateManager.AddAccount(message.NewAccount);
        Accounts.Add(new AccountVM(message.NewAccount));
        ApplyFilter();
    }

    private void OnAccountUpdated(object recipient, UpdateAccountMessage message)
    {
        if (_projectStateManager.UpdateAccount(message.UpdatedAccount))
        {
            var existing = _projectStateManager.CurrentProject.Accounts
                .FirstOrDefault(a => a.Id == message.UpdatedAccount.Id);

            if (existing != null)
            {
                var index = Accounts.ToList().FindIndex(vm => vm.GetAccount().Id == message.UpdatedAccount.Id);
                if (index >= 0)
                {
                    Accounts[index] = new AccountVM(existing);
                }
                ApplyFilter();
            }
        }
    }

    private void OnAccountDeleted(object recipient, DeleteAccountMessage message)
    {
        if (_projectStateManager.RemoveAccount(message.DeletedAccountId))
        {
            var vmToRemove = Accounts.FirstOrDefault(vm => vm.GetAccount().Id == message.DeletedAccountId);
            if (vmToRemove != null)
            {
                Accounts.Remove(vmToRemove);
            }
            ApplyFilter();
        }
    }

    [RelayCommand]
    private async Task AddAccount()
    {
        await Shell.Current.GoToAsync("CreateAccountPage");
    }
}
