using LeBrowsPremiere.Data;
using LeBrowsPremiere.Models;
using Microsoft.EntityFrameworkCore;

namespace LeBrowsPremiere.Managers
{
    public class SettingsManager
    {
        AppDbContext _appDbContext;
        public SettingsManager(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<SettingsViewModel> GetViewModel()
        {
            var settings = await _appDbContext.Settings.ToDictionaryAsync(s => s.Code, s => s.Value);
            var viewModel = new SettingsViewModel();
            viewModel.AppointmentStartTime = Convert.ToDateTime(settings["AppointmentStartTime"]);
            viewModel.AppointmentEndTime = Convert.ToDateTime(settings["AppointmentEndTime"]);
            viewModel.AppointmentIntervalInMinutes = Convert.ToInt32(settings["AppointmentIntervalInMinutes"]);
            return viewModel;
        }

        public async Task Update(SettingsViewModel viewModel)
        {
            var settingsDictionary = await _appDbContext.Settings.ToDictionaryAsync(s => s.Code, s => s);
            settingsDictionary["AppointmentStartTime"].Value = viewModel.AppointmentStartTime.ToString();
            settingsDictionary["AppointmentEndTime"].Value = viewModel.AppointmentEndTime.ToString();
            settingsDictionary["AppointmentIntervalInMinutes"].Value = viewModel.AppointmentIntervalInMinutes.ToString();
            var settings = settingsDictionary.Select(s => s.Value).ToList();
            settings.ForEach(s =>
                {
                    s.UpdatedBy = "admin";
                    s.UpdatedDate = DateTime.Now;
                }
            );
            _appDbContext.Settings.UpdateRange(settings);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
