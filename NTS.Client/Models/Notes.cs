using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NTS.Client.Models
{
    public class Notes
    {
        [Key]
        public Guid NoteId { get; set; }

        [ForeignKey("UserId")]
        public Guid UserId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public string Priority { get; set; } = string.Empty;

        public DateTime? CreatedAt { get; set; }

        public bool FavoriteNote { get; set; }
    }
}
