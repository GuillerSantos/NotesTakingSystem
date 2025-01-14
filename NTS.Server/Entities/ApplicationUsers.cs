using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTS.Server.Entities
{
    public class ApplicationUsers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UserId { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [StringLength(256)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(256)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [StringLength(256)]
        public string? PhoneNumber { get; set; }

        [Required]
        [StringLength(256)]
        public string? RecoveryEmail { get; set; }

        [Required]
        public DateTime? DateJoined { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(50)]
        public string Role { get; set; } = string.Empty;

        [StringLength(500)]
        public string RefreshToken { get; set; } = string.Empty;

        public DateTime? RefreshTokenExpiryTime { get; set; }

        public ICollection<Notes> Notes { get; set; }
    }
}
