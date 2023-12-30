using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeBrowsPremiere.Entities
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }

        public User User { get; set; }

        [ForeignKey("Service")]
        public int ServiceId { get; set; }
        public Service? Service { get; set; } //link this to service when the service db is created

        public DateTime AppointmentEndTime { get { return AppointmentDate.AddMinutes(Service?.Duration ?? 0); } }

        //public DateTime? OpeningTime { get; set; }
        //public DateTime? ClosingTime { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(20)]

        public string? Status { get; set; }
        public string? Comments { get; set; }

    }
}
