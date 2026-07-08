using ClinicFlow.Domain.Entities;
using ClinicFlow.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ClinicFlow.Infrastructure.Persistence.Repositories;

public class ClinicRepository(ClinicFlowDbContext context) : IClinicRepository
{
    public async Task<Clinic?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Clinics
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Clinic>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Clinics
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Clinic clinic, CancellationToken cancellationToken = default)
    {
        await context.Clinics.AddAsync(clinic, cancellationToken);
    }

    public void Update(Clinic clinic)
    {
        context.Clinics.Update(clinic);
    }

    public async Task<bool> NameExistsAsync(string name, Guid? excludeClinicId, CancellationToken cancellationToken = default)
    {
        return await context.Clinics
            .AnyAsync(c => c.Name == name && c.Id != excludeClinicId, cancellationToken);
    }
}
