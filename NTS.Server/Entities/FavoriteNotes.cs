using System.ComponentModel.DataAnnotations;

namespace NTS.Server.Entities
{
    public class FavoriteNotes
    {
        [Key]
        public Guid FavoriteNoteId { get; set; }

        public Guid NoteId { get; set; }

        public Guid UserId { get; set; }

        public DateTime CreatedAt { get; set; }


        public Notes Note { get; set; }

        public ApplicationUsers User { get; set; }
    }
}
