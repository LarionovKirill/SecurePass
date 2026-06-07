namespace SecurePass.Core.Interfaces;

public interface IPasswordManager
{
    Task<bool> CheckMasterPasswordAsync(string enteredPassword);
    Task<bool> SetMasterPasswordAsync(string newPassword);
    Task<bool> IsMasterPasswordSetAsync();
    void ClearMasterPassword();
}