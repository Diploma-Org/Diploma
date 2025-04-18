using BusinessLogic.Interfaces;
using DataAccess.Entities;
using DataAccess.Interfaces;
namespace BusinessLogic.Services;

public class MastersService : IMastersService
{
    private readonly IRepository<WorkingMaster> _workingMastersRepository;

    public MastersService(IRepository<WorkingMaster> workingMastersRepository)
    {
        _workingMastersRepository = workingMastersRepository;
    }
}