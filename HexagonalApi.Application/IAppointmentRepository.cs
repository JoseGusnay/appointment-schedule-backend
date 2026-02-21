using VehicleAppointments.Domain;

namespace VehicleAppointments.Application;

public interface IAppointmentRepository
{
    Task<IEnumerable<Appointment>> GetByLicensePlateAsync(string licensePlate);
    Task<bool> IsSlotOccupiedAsync(DateTime scheduledAt);
    Task AddAsync(Appointment appointment);
}
