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
            if (string.IsNullOrEmpty(value))
            {
                _style = "width: 100%";
                return;
            }

            if (value.Contains("width") &&
                !value.Contains("min-width") &&
                !value.Contains("max-width"))
            {
                _style = value;
                return;
            }

            var count = 0;
            var index = 0;
            while ((index = value.IndexOf("width", index, StringComparison.Ordinal)) != -1) 
            {
                count++;
                index += "width".Length;
            }

            if (count == 0)
            {
                _style = value[value.Length - 1] == ';'
                    ? _style = $"{value} width: 100%;"
                    : $"{value}; width: 100%;";
                return;
            }

            if (count == 1 && (value.Contains("min-width") || value.Contains("max-width")))
            {
                _style = value[value.Length - 1] == ';'
                    ? _style = $"{value} width: 100%;"
                    : $"{value}; width: 100%;";
                return;
            }
            
            if (count == 1 && !value.Contains("min-width") && !value.Contains("max-width"))
            {
                _style = value;
                return;           
            }
            
            if (count == 2 && value.Contains("min-width") && value.Contains("max-width"))
            {
                _style = value[value.Length - 1] == ';'
                    ? _style = $"{value} width: 100%;"
                    : $"{value}; width: 100%;";
            }
            else
            {
                _style = value;
            }
        }
    }

    public JamesFormField()
    {
        Variant = Variant.Text;
        Style = "width: 100%";
    }
}