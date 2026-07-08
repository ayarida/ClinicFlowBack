using ClinicFlow.Application.Features.Treatments.Commands.CreateTreatment;
using ClinicFlow.Application.Features.Treatments.Commands.DeactivateTreatment;
using ClinicFlow.Application.Features.Treatments.Commands.UpdateTreatment;
using ClinicFlow.Application.Features.Treatments.Queries.GetTreatmentById;
using ClinicFlow.Application.Features.Treatments.Queries.GetTreatments;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClinicFlow.API.Controllers;

[ApiController]
[Route("api/treatments")]
public class TreatmentsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetTreatments(CancellationToken ct)
    {
        var result = await sender.Send(new GetTreatmentsQuery(), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTreatmentById(Guid id, CancellationToken ct)
    {
        var result = await sender.Send(new GetTreatmentByIdQuery(id), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTreatment([FromBody] CreateTreatmentCommand command, CancellationToken ct)
    {
        var id = await sender.Send(command, ct);
        return CreatedAtAction(nameof(GetTreatmentById), new { id }, new { id });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateTreatment(Guid id, [FromBody] UpdateTreatmentCommand command, CancellationToken ct)
    {
        await sender.Send(command with { Id = id }, ct);
        return NoContent();
    }

    [HttpPatch("{id:guid}/deactivate")]
    public async Task<IActionResult> DeactivateTreatment(Guid id, CancellationToken ct)
    {
        await sender.Send(new DeactivateTreatmentCommand(id), ct);
        return NoContent();
    }
}
