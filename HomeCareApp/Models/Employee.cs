using System;
namespace HomeCareApp.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; } //PK
        public String Specialty { get; set; } = string.Empty;
        public String Availability { get; set; } = string.Empty;
        public String Work_area { get; set; } = string.Empty;

        public int UserId { get; set; } //FK to User

        //navigation properties
        public User User { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<AvailableDay> AvailableDays { get; set; }
        public ICollection<EmergencyCall> EmergencyCalls { get; set; }
         
    
    }
}