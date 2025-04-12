using DataAccess.Entities;

namespace WebApp.ViewModels
{
    public class HomeIndexViewModel
    {
        public required IEnumerable<AppointmentAllData> Appointments { get; set; }
        public required IEnumerable<ProvidedService> ProvidedServices { get; set; }
        public required IEnumerable<Master> Masters { get; set; }
        public required DateTime SelectedDate { get; set; }
            
      
    }
}