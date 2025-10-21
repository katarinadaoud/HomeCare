using HomeCareApp.Models;

namespace HomeCareApp.ViewModels
{ // ViewModel for managing appointments
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

