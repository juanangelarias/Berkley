using James.Shared.Model;
using JamesWebUI.Client.Components.AgencyComponents;
using Radzen.Blazor;

namespace JamesWebUI.Client.Test
{
    public class ComponentBaseTests
	{
        [Fact]
        public void NoSubstitutionsGridUrl()
        {
            var format = ExportFormat.CSV;
            var testComponent = new AgencyContactsGrid
            {
                JamesThemeService = null,
                LoggingService = null,
                DialogService = null,
                NotificationService = null,
                DataCache = null
            };
            
            var AgencyId = 131;
            var url = testComponent.ExportDataGridUrl(testDataGrid,
                $"/export/AgencyContacts/{AgencyId}", format);
            Assert.StartsWith("/export/AgencyContacts", url);
            Assert.Contains("IdNavigation.GivenName as IdNavigation_GivenName", url);
            Assert.Contains("IdNavigation.MiddleInitial as IdNavigation_MiddleInitial", url);
            Assert.Contains("IdNavigation.FamilyName as IdNavigation_FamilyName", url);
		}
        [Fact]
        public void TitleSubstitutionsGridUrl()
        {
            var format = ExportFormat.CSV;
            var testComponent = new AgencyContactsGrid
            {
                JamesThemeService = null,
                LoggingService = null,
                DialogService = null,
                NotificationService = null,
                DataCache = null
            };
            var AgencyId = 131;
            var substitutions = new ExportColumnSubstitutions();
            substitutions.AddSubstitution("IdNavigation.GivenName", title: "Given Name");
            substitutions.AddSubstitution("IdNavigation.MiddleInitial", title: "Middle Initial");
            substitutions.AddSubstitution("IdNavigation.FamilyName", title: "Family Name");

            var url = testComponent.ExportDataGridUrl(testDataGrid,
                $"/export/AgencyContacts/{AgencyId}", format, substitutions);
            Assert.StartsWith("/export/AgencyContacts", url);
            Assert.Contains("IdNavigation.GivenName as Given你Name", url);
            Assert.Contains("IdNavigation.MiddleInitial as Middle你Initial", url);
            Assert.Contains("IdNavigation.FamilyName as Family你Name", url);
		}
        [Fact]
        public void PropertyChangeSubstitutionsGridUrl()
        {
            var format = ExportFormat.CSV;
            var testComponent = new AgencyContactsGrid
            {
                JamesThemeService = null,
                LoggingService = null,
                DialogService = null,
                NotificationService = null,
                DataCache = null
            };
            var AgencyId = 131;
            var substitutions = new ExportColumnSubstitutions();
            substitutions.AddSubstitution(new ExportColumnSubstitution("IdNavigation.GivenName", "Asparagus"));
            substitutions.AddSubstitution(new ExportColumnSubstitution("IdNavigation.MiddleInitial", "Banana"));
            substitutions.AddSubstitution(new ExportColumnSubstitution("IdNavigation.FamilyName", "DragonFruit"));

            var url = testComponent.ExportDataGridUrl(testDataGrid,
                $"/export/AgencyContacts/{AgencyId}", format, substitutions);
            Assert.StartsWith("/export/AgencyContacts", url);
            Assert.Contains("Asparagus", url);
            Assert.Contains("Banana", url);
            Assert.Contains("DragonFruit", url);
		}
		[Fact]
        public void AddColumnsSubstitutionsGridUrl()
        {
            var format = ExportFormat.CSV;
            var testComponent = new AgencyContactsGrid
            {
                JamesThemeService = null,
                LoggingService = null,
                DialogService = null,
                NotificationService = null,
                DataCache = null
            };
            var AgencyId = 131;
            var substitutions = new ExportColumnSubstitutions();
            substitutions.AddSubstitution(new ExportColumnSubstitution("", "Asparagus", "Grilled Asparagus"));
            substitutions.AddSubstitution(new ExportColumnSubstitution("", "Banana", "Fried Banana"));
            substitutions.AddSubstitution(new ExportColumnSubstitution("", "DragonFruit", "Pureed DragonFruit"));

            var url = testComponent.ExportDataGridUrl(testDataGrid,
                $"/export/AgencyContacts/{AgencyId}", format, substitutions);
            Assert.StartsWith("/export/AgencyContacts", url);
            Assert.Contains("Asparagus as Grilled你Asparagus", url);
            Assert.Contains("Banana as Fried你Banana", url);
            Assert.Contains("DragonFruit as Pureed你DragonFruit", url);
        }

		private RadzenDataGrid<Agent> testDataGrid = new RadzenDataGrid<Agent>
        {
            ColumnsCollection =
            {
                new RadzenDataGridColumn<Agent> { Visible = true, Property = "IdNavigation.GivenName" },
                new RadzenDataGridColumn<Agent> { Visible = true, Property = "IdNavigation.MiddleInitial" },
                new RadzenDataGridColumn<Agent> { Visible = true, Property = "IdNavigation.FamilyName" },
                new RadzenDataGridColumn<Agent> { Visible = true, Property = "NationalProducerNumber" }
            }
        };
    }
}
