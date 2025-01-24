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
        public string Content { get; set; } = string.Empty;

        [ForeignKey(nameof(Note))]
        public Guid NoteId { get; set; }

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public string Color { get; set; } = "#ffffff";


        public Notes? Note { get; set; }
        public ApplicationUsers? User { get; set; }
    }
}
