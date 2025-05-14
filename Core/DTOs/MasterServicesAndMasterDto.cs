using DataAccess.Entities;

namespace BusinessLogic.DTOs
{
    public class MasterServicesAndMasterDto
    {
        public required List<MasterServiceDto> MasterServices { get; set; }
        public required List<ProvidedService> ProvidedServices { get; set; }
        public required Master Master { get; set; }
    }
}