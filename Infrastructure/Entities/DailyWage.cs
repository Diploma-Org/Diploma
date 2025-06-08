using DataAccess.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
namespace DataAccess.Entities;

public class DailyWage : IEntity
{
    [Column("IdDailyWage")]
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public float Wage { get; set; }
    
}