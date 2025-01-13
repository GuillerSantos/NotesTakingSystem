using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTS.Server.Domain.Entities
{
    public class StarredNotes
    {
        [Key]
        public Guid StarredNotesId { get; set; }

        [ForeignKey("NoteId")]
        public Guid NoteId { get; set; }

        [ForeignKey("UserId")]
        public Guid UserId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public Notes Notes { get; set; }

        public ApplicationUsers Users { get; set; }
    }
}
