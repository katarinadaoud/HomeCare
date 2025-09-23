using System;
namespace HomeCareApp.Models
{
    public class Task
    {
        public int Task_id { get; set; }
        public String Description { get; set; } = string.Empty;
        public String Status { get; set; } = string.Empty;
        public int Appointment_id { get; set; } //FK to appointment
         
    }
}