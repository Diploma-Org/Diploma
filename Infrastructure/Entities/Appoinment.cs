using DataAccess.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
namespace DataAccess.Entities;

public class Appoinment : IEntity
{
    [Column("IdAppointment")]
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public int Duration { get; set; }
    public int IdMaster { get; set; }
    public int IdProvidedService { get; set; }
    public string? VisitorName { get; set; }
    public string? VisitorPhone { get; set; }
}