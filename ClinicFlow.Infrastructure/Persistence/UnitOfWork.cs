using ClinicFlow.Domain.Interfaces;

namespace ClinicFlow.Infrastructure.Persistence;

public class UnitOfWork(ClinicFlowDbContext context) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return context.SaveChangesAsync(cancellationToken);
    }
}
