using ClinicFlow.Domain.Entities;
using ClinicFlow.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ClinicFlow.Infrastructure.Persistence.Repositories;

public class TreatmentRepository(ClinicFlowDbContext context) : ITreatmentRepository
{
    public async Task<Treatment?> GetByIdAsync(Guid id, Guid clinicId, CancellationToken cancellationToken = default)
    {
        return await context.Treatments
            .FirstOrDefaultAsync(t => t.Id == id && t.ClinicId == clinicId, cancellationToken);
    }

    public async Task<IReadOnlyList<Treatment>> GetAllActiveAsync(Guid clinicId, CancellationToken cancellationToken = default)
    {
        return await context.Treatments
            .Where(t => t.ClinicId == clinicId && t.IsActive)
            .OrderBy(t => t.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Treatment treatment, CancellationToken cancellationToken = default)
    {
        await context.Treatments.AddAsync(treatment, cancellationToken);
    }

    public void Update(Treatment treatment)
    {
        context.Treatments.Update(treatment);
    }

    public async Task<bool> NameExistsAsync(Guid clinicId, string name, Guid? excludeTreatmentId, CancellationToken cancellationToken = default)
    {
        return await context.Treatments
            .AnyAsync(t =>
                t.ClinicId == clinicId &&
                t.Name == name &&
                t.Id != excludeTreatmentId,
                cancellationToken);
    }
}
