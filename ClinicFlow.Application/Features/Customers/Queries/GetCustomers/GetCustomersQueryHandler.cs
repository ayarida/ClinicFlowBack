using ClinicFlow.Application.Common.Interfaces;
using ClinicFlow.Application.Features.Customers.DTOs;
using ClinicFlow.Domain.Interfaces.Repositories;
using MediatR;

namespace ClinicFlow.Application.Features.Customers.Queries.GetCustomers;

public class GetCustomersQueryHandler(
    ICustomerRepository customerRepository,
    ICurrentClinicService currentClinic
) : IRequestHandler<GetCustomersQuery, IReadOnlyList<CustomerSummaryDto>>
{
    public async Task<IReadOnlyList<CustomerSummaryDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        var customers = await customerRepository.SearchAsync(
            currentClinic.ClinicId, request.SearchTerm, cancellationToken);

        return customers
            .Select(c => new CustomerSummaryDto(c.Id, c.Name, c.Phone, c.Email))
            .ToList();
    }
}
