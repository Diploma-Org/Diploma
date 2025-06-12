using DataAccess.Entities;
namespace WebApp.ViewModels;

public class MastersIndexViewModel
{
    public List<Master> Masters { get; set; }
    public List<WorkingMaster> WorkingMasters { get; set; }
    public DateTime SelectedDate { get; set; }   

    public MastersIndexViewModel(List<Master> masters, List<WorkingMaster> workingMasters, DateTime selectedDate)
    {
        Masters = masters;
        WorkingMasters = workingMasters;
        SelectedDate = selectedDate;
    }
}