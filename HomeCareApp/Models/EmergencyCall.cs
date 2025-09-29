using System;
namespace HomeCareApp.Models
{
    public class EmergencyCall
    {
        public int EmergencyCallId { get; set; } //PK
        public DateTime Time { get; set; }
        public String Status { get; set; } = string.Empty;

        public int EmployeeId { get; set; } //FK to personnel, handled by
        public int PatientId { get; set; } //FK to patient

        //navigation properties
        public Employee Employee { get; set; }
        public Patient Patient { get; set; }

         
    }
}