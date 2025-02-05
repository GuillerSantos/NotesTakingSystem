namespace NTS.Client.Models
{
    public class Comment
    {
        public Guid CommentId { get; set; }
        public Guid NoteId { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
