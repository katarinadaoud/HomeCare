using System;
namespace HomeCareApp.Models
{
    public class Appointment
    {
        public int Appointment_id { get; set; }
        public int Patient_id { get; set; } //FK to patient
        public int Personnel_id { get; set; } //FK to personnel

        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public String Status { get; set; } = string.Empty;
    }
}