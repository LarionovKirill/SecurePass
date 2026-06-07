using Plugin.Maui.ScreenSecurity;
using SecurePass.VM.ViewModels;

namespace SecurePass.Views;

public partial class CreateAccountPage : ContentPage
{
	public CreateAccountPage(CreateAccountVM vm)
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

    protected override bool OnBackButtonPressed()
    {
        // Получаем ViewModel из контекста Binding
        if (BindingContext is CreateAccountVM viewModel)
        {
            // Проверяем, есть ли несохраненные изменения
            if (viewModel.HasChanged())
            {
                // Показываем диалог асинхронно
                Dispatcher.Dispatch(async () =>
                {
                    bool confirm = await DisplayAlert(
                        "Несохраненные изменения",
                        "У вас есть несохраненные данные. Выйти без сохранения?",
                        "Выйти",
                        "Отмена");

                    if (confirm)
                    {
                        // Пользователь подтвердил выход
                        await Shell.Current.GoToAsync("..");
                    }
                });

                // Говорим системе, что мы обработали нажатие сами
                return true;
            }

            // Нет изменений - можно выходить
            return false; // false позволяет системе выполнить стандартный выход
        }

        // Если ViewModel не найдена, пусть сработает стандартное поведение
        return base.OnBackButtonPressed();
    }
}