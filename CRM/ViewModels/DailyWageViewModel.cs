using BusinessLogic.DTOs;
using DataAccess.Entities;

namespace WebApp.ViewModels
{
    public class DailyWageViewModel
    {
        public required IEnumerable<AppointmentWithWage> AppointmentsWithWages { get; set; }
        public required IEnumerable<Master> Masters { get; set; }
        public required DailyWage DailyWage { get; set; }
    }
}