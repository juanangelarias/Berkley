using James.Data.Server.Model;
using James.Shared.Export;
using James.Shared.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JamesWebUI.Server.Controllers;

[ApiController]
[Authorize]
public class ExportToExcel(IExportToExcelService exportToExcelService, IDbContextFactory<JamesDatabaseContext> contextFactory)
    : ControllerBase
{
    [HttpGet("/exportToExcel/agencyRelatedParties/{agencyId:guid}")]
    public async Task<IActionResult> ExportAgencyRelatedPartiesExcel([FromRoute] Guid agencyId)
    {
        var relAgencies = await FetchRelatedAgenciesData(agencyId);
        if (relAgencies.Count == 0)
            return NotFound();

        var columns = new List<ExcelColumn>
        {
            new()
            {
                Number = 1,
                Type = ExcelColumnType.String,
                Title = "Status"
            },
            new()
            {
                Number = 2,
                Type = ExcelColumnType.Int,
                Title = "Agency Number"
            },
            new()
            {
                Number = 3,
                Type = ExcelColumnType.String,
                Title = "Agency Name"
            },
            new()
            {
                Number = 4,
                Type = ExcelColumnType.String,
                Title = "Address"
            },
            new()
            {
                Number = 5,
                Type = ExcelColumnType.String,
                Title = "Address 2"
            },
            new()
            {
                Number = 6,
                Type = ExcelColumnType.String,
                Title = "Address 3"
            },
            new()
            {
                Number = 7,
                Type = ExcelColumnType.String,
                Title = "City"
            },
            new()
            {
                Number = 8,
                Type = ExcelColumnType.String,
                Title = "State"
            },

            new()
            {
                Number = 9,
                Type = ExcelColumnType.String,
                Title = "Postal Code"
            },
        };

        var rows = new List<ExcelRow>();
        var r = 0;
        foreach (var ag in relAgencies)
        {
            var address = ag.IdNavigation.LegalEntityAddresses
                .FirstOrDefault()?
                .Address;
            r += 1;
            var cells = new List<ExcelCell>();
            foreach (var col in columns)
            {
                var value = col.Title switch
                {
                    "Status" => ag.Status,
                    "Agency Number" => ag.AgencyNumber,
                    "Agency Name" => ag.IdNavigation.FullName,
                    "Address" => address?.Address1 ?? "",
                    "Address 2" => address?.Address2 ?? "",
                    "Address 3" => address?.Address3 ?? "",
                    "City" => address?.City ?? "",
                    "State" => address?.StateCode ?? "",
                    "Postal Code" => address?.PostalCode ?? "",
                    _ => ""
                };

                cells.Add(new()
                {
                    Column = col,
                    RowNumber = r,
                    Value = value
                });
            }

            var row = new ExcelRow
            {
                Cells = cells
            };
            rows.Add(row);
        }

        var data = new ExcelData
        {
            Columns = columns,
            Rows = rows
        };

        var ms = exportToExcelService.ExportToExcel(data, "AgencyRelatedParties.xlsx");

        return File(ms, "application/octet-stream", "AgencyRelatedParties.xlsx");
    }

    [HttpGet("/exportToCsv/agencyRelatedParties/{agencyId:guid}")]
    public async Task<IActionResult> ExportAgencyRelatedPartiesCsv([FromRoute] Guid agencyId)
    {
        var relAgencies = await FetchRelatedAgenciesData(agencyId);
        if (relAgencies.Count == 0)
            return NotFound();
        
        using var ms = new MemoryStream();
        await using var sw = new StreamWriter(ms);
        
        await sw.WriteLineAsync("\"Status\",\"Agency Number\",\"Agency Name\",\"Address\",\"Address 2\",\"Address 3\",\"City\"," +
                                "\"State\",\"Postal Code\"");
        foreach (var ag in relAgencies)
        {
            var address = ag.IdNavigation.LegalEntityAddresses
                .FirstOrDefault(f=>f.Type == "Main")?
                .Address;

            await sw.WriteLineAsync($"\"{ag.Status.Trim()}\",\"{ag.AgencyNumber.Trim()}\"," +
                                    $"\"{ag.IdNavigation.FullName.Trim()}\",\"{address?.Address1.Trim()}\"," +
                                    $"\"{address?.Address2?.Trim()}\",\"{address?.Address3?.Trim()}\"," +
                                    $"\"{address?.City.Trim()}\",\"{address?.StateCode?.Trim()}\"," +
                                    $"\"{address?.PostalCode?.Trim()}\"");
        }

        await sw.FlushAsync();
        ms.Position = 0;
        
        
        return File(ms.ToArray(), "text/csv","AgencyRelatedParties.csv");
    }

    private async Task<List<Agency>> FetchRelatedAgenciesData(Guid agencyId)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        
        var topParent = await ctx.VAgencyParents
            .FirstOrDefaultAsync(r => r.Id == agencyId);

        if (topParent == null)
            return [];

        var relParIds = await ctx.VAgencyParents
            .Where(r => r.Parent == topParent.Parent)
            .Select(s => s.Id)
            .ToListAsync();

        var relAgencies = await ctx.Agencies
            .Where(r => relParIds.Contains(r.Id))
            .Include(i => i.AgencyLicenses)
            .ThenInclude(t => t.Agent)
            .Include(i => i.IdNavigation)
            .ThenInclude(t1 => t1.LegalEntityAddresses
                .Where(r => r.Type == "Main"))
            .ThenInclude(t2 => t2.Address)
            .ToListAsync();
        return relAgencies;
    }
}