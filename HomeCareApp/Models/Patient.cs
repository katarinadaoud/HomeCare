using System;
using System.ComponentModel.DataAnnotations;
using System.IO.Compression;

namespace HomeCareApp.Models
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; } //PK

        //Validation for name
        [Required, RegularExpression(@"^[A-Za-zÆØÅæøå '\-]{1,50}$", ErrorMessage = "FullName must be 1-50 characters and contain only letters.")]

        public String FullName { get; set; } = string.Empty;
        public String Address { get; set; } = string.Empty;
        public String HealthRelated_info { get; set; } = string.Empty;

        //Validation for phone number
        [Required, RegularExpression(@"^\+47(?:[ \-]?\d){8}$",
        ErrorMessage = "phonenumber must start with +47 and have 8 numbers).")]
        public String phonenumber { get; set; } = string.Empty;

        public String UserId { get; set; } = string.Empty; //FK to User
        public int? EmergencyContactId { get; set; } //FK to Emergency contact

        //navigation properties
        public required User User { get; set; }
        public required EmergencyContact? EmergencyContact { get; set; }

        public required ICollection<Appointment> Appointments { get; set; }
        public required ICollection<EmergencyCall> EmergencyCalls { get; set; }

    }
}