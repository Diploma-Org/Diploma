using DataAccess.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
namespace DataAccess.Entities;

public class Appointment : IEntity
{
    [Column("IdAppointment")]
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int IdMaster { get; set; }
    public int IdProvidedService { get; set; }
    public string? VisitorName { get; set; }
    public string? VisitorPhone { get; set; }
    public int? IdClient { get; set; }
    public bool IsPaid { get; set; }
}