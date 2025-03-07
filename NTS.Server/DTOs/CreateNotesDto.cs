using System.ComponentModel.DataAnnotations;

namespace NTS.Server.DTOs
{
    public class CreateNotesDto
    {
        #region Properties

        [Required]
        [StringLength(50)]
        public string Title { get; set; } = string.Empty;

        public string? FullName { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Content { get; set; } = string.Empty;

        public string Color { get; set; } = string.Empty;

        public DateTime? CreatedAt { get; set; }

        #endregion Properties
    }
}