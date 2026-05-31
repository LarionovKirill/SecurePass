namespace SecurePass.Controls;

public partial class SwitchWithLabel : ContentView
{
    public static readonly BindableProperty TextPropery;
    public static readonly BindableProperty IsSwitchedProperty;

    static SwitchWithLabel()
    {
        TextPropery
            = BindableProperty.Create(
                nameof(Text),
                typeof(string),
                typeof(SwitchWithLabel),
                string.Empty);

        IsSwitchedProperty
            = BindableProperty.Create(
                nameof(IsSwitched),
                typeof(bool),
                typeof(SwitchWithLabel),
                false,
                BindingMode.TwoWay);
    }

    public SwitchWithLabel()
	{
		InitializeComponent();
        BindingContext = this;
	}

	public string Text 
	{
		get => (string)GetValue(TextPropery);
        set => SetValue(TextPropery, value);
    }

    public bool IsSwitched
    {
        get => (bool)GetValue(IsSwitchedProperty);
        set => SetValue(IsSwitchedProperty, value);
    }
}