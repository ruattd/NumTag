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
        if (Behavior.StartVisible) WindowVisible = true;
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
        if (Behavior.DoubleClickToHideWindow) WindowVisible = false;
    }

    private long _lastShiftClickTick = 0;

    internal void OnKeyUp(Window sender, KeyEventArgs e)
    {
        if (e.Key is Key.LeftShift or Key.RightShift)
        {
            var thisClickTick = Environment.TickCount64;
            if (thisClickTick - _lastShiftClickTick < DoubleClickMaxLatency && Behavior.DoubleShiftToOpenSettings)
                OpenSettings(sender);
            _lastShiftClickTick = thisClickTick;
        }
    }

    private SettingsWindow? _currentSettingsWindow = null;

    [RelayCommand]
    private void OpenSettings(WindowBase owner)
    {
        if (_currentSettingsWindow != null)
        {
            _currentSettingsWindow.Activate();
            return;
        }
        _currentSettingsWindow = new SettingsWindow(new SettingsWindowViewModel { Behavior = Behavior }, owner)
        {
            // Topmost = true,
            WindowVisible = true,
        };
        _currentSettingsWindow.Closed += (_, _) => _currentSettingsWindow = null;
    }
}
