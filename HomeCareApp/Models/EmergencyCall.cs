using System.ComponentModel.DataAnnotations;

namespace HomeCareApp.Models
{
    public class EmergencyCall
    {
        [Key]
        public int EmergencyCallId { get; set; } //PK
        public DateTime Time { get; set; }
        public String Status { get; set; } = string.Empty;

        public int EmployeeId { get; set; } //FK to personnel
        public int PatientId { get; set; } //FK to patient

        //navigation properties
        public required Employee Employee { get; set; }
        public required Patient Patient { get; set; }

         
    }
}