using Plugin.Maui.ScreenSecurity;
using SecurePass.VM.ViewModels;

namespace SecurePass.Views;

public partial class StartPage : ContentPage
{
    private readonly StartPageVM _viewModel;

    public StartPage(StartPageVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
        _viewModel = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InitializeAsync();
        ScreenSecurity.Default.ActivateScreenSecurityProtection();
    }

    protected override void OnDisappearing()
    {
        ScreenSecurity.Default.DeactivateScreenSecurityProtection();
        base.OnDisappearing();
    }
}