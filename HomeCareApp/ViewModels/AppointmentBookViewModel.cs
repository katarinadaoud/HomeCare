using HomeCareApp.Models;

namespace HomeCareApp.ViewModels
{

     public class AppointmentBookViewModel
    {
        public IEnumerable<Appointment> Appointments;
        public string? CurrentViewName;

        public Appointment Appointment { get; set; }

    public AppointmentBookViewModel(IEnumerable<Appointment> appointments, string? currentViewName)
    {
        Appointments = appointments;
        CurrentViewName = currentViewName;
    }
         

    }
}