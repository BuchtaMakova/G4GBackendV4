namespace G4GBackendV4.Models
{
    public class Content
    {
        public Content()
        {
            Comments = new HashSet<Comment>();
        }

        public long Id { get; set; }
        public string? Headline { get; set; }
        public string? Text { get; set; }
        public long Views { get; set; }
        public DateTime Posted { get; set; }

        public long UserId { get; set; }
        public long SubcategoryId { get; set; }
        public User? User { get; set; }
        public SubCategory? Subcategory { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
