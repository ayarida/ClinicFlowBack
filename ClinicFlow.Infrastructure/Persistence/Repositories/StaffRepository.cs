using ClinicFlow.Domain.Entities;
using ClinicFlow.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ClinicFlow.Infrastructure.Persistence.Repositories;

public class StaffRepository(ClinicFlowDbContext context) : IStaffRepository
{
    public async Task<Staff?> GetByIdAsync(Guid id, Guid clinicId, CancellationToken cancellationToken = default)
    {
        return await context.Staff
            .FirstOrDefaultAsync(s => s.Id == id && s.ClinicId == clinicId, cancellationToken);
    }

    public async Task<IReadOnlyList<Staff>> GetAllAsync(Guid clinicId, CancellationToken cancellationToken = default)
    {
        return await context.Staff
            .Where(s => s.ClinicId == clinicId)
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Staff?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await context.Staff
            .FirstOrDefaultAsync(s => s.Email == email, cancellationToken);
    }

    public async Task AddAsync(Staff staff, CancellationToken cancellationToken = default)
    {
        await context.Staff.AddAsync(staff, cancellationToken);
    }

    public void Update(Staff staff)
    {
        context.Staff.Update(staff);
    }

    public async Task<bool> EmailExistsAsync(Guid clinicId, string email, Guid? excludeStaffId, CancellationToken cancellationToken = default)
    {
        return await context.Staff
            .AnyAsync(s => s.ClinicId == clinicId && s.Email == email && s.Id != excludeStaffId, cancellationToken);
    }
}
