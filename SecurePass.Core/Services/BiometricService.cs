using Maui.Biometric;
using Maui.Biometric.Abstractions;

namespace SecurePass.Core.Services;

public class BiometricService
{
    /// <summary>
    /// Проверяет, что на устройстве можно использовать биометрию.
    /// </summary>
    /// <returns>True, если имеется поддержка биометрии.</returns>
    public static async Task<bool> IsBiometicAvaliable()
    {
        var type = await BiometricAuthentication.Current.GetAuthenticationTypeAsync();
        if (type is AuthenticationType.Fingerprint or AuthenticationType.Face)
        {
            return true;
        }

        return false;
/*        await BiometricAuthentication.Current.IsAvailableAsync();*/
    }

    /// <summary>
    /// Проверяет биометрию пользователя.
    /// </summary>
    /// <returns>True, если биометрия совпадает, иначе false.</returns>
    public static async Task<bool> CheckBiometric()
    {
        var result = await BiometricAuthentication.Current.AuthenticateAsync(new AuthenticationRequest
            (
            "Авторизация",
            "Подтвердите вашу личность для входа"));

        if (result.Authenticated)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
