using CommunityToolkit.Mvvm.ComponentModel;
using SecurePass.Core.Models;

namespace SecurePass.VM.ViewModels;

/// <summary>
/// Модель представления учетной записи.
/// </summary>
public class AccountVM : ObservableObject
{
    /// <summary>
    /// Учетная запись.
    /// </summary>
    private readonly Account _account;

    /// <summary>
    /// Создает объект класса <see cref="AccountVM"/>.
    /// </summary>
    /// <param name="account">Учетная запись.</param>
    public AccountVM(Account account)
    {
        _account = account;
    }

    public string Name => _account.Name;

    public override string ToString()
    {
        return _account.Name;
    }
}
