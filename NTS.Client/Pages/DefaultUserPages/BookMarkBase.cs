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

        [Parameter] public List<ImportantNotes> importantNotes { get; set; } = new List<ImportantNotes>();
        [Parameter] public List<FavoriteNotes> favoriteNotes { get; set; } = new List<FavoriteNotes>();
        [Parameter] public List<StarredNotes> starredNotes { get; set; } = new List<StarredNotes>();

        public NoteDto note { get; set; } = new NoteDto();

        #endregion Properties

        #region Public Methods

        public async Task LoadAllMarkedNotesAsync()
        {
            try
            {
                var favoriteNotesTask = favoriteNotesService.GetAllFavoriteNotesAsync();
                var importantNotesTask = importantNotesService.GetAllImportantNotesAsync();
                var starredNotesTask = starredNotesService.GetAllStarredNotesAsync();

                await Task.WhenAll(favoriteNotesTask, importantNotesTask, starredNotesTask);

                favoriteNotes = favoriteNotesTask.Result?.ToList() ?? new List<FavoriteNotes>();
                importantNotes = importantNotesTask.Result?.ToList() ?? new List<ImportantNotes>();
                starredNotes = starredNotesTask.Result?.ToList() ?? new List<StarredNotes>();

                if (favoriteNotes is null && importantNotes is null && starredNotes is null)
                {
                    favoriteNotes = new List<FavoriteNotes>();
                    importantNotes = new List<ImportantNotes>();
                    starredNotes = new List<StarredNotes>();
                }
                else
                {
                    favoriteNotes = favoriteNotes.ToList();
                    importantNotes = importantNotes.ToList();
                    starredNotes = starredNotes.ToList();
                }

                isFetched = true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                isFetched = false;
            }
        }

        public async Task UnmarkNoteAsFavoriteAsync(Guid NoteId)
        {
            await favoriteNotesService.UnmarkNoteAsFavoriteNoteAsync(NoteId);
            await LoadAllMarkedNotesAsync();
        }

        public async Task UnmarkNoteAsImportantAsync(Guid noteId)
        {
            await importantNotesService.UnamrkNoteAsImportantAsync(noteId);
            await LoadAllMarkedNotesAsync();
        }

        public async Task UnmarkNoteAsStarredAsync(Guid noteId)
        {
            await starredNotesService.UnmarkNoteAsImportantNoteAsync(noteId);
            await LoadAllMarkedNotesAsync();
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