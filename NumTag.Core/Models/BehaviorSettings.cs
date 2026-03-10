namespace NumTag.Core.Models;

public record BehaviorSettings(
    string Title = "Title",
    double TitleTextSize = 96,
    string Subtitle = "Subtitle",
    double SubtitleTextSize = 32,
    string Hint = "双击空白处关闭",
    double HintTextSize = 16,
    BrushOption? Foreground = null,
    BrushOption? HintForeground = null,
    BrushOption? Background = null,
    bool StartVisible = true,
    bool DoubleClickToHideWindow = true
);
