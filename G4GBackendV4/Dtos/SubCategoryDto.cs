namespace G4GBackendV4.Dtos
{
    public class SubCategoryDto
    {
        public long IdSubcategory { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public ContentDto lastContentInSubCategory { get; set; }
        public long totalContentsInSubCategory { get; set; }
        public long totalCommentInInSubCategory { get; set; }
    }
}
