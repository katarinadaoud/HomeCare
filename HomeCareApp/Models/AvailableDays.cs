using System;
namespace HomeCareApp.Models
{
    public class AvailableDays
    {
        public int Day_id { get; set; }
        public int Personnel_id { get; set; } //FK to personnel
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}