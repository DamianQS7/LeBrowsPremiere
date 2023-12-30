namespace LeBrowsPremiere.Models
{
    public class SettingsViewModel
    {
        public DateTime AppointmentStartTime { get; set; } = DateTime.MinValue.AddHours(10);
        public DateTime AppointmentEndTime { get; set; } = DateTime.MinValue.AddHours(16);
        public int AppointmentIntervalInMinutes { get; set; } = 30;
    }
}
