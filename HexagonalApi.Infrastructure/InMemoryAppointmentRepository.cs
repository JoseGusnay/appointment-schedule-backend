using VehicleAppointments.Application;
using VehicleAppointments.Domain;

namespace VehicleAppointments.Infrastructure;

public class InMemoryAppointmentRepository : IAppointmentRepository
{
    private static readonly List<Appointment> _appointments = new();

    public Task<IEnumerable<Appointment>> GetByLicensePlateAsync(string licensePlate)
    {
        var history = _appointments
            .Where(a => a.LicensePlate == licensePlate)
            .OrderBy(a => a.ScheduledAt);
            
        return Task.FromResult<IEnumerable<Appointment>>(history);
    }

    public Task<bool> IsSlotOccupiedAsync(DateTime scheduledAt)
    {
        var exists = _appointments.Any(a => a.ScheduledAt == scheduledAt);
        return Task.FromResult(exists);
    }

    public Task AddAsync(Appointment appointment)
    {
        _appointments.Add(appointment);
        return Task.CompletedTask;
    }
}
