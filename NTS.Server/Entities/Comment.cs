using System.ComponentModel.DataAnnotations;

namespace NTS.Server.Entities
{
    public class Comment
    {
        [Key]
        public Guid CommentId { get; set; }
        public Guid NoteId { get; set; }
        public Guid UserId { get; set; }

        public string Title { get; set; } = string.Empty;

        [StringLength(300)]
        public string Content { get; set; } = string.Empty;

        [StringLength(50), Required]
        public string FullName { get; set; } = string.Empty;

        public DateTime CreateAt { get; set; } = DateTime.UtcNow;

        public ApplicationUsers Users { get; set; } = default!;
        public Notes Notes { get; set; } = default!;
    }
}
