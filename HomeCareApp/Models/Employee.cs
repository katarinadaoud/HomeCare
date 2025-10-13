using System;
namespace HomeCareApp.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; } //PK
         public String FullName { get; set; } = string.Empty;
        public String Address { get; set; } = string.Empty;

       // public String Availability { get; set; } = string.Empty; fjerner denne, tror det holder med FK til av.days
        public String Department { get; set; } = string.Empty;

        public String UserId { get; set; } //FK to User

        //navigation properties
        public User User { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<AvailableDay> AvailableDays { get; set; }
        public ICollection<EmergencyCall> EmergencyCalls { get; set; }
         
    
    }
}