namespace G4GBackendV4.Models
{
    public class SubCategory
    {
        public SubCategory()
        {
            Contents = new HashSet<Content>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }

        public long CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<Content> Contents { get; set; }
    }
}
