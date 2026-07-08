namespace ClinicFlow.Application.Features.Customers.DTOs;

public record CustomerSummaryDto(
    Guid Id,
    string Name,
    string Phone,
    string? Email
);
