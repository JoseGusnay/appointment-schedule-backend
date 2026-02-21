using Moq;
using VehicleAppointments.Application;
using VehicleAppointments.Domain;

namespace VehicleAppointments.Tests;

public class AppointmentServiceTests
{
    private readonly Mock<IAppointmentRepository> _repositoryMock;
    private readonly AppointmentService _service;

    public AppointmentServiceTests()
    {
        _repositoryMock = new Mock<IAppointmentRepository>();
        _service = new AppointmentService(_repositoryMock.Object);
    }

    [Fact]
    public async Task ScheduleAsync_ShouldFail_WhenDayIsWeekend()
    {
        var saturday = new DateTime(2025, 6, 28, 9, 0, 0);
        
        var result = await _service.ScheduleAsync("ABC-1234", saturday);

        Assert.False(result.Success);
        Assert.Equal("Las citas solo pueden ser de Lunes a Viernes.", result.Message);
    }

    [Fact]
    public async Task ScheduleAsync_ShouldFail_WhenTimeIsOutsideRange()
    {
        var outsideTime = new DateTime(2025, 6, 23, 15, 0, 0);
        
        var result = await _service.ScheduleAsync("ABC-1234", outsideTime);

        Assert.False(result.Success);
        Assert.Equal("El horario de atención es de 08:00 AM a 02:00 PM.", result.Message);
    }

    [Fact]
    public async Task ScheduleAsync_ShouldFail_WhenIntervalIsInvalid()
    {
        var invalidInterval = new DateTime(2025, 6, 23, 8, 15, 0);
        
        var result = await _service.ScheduleAsync("ABC-1234", invalidInterval);

        Assert.False(result.Success);
        Assert.Contains("intervalos de 30 minutos", result.Message);
    }

    [Fact]
    public async Task ScheduleAsync_ShouldFail_WhenSlotIsOccupied()
    {
        var validTime = new DateTime(2025, 6, 23, 10, 0, 0);
        _repositoryMock.Setup(r => r.IsSlotOccupiedAsync(validTime)).ReturnsAsync(true);
        
        var result = await _service.ScheduleAsync("ABC-1234", validTime);

        Assert.False(result.Success);
        Assert.Equal("Este horario ya está ocupado por otro vehículo.", result.Message);
    }

    [Fact]
    public async Task ScheduleAsync_ShouldSucceed_WhenAllRulesPass()
    {
        var validTime = new DateTime(2025, 6, 23, 11, 30, 0);
        _repositoryMock.Setup(r => r.IsSlotOccupiedAsync(validTime)).ReturnsAsync(false);
        
        var result = await _service.ScheduleAsync("ABC-1234", validTime);

        Assert.True(result.Success);
        Assert.Equal("Cita agendada correctamente.", result.Message);
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Appointment>()), Times.Once);
    }
}
