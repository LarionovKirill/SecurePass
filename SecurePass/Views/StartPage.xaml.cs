using SecurePass.VM.ViewModels;

namespace SecurePass.Views;

public partial class StartPage : ContentPage
{
	public StartPage(StartPageVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}