using Microsoft.JSInterop;

namespace JamesWebUI.Client.Helpers;

public class SiteExitService(IJSRuntime js): IDisposable
{
    private DotNetObjectReference<SiteExitService> _dotNetObjectReference;

    public EventHandler? SpaClosed;

    public async Task InitializeAsync()
    {
        await js.InvokeVoidAsync("blazor_setExitEvent", _dotNetObjectReference);
    }

    [JSInvokable]
    public void OnBeforeUnload()
    {
        SpaClosed?.Invoke(this, EventArgs.Empty);
    }

    public void Dispose()
    {
        _dotNetObjectReference?.Dispose();
    }
}