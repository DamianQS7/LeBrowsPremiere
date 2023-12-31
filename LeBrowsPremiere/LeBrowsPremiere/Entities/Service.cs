﻿namespace LeBrowsPremiere.Entities
{
    public class Service
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Duration { get; set; }
        public double? Price { get; set; }

        public ICollection<Appointment>? Appointments { get; set; }
         
    }
}
