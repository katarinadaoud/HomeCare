using System.ComponentModel.DataAnnotations;

namespace HomeCareApp.Models
{
    public class AvailableDay
    {
        [Key]
        public int AvailableDayId { get; set; } //PK
        public DateTime Date { get; set; }

        public int EmployeeId { get; set; } //FK to employee

        //navigation properties
        public required Employee Employee { get; set; }
    }
}