using System;
using System.ComponentModel.DataAnnotations;
using System.IO.Compression;

namespace HomeCareApp.Models
{
    public class Patient
    {

        public int PatientId { get; set; } //PK

        /*Validation på navn*/
        [Required, RegularExpression(@"^[A-Za-zÆØÅæøå '\-]{1,50}$", ErrorMessage = "FullName must be 1-50 characters and contain only letters.")]

        public String FullName { get; set; } = string.Empty;
        public String Address { get; set; } = string.Empty;
        public String HealthRelated_info { get; set; } = string.Empty;

        /*Validering på telefonnummer*/
        [Required, RegularExpression(@"^\+47(?:[ \-]?\d){8}$",
        ErrorMessage = "phonenumber must start with +47 and have 8 numbers).")]
        public String phonenumber { get; set; } = string.Empty;

        public String UserId { get; set; } //FK to User
        public int EmergencyContactId { get; set; } //FK to Emergency contact

        //navigation properties
        public User User { get; set; } //we will not set it as non-nullable because we need these for a patient
        public EmergencyContact EmergencyContact { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<EmergencyCall> EmergencyCalls { get; set; }

    }
}