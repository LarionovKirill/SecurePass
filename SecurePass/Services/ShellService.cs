using SecurePass.Core.Interfaces;
using SecurePass.Shells;

namespace SecurePass.Services;

class ShellService : IShellService
{
    public void SwitchToAuthenticatedShell()
    {
        Application.Current!.MainPage = new AuthenticatedAppShell();
    }

    public void SwitchToUnauthenticatedShell()
    {
        Application.Current!.MainPage = new AppShell();
    }
}
