namespace G4GBackendV4.Models
{
    public class Role
    {
        public long Id { get; set; }
        public string? Name { get; set; }

        public ICollection<User> Users { get; set; }
    }
}