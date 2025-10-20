using System;
using System.ComponentModel.DataAnnotations;
using System.IO.Compression;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation; 

namespace HomeCareApp.Models
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; } //PK

        //Validation for name
        [Required, RegularExpression(@"^[A-Za-zÆØÅæøå '\-]{1,50}$", ErrorMessage = "FullName must be 1-50 characters and contain only letters.")]

        public string FullName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string HealthRelated_info { get; set; } = string.Empty;

        //Validation for phone number
        [RegularExpression(@"^\+47\d{8}$", ErrorMessage = "phonenumber must start with +47 and have 8 numbers.")]
        [DataType(DataType.PhoneNumber)]
        public string phonenumber { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty; //FK to User
        public int? EmergencyContactId { get; set; } //FK to Emergency contact

        //navigation properties
        [ValidateNever]
        public required User? User { get; set; }
        [ValidateNever]
        public required EmergencyContact? EmergencyContact { get; set; }
        [ValidateNever]
        public required ICollection<Appointment> Appointments { get; set; }
        [ValidateNever]
        public required ICollection<EmergencyCall> EmergencyCalls { get; set; }

    }
}