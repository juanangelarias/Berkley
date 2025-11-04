using Radzen.Blazor;

namespace JamesWebUI.Client.Shared;

public sealed class JamesFullWidthPanel: RadzenPanel
{
    public JamesFullWidthPanel()
    {
        Style = "border-radius: 10px; width: 100%; margin-bottom:0.2rem; padding:0.3rem;";
    }
}