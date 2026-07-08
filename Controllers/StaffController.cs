using ClinicFlow.Application.Features.Staff.Commands.CreateStaff;
using ClinicFlow.Application.Features.Staff.Commands.DeactivateStaff;
using ClinicFlow.Application.Features.Staff.Commands.UpdateStaff;
using ClinicFlow.Application.Features.Staff.Queries.GetStaff;
using ClinicFlow.Application.Features.Staff.Queries.GetStaffById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClinicFlow.API.Controllers;

[ApiController]
[Route("api/staff")]
public class StaffController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetStaff(CancellationToken ct)
    {
        var result = await sender.Send(new GetStaffQuery(), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetStaffById(Guid id, CancellationToken ct)
    {
        var result = await sender.Send(new GetStaffByIdQuery(id), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateStaff([FromBody] CreateStaffCommand command, CancellationToken ct)
    {
        var id = await sender.Send(command, ct);
        return CreatedAtAction(nameof(GetStaffById), new { id }, new { id });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateStaff(Guid id, [FromBody] UpdateStaffCommand command, CancellationToken ct)
    {
        await sender.Send(command with { Id = id }, ct);
        return NoContent();
    }

    [HttpPatch("{id:guid}/deactivate")]
    public async Task<IActionResult> DeactivateStaff(Guid id, CancellationToken ct)
    {
        await sender.Send(new DeactivateStaffCommand(id), ct);
        return NoContent();
    }
}
