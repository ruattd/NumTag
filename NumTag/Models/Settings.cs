using System.Text.Json.Serialization;

namespace NumTag.Models;

[JsonSerializable(typeof(Settings))]
public record Settings(
    string Title = "Title",
    double TitleTextSize = 96,
    string Subtitle = "Subtitle",
    double SubtitleTextSize = 32,
    string Hint = "双击空白处关闭",
    double HintTextSize = 16,
    BrushOption? Foreground = null,
    BrushOption? HintForeground = null,
    BrushOption? Background = null
)
{
}
