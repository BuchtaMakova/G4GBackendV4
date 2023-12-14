namespace G4GBackendV4.Dtos
{
    public class UpdateContentDto
    {
        public long Id { get; set; }
        public string? Headline { get; set; }
        public string? Text { get; set; }
        public long AccountIdAccount { get; set; }
        public long SubcategoryIdSubcategory { get; set; }
    }
}
