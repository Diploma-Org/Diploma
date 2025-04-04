using DataAccess.Entities;
using WebApp.Models;

namespace WebApp.ViewModels
{
    public class HomeIndexViewModel
    {
        public required IEnumerable<AppoinmentAllData> Appointments { get; set; }
        public required IEnumerable<Master> Masters { get; set; }
        public required DateTime SelectedDate { get; set; }
            
      
    }
}