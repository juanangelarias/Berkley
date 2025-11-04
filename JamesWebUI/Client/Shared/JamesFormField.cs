using Radzen;
using Radzen.Blazor;

namespace JamesWebUI.Client.Shared;

public sealed class JamesFormField: RadzenFormField
{
    public JamesFormField()
    {
        Variant = Variant.Text;
        Style = "width: 100%";
    }
}