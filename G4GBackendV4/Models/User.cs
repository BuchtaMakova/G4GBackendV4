namespace G4GBackendV4.Models
{
    public class User
    {
        public User()
        {
            Comments = new HashSet<Comment>();
            Contents = new HashSet<Content>();
        }

        public long Id { get; set; }
        public string? Username { get; set; }
        public string? PasswordHash { get; set; }
        public string Role { get; } = "user";

        public ICollection<Comment> Comments { get; set; }
        public ICollection<Content> Contents { get; set; }
    }
}
