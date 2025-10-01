using System;
namespace HomeCareApp.Models
{
    public class AppointmentTask
    {
        public int TaskId { get; set; } //PK
        public String Description { get; set; } = string.Empty;
        public String Status { get; set; } = string.Empty;

        public int AppointmentId { get; set; } //FK to appointment

        //navigation properties
        public Appointment Appointment { get; set; }
         
    }
}