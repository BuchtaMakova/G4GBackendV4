namespace G4GBackendV4.Dtos
{
    public class UpdateCommentDto
    {
        public long Id { get; set; }
        public string? Text { get; set; }
        public long AccountIdAccount { get; set; }
        public long ContentIdContent { get; set; }
    }
}
