using SecurePass.Core.Models;

namespace SecurePass.Core.Services;

public static class PasswordManager
{
    public static bool CheckMasterPassword(string enteredPassword)
    {
        return enteredPassword == FakePassword.Password;
        
    }
}
