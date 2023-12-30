using LeBrowsPremiere.Entities;

namespace LeBrowsPremiere.Models
{
    public class BookAppointmentViewModel
    {
        public Appointment Appointment { get; set; }

        public List<Service> Services { get; set; }

        public DateTime AppointmentDate { get; set; }
        public DateTime AppointmentTime { get; set; }

        public DateTime AppointmentStartTime { get; set; } = DateTime.MinValue.AddHours(10);
        public DateTime AppointmentEndTime { get; set; } = DateTime.MinValue.AddHours(16);
        public int AppointmentIntervalInMinutes { get; set; } = 30;
    }
}
