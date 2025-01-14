﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTS.Server.Entities
{
    public class StarredNotes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid StarredNotesId { get; set; }

        [Required]
        [ForeignKey(nameof(Note))]
        public Guid NoteId { get; set; }

        [Required]
        [ForeignKey(nameof(ApplicationUser))]
        public Guid UserId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }


        public Notes Note { get; set; }
        public ApplicationUsers ApplicationUser { get; set; }
    }
}
