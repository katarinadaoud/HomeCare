using System;
namespace HomeCareApp.Models
{
    public class Item
    {
        
        public string? FullName { get; set; }
        public string? Description { get; set; }
        public String PasswordHash { get; set; } = string.Empty;
        
    }
}