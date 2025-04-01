using Microsoft.JSInterop;

namespace JamesWebUI.Client.Services
{
    /// <summary>
    /// This service exists because Blazored.LocalStorage is missing this functionality.
    /// </summary>
    /// <param name="js">JavaScript runtime reference</param>
    /// <remarks>If they ever implement this functionality, get rid of this and use that</remarks>
    public class LocalStorageKeyListingService(IJSRuntime js)
    {
        public async Task<string[]> GetAllLocalStorageKeys(string startsWith = "")
        {
            var allKeys = await js.InvokeAsync<string[]>("getAllLocalStorageKeys");
            if (null! == allKeys)
                return Array.Empty<string>();
            if (string.IsNullOrEmpty(startsWith))
                return allKeys;
            return allKeys.Where(k => k.StartsWith(startsWith)).ToArray();
        }
    }
}

