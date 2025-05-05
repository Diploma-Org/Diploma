using DataAccess.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
namespace DataAccess.Entities;

public class MasterService : IEntity
{
    [Column("IdMasterService")]
    public int Id { get; set; }
    public int IdMaster { get; set; }
    public int IdProvidedService { get; set; }
}
