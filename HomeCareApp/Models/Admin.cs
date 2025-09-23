using System;
namespace HomeCareApp.Models
{
    public class Admin
    {
        public int Admin_id { get; set; }
        public int User_id { get; set; } //FK to user
        public String Accesses { get; set; } = string.Empty; //vet ikke 
         
    }
}