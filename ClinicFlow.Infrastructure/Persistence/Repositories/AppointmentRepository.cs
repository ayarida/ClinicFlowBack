using ClinicFlow.Domain.Entities;
using ClinicFlow.Domain.Enums;
using ClinicFlow.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ClinicFlow.Infrastructure.Persistence.Repositories;

public class AppointmentRepository(ClinicFlowDbContext context) : IAppointmentRepository
{
    public async Task<Appointment?> GetByIdAsync(Guid id, Guid clinicId, CancellationToken cancellationToken = default)
    {
        return await context.Appointments
            .Include(a => a.Customer)
            .Include(a => a.Treatments).ThenInclude(t => t.Treatment)
            .FirstOrDefaultAsync(a => a.Id == id && a.ClinicId == clinicId, cancellationToken);
    }

    public async Task<IReadOnlyList<Appointment>> GetAllAsync(
        Guid clinicId,
        DateOnly? date,
        Guid? customerId,
        CancellationToken cancellationToken = default)
    {
        var query = context.Appointments
            .Include(a => a.Customer)
            .Include(a => a.Treatments).ThenInclude(t => t.Treatment)
            .Where(a => a.ClinicId == clinicId);

        if (date.HasValue)
            query = query.Where(a => a.Date == date.Value);

        if (customerId.HasValue)
            query = query.Where(a => a.CustomerId == customerId.Value);

        return await query
            .OrderBy(a => a.Date)
            .ThenBy(a => a.TimeSlot)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Appointment>> GetByMonthAsync(
        Guid clinicId,
        int year,
        int month,
        CancellationToken cancellationToken = default)
    {
        var start = new DateOnly(year, month, 1);
        var end = start.AddMonths(1).AddDays(-1);

        return await context.Appointments
            .Include(a => a.Customer)
            .Include(a => a.Treatments).ThenInclude(t => t.Treatment)
            .Where(a => a.ClinicId == clinicId && a.Date >= start && a.Date <= end)
            .OrderBy(a => a.Date)
            .ThenBy(a => a.TimeSlot)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Appointment appointment, CancellationToken cancellationToken = default)
    {
        await context.Appointments.AddAsync(appointment, cancellationToken);
    }

    public void Update(Appointment appointment)
    {
        context.Appointments.Update(appointment);
    }

    public void ReplaceTreatments(Appointment appointment, IEnumerable<AppointmentTreatment> newTreatments)
    {
        context.AppointmentTreatments.RemoveRange(appointment.Treatments);
        appointment.Treatments.Clear();

        var list = newTreatments.ToList();
        context.AppointmentTreatments.AddRange(list);
        appointment.Treatments.AddRange(list);
    }

    public async Task<bool> TimeSlotTakenAsync(
        Guid clinicId,
        DateOnly date,
        TimeOnly timeSlot,
        Guid? excludeAppointmentId,
        CancellationToken cancellationToken = default)
    {
        return await context.Appointments
            .AnyAsync(a =>
                a.ClinicId == clinicId &&
                a.Date == date &&
                a.TimeSlot == timeSlot &&
                a.Status != AppointmentStatus.Cancelled &&
                a.Id != excludeAppointmentId,
                cancellationToken);
    }
}
