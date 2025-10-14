// ViewModels/AppointmentBookViewModel.cs
using HomeCareApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace HomeCareApp.ViewModels
{
    public class AppointmentBookViewModel
    {
        // Til listevisninger hvis du trenger å vise eksisterende avtaler
        public IEnumerable<Appointment> Appointments { get; set; } = new List<Appointment>();

        // Til navigasjon/tilstand i UI (valgfritt)
        public string? CurrentViewName { get; set; }

        // Selve modellen som skal postes fra Book.cshtml
        public Appointment Appointment { get; set; } = new Appointment();

        // Dropdown for å velge pasient (binder til Appointment.PatientId)
        public SelectList PatientOptions { get; set; } = new SelectList(new List<SelectListItem>());

        // Parameterløs ctor nødvendig for GET/POST-binding
        public AppointmentBookViewModel() { }

        // Valgfri bekvemmelighetskonstruktør (hvis du vil bruke i andre views)
        public AppointmentBookViewModel(IEnumerable<Appointment> appointments, string? currentViewName)
        {
            Appointments = appointments ?? new List<Appointment>();
            CurrentViewName = currentViewName;
        }
    }
}
