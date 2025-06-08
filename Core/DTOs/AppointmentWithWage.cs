using DataAccess.Entities;

namespace BusinessLogic.DTOs
{
    public class AppointmentWithWage
    {
        public string MasterName { get; set; }
        public float AppointmentMasterWage { get; set; }
        public float AppointmentSalonWage { get; set; }
        public DateTime AppointmentTime { get; set; }
        public string ClientName { get; set; }
        public string ServiceName { get; set; } 
        public AppointmentWithWage(Appointment appointment, Master master, ProvidedService service)
        {
            MasterName = master.Name;
            AppointmentMasterWage = service.Price * (master.WagePercent / 100);
            AppointmentSalonWage = service.Price - AppointmentMasterWage;
            AppointmentTime = appointment.StartTime;
            ClientName = appointment.VisitorName;
            ServiceName = service.ServiceName;
        }
    }
}