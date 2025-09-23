using System;
namespace HomeCareApp.Models
{
    public class AdminLog
    {
        public int LogId { get; set; } //PK
        public String Action { get; set; } = string.Empty;
        public DateTime Time { get; set; }

        public int AdminId { get; set; } //FK to Admin, litt usikker her hva som skal være

        //navigation properties
        public Admin Admin { get; set; }
        
         
    }
}