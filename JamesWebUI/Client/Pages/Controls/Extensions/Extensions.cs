using James.Shared.Data;
using Microsoft.AspNetCore.Components;
using StrawberryShake;

namespace JamesWebUI.Client.Controls.Extensions
{
    /// <summary>
    /// Generic static class for extension methods not worth putting in their own class
    /// </summary>
    public static class Extensions
    {
        public static string Page(this NavigationManager navigation)
        {
            return navigation.Uri.Substring(navigation.BaseUri.Length - 1);
        }
    }
}
