using System;
namespace HomeCareApp.Models
{
    public class User
    {
        public int User_id { get; set; }
        public String Username { get; set; } = string.Empty;
        public String Passwordhash { get; set; } = string.Empty;
        
        public String Name { get; set; } = string.Empty;
        public String Phone { get; set; } = string.Empty;
        public String Email { get; set; } = string.Empty;
        public String Role { get; set; } = string.Empty;
         
    }
}