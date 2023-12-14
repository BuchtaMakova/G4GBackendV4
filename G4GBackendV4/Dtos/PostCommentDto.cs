namespace G4GBackendV4.Dtos
{
    public class PostCommentDto
    {
        public string? Text { get; set; }
        public DateTime Posted { get; set; }
        public long AccountIdAccount { get; set; }
        public long ContentIdContent { get; set; }
    }
}
