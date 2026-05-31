namespace SecurePass.Core.Interfaces;

public interface IShellService
{
    void SwitchToAuthenticatedShell();
    void SwitchToUnauthenticatedShell();
}
