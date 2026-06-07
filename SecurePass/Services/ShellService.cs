using SecurePass.Core.Interfaces;
using SecurePass.Shells;

namespace SecurePass.Services;

public class ShellService : IShellService
{
    public void SwitchToAuthenticatedShell()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Application.Current!.MainPage = new AuthenticatedAppShell();
        });
    }

    public void SwitchToUnauthenticatedShell()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Application.Current!.MainPage = new AppShell();
        });
    }
}