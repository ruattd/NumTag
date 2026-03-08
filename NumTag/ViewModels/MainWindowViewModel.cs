using System;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace NumTag.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private const long DoubleClickMaxLatency = 300;

    [ObservableProperty] private string _title = "Loading Title";
    [ObservableProperty] private double _titleTextSize = 96;
    [ObservableProperty] private string _subtitle = "Loading subtitle";
    [ObservableProperty] private double _subtitleTextSize = 32;
    [ObservableProperty] private string _hint = "Put the hint on this text area.";
    [ObservableProperty] private double _hintTextSize = 16;
    [ObservableProperty] private Brush _foreground = new SolidColorBrush(Colors.White);
    [ObservableProperty] private Brush _hintForeground = new SolidColorBrush(Colors.LightGray);
    [ObservableProperty] private Brush _background = new SolidColorBrush(Colors.Black);
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
