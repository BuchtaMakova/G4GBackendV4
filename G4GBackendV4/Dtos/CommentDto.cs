namespace G4GBackendV4.Dtos
{
    public class CommentDto
    {
        public long IdComment { get; set; }
        public string? Text { get; set; }
        public DateTime Posted { get; set; }
        public long AccountIdAccount { get; set; }
        public string? AccountUsername { get; set; }
        public long ContentIdContent { get; set; }
        public AccountDto Account { get; set; }
    }
}
