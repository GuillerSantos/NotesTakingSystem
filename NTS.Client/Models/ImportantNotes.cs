namespace NTS.Client.Models
{
    public class ImportantNotes
    {
        public Guid ImportantNoteId { get; set; }
        public Guid NoteId { get; set; }
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string Color { get; set; } = string.Empty;
    }
}
