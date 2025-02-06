using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NTS.Server.Entities
{
    public class SharedNotes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid SharedNoteId { get; set; }

        [ForeignKey(nameof(Note))]
        public Guid NoteId { get; set; }

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public string Color { get; set; } = "#ffffff";

        [JsonIgnore]
        public Notes? Note { get; set; }

        [JsonIgnore]
        public ApplicationUsers? User { get; set; }
    }
}
