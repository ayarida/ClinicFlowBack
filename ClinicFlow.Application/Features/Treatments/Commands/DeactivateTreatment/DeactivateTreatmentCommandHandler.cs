using ClinicFlow.Application.Common.Exceptions;
using ClinicFlow.Application.Common.Interfaces;
using ClinicFlow.Domain.Interfaces;
using ClinicFlow.Domain.Interfaces.Repositories;
using ClinicFlow.Domain.Entities;
using MediatR;

namespace ClinicFlow.Application.Features.Treatments.Commands.DeactivateTreatment;

public class DeactivateTreatmentCommandHandler(
    ITreatmentRepository treatmentRepository,
    IUnitOfWork unitOfWork,
    ICurrentClinicService currentClinic
) : IRequestHandler<DeactivateTreatmentCommand, Unit>
{
    public async Task<Unit> Handle(DeactivateTreatmentCommand request, CancellationToken cancellationToken)
    {
        var treatment = await treatmentRepository.GetByIdAsync(
            request.Id, currentClinic.ClinicId, cancellationToken);

        if (treatment is null)
            throw new NotFoundException(nameof(Treatment), request.Id);

        if (!treatment.IsActive)
            throw new ConflictException($"Treatment '{treatment.Name}' is already inactive.");

        treatment.Deactivate();
        treatmentRepository.Update(treatment);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
