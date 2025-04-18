using DataAccess.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
namespace DataAccess.Entities;

public class WorkingMaster : IEntity
{
    [Column("IdWorkingMaster")]
    public int Id { get; set; }
    public DateTime Date { get; set; }
}
