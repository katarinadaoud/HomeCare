using System;
namespace HomeCareApp.Models
{
    public class Employee
    {
        public int User_id { get; set; } //FK to User
        public int Employee_id { get; set; }
        public String Specialty { get; set; } = string.Empty;
        public String Availability { get; set; } = string.Empty;
        public String Work_area { get; set; } = string.Empty;
         
    }
}