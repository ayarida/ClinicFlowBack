using ClinicFlow.Application.Common.Exceptions;
using ClinicFlow.Application.Common.Interfaces;
using ClinicFlow.Domain.Entities;
using ClinicFlow.Domain.Interfaces;
using ClinicFlow.Domain.Interfaces.Repositories;
using MediatR;

namespace ClinicFlow.Application.Features.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandHandler(
    ICustomerRepository customerRepository,
    IUnitOfWork unitOfWork,
    ICurrentClinicService currentClinic
) : IRequestHandler<CreateCustomerCommand, Guid>
{
    public async Task<Guid> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var phoneInUse = await customerRepository.PhoneExistsAsync(
            currentClinic.ClinicId, request.Phone, excludeCustomerId: null, cancellationToken);

        if (phoneInUse)
            throw new ConflictException($"A customer with phone '{request.Phone}' already exists.");

        var customer = Customer.Create(
            currentClinic.ClinicId,
            request.Name,
            request.Phone,
            request.Email,
            request.DateOfBirth,
            request.Gender,
            request.Notes);

        await customerRepository.AddAsync(customer, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return customer.Id;
    }
}
