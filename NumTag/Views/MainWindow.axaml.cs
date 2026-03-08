using NumTag.Controls;
using NumTag.ViewModels;

namespace NumTag.Views;

public partial class MainWindow : BaseWindow
{
    public MainWindow()
    {
        // create view model
        var vm = new MainWindowViewModel();
        DataContext = vm;

        // register window events
        Initialized += (_, _) => vm.OnInitialized();
        PointerReleased += (_, _) => vm.OnClickBackground();

        // load component
        InitializeComponent();
    }
}
