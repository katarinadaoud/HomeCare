using System;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Net.Http.Headers;
namespace HomeCareApp.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; } //PK


        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public String Status { get; set; } = string.Empty;

        public int PatientId { get; set; } //FK to patient
        public int EmployeeId { get; set; } //FK to employee

        //navigation keys
        public Patient Patient { get; set; } //we will not set it as non-nullable because we need these for an appointment
        public Employee Employee { get; set; }

    }

   
}