using CommunityToolkit.Mvvm.ComponentModel;
using NumTag.Core.ViewModels;

namespace NumTag.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private const long DoubleClickMaxLatency = 300;

    [ObservableProperty] private bool _windowVisible = false;

    internal void OnInitialized()
    {
        // TODO check config: start visible
        WindowVisible = true;
    }

    private long _lastClickTick = 0;

    internal void OnClickBackground()
    {
        var thisClickTick = Environment.TickCount64;
        if (thisClickTick - _lastClickTick < DoubleClickMaxLatency)
        {
            // double click
            OnDoubleClickClose();
        }
        // update last click
        _lastClickTick = thisClickTick;
    }

    private void OnDoubleClickClose()
    {
        // TODO check config: double click to close window
        WindowVisible = false;
    }
}
