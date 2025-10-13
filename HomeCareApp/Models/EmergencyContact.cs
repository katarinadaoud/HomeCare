using System;
namespace HomeCareApp.Models
{
    public class EmergencyContact
    {
        public int EmergencyContactId { get; set; } //PK
        public String Name { get; set; } = string.Empty;
        public String Phone { get; set; } = string.Empty;
        public String Email { get; set; } = string.Empty;
        public String PatientRelation { get; set; } = string.Empty;


        //navigation properties
        public Patient Patient { get; set; } //we dont need an FK to one-to-one in patient
         
    }
}