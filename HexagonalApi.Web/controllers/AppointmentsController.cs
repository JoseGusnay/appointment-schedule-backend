using Microsoft.AspNetCore.Mvc;
using VehicleAppointments.Application;

namespace VehicleAppointments.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly AppointmentService _appointmentService;

    public AppointmentsController(AppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [HttpGet("{licensePlate}")]
    public async Task<IActionResult> GetHistory(string licensePlate)
    {
        var history = await _appointmentService.GetHistoryAsync(licensePlate);
        if (!history.Any()) return NotFound("No se encontraron citas para esta placa.");
        return Ok(history);
    }

    [HttpPost]
    public async Task<IActionResult> Schedule(CreateAppointmentRequest request)
    {
        var (success, message) = await _appointmentService.ScheduleAsync(request.LicensePlate, request.ScheduledAt);
        
        if (!success) return BadRequest(message);
        return Ok(message);
    }
}
