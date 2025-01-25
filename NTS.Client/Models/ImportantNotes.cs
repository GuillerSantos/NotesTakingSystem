namespace NTS.Client.Models
{
    public class ImportantNotes
    {
        public Guid FavoriteNoteId { get; set; }
        public string Title { get; set; } = string.Empty;
        public Guid NoteId { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Color { get; set; } = string.Empty;
    }
}
