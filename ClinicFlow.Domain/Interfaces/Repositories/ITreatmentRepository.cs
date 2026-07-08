using ClinicFlow.Domain.Entities;

namespace ClinicFlow.Domain.Interfaces.Repositories;

public interface ITreatmentRepository
{
    Task<Treatment?> GetByIdAsync(Guid id, Guid clinicId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Treatment>> GetAllActiveAsync(Guid clinicId, CancellationToken cancellationToken = default);
    Task AddAsync(Treatment treatment, CancellationToken cancellationToken = default);
    void Update(Treatment treatment);
    Task<bool> NameExistsAsync(Guid clinicId, string name, Guid? excludeTreatmentId, CancellationToken cancellationToken = default);
}
