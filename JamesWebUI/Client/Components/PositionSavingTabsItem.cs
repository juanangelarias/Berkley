using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Radzen.Blazor;

namespace JamesWebUI.Client.Components
{
    public partial class PositionSavingTabsItem : RadzenTabsItem
    {


        [Parameter]
        public string TabName { get; set; } = "";
        [Parameter]
        public EventCallback<string> OnSelected { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            
            await ((PositionSavingTabs)Tabs).AddTab(this);
        }
    }
}
