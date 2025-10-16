using System;
namespace HomeCareApp.Models;
using Microsoft.AspNetCore.Identity;
    public class User : IdentityUser
    {
    //Since we are inheriting IdentityUser we dont need any attributes, only nav 
    //navigation properties
    public required ICollection<Admin> Admins { get; set; }
        public required ICollection<Employee> Employees { get; set; }
        public required ICollection<Patient> Patients { get; set; }
         
    }
