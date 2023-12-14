namespace G4GBackendV4.Dtos
{
    public class CategoryDto
    {
        public CategoryDto()
        {
        }

        public long IdCategory { get; set; }
        public string Name { get; set; }

        public ICollection<SubCategoryDto> SubCategory { get; set; }
    }
}
