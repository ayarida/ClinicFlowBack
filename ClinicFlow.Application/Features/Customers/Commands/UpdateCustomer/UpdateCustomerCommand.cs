using ClinicFlow.Domain.Enums;
using MediatR;

namespace ClinicFlow.Application.Features.Customers.Commands.UpdateCustomer;

public record UpdateCustomerCommand(
    Guid Id,
    string Name,
    string Phone,
    string? Email,
    DateOnly? DateOfBirth,
    Gender? Gender,
    string? Notes
) : IRequest<Unit>;
