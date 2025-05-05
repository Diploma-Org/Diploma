namespace BusinessLogic.DTOs
{
    public class AppointmentBookingDto
    {
        public int MasterId { get; set; }
        public int ServiceId { get; set; }
        public string? ClientName { get; set; }
        public string? ClientPhone { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public DateTime Date { get; set; }
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(ClientName) && !string.IsNullOrEmpty(ClientPhone) &&
                   !string.IsNullOrEmpty(StartTime) && !string.IsNullOrEmpty(EndTime) && Date != default;
        }
    }
}