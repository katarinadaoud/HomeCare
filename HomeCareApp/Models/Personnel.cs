using System;
namespace HomeCareApp.Models
{
    public class Personnel
    {
        public int User_id { get; set; } //FK to User
        public int Personnel_id { get; set; }
        public String Specialty { get; set; } = string.Empty;
        public String Availability { get; set; } = string.Empty;
        public String Work_area { get; set; } = string.Empty;
         
    }
}