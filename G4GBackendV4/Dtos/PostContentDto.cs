namespace G4GBackendV4.Dtos
{
    public class PostContentDto
    {
        public string? Headline { get; set; }
        public string? Text { get; set; }
        public DateTime Posted { get; set; }
        public long AccountIdAccount { get; set; }
        public long SubcategoryIdSubcategory { get; set; }
    }
}
