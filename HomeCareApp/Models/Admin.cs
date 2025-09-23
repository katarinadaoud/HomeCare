using System;
namespace HomeCareApp.Models
{
    public class Admin
    {
        public int Adminid { get; set; } //PK
        public String Accesses { get; set; } = string.Empty;

        public int Userid { get; set; } //FK to user

        //navigation propeties
        public User User { get; set; }
        public ICollection<AdminLog> AdminLogs { get; set; }

    }
}