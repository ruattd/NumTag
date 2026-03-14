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

        // register events
        ViewModel.CloseWindow += Close;
        KeyUp += (_, e) => ViewModel.OnKeyUp(this, e);

        // load component
        InitializeComponent();

        // set basic props
        if (owner != null)
        {
            Owner = owner;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }
    }
}
