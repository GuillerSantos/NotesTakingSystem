﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTS.Server.Entities
{
    public class FavoriteNotes
    {
        #region Properties

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid FavoriteNoteId { get; set; }

        [Required]
        [ForeignKey(nameof(Note))]
        public Guid NoteId { get; set; }

        [Required]
        [ForeignKey(nameof(ApplicationUser))]
        public Guid UserId { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public string Color { get; set; } = "#ffffff";

        public Notes? Note { get; set; }
        public ApplicationUsers? ApplicationUser { get; set; }

        #endregion Properties
    }
}