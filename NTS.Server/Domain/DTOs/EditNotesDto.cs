namespace NTS.Server.Domain.DTOs
{
    public class EditNotesDto
    {
        public Guid NoteId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string Priority { get; set; }
    }
}
