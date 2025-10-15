using System;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Net.Http.Headers;
using System.ComponentModel.DataAnnotations;       // for [Required], [Display], [DataType], osv.
using System.ComponentModel.DataAnnotations.Schema; // for [Column], [ForeignKey], osv.
namespace HomeCareApp.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; } //PK

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "date")]   // lagres som DATE i SQL
        public DateTime Date { get; set; }   // Valgt dag

        public int? PatientId { get; set; } //FK to patient
        public int? EmployeeId { get; set; } //FK to employee
    
        //navigation keys
        public Patient? Patient { get; set; }//we will not set it as non-nullable because we need these for an appointment
        public Employee? Employee { get; set; }
        public ICollection<AppointmentTask>? AppointmentTasks { get; set;}

    }

   
}