namespace SecurePass.Controls;

/// <summary>
/// Контрол с возможностью скрытия пароля.
/// </summary>
public partial class PasswordEntry : ContentView
{
    /// <summary>
    /// Свойтсво зависимости текста контрола.
    /// </summary>
    public static readonly BindableProperty TextProperty
        = BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(PasswordEntry),
            string.Empty,
            defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Свойтсво зависимости видимости текста.
    /// </summary>
    public static readonly BindableProperty IsPasswordTrueProperty
        = BindableProperty.Create(
            nameof(IsPasswordTrue),
            typeof(bool),
            typeof(PasswordEntry),
            true,
            defaultBindingMode: BindingMode.OneWay);

    /// <summary>
    /// Возвращает и задает текст на контроле.
    /// </summary>
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <summary>
    /// Возвращает и задает видимость пароля.
    /// </summary>
    public bool IsPasswordTrue
    {
        get => (bool)GetValue(IsPasswordTrueProperty);
        set => SetValue(IsPasswordTrueProperty, value);
    }

    /// <summary>
    /// Создает объект класса <see cref="PasswordEntry"/>.
    /// </summary>
    public PasswordEntry()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Обработчик нажатия на кнопку видимости.
    /// </summary>
    private void TogglePasswordButton_Clicked(object sender, EventArgs e)
    {
        IsPasswordTrue = !IsPasswordTrue;
        if (IsPasswordTrue)
        {
            TogglePasswordButton.Source = "eye_icon_128x128.png";
        }
        else
        {
            TogglePasswordButton.Source = "hide_eye_icon_128x128.png";
        }
    }
}
