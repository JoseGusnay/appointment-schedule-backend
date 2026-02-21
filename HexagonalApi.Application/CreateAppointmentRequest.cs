namespace VehicleAppointments.Application;

public record CreateAppointmentRequest(string LicensePlate, DateTime ScheduledAt);
