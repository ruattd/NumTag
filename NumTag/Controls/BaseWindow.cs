using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;

namespace NumTag.Controls;

public class BaseWindow : Window
{
    public static readonly StyledProperty<bool> WindowVisibleProperty
        = AvaloniaProperty.Register<BaseWindow, bool>(nameof(WindowVisible), defaultBindingMode: BindingMode.TwoWay);

    public bool WindowVisible
    {
        get => GetValue(WindowVisibleProperty);
        set => SetValue(WindowVisibleProperty, value);
    }

    private static void WindowVisibleChanged(BaseWindow sender, AvaloniaPropertyChangedEventArgs<bool> e)
    {
        if (e.NewValue.Value) sender.Show();
        else sender.Hide();
    }

    static BaseWindow()
    {
        WindowVisibleProperty.Changed.AddClassHandler<BaseWindow, bool>(WindowVisibleChanged);
    }

    public void FlipVisibility()
    {
        WindowVisible = !WindowVisible;
    }
}
