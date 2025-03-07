namespace NTS.Server.DTOs
{
    public class SharedNoteUpdateDto
    {
        #region Properties

        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;

        #endregion Properties
    }
}