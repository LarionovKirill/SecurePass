using SecurePass.Views;

namespace SecurePass.Shells;

public partial class AuthenticatedAppShell : Shell
{
	public AuthenticatedAppShell()
	{
		InitializeComponent();
        Routing.RegisterRoute(nameof(CreateAccountPage), typeof(CreateAccountPage));
        Routing.RegisterRoute(nameof(EditAccountPage), typeof(EditAccountPage));
    }
}