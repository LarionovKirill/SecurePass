using Plugin.Maui.ScreenSecurity;
using SecurePass.VM.ViewModels;

namespace SecurePass.Views;

public partial class SettingsPage : ContentPage
{
	public SettingsPage(SettingsPageVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ScreenSecurity.Default.ActivateScreenSecurityProtection();
    }

    protected override void OnDisappearing()
    {
        ScreenSecurity.Default.DeactivateScreenSecurityProtection();
        base.OnDisappearing();
    }
}