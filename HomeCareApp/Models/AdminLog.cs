using System;
namespace HomeCareApp.Models
{
    public class AdminLog
    {
        public int Log_id { get; set; }
        public int Admin_id { get; set; } //FK to Admin, litt usikker her hva som skal være

        public String Action { get; set; } = string.Empty;
        public DateTime Time { get; set; }
        
         
    }
}