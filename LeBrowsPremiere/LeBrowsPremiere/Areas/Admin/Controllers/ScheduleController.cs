using LeBrowsPremiere.Data;
using LeBrowsPremiere.Managers;
using LeBrowsPremiere.Models;
using LeBrowsPremiere.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LeBrowsPremiere.Areas.Admin.Controllers
{
    public class ScheduleController : Controller
    {
        private AppDbContext _appDbContext;
        private AppointmentManager _appointmentManager;
        private SettingsManager _settingsManager;
        public ScheduleController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _appointmentManager = new AppointmentManager(appDbContext);
            _settingsManager = new SettingsManager(appDbContext);

        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Appointments()
        {
            ListAppointmentViewModel appointmentViewModel = await _appointmentManager.GetAllAppointment();

            return View(appointmentViewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet()]
        public async Task<IActionResult> Settings()
        {
            SettingsViewModel viewModel = await _settingsManager.GetViewModel();
            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost()]
        public async Task<IActionResult> Settings(SettingsViewModel viewModel)
        {
            try
            {
                await _settingsManager.Update(viewModel);
                TempData["toastr:success"] = AdminResource.SettingsSaveSuccessMessage;
            }
            catch
            {
                TempData["toastr:error"] = AdminResource.SettingsSaveErrorMessage;
            }

            return await Settings();
        }
    }
}
