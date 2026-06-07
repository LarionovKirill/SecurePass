using SecurePass.Core.Interfaces;

namespace SecurePass.Services;

public class DialogService : IDialogService
{
    // ИСПРАВЛЕНИЕ 1: Внедряем менеджер паролей напрямую, без ручного IServiceProvider
    private readonly IPasswordManager _passwordManager;

    public DialogService(IPasswordManager passwordManager)
    {
        _passwordManager = passwordManager;
    }

    public async Task<bool> ShowCreateMasterPasswordDialogAsync()
    {
        var mainPage = Application.Current?.MainPage;
        if (mainPage == null) return false;

        // Первый диалог — ввод пароля
        var password = await mainPage.DisplayPromptAsync(
            "Создание мастер-пароля",
            "Придумайте мастер-пароль для защиты ваших данных:",
            "Далее",
            "Отмена",
            placeholder: "Введите пароль",
            maxLength: 50,
            keyboard: Keyboard.Text);

        if (string.IsNullOrWhiteSpace(password))
            return false;

        if (password.Length < 4)
        {
            await mainPage.DisplayAlert("Ошибка", "Пароль должен содержать минимум 4 символа", "OK");
            return false;
        }

        // Второй диалог — подтверждение
        var confirmPassword = await mainPage.DisplayPromptAsync(
            "Подтверждение пароля",
            "Повторите мастер-пароль:",
            "Создать",
            "Отмена",
            placeholder: "Повторите пароль",
            maxLength: 50,
            keyboard: Keyboard.Text);

        if (password != confirmPassword)
        {
            await mainPage.DisplayAlert("Ошибка", "Пароли не совпадают", "OK");
            return false;
        }

        // Вызываем сохранение и создание зашифрованной структуры проекта
        var success = await _passwordManager.SetMasterPasswordAsync(password);

        if (success)
        {
            try
            {
                await SecureStorage.Default.SetAsync("secure_master_password", password);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Критическая ошибка SecureStorage: {ex.Message}");
            }

            await mainPage.DisplayAlert("Успех", "Мастер-пароль создан", "OK");
            return true;
        }
        else
        {
            await mainPage.DisplayAlert("Ошибка", "Не удалось сохранить мастер-пароль", "OK");
            return false;
        }
    }

    public async Task ShowAlertAsync(string title, string message)
    {
        var mainPage = Application.Current?.MainPage;
        if (mainPage != null)
        {
            await mainPage.DisplayAlert(title, message, "OK");
        }
    }

    public async Task<bool> ShowConfirmationAsync(string title, string message)
    {
        var mainPage = Application.Current?.MainPage;
        if (mainPage != null)
        {
            return await mainPage.DisplayAlert(title, message, "Да", "Нет");
        }
        return false;
    }
}
