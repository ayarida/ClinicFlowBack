using ClinicFlow.Application.Common.Exceptions;
using ClinicFlow.Application.Common.Interfaces;
using ClinicFlow.Domain.Interfaces;
using ClinicFlow.Domain.Interfaces.Repositories;
using MediatR;

namespace ClinicFlow.Application.Features.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommandHandler(
    ICustomerRepository customerRepository,
    IUnitOfWork unitOfWork,
    ICurrentClinicService currentClinic
) : IRequestHandler<UpdateCustomerCommand, Unit>
{
    public async Task<Unit> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetByIdAsync(
            request.Id, currentClinic.ClinicId, cancellationToken);

        if (customer is null)
            throw new NotFoundException(nameof(customer), request.Id);

        var phoneInUse = await customerRepository.PhoneExistsAsync(
            currentClinic.ClinicId, request.Phone, excludeCustomerId: request.Id, cancellationToken);

        if (phoneInUse)
            throw new ConflictException($"A customer with phone '{request.Phone}' already exists.");

        customer.Update(
            request.Name,
            request.Phone,
            request.Email,
            request.DateOfBirth,
            request.Gender,
            request.Notes);

        customerRepository.Update(customer);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
