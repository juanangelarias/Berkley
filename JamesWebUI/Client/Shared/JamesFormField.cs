using Radzen;
using Radzen.Blazor;

namespace JamesWebUI.Client.Shared;

public sealed class JamesFormField : RadzenFormField
{
    private string? _style;

    public override string? Style
    {
        get => _style;
        set
        {
            _style = string.IsNullOrEmpty(value)
                ? _style = "width: 100%"
                : value.Contains("width")
                    ? _style = value
                    : value[value.Length - 1] == ';'
                        ? $"{value} width: 100%;"
                        : $"{value}; width: 100%;";
        }
    }

    public JamesFormField()
    {
        Variant = Variant.Text;
        Style = "width: 100%";
    }
}