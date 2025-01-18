namespace NTS.Server.Entities.DTOs
{
    public class UpdateNotesDto
    {
        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public string Priority { get; set; } = string.Empty;
    }
}
