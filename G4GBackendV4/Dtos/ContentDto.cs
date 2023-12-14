namespace G4GBackendV4.Dtos
{
    public class ContentDto
    {
        public ContentDto()
        {
            Comment = new HashSet<CommentDto>();
        }

        public long IdContent { get; set; }
        public string? Headline { get; set; }
        public string? Text { get; set; }
        public long? Views { get; set; }
        public DateTime Posted { get; set; }
        public long SubcategoryIdSubcategory { get; set; }
        public AccountDto Account { get; set; }

        public virtual ICollection<CommentDto> Comment { get; set; }
        public long CommentsCount { get; set; }
    }
}
