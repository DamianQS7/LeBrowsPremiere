using LeBrowsPremiere.Entities;
using LeBrowsPremiere.Enumerables;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeBrowsPremiere.Models
{
    public class ListAppointmentViewModel
    {
        public List<Appointment> Appointments { get; set; } = new();
    }
}
