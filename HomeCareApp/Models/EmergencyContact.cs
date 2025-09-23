using System;
namespace HomeCareApp.Models
{
    public class EmergencyContact
    {
        public int Contact_id { get; set; }
        public String Name { get; set; } = string.Empty;
        public String Phone { get; set; } = string.Empty;
        
        public String Email { get; set; } = string.Empty;
        public String PatientRelation { get; set; } = string.Empty;
        public int Patient_id { get; set; } //FK to patient
         
    }
}