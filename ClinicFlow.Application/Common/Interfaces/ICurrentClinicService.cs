namespace ClinicFlow.Application.Common.Interfaces;

public interface ICurrentClinicService
{
    Guid ClinicId { get; }
    string UserEmail { get; }
}
