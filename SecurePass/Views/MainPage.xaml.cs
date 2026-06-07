using Plugin.Maui.ScreenSecurity;
using SecurePass.VM.ViewModels;

namespace SecurePass.Views;

public partial class MainPage : ContentPage
{
	public MainPage(MainVM vm)
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