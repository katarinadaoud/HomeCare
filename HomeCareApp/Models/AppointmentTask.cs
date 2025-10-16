using System.ComponentModel.DataAnnotations;

namespace HomeCareApp.Models
{
    public class AppointmentTask
    {
        [Key]
        public int AppointmentTaskId { get; set; } //PK
        public String Description { get; set; } = string.Empty;
        public String Status { get; set; } = string.Empty;

        public int AppointmentId { get; set; } //FK to appointment

        //navigation properties
        public required Appointment Appointment { get; set; }
         
    }
}