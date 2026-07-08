using ClinicFlow.Domain.Entities;

namespace ClinicFlow.Domain.Interfaces.Repositories;

public interface IStaffRepository
{
    Task<Staff?> GetByIdAsync(Guid id, Guid clinicId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Staff>> GetAllAsync(Guid clinicId, CancellationToken cancellationToken = default);
    Task<Staff?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task AddAsync(Staff staff, CancellationToken cancellationToken = default);
    void Update(Staff staff);
    Task<bool> EmailExistsAsync(Guid clinicId, string email, Guid? excludeStaffId, CancellationToken cancellationToken = default);
}
