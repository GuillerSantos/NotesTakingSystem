using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTS.Server.Domain.Entities
{
    public class Notes
    {
        [Key]
        public Guid NoteId { get; set; }

        [ForeignKey("UserId")]
        public Guid UserId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; }= string.Empty;

        public string Priority { get; set; } = string.Empty;

        public DateTime? CreatedAt { get; set; }

        public bool FavoriteNote { get; set; }


        public ApplicationUsers Users { get; set; }
    }
}
