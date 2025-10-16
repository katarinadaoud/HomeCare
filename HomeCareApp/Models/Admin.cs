using System.ComponentModel.DataAnnotations;

namespace HomeCareApp.Models
{
    public class Admin
    {
        [Key]
        public int AdminId { get; set; } //PK
        public String Accesses { get; set; } = string.Empty;

        public String UserId { get; set; } = string.Empty; //FK to user

        //navigation properties
        public required User User { get; set; }
        public required ICollection<AdminLog> AdminLogs { get; set; }

    }
}