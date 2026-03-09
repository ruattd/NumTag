using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace NumTag.Core.ViewModels;

public abstract partial class ViewModelBase : ObservableObject
{
    [ObservableProperty] private string _title = "Loading Title";
    [ObservableProperty] private double _titleTextSize = 96;
    [ObservableProperty] private string _subtitle = "Loading subtitle";
    [ObservableProperty] private double _subtitleTextSize = 32;
    [ObservableProperty] private string _hint = "Loading hint";
    [ObservableProperty] private double _hintTextSize = 16;
    [ObservableProperty] private Brush _foreground = new SolidColorBrush(Colors.White);
    [ObservableProperty] private Brush _hintForeground = new SolidColorBrush(Colors.LightGray);
    [ObservableProperty] private Brush _background = new SolidColorBrush(Colors.Black);
}
