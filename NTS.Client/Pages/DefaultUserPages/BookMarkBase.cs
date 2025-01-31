using Microsoft.AspNetCore.Components;
using NTS.Client.Models;
using NTS.Client.Models.DTOs;
using NTS.Client.Services.Contracts;

namespace NTS.Client.Pages.DefaultUserPages
{
    public class BookMarkBase : ComponentBase
    {
        [Inject] public IFavoriteNotesService favoriteNotesService { get; set; }
        [Inject] public IImportantNotesService importantNotesService { get; set; }


        public NoteDto note { get; set; } = new NoteDto();
        public List<ImportantNotes> importantNotes { get; set; }
        public List<FavoriteNotes> favoriteNotes { get; set; }
        public bool isFetched = false;

        protected override async Task OnInitializedAsync()
        {
            await LoadFavoriteNotesASYNC();
            isFetched = true;
        }

        public async Task LoadFavoriteNotesASYNC()
        {
            try
            {
                var favoriteNotesTask = favoriteNotesService.GetAllFavoriteNotesAsync();
                var importantNotesTask = importantNotesService.GetAllImportantNotesAsync();

                await Task.WhenAll(favoriteNotesTask, importantNotesTask);

                favoriteNotes = favoriteNotesTask.Result?.ToList() ?? new List<FavoriteNotes>();
                importantNotes = importantNotesTask.Result?.ToList() ?? new List<ImportantNotes>();

                if (favoriteNotes == null)
                {
                    Console.WriteLine("GetAllFavoriteNotesAsync Returned Null");
                    favoriteNotes = new List<FavoriteNotes>();
                }
                else
                {
                    favoriteNotes = favoriteNotes.ToList();
                }

                isFetched = true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                isFetched = false;
            }
        }


        public async Task UnmarkNoteAsFavoriteAsync()
        {
            try
            {
                await favoriteNotesService.UnmarkNoteAsFavoriteNoteAsync(note.NoteId, note.UserId);
            }
            catch (Exception error)
            {
                Console.WriteLine($"Error Unmarking Note: {error.Message}");
            }
        }
    }
}