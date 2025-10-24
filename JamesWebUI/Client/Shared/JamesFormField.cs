using Radzen;
using Radzen.Blazor;

namespace JamesWebUI.Client.Shared
{
    /// <summary>
    /// RadzenFormField with styling for James to keep the UI consistent
    /// </summary>
    public class JamesFormField:RadzenFormField
    {
        public JamesFormField()
        {
            //Set default style
            Variant = Variant.Text;
        }
    }
}
