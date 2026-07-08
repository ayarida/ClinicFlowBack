using ClinicFlow.Application.Features.Reports.Queries.GetDailyReport;
using ClinicFlow.Application.Features.Reports.Queries.GetMonthlyReport;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClinicFlow.API.Controllers;

[ApiController]
[Route("api/reports")]
public class ReportsController(ISender sender) : ControllerBase
{
    [HttpGet("daily")]
    public async Task<IActionResult> GetDailyReport([FromQuery] DateOnly date, CancellationToken ct)
    {
        var result = await sender.Send(new GetDailyReportQuery(date), ct);
        return Ok(result);
    }

    [HttpGet("monthly")]
    public async Task<IActionResult> GetMonthlyReport(
        [FromQuery] int year,
        [FromQuery] int month,
        CancellationToken ct)
    {
        var result = await sender.Send(new GetMonthlyReportQuery(year, month), ct);
        return Ok(result);
    }
}
