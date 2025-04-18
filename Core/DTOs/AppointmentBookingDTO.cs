namespace BusinessLogic.DTOs
{
    public class AppointmentBookingDTO
    {
        public int MasterId { get; set; }
        public int ServiceId { get; set; }
        public string ClientName { get; set; }
        public string ClientPhone { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public DateTime Date { get; set; }
    }
}