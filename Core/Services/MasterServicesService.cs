using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using DataAccess.Entities;
using DataAccess.Interfaces;
namespace BusinessLogic.Services;

public class MasterServicesService : IMasterServicesService
{
    private readonly IRepository<MasterService> _masterServicesRepository;
    private readonly IRepository<ProvidedService> _providedServicesRepository;
    private readonly IRepository<Master> _masterRepository;

    public MasterServicesService(IRepository<MasterService> masterServicesRepository,
        IRepository<ProvidedService> providedServicesRepository,
        IRepository<Master> masterRepository)
    {
        _masterServicesRepository = masterServicesRepository;
        _providedServicesRepository = providedServicesRepository;
        _masterRepository = masterRepository;
    }

    public List<MasterService> GetMasterServices(int masterId)
    {
        var masterServices = _masterServicesRepository.GetAll()
            .Where(a => a.IdMaster == masterId).ToList();

        if (masterServices == null)
            throw new ArgumentNullException($"Master services with ID {masterId} not found.");

        return masterServices;
    }
    public MasterServicesAndMasterDto GetSevicesToDisplayForMaster(int? masterId)
    {
        if (masterId == null)
            masterId = _masterRepository.GetAll().FirstOrDefault()?.Id;
        var masterIdNN = (int)masterId;


        var masterServices = GetMasterServices(masterIdNN);

        var servicesToDisplay = masterServices.Select(masterService => new MasterServiceDto
        {
            Id = masterService.Id,
            ServiceName = _providedServicesRepository.GetById(masterService.IdProvidedService)?.ServiceName ?? "Unknown Service",
            Price = _providedServicesRepository.GetById(masterService.IdProvidedService)?.Price ?? 0,
        }).ToList();

        if (servicesToDisplay == null)
            throw new ArgumentNullException($"Master services with ID {masterIdNN} not found.");

        var master = _masterRepository.GetById(masterIdNN);

        if (master == null)
            throw new ArgumentNullException($"Master with ID {masterIdNN} not found.");
        var masterServicesDto = new List<MasterServiceDto>();
        foreach (var service in servicesToDisplay)
        {
            var masterService = new MasterServiceDto
            {
                Id = service.Id,
                ServiceName = service.ServiceName,
                Price = service.Price
            };
            masterServicesDto.Add(masterService);
        }

        var masterServicesAndMasterDto = new MasterServicesAndMasterDto
        {
            MasterServices = masterServicesDto,
            Master = master,
            ProvidedServices = _providedServicesRepository.GetAll().ToList()
        };
        return masterServicesAndMasterDto;
    }
    public void AddMasterServiceFromList(int idMaster, int idProvidedService)
    {
        var masterService = new MasterService
        {
            IdMaster = idMaster,
            IdProvidedService = idProvidedService
        };
        if (_masterServicesRepository.GetAll().Any(a => a.IdMaster == idMaster && a.IdProvidedService == idProvidedService))
            throw new ArgumentException($"Master service with ID {idMaster} and provided service ID {idProvidedService} already exists.");

        _masterServicesRepository.Insert(masterService);
        _masterServicesRepository.Save();
    }

    public void DeleteService(int idMaster, int idService)
    {
        var masterService = _masterServicesRepository.GetById(idService);
        if (masterService == null)
            throw new ArgumentNullException($"Master service with ID {idMaster} not found.");
        _masterServicesRepository.Delete(masterService.Id);
        _masterServicesRepository.Save();
    }
    public void DeleteAllMasterServices(int idMaster)
    {
        var masterServices = _masterServicesRepository.GetAll()
            .Where(a => a.IdMaster == idMaster).ToList();
        if (masterServices == null || !masterServices.Any())
            throw new ArgumentNullException($"Master services with ID {idMaster} not found.");
        
        foreach (var masterService in masterServices)
        {
            _masterServicesRepository.Delete(masterService.Id);
        }
        _masterServicesRepository.Save();
    }
}