namespace NTS.Server.Entities.DTOs
{
    public class CreateNotesDto
    {
        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public string Priority { get; set; } = string.Empty;

        public DateTime? CreatedAt { get; set; }
    }
}
