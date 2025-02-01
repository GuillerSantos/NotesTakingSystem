using System.ComponentModel.DataAnnotations;

namespace NTS.Server.Entities.DTOs
{
    public class CreateNotesDto
    {
        [Required]
        [StringLength(50)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Content { get; set; } = string.Empty;

        public string Color { get; set; } = string.Empty;

        public DateTime? CreatedAt { get; set; }
    }
}
