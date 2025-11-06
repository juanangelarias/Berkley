using Radzen;
using Radzen.Blazor;

namespace JamesWebUI.Client.Shared;

public sealed class JamesFormField: RadzenFormField
{
    public override string Style { get; set; }
    
    public JamesFormField()
    {
        Variant = Variant.Text;
        
        if(string.IsNullOrEmpty(Style))
            Style = "width: 100%";
        else
        {
            if(Style.Contains("width"))
                return;
            
            Style = Style[Style.Length - 1] == ';' 
                ? $"{Style} width: 100%" 
                : $"{Style}; width: 100%";
        }
    }
}