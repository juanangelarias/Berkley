using Radzen;
using Radzen.Blazor;

namespace JamesWebUI.Client.BsgComponents;

public sealed class BsgFormField: RadzenFormField
{
    public BsgFormField()
    {
        Variant = Variant.Text;
        Style = "width: 100%";
    }
}