using System;
namespace HomeCareApp.Models;
using Microsoft.AspNetCore.Identity;
    public class User : IdentityUser
    {
    //Since we are inheriting IdentityUser we dont need any attributes, only nav 
    //navigation properties
    public ICollection<Admin> Admins { get; set; }
        public ICollection<Employee> Employees { get; set; }
        public ICollection<Patient> Patients { get; set; }
         
    }
