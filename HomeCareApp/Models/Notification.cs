namespace HomeCareApp.Models
{
    public class Notification
    {
        public int NotificationId { get; set; } //PK
        public Patient? Patient { get; set; } //navigation property
        public int PatientId { get; set; } //FK to Patient
        public string Message { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }  



       
    }
}   