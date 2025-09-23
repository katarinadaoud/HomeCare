using System;
namespace HomeCareApp.Models
{
    public class EmergencyCall
    {
        public int Emergency_id { get; set; }
        public int Patient_id { get; set; } //FK to patient
        public DateTime Time { get; set; }
        public String Status { get; set; } = string.Empty;
    
        public int Personnel_id { get; set; } //FK to personnel, handled by

         
    }
}