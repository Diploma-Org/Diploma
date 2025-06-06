namespace BusinessLogic.DTOs
{
    public class MasterServiceDto
    {
        public int Id { get; set; }
        public required string ServiceName { get; set; }
        public float Price { get; set; }
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(ServiceName) && Price > 0;
        }
    }
}