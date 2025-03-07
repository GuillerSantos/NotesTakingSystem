using System.ComponentModel.DataAnnotations;

namespace NTS.Client.DTOs
{
    public class NoteDto
    {
        #region Properties

        public Guid NoteId { get; set; }

        public Guid UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; } = string.Empty;

        public string? FullName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Content { get; set; } = string.Empty;

        public string Color { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        #endregion Properties
    }
}