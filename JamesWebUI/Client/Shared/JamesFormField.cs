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
            
            var lowValue = value.ToLower();

            if (lowValue.Contains("width") &&
                !lowValue.Contains("min-width") &&
                !lowValue.Contains("max-width"))
            {
                _style = lowValue;
                return;
            }

            var count = 0;
            var index = 0;
            while ((index = lowValue.IndexOf("width", index, StringComparison.Ordinal)) != -1) 
            {
                count++;
                index += "width".Length;
            }

            if (count == 0)
            {
                _style = value[lowValue.Length - 1] == ';'
                    ? _style = $"{value} width: 100%;"
                    : $"{lowValue}; width: 100%;";
                return;
            }

            if (count == 1 && (lowValue.Contains("min-width") || lowValue.Contains("max-width")))
            {
                _style = value[lowValue.Length - 1] == ';'
                    ? _style = $"{lowValue} width: 100%;"
                    : $"{lowValue}; width: 100%;";
                return;
            }
            
            if (count == 1 && !lowValue.Contains("min-width") && !lowValue.Contains("max-width"))
            {
                _style = lowValue;
                return;           
            }
            
            if (count == 2 && lowValue.Contains("min-width") && lowValue.Contains("max-width"))
            {
                _style = value[lowValue.Length - 1] == ';'
                    ? _style = $"{lowValue} width: 100%;"
                    : $"{lowValue}; width: 100%;";
            }
            else
            {
                _style = lowValue;
            }
        }
    }

    public JamesFormField()
    {
        Variant = Variant.Text;
        Style = "width: 100%";
    }
}