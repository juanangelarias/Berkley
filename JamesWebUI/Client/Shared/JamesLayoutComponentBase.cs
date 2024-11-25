using System.Security.Claims;
using James.Shared;
using James.Shared.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;
using Radzen.Blazor;
using ThemeService = JamesWebUI.Client.Services.ThemeService;

namespace JamesWebUI.Client.Shared
{
    public abstract class JamesLayoutComponentBase : LayoutComponentBase
    {
        [Inject]
        public required ThemeService JamesThemeService { get; set; }
        [Inject]
        public required ILoggingService LoggingService { get; set; }
        [Inject]
        public required DialogService DialogService { get; set; }
        [Inject]
        public required NotificationService NotificationService { get; set; }

        #region Authentication Code
        [CascadingParameter]
        protected Task<AuthenticationState>? AuthenticationState { get; set; }
        private Task<AuthenticationState> LoadAuthenticationStateAsync { get; set; } = null!;

        //TODO: Discuss whether to allow only synchronous or asynchronous access to values.
        //Synchronous access properties
        protected AuthenticationState? State { get; private set; }
        protected ClaimsPrincipal? UserPrincipal => State?.User;
        protected bool IsAuthenticationStateLoaded { get; private set; }
        protected bool IsAuthenticated => IsAuthenticationStateLoaded && State!.User.Identity != null;

        //Asynchronous access to properties
        protected async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (LoadAuthenticationStateAsync.IsCompleted)
                return LoadAuthenticationStateAsync.Result;
            return await LoadAuthenticationStateAsync;
        }

        protected async Task<ClaimsPrincipal> GetUserPrincipalAsync()
        {
            if (LoadAuthenticationStateAsync.IsCompleted)
                return LoadAuthenticationStateAsync.Result.User;
            return (await LoadAuthenticationStateAsync).User;
        }

        protected async Task<bool> GetIsAuthenticatedAsync()
        {
            if (LoadAuthenticationStateAsync.IsCompleted)
                return null != LoadAuthenticationStateAsync.Result.User.Identity;
            return null != (await LoadAuthenticationStateAsync).User.Identity;
        }

        #endregion

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            if (null != AuthenticationState)
                //NOTE:Running in background can cause issues if values are queried immediately upon return.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                Task.Run(async () =>
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                {
                    LoadAuthenticationStateAsync = AuthenticationState;
                    State = await AuthenticationState;
                    IsAuthenticationStateLoaded = true;
                });
        }

        #region Common Client Actions
        /// <summary>
        /// Generates standard notification that changes were saved successfully.
        /// </summary>
        /// <param name="itemSaved">The item being saved, default is "Changes".  Should be title cased.</param>
        protected void NotifySuccessfulSave(string itemSaved = "Changes")
        {
            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Info, Summary = $"{itemSaved} saved successfully.", Duration = 5000 });
        }

        /// <summary>
        /// Generates standard notification that a save failed.
        /// </summary>
        /// <param name="errors">Errors that were returned.</param>
        /// <param name="itemSaved">The item that didn't save, default is "changes".  Should NOT be title cased.</param>
        protected void NotifySaveError(string[] errors, string itemSaved = "changes")
        {
            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = $"There {(errors.Length == 1 ? "was an error" : "were errors")} saving {itemSaved}.  {string.Join("  ", errors)}", Duration = 15000 });
        }

        /// <summary>
        /// Generates standard notification that a load failed.
        /// </summary>
        /// <param name="errors">Errors that were returned.</param>
        /// <param name="itemSaved">The item that didn't load, default is "data".  Should NOT be title cased.</param>
        protected void NotifyLoadError(string[] errors, string itemSaved = "data")
        {
            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = $"There {(errors.Length == 1 ? "was an error" : "were errors")} retrieving {itemSaved}.  {string.Join("  ", errors)}", Duration = 15000 });
        }

        protected void LogGraphQlLoadError()
        {
            throw new NotImplementedException("If you need it, create it.");
        }

        private string SubstitutePropertyIfNeeded(string original, ExportColumnSubstitutions substitutions) =>
            substitutions[original].Property;

        private string SubstituteTitleIfNeeded(string original, ExportColumnSubstitutions substitutions) =>
            substitutions[original].Title;

        protected string ExportDataGridUrl<T>(RadzenDataGrid<T> dataGrid, string url, ExportFormat format, ExportColumnSubstitutions? propertySubstitutions = null)
        {
            propertySubstitutions ??= new();
            var query = new Query()
            {
                OrderBy = SubstitutePropertyIfNeeded(dataGrid.Query.OrderBy, propertySubstitutions),
                Filter = SubstitutePropertyIfNeeded(dataGrid.Query.Filter, propertySubstitutions),
                Select = string.Join(",", dataGrid.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property))
                    .Select(c => new {Property= SubstitutePropertyIfNeeded(c.Property, propertySubstitutions), Title = SubstituteTitleIfNeeded(c.Property, propertySubstitutions)})
                    .Select(cSub => new {Property= cSub.Property, Title = cSub.Title.Contains(".") ? $"{cSub.Property} as {cSub.Title.Replace(".", "_")}" : cSub.Title })
                            .Select(pt => pt.Property== pt.Title? pt.Property:$"{pt.Property} as {pt.Title.Replace(' ',ExportColumnSubstitution.SpaceSubstitution)}"))
            };
            return query.ToUrl($"{url}/{(format == ExportFormat.CSV ? "CSV" : "Excel")}");
        }
        #endregion
    }

    public static class AuthUserExtensions
    {
        public static string? Username(ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(c => c.Type == "nickname")?.Value;
        }

        public static string? EmailAddress(ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(c => c.Type == "email_address")?.Value;
        }
        public static string? FirstName(ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;
        }
        public static string? LastName(ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value;
        }
        public static string? PictureUrl(ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(c => c.Type == "picture")?.Value;
        }
        public static string? UpdatedAt(ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(c => c.Type == "updated_at")?.Value;
        }
        public static string? NameIdentifier(ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(c => c.Type == "email_address")?.Value;
        }
        public static string? SID(ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(c => c.Type == "sid")?.Value;
        }
    }
}
