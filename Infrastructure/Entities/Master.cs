using DataAccess.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
namespace DataAccess.Entities;

public class Master : IEntity
{
    [Column("IdMaster")]
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Surname { get; set; }
    public string? PhoneNumber { get; set; }
    public Master() { }
}
