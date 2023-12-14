namespace G4GBackendV4.Models
{
    public class Comment
    {
        public long Id { get; set; }
        public string? Text { get; set; }
        public DateTime Posted { get; set; }

        public long UserId { get; set; }
        public long ContentId { get; set; }
        public virtual User? User { get; set; }
        public virtual Content? Content { get; set; }
    }
}
