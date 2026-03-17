using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using NumTag.Core.Models;

namespace NumTag.Core.States;

public partial class BehaviorState : ObservableObject
{
    private static readonly Brush DefaultForeground = new SolidColorBrush(Colors.White);
    private static readonly Brush DefaultHintForeground = new SolidColorBrush(Colors.LightGray);
    private static readonly Brush DefaultBackground = new SolidColorBrush(Colors.Black);

    [ObservableProperty] private string _title = "Loading Title";
    [ObservableProperty] private double _titleTextSize = 96;
    [ObservableProperty] private string _subtitle = "Loading subtitle";
    [ObservableProperty] private double _subtitleTextSize = 32;
    [ObservableProperty] private string _hint = "Loading hint";
    [ObservableProperty] private double _hintTextSize = 16;
    [ObservableProperty] private Brush _foreground = DefaultForeground;
    [ObservableProperty] private Brush _hintForeground = DefaultHintForeground;
    [ObservableProperty] private Brush _background = DefaultBackground;
    [ObservableProperty] private bool _startVisible = true;
    [ObservableProperty] private bool _doubleClickToHideWindow = true;
    [ObservableProperty] private bool _rightDownCornerToOpenSettings = true;
    [ObservableProperty] private bool _doubleShiftToOpenSettings = true;

    public BehaviorState(BehaviorSettings? settings = null)
    {
        if (settings != null) LoadSettings(settings);
    }

    public void LoadSettings(BehaviorSettings settings)
    {
        Title = settings.Title;
        TitleTextSize = settings.TitleTextSize;
        Subtitle = settings.Subtitle;
        SubtitleTextSize = settings.SubtitleTextSize;
        Hint = settings.Hint;
        HintTextSize = settings.HintTextSize;
        StartVisible = settings.StartVisible;
        DoubleClickToHideWindow = settings.DoubleClickToHideWindow;
        RightDownCornerToOpenSettings = settings.RightDownCornerToOpenSettings;
        DoubleShiftToOpenSettings = settings.DoubleShiftToOpenSettings;
        if (settings.Foreground == null) Foreground = DefaultForeground;
        else Foreground = settings.Foreground;
        if (settings.HintForeground == null) HintForeground = DefaultHintForeground;
        else HintForeground = settings.HintForeground;
        if (settings.Background == null) Background = DefaultBackground;
        else Background = settings.Background;
    }

    public BehaviorSettings ToSettings()
    {
        BrushOption? foreground = null;
        if (Foreground != DefaultForeground) foreground = Foreground;
        BrushOption? hintForeground = null;
        if (HintForeground != DefaultHintForeground) hintForeground = HintForeground;
        BrushOption? background = null;
        if (Background != DefaultBackground) background = Background;

        return new BehaviorSettings
        {
            Title = Title,
            TitleTextSize = TitleTextSize,
            Subtitle = Subtitle,
            SubtitleTextSize = SubtitleTextSize,
            Hint = Hint,
            HintTextSize = HintTextSize,
            StartVisible = StartVisible,
            DoubleClickToHideWindow = DoubleClickToHideWindow,
            RightDownCornerToOpenSettings = RightDownCornerToOpenSettings,
            DoubleShiftToOpenSettings = DoubleShiftToOpenSettings,
            Foreground = foreground,
            HintForeground = hintForeground,
            Background = background,
        };
    }
}
