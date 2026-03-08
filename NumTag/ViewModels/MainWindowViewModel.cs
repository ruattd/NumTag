using System;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using Xaml.Behaviors.SourceGenerators;

namespace NumTag.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private const long DoubleClickMaxLatency = 500;

    [ObservableProperty] private string _title = "Loading Title";
    [ObservableProperty] private double _titleTextSize = 24;
    [ObservableProperty] private string _subtitle = "Loading subtitle";
    [ObservableProperty] private double _subtitleTextSize = 16;
    [ObservableProperty] private string _hint = "双击空白处关闭";
    [ObservableProperty] private double _hintTextSize = 12;
    [ObservableProperty] private Brush _foreground = new SolidColorBrush(Colors.White);
    [ObservableProperty] private Brush _background = new SolidColorBrush(Colors.Black);
    [ObservableProperty] private bool _windowVisible = false;

    [GenerateTypedAction]
    internal void OnInitialized()
    {
        // TODO check config: start visible
        WindowVisible = true;
    }

    private long _lastClickTick = 0;

    [GenerateTypedAction]
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
