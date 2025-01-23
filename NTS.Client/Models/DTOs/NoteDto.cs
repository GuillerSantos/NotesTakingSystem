namespace NTS.Client.Models.DTOs
{
    public class NoteDto
    {
        public Guid NoteId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Priority { get; set; } = "Normal";
        public DateTime CreatedAt { get; set; }
    }
}
