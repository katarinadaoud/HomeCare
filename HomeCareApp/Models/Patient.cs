using System;
namespace HomeCareApp.Models
{
    public class Patient
    {

        public int PatientId { get; set; } //PK
        public String Address { get; set; } = string.Empty;
        public String HealthRelated_info { get; set; } = string.Empty;

        public int UserId { get; set; } //FK to User
        public int EmergencyContactId { get; set; } //FK to Emergency contact

        //navigation properties
        public User User { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<EmergencyCall> EmergencyCalls { get; set; }
        public ICollection<EmergencyContact> EmergencyContacts { get; set; }
        
        
    }
}