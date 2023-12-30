using LeBrowsPremiere.Data;
using LeBrowsPremiere.Entities;
using LeBrowsPremiere.Enumerables;
using LeBrowsPremiere.Managers;
using LeBrowsPremiere.Models;
using Moq;
using EntityFramework.Testing;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace LeBrowsPremiereTests
{
    public class AppointmentManagerTests
    {
        private readonly DbContextOptions<AppDbContext> _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "AppointmentManagerTest")
            .Options;

        [Fact]
        public async Task Add_ValidViewModel_AddsAppointment()
        {
            // Arrange
            var viewModel = new BookAppointmentViewModel
            {
                Appointment = new Appointment
                {
                    ServiceId = 1,
                    UserId = "testuser",
                    AppointmentDate = DateTime.Today,

                    Status = AppointmentStatusTypes.Confirmed.ToString()
                },
                AppointmentDate = DateTime.Today,

                AppointmentIntervalInMinutes = 30,
                AppointmentStartTime = DateTime.Today.AddHours(9),
                AppointmentEndTime = DateTime.Today.AddHours(17),
                Services = new List<Service> { new Service { Id = 1, Name = "Service 1", Duration = 60 } }
            };

            using var context = new AppDbContext(_options);
            var appointmentManager = new AppointmentManager(context);
            var appointment = context.Appointments.FirstOrDefault(a => a.Id == 1);
            if (appointment != null) context.Appointments.Remove(appointment);

            // Act
            await appointmentManager.Add(viewModel);
            var savedAppointment = await context.Appointments.FirstOrDefaultAsync();

            // Assert
            Assert.NotNull(savedAppointment);
            Assert.Equal(1, savedAppointment.ServiceId);
            Assert.Equal("testuser", savedAppointment.UserId);
            Assert.Equal(DateTime.Today.Date, savedAppointment.AppointmentDate);
            Assert.Equal(AppointmentStatusTypes.Confirmed.ToString(), savedAppointment.Status);
        }

        [Fact]
        public async Task Update_ValidViewModel_UpdatesAppointment()
        {
            // Arrange
            var existingAppointment = new Appointment
            {
                Id = 1,
                ServiceId = 1,
                UserId = "testuser",
                AppointmentDate = DateTime.Today,
                Status = AppointmentStatusTypes.Confirmed.ToString()
            };
            using var context = new AppDbContext(_options);
            var appointment = context.Appointments.FirstOrDefault(a => a.Id == 1);
            if (appointment != null) context.Appointments.Remove(appointment);
            context.Appointments.Add(existingAppointment);
            await context.SaveChangesAsync();

            var viewModel = new BookAppointmentViewModel
            {
                Appointment = new Appointment
                {
                    Id = 1,
                    ServiceId = 2,
                    UserId = "testuser",
                    AppointmentDate = DateTime.Today.AddDays(1),
                    Status = AppointmentStatusTypes.Confirmed.ToString()
                }
            };

            var appointmentManager = new AppointmentManager(context);

            // Act
            await appointmentManager.Update(viewModel);

            // Assert
            Assert.NotNull(viewModel);
            Assert.Equal(2, viewModel.Appointment.ServiceId);
            Assert.Equal(DateTime.Today.AddDays(1).Date, viewModel.Appointment.AppointmentDate);
            Assert.Equal(AppointmentStatusTypes.Confirmed.ToString(), viewModel.Appointment.Status);
        }

        [Fact]
        public async Task CancelAppointment_ValidAppointment_CancelsAppointment()
        {
            // Arrange
            var existingAppointment = new Appointment
            {
                Id = 1,
                ServiceId = 1,
                UserId = "testuser",
                AppointmentDate = DateTime.Today,
                Status = AppointmentStatusTypes.Confirmed.ToString()
            };
            using var context = new AppDbContext(_options);
            var appointment = context.Appointments.FirstOrDefault(a => a.Id == 1);
            if (appointment != null) context.Appointments.Remove(appointment);
            context.Appointments.Add(existingAppointment);
            await context.SaveChangesAsync();

            var appointmentManager = new AppointmentManager(context);

            // Act
            await appointmentManager.CancelAppointment(existingAppointment);
            var cancelledAppointment = await context.Appointments.FirstOrDefaultAsync(a => a.Id == 1);

            // Assert
            Assert.NotNull(cancelledAppointment);
            Assert.Equal(AppointmentStatusTypes.Cancelled.ToString(), cancelledAppointment.Status);

        }
                






        //ColorCoding

        [Fact]
    public void AppointmentColor_Confirmed_ReturnsGreen()
    {
        // Arrange
        var statusText = "Confirmed";

        // Act
        var result = AppointmentManager.AppointmentColor(statusText);

        // Assert
        Assert.Equal("green", result);
    }

    [Fact]
    public void AppointmentColor_Completed_ReturnsGray()
    {
        // Arrange
        var statusText = "Completed";

        // Act
        var result = AppointmentManager.AppointmentColor(statusText);

        // Assert
        Assert.Equal("gray", result);
    }

    [Fact]
    public void AppointmentColor_Cancelled_ReturnsRed()
    {
        // Arrange
        var statusText = "Cancelled";

        // Act
        var result = AppointmentManager.AppointmentColor(statusText);

        // Assert
        Assert.Equal("red", result);
    }

    [Fact]
    public void AppointmentColor_InvalidStatusText_ThrowsArgumentException()
    {
        // Arrange
        var invalidStatusText = "Invalid";

        // Act and Assert
        Assert.Throws<ArgumentException>(() => AppointmentManager.AppointmentColor(invalidStatusText));
    }

}
}