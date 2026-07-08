using ClinicFlow.Application.Common.Exceptions;
using ClinicFlow.Application.Common.Interfaces;
using ClinicFlow.Domain.Entities;
using ClinicFlow.Domain.Interfaces;
using ClinicFlow.Domain.Interfaces.Repositories;
using MediatR;

namespace ClinicFlow.Application.Features.Treatments.Commands.CreateTreatment;

public class CreateTreatmentCommandHandler(
    ITreatmentRepository treatmentRepository,
    IUnitOfWork unitOfWork,
    ICurrentClinicService currentClinic
) : IRequestHandler<CreateTreatmentCommand, Guid>
{
    public async Task<Guid> Handle(CreateTreatmentCommand request, CancellationToken cancellationToken)
    {
        var nameInUse = await treatmentRepository.NameExistsAsync(
            currentClinic.ClinicId, request.Name, excludeTreatmentId: null, cancellationToken);

        if (nameInUse)
            throw new ConflictException($"A treatment named '{request.Name}' already exists.");

        var treatment = Treatment.Create(
            currentClinic.ClinicId,
            request.Name,
            request.Description,
            request.DurationMinutes,
            request.FollowUpIntervalDays,
            request.Price);

        await treatmentRepository.AddAsync(treatment, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return treatment.Id;
    }
}
