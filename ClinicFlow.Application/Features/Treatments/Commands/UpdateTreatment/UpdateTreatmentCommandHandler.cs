using ClinicFlow.Application.Common.Exceptions;
using ClinicFlow.Application.Common.Interfaces;
using ClinicFlow.Domain.Interfaces;
using ClinicFlow.Domain.Entities;
using ClinicFlow.Domain.Interfaces.Repositories;
using MediatR;

namespace ClinicFlow.Application.Features.Treatments.Commands.UpdateTreatment;

public class UpdateTreatmentCommandHandler(
    ITreatmentRepository treatmentRepository,
    IUnitOfWork unitOfWork,
    ICurrentClinicService currentClinic
) : IRequestHandler<UpdateTreatmentCommand, Unit>
{
    public async Task<Unit> Handle(UpdateTreatmentCommand request, CancellationToken cancellationToken)
    {
        var treatment = await treatmentRepository.GetByIdAsync(
            request.Id, currentClinic.ClinicId, cancellationToken);

        if (treatment is null)
            throw new NotFoundException(nameof(Treatment), request.Id);

        var nameInUse = await treatmentRepository.NameExistsAsync(
            currentClinic.ClinicId, request.Name, excludeTreatmentId: request.Id, cancellationToken);

        if (nameInUse)
            throw new ConflictException($"A treatment named '{request.Name}' already exists.");

        treatment.Update(
            request.Name,
            request.Description,
            request.DurationMinutes,
            request.FollowUpIntervalDays,
            request.Price);

        treatmentRepository.Update(treatment);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
