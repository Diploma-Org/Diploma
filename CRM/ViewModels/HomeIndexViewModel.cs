using DataAccess.Entities;
using WebApp.Models;

namespace WebApp.ViewModels
{
    public class HomeIndexViewModel
    {
        public required IEnumerable<AppoinmentAllData> Appoinments { get; set; }
        public required IEnumerable<Master> Masters { get; set; }
    }
}