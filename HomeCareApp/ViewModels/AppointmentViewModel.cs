using HomeCareApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace HomeCareApp.ViewModels
{
   public class AppointmentViewModel
    {
        public IEnumerable<Appointment> Appointments;
        public string? CurrentViewName;

        public AppointmentViewModel(IEnumerable<Appointment> appointments, string? currentViewName)
        {
             Appointments = appointments;
            CurrentViewName = currentViewName;
            
        }
        
           
    }
}

