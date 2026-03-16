using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Metadata;
using NumTag.Core.Extensions;

namespace NumTag.Core.Controls;

public class IconButton : ContentControl
{
    public PathIcon InnerIcon { get; }

    public Button InnerButton { get; }

    public static readonly StyledProperty<Geometry?> IconPathProperty =
        AvaloniaProperty.Register<IconButton, Geometry?>(nameof(IconPath));

    [Content]
    public Geometry? IconPath
    {
        get => GetValue(IconPathProperty);
        set => SetValue(IconPathProperty, value);
    }

    public static readonly StyledProperty<double> IconSizeProperty =
        AvaloniaProperty.Register<IconButton, double>(nameof(IconSize), 10);

    public double IconSize
    {
        get => GetValue(IconSizeProperty);
        set => SetValue(IconSizeProperty, value);
    }

    public static readonly StyledProperty<Thickness> IconPaddingProperty =
        AvaloniaProperty.Register<IconButton, Thickness>(nameof(IconPadding), new Thickness(5));

    public Thickness IconPadding
    {
        get => GetValue(IconPaddingProperty);
        set => SetValue(IconPaddingProperty, value);
    }

    public static readonly StyledProperty<ICommand?> CommandProperty =
        AvaloniaProperty.Register<IconButton, ICommand?>(nameof(Command));

    public ICommand? Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly StyledProperty<object?> CommandParameterProperty =
        AvaloniaProperty.Register<IconButton, object?>(nameof(CommandParameter));

    public object? CommandParameter
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public IconButton()
    {
        Content = InnerButton = new Button {
            Content = InnerIcon = new PathIcon().Also(it => {
                it.Bind(PathIcon.DataProperty, this.GetObservable(IconPathProperty));
                it.Bind(WidthProperty, this.GetObservable(IconSizeProperty));
                it.Bind(HeightProperty, this.GetObservable(IconSizeProperty));
                it.Bind(ForegroundProperty, this.GetObservable(ForegroundProperty));
            }),
            Background = Brushes.Transparent,
        }.Also(it => {
            it.Bind(Button.CommandProperty, this.GetObservable(CommandProperty));
            it.Bind(Button.CommandParameterProperty, this.GetObservable(CommandParameterProperty));
            it.Bind(PaddingProperty, this.GetObservable(IconPaddingProperty));
            it.Bind(CornerRadiusProperty, this.GetObservable(CornerRadiusProperty));
        });
    }

    static IconButton()
    {
        CornerRadiusProperty.OverrideDefaultValue<IconButton>(new CornerRadius(65536));
    }
}
