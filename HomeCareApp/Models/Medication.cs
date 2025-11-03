using System.ComponentModel.DataAnnotations;

namespace HomeCareApp.Models
{
    public class Medication
    {
        [Key]
        public int MedicationId { get; set; } //PK
        public string Name { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
        public string Frequency { get; set; } = string.Empty;
        public string Instructions { get; set; } = string.Empty;


        public int? PatientId { get; set; } //FK to patient
        public int? EmployeeId { get; set; } //FK to employee

        //navigation properties
        public required Employee? Employee { get; set; }
        public required Patient? Patient { get; set; }
    }
}