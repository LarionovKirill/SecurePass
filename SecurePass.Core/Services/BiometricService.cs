using Maui.Biometric;
using Maui.Biometric.Abstractions;
using SecurePass.Core.Interfaces;

namespace SecurePass.Core.Services;

public class BiometricService : IBiometricService
{
    public async Task<bool> IsBiometricAvailableAsync()
    {
        try
        {
            var type = await BiometricAuthentication.Current.GetAuthenticationTypeAsync();
            return type is AuthenticationType.Fingerprint or AuthenticationType.Face;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> AuthenticateAsync(string reason = "Подтвердите вашу личность для входа")
    {
        try
        {
            var result = await BiometricAuthentication.Current.AuthenticateAsync(
                new AuthenticationRequest("Авторизация", reason));
            return result.Authenticated;
        }
        catch
        {
            return false;
        }
    }
}