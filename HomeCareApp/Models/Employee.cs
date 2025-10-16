using System.ComponentModel.DataAnnotations;

namespace HomeCareApp.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; } //PK

        //Validation for name 
        [Required, RegularExpression(@"^[A-Za-zÆØÅæøå '\-]{1,50}$", ErrorMessage = "FullName must be 1-50 characters and contain only letters.")]

        public String FullName { get; set; } = string.Empty;
        public String Address { get; set; } = string.Empty;
        public String Department { get; set; } = string.Empty;

        public required String UserId { get; set; } //FK to User

        //navigation properties
        public required User User { get; set; }
        public required ICollection<Appointment> Appointments { get; set; }
        public required ICollection<AvailableDay> AvailableDays { get; set; }
        public required ICollection<EmergencyCall> EmergencyCalls { get; set; }

        
    }
}