using System;
namespace HomeCareApp.Models
{
    public class Patient
    {
        public int User_id { get; set; } //FK to User
        public int Patient_id { get; set; }
        public String Address { get; set; } = string.Empty;
        public String EmergencyContact_info { get; set; } = string.Empty; //FK to Emergency contact
        public String HelthRelated_info { get; set; } = string.Empty;
        
    }
}