namespace G4GBackendV4.Dtos
{
    public class SubCategoryDto
    {
        public long IdSubcategory { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public ContentDto LastContentInSubCategory { get; set; }
        public long TotalContentsInSubCategory { get; set; }
        public long TotalCommentInInSubCategory { get; set; }
    }
}
