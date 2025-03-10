using Microsoft.AspNetCore.Components;
using MudBlazor;
using NTS.Client.DTOs;
using NTS.Client.Models;
using NTS.Client.Services.Contracts;

namespace NTS.Client.Pages.DefaultUserPages
{
    public class BookMarkBase : ComponentBase
    {
        #region Fields

        public bool isFetched = false;

        #endregion Fields

        #region Properties

        [Inject] public IFavoriteNotesService favoriteNotesService { get; set; } = default!;
        [Inject] public IImportantNotesService importantNotesService { get; set; } = default!;
        [Inject] public IStarredNotesService starredNotesService { get; set; } = default!;
        [Inject] public IDialogService dialogService { get; set; } = default!;

        [Parameter] public List<ImportantNotes> importantNotes { get; set; } = new();
        [Parameter] public List<FavoriteNotes> favoriteNotes { get; set; } = new();
        [Parameter] public List<StarredNotes> starredNotes { get; set; } = new();

        public NoteDto note { get; set; } = new();

        #endregion Properties

        #region Public Methods

        public async Task LoadAllMarkedNotesAsync()
        {
            try
            {
                var favoriteNotesTask = favoriteNotesService.GetAllFavoriteNotesAsync();
                var importantNotesTask = importantNotesService.GetAllImportantNotesAsync();
                var starredNotesTask = starredNotesService.GetAllStarredNotesAsync();

                var favoriteNotesResult = await favoriteNotesTask;
                var importantNotesResult = await importantNotesTask;
                var starredNotesResult = await starredNotesTask;

                favoriteNotes = favoriteNotesResult?.ToList() ?? new();
                importantNotes = importantNotesResult?.ToList() ?? new();
                starredNotes = starredNotesResult?.ToList() ?? new();

                isFetched = true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                isFetched = false;
            }
        }

        public async Task UnmarkNoteAsFavoriteAsync(Guid noteId)
        {
            try
            {
                await favoriteNotesService.UnmarkNoteAsFavoriteNoteAsync(noteId);
                favoriteNotes.RemoveAll(n => n.NoteId == noteId);
            }
            catch (Exception error)
            {
                Console.WriteLine($"Error unmarking favorite: {error.Message}");
            }
        }

        public async Task UnmarkNoteAsImportantAsync(Guid noteId)
        {
            try
            {
                await importantNotesService.UnmarkNoteAsImportantAsync(noteId);
                importantNotes.RemoveAll(n => n.NoteId == noteId);
            }
            catch (Exception error)
            {
                Console.WriteLine($"Error unmarking important: {error.Message}");
            }
        }

        public async Task UnmarkNoteAsStarredAsync(Guid noteId)
        {
            try
            {
                await starredNotesService.UnmarkNoteAsImportantNoteAsync(noteId);
                starredNotes.RemoveAll(n => n.NoteId == noteId);
            }
            catch (Exception error)
            {
                Console.WriteLine($"Error unmarking starred: {error.Message}");
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected override async Task OnInitializedAsync()
        {
            await LoadAllMarkedNotesAsync();
            isFetched = true;
        }

        #endregion Protected Methods
    }
}