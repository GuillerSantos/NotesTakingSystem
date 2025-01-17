using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTS.Server.Entities
{
    public class SharedNotes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid SharedNoteId { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        [ForeignKey(nameof(SharedWithUser))]
        public Guid SharedWithUserId { get; set; }

        [ForeignKey(nameof(Note))]
        public Guid NoteId { get; set; }

        [ForeignKey(nameof(ApplicationUser))]
        public Guid UserId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }


        public NotesBase Note { get; set; }
        public ApplicationUsers ApplicationUser { get; set; }
        public ApplicationUsers SharedWithUser { get; set; }
    }
}
