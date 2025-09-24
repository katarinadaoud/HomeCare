using System;
namespace HomeCareApp.Models
{
    public class Patient
    {

        public int PatientId { get; set; } //PK
        public String Address { get; set; } = string.Empty;
        public String HelthRelated_info { get; set; } = string.Empty;

        public int UserId { get; set; } //FK to User
        public String EmergencyContact_Id { get; set; } = string.Empty; //FK to Emergency contact

        //navigation properties
        public User User { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<EmergencyCall> EmergencyCalls { get; set; }
        public ICollection<EmergencyContact> EmergencyContacts { get; set; }
        
        
    }
}