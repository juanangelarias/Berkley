using Radzen;
using Radzen.Blazor;

namespace JamesWebUI.Client.Shared;

public sealed class JamesFormField : RadzenFormField
{
    private string? _style;

    // Separates inline CSS style string into individual property segments, preserving original spacing around ':'
    private static List<string> SeparatePropertySegments(string? style)
    {
        var segments = new List<string>();
        if (string.IsNullOrWhiteSpace(style)) return segments;

        var parts = style.Split(';');
        foreach (var raw in parts)
        {
            var part = raw.Trim();
            if (string.IsNullOrEmpty(part)) continue;
            var colonIndex = part.IndexOf(':');
            if (colonIndex <= 0 || colonIndex == part.Length - 1) continue;
            segments.Add(part);
        }
        return segments;
    }

    public override string? Style
    {
        get => _style;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                _style = "width: 100%";
                return;
            }

            var segments = SeparatePropertySegments(value);

            var hasWidth = false;
            foreach (var seg in segments)
            {
                var colonIndex = seg.IndexOf(':');
                if (colonIndex <= 0) continue;
                var name = seg.Substring(0, colonIndex).Trim();
                if (name.Equals("width", StringComparison.OrdinalIgnoreCase))
                {
                    hasWidth = true;
                    break;
                }
            }

            if (!hasWidth)
            {
                segments.Add("width: 100%");
            }

            _style = string.Join("; ", segments).ToLowerInvariant();
        }
    }

    public JamesFormField()
    {
        Variant = Variant.Text;
        Style = "width: 100%";
    }
}