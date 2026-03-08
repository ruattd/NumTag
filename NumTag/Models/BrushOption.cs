using System;
using Avalonia.Media;

namespace NumTag.Models;

public enum BrushKind
{
    SolidColor,
    GradientColor,
}

public interface IBrushOptionParser
{
    public Brush Decode(string content);
    public string Encode(Brush brush);
}

public record BrushOption
{
    public required BrushKind Kind { get; init; }
    public required string Content { get; init; }

    private static IBrushOptionParser GetParser(BrushKind kind) => kind switch
    {
        _ => throw new NotSupportedException($"Unsupported brush kind: {kind}"),
    };

    private static BrushKind GetKindFromBrush(Brush brush)
    {
        BrushKind kind;
        if (brush is SolidColorBrush) kind = BrushKind.SolidColor;
        else if (brush is GradientBrush) kind = BrushKind.GradientColor;
        else throw new NotSupportedException($"Unsupported brush type: {brush.GetType().FullName}");
        return kind;
    }

    private Brush? _cache = null;

    public Brush Parse()
    {
        if (_cache != null) return _cache;
        var parser = GetParser(Kind);
        return _cache = parser.Decode(Content);
    }

    public static BrushOption FromBrush(Brush brush)
    {
        var kind = GetKindFromBrush(brush);
        var parser = GetParser(kind);
        return new BrushOption
        {
            Kind = kind,
            Content = parser.Encode(brush),
            _cache = brush
        };
    }

    public static implicit operator BrushOption(Brush brush) => FromBrush(brush);
    public static implicit operator Brush(BrushOption option) => option.Parse();
}
