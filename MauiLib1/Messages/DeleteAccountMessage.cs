namespace SecurePass.VM.Messages;

public class DeleteAccountMessage
{
    public DeleteAccountMessage(string deletedAccountId)
    {
        DeletedAccountId = deletedAccountId;
    }

    public string DeletedAccountId { get; }
}