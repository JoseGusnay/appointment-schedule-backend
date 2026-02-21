namespace VehicleAppointments.Domain;

public class Appointment
{
    public Guid Id { get; set; }
    public string LicensePlate { get; set; } = string.Empty;
    public DateTime ScheduledAt { get; set; }

    public (bool IsValid, string Message) Validate()
    {
        // Convertimos a la hora de Ecuador (UTC-5)
        // Nota: En Docker (Linux) usamos "America/Guayaquil". En Windows "SA Pacific Standard Time".
        TimeZoneInfo ecuadorZone;
        try 
        {
            ecuadorZone = TimeZoneInfo.FindSystemTimeZoneById("America/Guayaquil");
        }
        catch 
        {
            ecuadorZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
        }

        var ecuadorTime = TimeZoneInfo.ConvertTimeFromUtc(ScheduledAt.ToUniversalTime(), ecuadorZone);

        if (ecuadorTime.DayOfWeek == DayOfWeek.Saturday || ecuadorTime.DayOfWeek == DayOfWeek.Sunday)
            return (false, "Las citas solo pueden ser de Lunes a Viernes.");

        var time = ecuadorTime.TimeOfDay;
        if (time < TimeSpan.FromHours(8) || time >= TimeSpan.FromHours(14))
            return (false, "El horario de atención es de 08:00 AM a 02:00 PM (Hora Ecuador).");

        if (ecuadorTime.Minute != 0 && ecuadorTime.Minute != 30 || ecuadorTime.Second != 0)
            return (false, "Las citas deben programarse en intervalos de 30 minutos (ej: 08:00, 08:30).");

        return (true, string.Empty);
    }
}
