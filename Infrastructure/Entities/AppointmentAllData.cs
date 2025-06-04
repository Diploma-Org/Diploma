namespace DataAccess.Entities
{
    public class AppointmentAllData : Appointment
    {
        public string ServiceName { get; set; }
        public AppointmentAllData(Appointment appointment, string serviceName)
        {
            Id = appointment.Id;
            StartTime = appointment.StartTime;
            EndTime = appointment.EndTime;
            IdMaster = appointment.IdMaster;
            IdProvidedService = appointment.IdProvidedService;
            VisitorName = appointment.VisitorName;
            VisitorPhone = appointment.VisitorPhone;
            IdMaster = appointment.IdMaster;
            ServiceName = serviceName;
            IsPaid = appointment.IsPaid;
        }
    }
}