namespace G4GBackendV4.Models
{
    public class Category
    {
        public Category()
        {
            SubCategories = new HashSet<SubCategory>();
        }

        public long Id { get; set; }
        public string Name { get; set; }

        public ICollection<SubCategory> SubCategories { get; set; }
    }
}
