using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NTS.Server.Entities
{
    public class Comment
    {
        [Key]
        public Guid CommentId { get; set; }

        public Guid SharedNoteId { get; set; }

        public Guid NoteId { get; set; }
        
        public Guid UserId { get; set; }

        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [StringLength(300)]
        public string Content { get; set; } = string.Empty;

        public string Color { get; set; } = string.Empty;

        [StringLength(50), Required]
        public string FullName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
