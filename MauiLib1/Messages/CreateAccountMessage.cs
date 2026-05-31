using SecurePass.Core.Models;

namespace SecurePass.VM.Messages;

/// <summary>
/// Сообщение о создании нового аккаунта.
/// </summary>
public class CreateAccountMessage
{
    /// <summary>
    /// Создает объект класса <see cref="CreateAccountMessage"/>.
    /// </summary>
    /// <param name="newAccount">Новый аккаунт.</param>
    public CreateAccountMessage(Account newAccount)
    {
        NewAccount = newAccount;
    }


    /// <summary>
    /// Новый аккаунт.
    /// </summary>
    public Account NewAccount { get; private set; }
}
