using Radzen;

namespace JamesWebUI.Client.Shared;

public static class DialogTools
{
    public static DialogOptions GetDialogOptions(int? width = null)
    {
        var dialogOptions = new DialogOptions
        {
            Draggable = true,
            Resizable = false,
            CloseDialogOnEsc = false,
            CloseDialogOnOverlayClick = false,
            ShowClose = true,
        };
        
        if(width != null)
            dialogOptions.Width = width.ToString();
        
        return dialogOptions;
    }
}