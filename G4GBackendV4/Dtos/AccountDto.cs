namespace G4GBackendV4.Dtos
{
    public class AccountDto
    {
        public long IdAccount { get; set; }
        public string? Username { get; set; }
        public long CommentsPosted { get; set; }
        public long ContentsPosted { get; set; }
        public List<CommentDto>? Comments { get; set; }
        public List<ContentDto>? Contents { get; set; }
        public List<string>? Roles { get; set; }
    }
}