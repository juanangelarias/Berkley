using Radzen.Blazor;

namespace JamesWebUI.Client.BsgComponents;

public sealed class BsgFullWidthPanel: RadzenPanel
{
    public BsgFullWidthPanel()
    {
        Style = "border-radius: 10px; width: 100%; margin-bottom:0.2rem; padding:0.3rem;";
    }
}