using Avalonia.Controls;
using NumTag.Controls;
using NumTag.ViewModels;

namespace NumTag.Views;

public partial class MainWindow : BaseWindow
{
    public MainWindowViewModel ViewModel { get; }
    public MainWindow()
    {
        // create view model
        var vm = new MainWindowViewModel();
        DataContext = ViewModel = vm;

        // register window events
        Initialized += (_, _) => vm.OnInitialized();
        PointerReleased += (_, _) => vm.OnClickBackground();

        // load component
        InitializeComponent();
    }

    private void Window_OnClosing(object? sender, WindowClosingEventArgs e)
    {
        e.Cancel = true;
    }
}
