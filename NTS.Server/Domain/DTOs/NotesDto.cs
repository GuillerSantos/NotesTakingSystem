namespace NTS.Server.Domain.DTOs
{
    public class NotesDto
    {
        public Guid UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty ;
        public string Priority { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
    }
}
