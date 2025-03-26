namespace DataAccess.Entities
{
    public class AppoinmentAllData : Appoinment
    {
        public string ServiceName { get; set; }
        public AppoinmentAllData(Appoinment appoinment, string serviceName)
        {
            Id = appoinment.Id;
            StartTime = appoinment.StartTime;
            IdMaster = appoinment.IdMaster;
            IdProvidedService = appoinment.IdProvidedService;
            VisitorName = appoinment.VisitorName;
            VisitorPhone = appoinment.VisitorPhone;
            IdMaster = appoinment.IdMaster;
            ServiceName = serviceName;
        }
    }
}