using System;
namespace HomeCareApp.Models
{
    public class AvailableDay
    {
        public int AvailableDayId { get; set; } //PK
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public int EmployeeId { get; set; } //FK to employee

        //navigation properties
         public Employee Employee { get; set; }
    }
}