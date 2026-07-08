using ClinicFlow.Application.Common.Exceptions;
using ClinicFlow.Application.Common.Interfaces;
using ClinicFlow.Application.Features.Customers.DTOs;
using ClinicFlow.Domain.Entities;
using ClinicFlow.Domain.Interfaces.Repositories;
using MediatR;

namespace ClinicFlow.Application.Features.Customers.Queries.GetCustomerById;

public class GetCustomerByIdQueryHandler(
    ICustomerRepository customerRepository,
    ICurrentClinicService currentClinic
) : IRequestHandler<GetCustomerByIdQuery, CustomerDto>
{
    public async Task<CustomerDto> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetByIdAsync(
            request.Id, currentClinic.ClinicId, cancellationToken);

        if (customer is null)
            throw new NotFoundException(nameof(Customer), request.Id);

        return new CustomerDto(
            customer.Id,
            customer.Name,
            customer.Phone,
            customer.Email,
            customer.DateOfBirth,
            customer.Gender,
            customer.Notes,
            customer.CreatedAt,
            customer.UpdatedAt);
    }
}
