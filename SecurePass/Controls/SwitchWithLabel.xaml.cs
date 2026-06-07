namespace SecurePass.Controls;

public partial class SwitchWithLabel : ContentView
{
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(SwitchWithLabel), string.Empty);

    public static readonly BindableProperty IsSwitchedProperty =
        BindableProperty.Create(nameof(IsSwitched), typeof(bool), typeof(SwitchWithLabel), false, BindingMode.TwoWay);

    public SwitchWithLabel()
    {
        InitializeComponent();
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public bool IsSwitched
    {
        get => (bool)GetValue(IsSwitchedProperty);
        set => SetValue(IsSwitchedProperty, value);
    }
}