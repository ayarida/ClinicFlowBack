using ClinicFlow.Domain.Enums;

namespace ClinicFlow.Application.Features.Customers.DTOs;

public record CustomerDto(
    Guid Id,
    string Name,
    string Phone,
    string? Email,
    DateOnly? DateOfBirth,
    Gender? Gender,
    string? Notes,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
