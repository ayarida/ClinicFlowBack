using ClinicFlow.Domain.Enums;

namespace ClinicFlow.Application.Features.Staff.DTOs;

public record StaffDto(
    Guid Id,
    Guid ClinicId,
    string Name,
    string Email,
    StaffRole Role,
    bool IsActive);
