namespace NTS.Client.Models
{
    public class Comment
    {
        #region Properties

        public Guid CommentId { get; set; }
        public Guid SharedNoteId { get; set; }
        public Guid NoteId { get; set; }
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string CommentContent { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        #endregion Properties
    }
}