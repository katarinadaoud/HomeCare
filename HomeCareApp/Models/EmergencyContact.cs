using System.ComponentModel.DataAnnotations;

namespace HomeCareApp.Models
{
    public class EmergencyContact
    {
        [Key]
        public int EmergencyContactId { get; set; } //PK

        //Validation for name
        [Required, RegularExpression(@"^[A-Za-zÆØÅæøå '\-]{1,50}$", ErrorMessage = "Name must be 1-50 characters and contain only letters.")]
        public String Name { get; set; } = string.Empty;

        //Validation for phone number
        [Required, RegularExpression(@"^\+47(?:[ \-]?\d){8}$",
        ErrorMessage = "Phone must start with +47 and have 8 numbers).")]
        public String Phone { get; set; } = string.Empty;
        public String Email { get; set; } = string.Empty;
        public String PatientRelation { get; set; } = string.Empty;


        //navigation properties
        public required Patient Patient { get; set; } //we dont need an FK to one-to-one in patient
         
    }
}