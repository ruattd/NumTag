using Avalonia.Controls;
using NumTag.Controls;
using NumTag.ViewModels;

namespace NumTag.Views;

public partial class SettingsWindow : BaseWindow
{
    public SettingsWindowViewModel ViewModel { get; }

    public SettingsWindow(SettingsWindowViewModel vm, WindowBase? owner = null)
    {
        // apply view model
        DataContext = ViewModel = vm;

        // set basic props
        if (owner != null) Owner = owner;

        // load component
        InitializeComponent();
    }
}
