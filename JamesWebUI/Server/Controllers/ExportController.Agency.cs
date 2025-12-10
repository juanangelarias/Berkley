using James.Shared;
using James.Shared.Constants;
using James.Shared.Data;
using James.Shared.Dto;
using James.Shared.Model;
using Microsoft.AspNetCore.Mvc;
using SharedBusinessLogic;

namespace JamesWebUI.Server.Controllers
{
    public partial class ExportController
    {
        [HttpGet("/export/AgencyLicenses/{agencyId:guid}/{producers:bool}/{format=Excel}")]
        public async Task<ActionResult> ExportAgencyLicenses(Guid agencyId, bool producers, ExportFormat format)
        {
            if (Guid.Empty == agencyId)
                return new StatusCodeResult(422); //Unprocessable content

            IDataAccessResult<List<AgencyLicense>> licenseResult = null!;
            IDataAccessResult<Agency?> agencyNameNumberResult = null!;
            var loads = new List<Func<Task>>
            {
                async () => { licenseResult = await DataAccess.GetAgencyLicenses(agencyId, producers); },
                async () => { agencyNameNumberResult = await DataAccess.GetAgencyNameAndNumberById(agencyId); }
            };
            await Task.WhenAll(loads.Select(l=>l()));
            if (!licenseResult.Success)
            {
                LoggingService.LogError("Failed to retrieve agency license data", licenseResult.Errors,
                    StandardLoggingCategories.DataAccess,
                    new Dictionary<string, string> { { "AgencyId", agencyId.ToString() } });
                return new StatusCodeResult(500);
            }

            string fileName;
            if (agencyNameNumberResult.Success)
                fileName = $"{agencyNameNumberResult.Data!.ToFileName()}-Licenses";
            else
            {
                LoggingService.LogError("Failed to retrieve agency number and name", licenseResult.Errors,
                    StandardLoggingCategories.DataAccess,
                    new Dictionary<string, string> { { "AgencyId", agencyId.ToString() } });
                fileName = "AgencyLicenses";
            }

            var licenseQuery = licenseResult.Data!.AsQueryable();
            return format == ExportFormat.CSV
                ? ToCsv(ApplyQuery(licenseQuery, Request.Query), $"{fileName}.csv")
                : ToExcel(ApplyQuery(licenseQuery, Request.Query), $"{fileName}.xlsx");
        }

        [HttpGet("/export/AgencyPOAs/{agencyId:guid}/{activeOnly:bool}/{format=Excel}")]
        public async Task<ActionResult> ExportAgencyPOAs(Guid agencyId, bool activeOnly, ExportFormat format)
        {
            if (Guid.Empty == agencyId)
                return new StatusCodeResult(422); //Unprocessable content

            IDataAccessResult<List<PowerOfAttorney>> poaResult = null!;
            IDataAccessResult<Agency?> agencyNameNumberResult = null!;
            var loads = new List<Func<Task>>
            {
                async () => { poaResult = await DataAccess.GetAgencyPoas(agencyId, activeOnly); },
                async () => { agencyNameNumberResult = await DataAccess.GetAgencyNameAndNumberById(agencyId); }
            };
            await Task.WhenAll(loads.Select(l => l()));
            if (!poaResult.Success)
            {
                LoggingService.LogError("Failed to retrieve agency POA data", poaResult.Errors,
                    StandardLoggingCategories.DataAccess,
                    new Dictionary<string, string> { { "AgencyId", agencyId.ToString() } });
                return new StatusCodeResult(500);
            }

            string fileName;
            if (agencyNameNumberResult.Success)
                fileName = $"{agencyNameNumberResult.Data!.ToFileName()}-POAs";
            else
            {
                LoggingService.LogError("Failed to retrieve agency number and name", poaResult.Errors,
                    StandardLoggingCategories.DataAccess,
                    new Dictionary<string, string> { { "AgencyId", agencyId.ToString() } });
                fileName = "AgencyLicenses";
            }

            var poaQuery = poaResult.Data!.AsQueryable();
            return format == ExportFormat.CSV
                ? ToCsv(ApplyQuery(poaQuery, Request.Query), $"{fileName}.csv")
                : ToExcel(ApplyQuery(poaQuery, Request.Query), $"{fileName}.xlsx");
        }

        [HttpGet("/export/AgencyRelatedParties/{agencyId:guid}/{format=Excel}")]
        public async Task<ActionResult> ExportAgencyRelatedParties(Guid agencyId, ExportFormat format)
        {
            if (Guid.Empty == agencyId)
                return new StatusCodeResult(422); //Unprocessable content

            IDataAccessResult<List<AgencyLocationsDto>> poaResult = null!;
            IDataAccessResult<Agency?> agencyNameNumberResult = null!;

            var loads = new List<Func<Task>>
            {
                async () => { poaResult = await DataAccess.GetAgencyRelatedParties(agencyId); },
                async () => { agencyNameNumberResult = await DataAccess.GetAgencyNameAndNumberById(agencyId); }
            };
            await Task.WhenAll(loads.Select(l => l()));
            if (!poaResult.Success)
            {
                LoggingService.LogError("Failed to retrieve agency Related Party data", poaResult.Errors,
                    StandardLoggingCategories.DataAccess,
                    new Dictionary<string, string> { { "AgencyId", agencyId.ToString() } });
                return new StatusCodeResult(500);
            }

            string fileName;
            if (agencyNameNumberResult.Success)
                fileName = $"{agencyNameNumberResult.Data!.ToFileName()}-RelatedParties";
            else
            {
                LoggingService.LogError("Failed to retrieve agency number and name", poaResult.Errors,
                    StandardLoggingCategories.DataAccess,
                    new Dictionary<string, string> { { "AgencyId", agencyId.ToString() } });
                fileName = "AgencyRelatedParties";
            }

            var poaQuery = poaResult.Data!.AsQueryable();
            return format == ExportFormat.CSV
                ? ToCsv(ApplyQuery(poaQuery, Request.Query), $"{fileName}.csv")
                : ToExcel(ApplyQuery(poaQuery, Request.Query), $"{fileName}.xlsx");
        }

        [HttpGet("/export/AgencyContacts/{agencyId:guid}/{format=Excel}")]
        public async Task<ActionResult> ExportAgencyContacts(Guid agencyId, ExportFormat format)
        {
            if (Guid.Empty == agencyId)
                return new StatusCodeResult(422); //Unprocessable content

            IDataAccessResult<List<Agent>> contactsResult = null!;
            IDataAccessResult<Agency?> agencyNameNumberResult = null!;

            var loads = new List<Func<Task>>
            {
                async () => { contactsResult = await DataAccess.GetAgencyAgents(agencyId); },
                async () => { agencyNameNumberResult = await DataAccess.GetAgencyNameAndNumberById(agencyId); }
            };
            await Task.WhenAll(loads.Select(l => l()));
            if (!contactsResult.Success)
            {
                LoggingService.LogError("Failed to retrieve agency Related Party data", contactsResult.Errors,
                    StandardLoggingCategories.DataAccess,
                    new Dictionary<string, string> { { "AgencyId", agencyId.ToString() } });
                return new StatusCodeResult(500);
            }

            string fileName;
            if (agencyNameNumberResult.Success)
                fileName = $"{agencyNameNumberResult.Data!.ToFileName()}-RelatedParties";
            else
            {
                LoggingService.LogError("Failed to retrieve agency number and name", contactsResult.Errors,
                    StandardLoggingCategories.DataAccess,
                    new Dictionary<string, string> { { "AgencyId", agencyId.ToString() } });
                fileName = "AgencyRelatedParties";
            }

            var singleLicenseAgents = contactsResult.Data!.SelectMany(a=> a.AgencyLicenses.Select(al=>
            {
                var asl = ThisToThat.ToEntityType<AgentSingleLicense>(a);
                asl.License = al;
                return asl;
            })).ToList();
            //Initial list omits Agents with no licenses, must add here
            singleLicenseAgents.AddRange(contactsResult.Data!.Where(ag => ag.AgencyLicenses.Count == 0).Select(a=>
            {
                var asl = ThisToThat.ToEntityType<AgentSingleLicense>(a);
                asl.License = new AgencyLicense{Agent = a, State = "", LicenseNumber = "", Insurer = new Insurer{IdNavigation = new LegalEntity{FullName = ""}}};
                return asl;
            }));

            var agentLicenseQuery = singleLicenseAgents.AsQueryable();
            return format == ExportFormat.CSV
                ? ToCsv(ApplyQuery(agentLicenseQuery, Request.Query), $"{fileName}.csv")
                : ToExcel(ApplyQuery(agentLicenseQuery, Request.Query), $"{fileName}.xlsx");
        }

        private class AgentSingleLicense : Agent
        {
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public AgencyLicense? License { get; set; }
        }

        [HttpGet("/export/AgencyInventory/{agencyId:guid}/{format=Excel}")]
        public async Task<ActionResult> ExportAgencyInventory(Guid agencyId, ExportFormat format)
        {
            if (Guid.Empty == agencyId)
                return new StatusCodeResult(422); //Unprocessable content

            IDataAccessResult<List<AgencyInventory>> inventoryResult = null!;
            IDataAccessResult<Agency?> agencyNameNumberResult = null!;

            var loads = new List<Func<Task>>
            {
                async () => { inventoryResult = await DataAccess.GetAgencyInventory(agencyId);},
                async () => { agencyNameNumberResult = await DataAccess.GetAgencyNameAndNumberById(agencyId); }
            };
            await Task.WhenAll(loads.Select(l => l()));
            if (!inventoryResult.Success)
            {
                LoggingService.LogError("Failed to retrieve agency Related Party data", inventoryResult.Errors,
                    StandardLoggingCategories.DataAccess,
                    new Dictionary<string, string> { { "AgencyId", agencyId.ToString() } });
                return new StatusCodeResult(500);
            }

            string fileName;
            if (agencyNameNumberResult.Success)
                fileName = $"{agencyNameNumberResult.Data!.ToFileName()}-Inventory";
            else
            {
                LoggingService.LogError("Failed to retrieve agency number and name", inventoryResult.Errors,
                    StandardLoggingCategories.DataAccess,
                    new Dictionary<string, string> { { "AgencyId", agencyId.ToString() } });
                fileName = "AgencyInventory";
            }

            var inventoryQuery = inventoryResult.Data!.AsQueryable();
            return format == ExportFormat.CSV
                ? ToCsv(ApplyQuery(inventoryQuery, Request.Query), $"{fileName}.csv")
                : ToExcel(ApplyQuery(inventoryQuery, Request.Query), $"{fileName}.xlsx");
        }
    }
}
