using LeBrowsPremiere.Data;
using LeBrowsPremiere.Entities;
using LeBrowsPremiere.Enumerables;
using LeBrowsPremiere.Managers;
using LeBrowsPremiere.Models;
using LeBrowsPremiere.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

namespace LeBrowsPremiere.Controllers
{
    public class AppointmentController : Controller
    {
        private AppointmentManager _appointmentManager;
        public AppointmentController(IConfiguration configuration, AppDbContext appDbContext)
        {
            _appointmentManager = new AppointmentManager(appDbContext, configuration);
        }

        [Authorize]
        public async Task<IActionResult> ListAppointment()
        {
            ClaimsPrincipal currentUser = this.User;
            ListAppointmentViewModel appointmentViewModel = await _appointmentManager.GetListAppointmentViewModel(currentUser.FindFirst(ClaimTypes.NameIdentifier).Value);

            return View(appointmentViewModel);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> BookAppointment(int? id, int? serviceId)
        {
            BookAppointmentViewModel viewModel = new();
            Appointment? appointment = await _appointmentManager.GetAppointmentById(id ?? 0);
            if (appointment == null) viewModel = await _appointmentManager.GetAddViewModel(serviceId ?? 0);
            else viewModel = await _appointmentManager.GetEditViewModel(appointment);
            return View(viewModel);
        }

        [Authorize]
        [HttpPost()]
        public async Task<IActionResult> BookAppointment(BookAppointmentViewModel viewModel)
        {
            ClaimsPrincipal currentUser = this.User;
            viewModel.Appointment.UserId = currentUser.FindFirstValue(ClaimTypes.NameIdentifier);
            var isValid = await _appointmentManager.IsValid(viewModel);
            if (isValid)
            {
                var isAdd = viewModel.Appointment.Id == 0;
                if (isAdd)
                {
                    await _appointmentManager.Add(viewModel);
                }
                else
                {
                    await _appointmentManager.Update(viewModel);
                }

                TempData["success"] = AppointmentResource.SaveSuccessMessage;
            }
            else
            {
                TempData["error"] = AppointmentResource.DateIsNotValidMessage;
            }

            return RedirectToAction("ListAppointment", "Appointment");
        }

        [Authorize]
        [HttpGet()]
        public async Task<IActionResult> CancelAppointment(int id)
        {
            Appointment? appointment = await _appointmentManager.GetAppointmentById(id);
            if (appointment == null || appointment.Status != AppointmentStatusTypes.Confirmed.ToString())
            {
                TempData["toastr:error"] = AppointmentResource.CancelFailedMessage;
                return RedirectToListAppointment();
            }

            await _appointmentManager.CancelAppointment(appointment);
            TempData["toastr:info"] = AppointmentResource.CancelMessage;
            return RedirectToListAppointment();
        }

        [Authorize]
        [HttpGet()]
        public async Task<IActionResult> ValidateAppointmentDateTime(DateTime appointmentDate, DateTime appointmentTime, int serviceId, int appointmentId)
        {
            var viewModel = new BookAppointmentViewModel();
            viewModel.AppointmentDate = appointmentDate;
            viewModel.AppointmentTime = appointmentTime;
            viewModel.Appointment = new Appointment();
            viewModel.Appointment.Id = appointmentId;
            viewModel.Appointment.ServiceId = serviceId;
            var isValid = await _appointmentManager.IsValid(viewModel);
            var message = isValid ? AppointmentResource.DateIsValidMessage : AppointmentResource.DateIsNotValidMessage;
            return await Task.FromResult(Json(new { success = isValid, message = message }));
        }
        [Authorize]
        [HttpGet()]
        public async Task<IActionResult> GetService(int serviceId)
        {
            try
            {
                var service = await _appointmentManager.GetService(serviceId);
                var description = $"Description: {service.Description}";
                var price = $"Price: {service.Price}";
                var duration = $"Duration: {service.Duration}";
                return await Task.FromResult(Json(new {
                    success = true,
                    serviceDescription = description,
                    servicePrice = price,
                    serviceDuration = duration }));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(Json(new { success = false, message = ex.Message }));
            }
        }


        private RedirectToActionResult RedirectToListAppointment()
        {
            return RedirectToAction("ListAppointment", "Appointment");
        }
    }
}
