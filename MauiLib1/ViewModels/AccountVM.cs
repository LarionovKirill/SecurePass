using CommunityToolkit.Mvvm.ComponentModel;
using SecurePass.Core.Models;

namespace SecurePass.VM.ViewModels;

public partial class AccountVM : ObservableObject
{
    private readonly Account _account;

    public AccountVM(Account account)
    {
        _account = account;
    }

    public string Name => _account.Name;
    public string Login => _account.Login;
    public string Password => _account.Password;
    public string Description => _account.Description;
    public string Id => _account.Id;

    public Account GetAccount() => _account;

    public string IconSource => "key_icon_128x128.png";

    public override string ToString() => Name;
}