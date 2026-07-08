using ClinicFlow.Domain.Entities;
using ClinicFlow.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ClinicFlow.Infrastructure.Persistence.Repositories;

public class CustomerRepository(ClinicFlowDbContext context) : ICustomerRepository
{
    public async Task<Customer?> GetByIdAsync(Guid id, Guid clinicId, CancellationToken cancellationToken = default)
    {
        return await context.Customers
            .FirstOrDefaultAsync(c => c.Id == id && c.ClinicId == clinicId, cancellationToken);
    }

    public async Task<IReadOnlyList<Customer>> SearchAsync(Guid clinicId, string? searchTerm, CancellationToken cancellationToken = default)
    {
        var query = context.Customers
            .Where(c => c.ClinicId == clinicId);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(c =>
                c.Name.Contains(searchTerm) ||
                c.Phone.Contains(searchTerm));
        }

        return await query
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        await context.Customers.AddAsync(customer, cancellationToken);
    }

    public void Update(Customer customer)
    {
        context.Customers.Update(customer);
    }

    public async Task<bool> PhoneExistsAsync(Guid clinicId, string phone, Guid? excludeCustomerId, CancellationToken cancellationToken = default)
    {
        return await context.Customers
            .AnyAsync(c =>
                c.ClinicId == clinicId &&
                c.Phone == phone &&
                c.Id != excludeCustomerId,
                cancellationToken);
    }
}
