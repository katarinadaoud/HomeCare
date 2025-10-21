using System.ComponentModel.DataAnnotations;

namespace HomeCareApp.Models
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; } //PK
        
        public Patient? Patient { get; set; } //navigation property
        
        [Required]
        public int PatientId { get; set; } //FK to Patient
       
        [Required, MaxLength(300)]
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; }  = false;



       
    }
}   