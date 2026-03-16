using Avalonia.Controls;
using NumTag.Core.Controls;
using NumTag.ViewModels;

namespace NumTag.Views;

public partial class MainWindow : BaseWindow
{
    public MainWindowViewModel ViewModel { get; }

    public MainWindow(MainWindowViewModel vm)
    {
        // apply view model
        DataContext = ViewModel = vm;

        // register window events
        Initialized += (_, _) => vm.OnInitialized();
        PointerReleased += (_, _) => vm.OnClickBackground();
        KeyUp += (_, e) => vm.OnKeyUp(this, e);

        // load component
        InitializeComponent();
    }

    private void Window_OnClosing(object? sender, WindowClosingEventArgs e)
    {
        e.Cancel = true;
    }
}
