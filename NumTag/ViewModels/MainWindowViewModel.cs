using Avalonia.Controls;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NumTag.Core.ViewModels;
using NumTag.Views;

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

    private long _lastMouseClickTick = 0;

    internal void OnClickBackground()
    {
        var thisClickTick = Environment.TickCount64;
        // double click
        if (thisClickTick - _lastMouseClickTick < DoubleClickMaxLatency) OnDoubleClickClose();
        // update last click
        _lastMouseClickTick = thisClickTick;
    }

    private void OnDoubleClickClose()
    {
        // TODO check config: double click to close window
        WindowVisible = false;
    }

    private long _lastShiftClickTick = 0;

    internal void OnKeyUp(Window sender, KeyEventArgs e)
    {
        if (e.Key is Key.LeftShift or Key.RightShift)
        {
            var thisClickTick = Environment.TickCount64;
            if (thisClickTick - _lastShiftClickTick < DoubleClickMaxLatency) OpenSettings();
            _lastShiftClickTick = thisClickTick;
        }
    }

    [RelayCommand]
    public void OpenSettings(WindowBase? owner = null)
    {
        _ = new SettingsWindow(new SettingsWindowViewModel { Behavior = Behavior }, owner)
        {
            Topmost = true,
            WindowVisible = true
        };
    }
}
