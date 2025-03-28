﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NTS.Server.Entities
{
    public class ApplicationUsers
    {
        #region Properties

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
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(256)]
        public string RecoveryEmail { get; set; } = string.Empty;

        public DateTime? DateJoined { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(50)]
        public string Role { get; set; } = string.Empty;

        [StringLength(500)]
        public string? RefreshToken { get; set; } = string.Empty;

        public DateTime? RefreshTokenExpiryTime { get; set; }

        [JsonIgnore]
        public ICollection<Notes> Notes { get; set; }

        #endregion Properties
    }
}