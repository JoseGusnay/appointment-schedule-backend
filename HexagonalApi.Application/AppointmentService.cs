using VehicleAppointments.Domain;

namespace VehicleAppointments.Application;

public class AppointmentService
{
    private readonly IAppointmentRepository _repository;

    public AppointmentService(IAppointmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Appointment>> GetHistoryAsync(string licensePlate)
    {
        return await _repository.GetByLicensePlateAsync(licensePlate.ToUpper());
    }

    public async Task<(bool Success, string Message)> ScheduleAsync(string licensePlate, DateTime scheduledAt)
    {
        var appointment = new Appointment
        {
            Id = Guid.NewGuid(),
            LicensePlate = licensePlate.ToUpper(),
            ScheduledAt = scheduledAt
        };

        var validation = appointment.Validate();
        if (!validation.IsValid) return (false, validation.Message);

        var isOccupied = await _repository.IsSlotOccupiedAsync(scheduledAt);
        if (isOccupied) return (false, "Este horario ya está ocupado por otro vehículo.");

        await _repository.AddAsync(appointment);
        return (true, "Cita agendada correctamente.");
    }
}
