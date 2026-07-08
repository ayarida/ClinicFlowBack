using ClinicFlow.Domain.Enums;
using MediatR;

namespace ClinicFlow.Application.Features.Customers.Commands.CreateCustomer;

public record CreateCustomerCommand(
    string Name,
    string Phone,
    string? Email,
    DateOnly? DateOfBirth,
    Gender? Gender,
    string? Notes
) : IRequest<Guid>;
