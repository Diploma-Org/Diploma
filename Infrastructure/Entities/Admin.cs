namespace DataAccess.Entities
{
    public class Admin
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
