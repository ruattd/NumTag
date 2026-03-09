using System.Text;
using Avalonia;
using Avalonia.Media;

namespace NumTag.Core.Models;

public enum BrushKind
{
    SolidColor,
    LinearGradient,
}

public interface IBrushOptionParser
{
    public Brush Decode(string content);
    public string Encode(Brush brush);
}

file sealed class SolidColorBrushParser : IBrushOptionParser
{
    private SolidColorBrushParser() {}
    public static readonly SolidColorBrushParser Instance = new();

    public Brush Decode(string content)
    {
        var color = Color.Parse(content);
        return new SolidColorBrush(color);
    }

    public string Encode(Brush brush)
    {
        return brush is SolidColorBrush solid ? solid.Color.ToString() : throw new InvalidOperationException();
    }
}

file sealed class LinearGradientBrushParser : IBrushOptionParser
{
    private LinearGradientBrushParser() {}
    public static readonly LinearGradientBrushParser Instance = new();

    public Brush Decode(string content)
    {
        var parts = content.Split(':');
        var startPoint = RelativePoint.Parse(parts[0]);
        var endPoint = RelativePoint.Parse(parts[1]);
        Enum.TryParse<GradientSpreadMethod>(parts[2], true, out var spreadMethod);
        var stopParts = parts.Length > 3 ? parts[3].Split(';') : [];
        var stops = new GradientStops();
        foreach (var part in stopParts)
        {
            var parts2 = part.Split(',', 2);
            var offset = double.Parse(parts2[0]);
            var color = Color.Parse(parts2[1]);
            stops.Add(new GradientStop(color, offset));
        }
        return new LinearGradientBrush
        {
            StartPoint = startPoint,
            EndPoint = endPoint,
            SpreadMethod = spreadMethod,
            GradientStops = stops
        };
    }

    public string Encode(Brush brush)
    {
        if (brush is not LinearGradientBrush gradient) throw new InvalidOperationException();
        var sb = new StringBuilder();
        sb.Append(gradient.StartPoint).Append(':').Append(gradient.EndPoint).Append(':');
        sb.Append(gradient.SpreadMethod.ToString().ToLowerInvariant()).Append(':');
        foreach (var stop in gradient.GradientStops)
            sb.Append(stop.Offset).Append(',').Append(stop.Color).Append(';');
        sb.Remove(sb.Length - 1, 1);
        return sb.ToString();
    }
}

public record BrushOption
{
    public required BrushKind Kind { get; init; }
    public required string Content { get; init; }

    private static IBrushOptionParser GetParser(BrushKind kind) => kind switch
    {
        BrushKind.SolidColor => SolidColorBrushParser.Instance,
        BrushKind.LinearGradient => LinearGradientBrushParser.Instance,
        _ => throw new NotSupportedException($"Unsupported brush kind: {kind}"),
    };

    private static BrushKind GetKindFromBrush(Brush brush)
    {
        BrushKind kind;
        if (brush is SolidColorBrush) kind = BrushKind.SolidColor;
        else if (brush is GradientBrush) kind = BrushKind.LinearGradient;
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
