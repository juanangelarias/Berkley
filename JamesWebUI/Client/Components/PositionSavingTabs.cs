using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using Microsoft.AspNetCore.WebUtilities;

namespace JamesWebUI.Client.Components
{
    /// <summary>
    /// This control will auto-select a default tab based on the current application Uri.
    /// A proper query string will be in the format of: ?{TabControlName}={TabName}.
    /// </summary>
    public partial class PositionSavingTabs : RadzenTabs
    {
        [Inject] 
        private NavigationManager NavigationManager { get; set; } = null!;

        [Parameter] 
        public string TabControlName { get; set; } = null!;
        public List<PositionSavingTabsItem> AllTabs { get; set; } = [];

        private bool shouldSwitch = true;

        private int newSelectedIndex = 0;
        public int NewSelectedIndex 
        {
            get => newSelectedIndex;
            set
            {
                var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);

                var tab = AllTabs.FirstOrDefault(t => t.Index == value);
                if (tab != null)
                {
                    var newUri = NavigationManager.GetUriWithQueryParameter(TabControlName, tab.TabName);

                    NavigationManager.NavigateTo(newUri, false, true);

                    newSelectedIndex = value;
                    shouldSwitch = false;
                }
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            if (AllTabs.Count > 0 && shouldSwitch)
            {
                var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
                if (QueryHelpers.ParseQuery(uri.Query).TryGetValue(TabControlName, out var _tabToSelect))
                {
                    var tabToSelect = AllTabs.FirstOrDefault(t => t.TabName == _tabToSelect);
                    if ((tabToSelect?.Index ?? 0) != NewSelectedIndex)
                    {
                        NewSelectedIndex = tabToSelect?.Index ?? 0;
                    }
                }
            }
            else
            {
                shouldSwitch = true;
            }

            await base.OnParametersSetAsync();
        }

        public async Task AddTab(PositionSavingTabsItem tab)
        {
            AllTabs.Add(tab);
            await base.AddTab(tab);
        }
        public async Task RemoveTab(PositionSavingTabsItem tab)
        {
            AllTabs.Remove(tab);
            base.RemoveItem(tab);
        }
    }
}
