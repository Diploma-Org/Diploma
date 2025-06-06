using DataAccess.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
namespace DataAccess.Entities;

public class Salary : IEntity
{
    [Column("IdSalary")]
    public int Id { get; set; }
    public required int IdMaster { get; set; }
    public required float Earnings { get; set; }
}