using DataAccess.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
namespace DataAccess.Entities;

public class ProvidedService : IEntity
{
    [Column("IdProvidedService")]
    public int Id { get; set; }
    public required string ServiceName { get; set; }
    public decimal Price { get; set; }
}