using NumTag.Controls;
using NumTag.ViewModels;

namespace NumTag.Views;

public partial class MainWindow : BaseWindow
{
    public MainWindow()
    {
        DataContext = new MainWindowViewModel();
        InitializeComponent();
    }
}
