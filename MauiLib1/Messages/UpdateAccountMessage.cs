using SecurePass.Core.Models;

namespace SecurePass.VM.Messages;

public class UpdateAccountMessage
{
    public UpdateAccountMessage(Account updatedAccount)
    {
        UpdatedAccount = updatedAccount;
    }

    public Account UpdatedAccount { get; }
}