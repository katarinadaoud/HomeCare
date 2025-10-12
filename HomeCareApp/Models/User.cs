using System;
namespace HomeCareApp.Models;
using Microsoft.AspNetCore.Identity;
    public class User : IdentityUser
    {
        public int UserId { get; set; } //PK
                                        //public String Username { get; set; } = string.Empty; Dont need this bc of identityuser inheritance
                                        // public String Passwordhash { get; set; } = string.Empty;

    /* public String Name { get; set; } = string.Empty;
     public String Phone { get; set; } = string.Empty;
     public String Email { get; set; } = string.Empty;
     public String Role { get; set; } = string.Empty;*/

    //sletter^^ dont need this either bc of identityuser inheritance

    //navigation properties
    public ICollection<Admin> Admins { get; set; }
        public ICollection<Employee> Employees { get; set; }
        public ICollection<Patient> Patients { get; set; }
         
    }
