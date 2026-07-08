using ClinicFlow.Application.Features.Clinics.Commands.ActivateClinic;
using ClinicFlow.Application.Features.Clinics.Commands.CreateClinic;
using ClinicFlow.Application.Features.Clinics.Commands.DeactivateClinic;
using ClinicFlow.Application.Features.Clinics.Commands.UpdateClinic;
using ClinicFlow.Application.Features.Clinics.Commands.UpdateClinicById;
using ClinicFlow.Application.Features.Clinics.Queries.GetClinic;
using ClinicFlow.Application.Features.Clinics.Queries.GetClinicById;
using ClinicFlow.Application.Features.Clinics.Queries.GetClinics;
using ClinicFlow.Application.Features.Staff.Commands.CreateStaffForClinic;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicFlow.API.Controllers;

[ApiController]
[Route("api/clinics")]
[Authorize(Roles = "SystemOwner")]
public class ClinicsController(ISender sender) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "SystemOwner")]
    public async Task<IActionResult> GetClinics(CancellationToken ct)
    {
        var result = await sender.Send(new GetClinicsQuery(), ct);
        return Ok(result);
    }

    [HttpGet("me")]
    [AllowAnonymous]
    public async Task<IActionResult> GetClinic(CancellationToken ct)
    {
        var result = await sender.Send(new GetClinicQuery(), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetClinicById(Guid id, CancellationToken ct)
    {
        var result = await sender.Send(new GetClinicByIdQuery(id), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateClinic([FromBody] CreateClinicCommand command, CancellationToken ct)
    {
        var id = await sender.Send(command, ct);
        return CreatedAtAction(nameof(GetClinicById), new { id }, new { id });
    }

    [HttpPut("me")]
    [AllowAnonymous]
    public async Task<IActionResult> UpdateClinic([FromBody] UpdateClinicCommand command, CancellationToken ct)
    {
        await sender.Send(command, ct);
        return NoContent();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateClinic(Guid id, [FromBody] UpdateClinicByIdCommand command, CancellationToken ct)
    {
        await sender.Send(command with { Id = id }, ct);
        return NoContent();
    }

    [HttpPatch("{id:guid}/deactivate")]
    public async Task<IActionResult> DeactivateClinic(Guid id, CancellationToken ct)
    {
        await sender.Send(new DeactivateClinicCommand(id), ct);
        return NoContent();
    }

    [HttpPatch("{id:guid}/activate")]
    public async Task<IActionResult> ActivateClinic(Guid id, CancellationToken ct)
    {
        await sender.Send(new ActivateClinicCommand(id), ct);
        return NoContent();
    }

    [HttpPost("{id:guid}/staff")]
    public async Task<IActionResult> CreateStaffForClinic(Guid id, [FromBody] CreateStaffForClinicCommand command, CancellationToken ct)
    {
        var staffId = await sender.Send(command with { ClinicId = id }, ct);
        return StatusCode(StatusCodes.Status201Created, new { id = staffId });
    }
}
