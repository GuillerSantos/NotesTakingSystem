namespace NTS.Server.Domain.DTOs
{
    public class NotesDto
    {
        public Guid NoteId { get; set; }
        public Guid UserID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty ;
        public string Priority { get; set; } = string.Empty;
        public bool IsPublic { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
