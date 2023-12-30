using LeBrowsPremiere.Data;
using LeBrowsPremiere.Entities;
using LeBrowsPremiere.Enumerables;
using LeBrowsPremiere.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LeBrowsPremiere.Managers
{
    public class AppointmentManager
    {
        AppDbContext _appDbContext;
        EmailConfigurationModel? _emailConfiguration;

        public AppointmentManager(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public AppointmentManager(AppDbContext appDbContext, IConfiguration configuration)
        {
            _appDbContext = appDbContext;
            var emailConfiguration = new EmailConfigurationModel();
            emailConfiguration.NetworkCredentialEmail = configuration["NetworkCredentialEmail"] ?? "";
            emailConfiguration.NetworkCredentialPassword = configuration["NetworkCredentialPassword"] ?? "";
            emailConfiguration.SmtpClientPort = Int32.Parse(configuration["SmtpClientPort"] ?? "0");
            emailConfiguration.SmtpClientHost = configuration["SmtpClientHost"] ?? "";
            _emailConfiguration = emailConfiguration;
        }
        public async Task CancelAppointment(Appointment? appointment)
        {
            appointment.Status = AppointmentStatusTypes.Cancelled.ToString();
            _appDbContext.Appointments.Update(appointment);
            await _appDbContext.SaveChangesAsync();
            await SendEmail(appointment.Id, "Cancel");
        }

        public async Task Update(BookAppointmentViewModel viewModel)
        {
            Appointment? appointment = await GetAppointmentById(viewModel.Appointment.Id);
            if (appointment == null) return;

            appointment.AppointmentDate = viewModel.AppointmentDate
                            .AddHours(viewModel.AppointmentTime.Hour)
                            .AddMinutes(viewModel.AppointmentTime.Minute);
            appointment.ServiceId = viewModel.Appointment.ServiceId;
            appointment.CreatedDate = DateTime.Now.Date;
            appointment.Status = AppointmentStatusTypes.Confirmed.ToString();
            _appDbContext.Appointments.Update(appointment);
            await _appDbContext.SaveChangesAsync();
            await SendEmail(appointment.Id, "Update");
        }

        public async Task Add(BookAppointmentViewModel viewModel)
        {
            viewModel.Appointment.AppointmentDate = viewModel.AppointmentDate
                            .AddHours(viewModel.AppointmentTime.Hour)
                            .AddMinutes(viewModel.AppointmentTime.Minute);

            viewModel.Appointment.CreatedDate = DateTime.Now.Date;
            viewModel.Appointment.Status = AppointmentStatusTypes.Confirmed.ToString();
            _appDbContext.Appointments.Add(viewModel.Appointment);

            await _appDbContext.SaveChangesAsync();
            await SendEmail(viewModel.Appointment.Id, "Booking");
        }

        public async Task<BookAppointmentViewModel> GetEditViewModel(Appointment appointment)
        {
            BookAppointmentViewModel viewModel = new();
            viewModel.Appointment = appointment;

            viewModel.AppointmentDate = appointment.AppointmentDate.Date;
            viewModel.AppointmentTime = DateTime.MinValue
                .AddHours(appointment.AppointmentDate.Hour)
                .AddMinutes(appointment.AppointmentDate.Minute);
            viewModel.Services = await _appDbContext.Services.OrderBy(s => s.Name).ToListAsync();
            var settings = await _appDbContext.Settings.ToDictionaryAsync(s => s.Code, s => s.Value);
            viewModel.AppointmentStartTime = Convert.ToDateTime(settings["AppointmentStartTime"]);
            viewModel.AppointmentEndTime = Convert.ToDateTime(settings["AppointmentEndTime"]);
            viewModel.AppointmentIntervalInMinutes = Convert.ToInt32(settings["AppointmentIntervalInMinutes"]);
            return viewModel;
        }

        public async Task<BookAppointmentViewModel> GetAddViewModel(int serviceId)
        {
            BookAppointmentViewModel viewModel = new();
            var settings = await _appDbContext.Settings.ToDictionaryAsync(s => s.Code, s => s.Value);
            viewModel.Appointment = new();
            viewModel.AppointmentDate = DateTime.Now.Date;
            viewModel.Services = await _appDbContext.Services.OrderBy(s => s.Name).ToListAsync();
            viewModel.Appointment.ServiceId = serviceId;
            viewModel.AppointmentStartTime = Convert.ToDateTime(settings["AppointmentStartTime"]);
            viewModel.AppointmentEndTime = Convert.ToDateTime(settings["AppointmentEndTime"]);
            viewModel.AppointmentIntervalInMinutes = Convert.ToInt32(settings["AppointmentIntervalInMinutes"]);
            viewModel.AppointmentTime = viewModel.AppointmentStartTime;
            return viewModel;
        }
        public async Task<ListAppointmentViewModel> GetListAppointmentViewModel(string userId)
        {
            ListAppointmentViewModel appointmentViewModel = new ListAppointmentViewModel();
            appointmentViewModel.Appointments = await _appDbContext.Appointments
                .Include(a => a.Service)
                .Where(a => a.UserId == userId && a.Status != AppointmentStatusTypes.Cancelled.ToString())
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
            return appointmentViewModel;
        }

        public async Task<ListAppointmentViewModel> GetAllAppointment()
        {
            ListAppointmentViewModel appointmentViewModel = new ListAppointmentViewModel();
            appointmentViewModel.Appointments = await _appDbContext.Appointments
                .Include(a => a.Service)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
            return appointmentViewModel;
        }

        public async Task<Appointment?> GetAppointmentById(int id)
        {
            return await _appDbContext.Appointments.Include(a => a.Service).FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<bool> IsValid(BookAppointmentViewModel viewModel)
        {
            //check for schedule conflict
            var appointmentDatePlusOneDay = viewModel.AppointmentDate.AddDays(1);
            var sameDayAppointments = await _appDbContext.Appointments.Include(a => a.Service)
                .Where(a => a.Id != viewModel.Appointment.Id
                            && a.AppointmentDate >= viewModel.AppointmentDate
                            && a.AppointmentDate < appointmentDatePlusOneDay
                            && a.Status == AppointmentStatusTypes.Confirmed.ToString())
                .ToListAsync();
            var service = _appDbContext.Services.FirstOrDefault(s => s.Id == viewModel.Appointment.ServiceId);
            var appointmentStartTime = viewModel.AppointmentDate
                .AddHours(viewModel.AppointmentTime.Hour)
                .AddMinutes(viewModel.AppointmentTime.Minute);
            var appointmentEndTime = appointmentStartTime.AddMinutes(service?.Duration ?? 0);
            var hasNoScheduleConflict = !sameDayAppointments.Any(a =>
                                                        (a.AppointmentDate >= appointmentStartTime && a.AppointmentDate <= appointmentEndTime)
                                                        || (a.AppointmentEndTime >= appointmentStartTime && a.AppointmentEndTime <= appointmentEndTime));

            return hasNoScheduleConflict;
        }
        private async Task SendEmail(int appointmentId, string appointmentType)
        {
            var appointment = await _appDbContext.Appointments
                            .Include(a => a.User)
                            .Include(a => a.Service)
                            .FirstOrDefaultAsync(a => a.Id == appointmentId);
            if (appointment == null) return;

            var customer = await _appDbContext.Customers.FirstOrDefaultAsync(c => c.Email == appointment.User.UserName);
            if (customer == null) return;

            var clientEmailInfo = new ClientEmailInformationModel();
            clientEmailInfo.FirstName = customer.FirstName ?? "";
            clientEmailInfo.LastName = customer.LastName ?? "";
            clientEmailInfo.Email = customer.Email ?? "";
            clientEmailInfo.MessageSubject = $"Lebrows Premier {appointment.Service?.Name} Appointment {appointmentType} Confirmation";
            clientEmailInfo.MessageBody = $"Hello {clientEmailInfo.FirstName},";
            clientEmailInfo.MessageBody += $"{Environment.NewLine}{Environment.NewLine}Please be advised that your {appointment.Service?.Name} appointment on {appointment.AppointmentDate.Date} from {appointment.AppointmentDate.ToString("hh:mm tt")} to {appointment.AppointmentEndTime.ToString("hh:mm tt")}";
            clientEmailInfo.MessageBody += $"{Environment.NewLine}{Environment.NewLine}Thank you,";
            clientEmailInfo.MessageBody += $"{Environment.NewLine}Lebrows Premiere Management";
            await EmailManager.SendEmail(_emailConfiguration, clientEmailInfo);

        }

        public static string AppointmentColor(string appointmentStatusText)
        {
            var appointmentStatus = Enum.Parse<AppointmentStatusTypes>(appointmentStatusText);
            var color = "black";
            switch (appointmentStatus)
            {
                case AppointmentStatusTypes.Confirmed:
                    color = "green";
                    break;
                case AppointmentStatusTypes.Completed:
                    color = "gray";
                    break;
                case AppointmentStatusTypes.Cancelled:
                    color = "red";
                    break;
            }
            return color;
        }

        public async Task<Service> GetService(int serviceId)
        {
            var service = await _appDbContext.Services.FirstOrDefaultAsync(s => s.Id == serviceId);
            if (service == null) return new Service();
            return service;
        }
    }
}
