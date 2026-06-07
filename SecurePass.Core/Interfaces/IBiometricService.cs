namespace SecurePass.Core.Interfaces;

public interface IBiometricService
{
    Task<bool> IsBiometricAvailableAsync();
    Task<bool> AuthenticateAsync(string reason = "Подтвердите вашу личность для входа");
}