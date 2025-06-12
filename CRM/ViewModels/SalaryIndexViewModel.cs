using DataAccess.Entities;
namespace WebApp.ViewModels
{
    public class SalaryIndexViewModel
    {
        public List<Salary> Salaries { get; set; }
        public List<Master> Masters { get; set; }

        public SalaryIndexViewModel(List<Salary> salaries, List<Master> masters)
        {
            Salaries = salaries;
            Masters = masters;
        }
    }
}