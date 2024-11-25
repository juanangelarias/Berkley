using James.Shared;
using James.Shared.Model;
using Microsoft.AspNetCore.Mvc;
using SharedBusinessLogic;

namespace JamesWebUI.Server.Controllers
{
    public partial class ExportController
    {
        [HttpGet("/export/AgencyLicenses/{agencyId:guid}/{format=Excel}")]
        public async Task<ActionResult> ExportAgencyLicenses(Guid agencyId, ExportFormat format)
        {
            if (Guid.Empty == agencyId)
                return new StatusCodeResult(422); //Unprocessable content
            //TODO:  Do the below in parallel
            var licenseResult = await DataAccess.GetAgencyLicenses(agencyId);
            if (!licenseResult.Success)
            {
                LoggingService.LogError("Failed to retrieve agency license data", licenseResult.Errors,
                    StandardLoggingCategories.DataAccess,
                    new Dictionary<string, string> { { "AgencyId", agencyId.ToString() } });
                return new StatusCodeResult(500);
            }

            var agencyNameNumberResult = await DataAccess.GetAgencyNameAndNumberById(agencyId);

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

        [HttpGet("/export/AgencyPOAs/{agencyId:guid}/{format=Excel}")]
        public async Task<ActionResult> ExportAgencyPOAs(Guid agencyId, ExportFormat format)
        {
            if (Guid.Empty == agencyId)
                return new StatusCodeResult(422); //Unprocessable content
            //TODO:  Do the below in parallel
            var poaResult = await DataAccess.GetAgencyPoas(agencyId);
            if (!poaResult.Success)
            {
                LoggingService.LogError("Failed to retrieve agency POA data", poaResult.Errors,
                    StandardLoggingCategories.DataAccess,
                    new Dictionary<string, string> { { "AgencyId", agencyId.ToString() } });
                return new StatusCodeResult(500);
            }

            var agencyNameNumberResult = await DataAccess.GetAgencyNameAndNumberById(agencyId);

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
    }
}
