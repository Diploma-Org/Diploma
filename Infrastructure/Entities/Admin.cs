using DataAccess.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
namespace DataAccess.Entities
{
    public class Admin : IEntity
    {
        [Column("IdAdmin")]
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
