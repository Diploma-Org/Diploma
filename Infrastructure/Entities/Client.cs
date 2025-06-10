using DataAccess.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
namespace DataAccess.Entities;

public class Client : IEntity
{
    [Column("IdClient")]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime Birthday { get; set; }
    public Client()
    {
        Name = string.Empty;
        Surname = string.Empty;
        PhoneNumber = string.Empty;
        Birthday = DateTime.MinValue;
    }
}