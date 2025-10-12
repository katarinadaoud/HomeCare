using System;
namespace HomeCareApp.Models
{
    public class Patient
    {

        public int PatientId { get; set; } //PK
        public String Address { get; set; } = string.Empty;
        public String HealthRelated_info { get; set; } = string.Empty;

        public String UserId { get; set; } //FK to User
        public int EmergencyContactId { get; set; } //FK to Emergency contact

        //navigation properties
        public User User { get; set; } //we will not set it as non-nullable because we need these for a patient
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<EmergencyCall> EmergencyCalls { get; set; }
        public ICollection<EmergencyContact> EmergencyContacts { get; set; }
        
        
    }
}