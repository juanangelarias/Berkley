using JamesWebUI.Client.Controls.Extensions;
using Radzen.Blazor;

namespace JamesWebUI.Client.Shared
{

    internal class ConflictResolver
    {
        public required string PropertyName { get; set; }
        public object? OurChange { get; set; }
        public object? TheirChange { get; set; }
        public string OurChangeText => (OurChange?.ToString()).ToScreenText();
        public string TheirChangeText => (TheirChange?.ToString()).ToScreenText();
        public object? SelectedChange { get; set; }
        public RadzenSelectBar<object> ResolveEditor { get; set; } = null!;
        public required object OurRootObject { get; set; }
        public required object TheirRootObject { get;set; }
    }
}
