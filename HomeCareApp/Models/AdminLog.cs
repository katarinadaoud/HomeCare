using System;
using System.ComponentModel.DataAnnotations;
namespace HomeCareApp.Models
{
    public class AdminLog
    {

        [Key]
        public int AdminLogId { get; set; } //PK
        public String Action { get; set; } = string.Empty;
        public DateTime Time { get; set; }

        public int AdminId { get; set; } //FK to Admin, litt usikker her hva som skal være

        //navigation properties
        public required Admin Admin { get; set; }
        
         
    }
}