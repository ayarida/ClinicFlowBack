using ClinicFlow.Application.Features.Appointments.Commands.CancelAppointment;
using ClinicFlow.Application.Features.Appointments.Commands.CompleteAppointment;
using ClinicFlow.Application.Features.Appointments.Commands.CreateAppointment;
using ClinicFlow.Application.Features.Appointments.Commands.MarkAppointmentNoShow;
using ClinicFlow.Application.Features.Appointments.Commands.UpdateAppointment;
using ClinicFlow.Application.Features.Appointments.Queries.GetAppointmentById;
using ClinicFlow.Application.Features.Appointments.Queries.GetAppointments;
using ClinicFlow.Application.Features.Appointments.Queries.GetCalendar;
using ClinicFlow.Application.Features.Appointments.Queries.GetFollowUpSuggestions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClinicFlow.API.Controllers;

[ApiController]
[Route("api/appointments")]
public class AppointmentsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAppointments(
        [FromQuery] DateOnly? date,
        [FromQuery] Guid? customerId,
        CancellationToken ct)
    {
        var result = await sender.Send(new GetAppointmentsQuery(date, customerId), ct);
        return Ok(result);
    }

    [HttpGet("calendar")]
    public async Task<IActionResult> GetCalendar(
        [FromQuery] int year,
        [FromQuery] int month,
        CancellationToken ct)
    {
        var result = await sender.Send(new GetCalendarQuery(year, month), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAppointmentById(Guid id, CancellationToken ct)
    {
        var result = await sender.Send(new GetAppointmentByIdQuery(id), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentCommand command, CancellationToken ct)
    {
        var id = await sender.Send(command, ct);
        return CreatedAtAction(nameof(GetAppointmentById), new { id }, new { id });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAppointment(Guid id, [FromBody] UpdateAppointmentCommand command, CancellationToken ct)
    {
        await sender.Send(command with { Id = id }, ct);
        return NoContent();
    }

    [HttpPatch("{id:guid}/cancel")]
    public async Task<IActionResult> CancelAppointment(Guid id, CancellationToken ct)
    {
        await sender.Send(new CancelAppointmentCommand(id), ct);
        return NoContent();
    }

    [HttpPatch("{id:guid}/complete")]
    public async Task<IActionResult> CompleteAppointment(Guid id, CancellationToken ct)
    {
        await sender.Send(new CompleteAppointmentCommand(id), ct);
        return NoContent();
    }

    [HttpPatch("{id:guid}/no-show")]
    public async Task<IActionResult> MarkNoShow(Guid id, CancellationToken ct)
    {
        await sender.Send(new MarkAppointmentNoShowCommand(id), ct);
        return NoContent();
    }

    [HttpGet("{id:guid}/follow-up-suggestions")]
    public async Task<IActionResult> GetFollowUpSuggestions(Guid id, CancellationToken ct)
    {
        var result = await sender.Send(new GetFollowUpSuggestionsQuery(id), ct);
        return Ok(result);
    }
}
