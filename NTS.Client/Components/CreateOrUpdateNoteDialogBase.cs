using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Utilities;
using NTS.Client.DTOs;
using NTS.Client.Models;
using NTS.Client.Pages.DefaultUserPages;
using NTS.Client.Services.Contracts;

namespace NTS.Client.Components
{
    public class CreateOrUpdateNoteDialogBase : ComponentBase
    {
        #region Fields

        public MudTextField<string>? multilineReference;

        #endregion Fields

        #region Properties

        [Inject] public INotesService notesService { get; set; } = default!;
        [Inject] public IFavoriteNotesService favoriteNotesService { get; set; } = default!;
        [Inject] public IImportantNotesService importantNotesService { get; set; } = default!;
        [Inject] public ISharedNotesService sharedNotesService { get; set; } = default!;
        [Inject] public IStarredNotesService starredNotesService { get; set; } = default!;
        [Inject] public IDialogService dialogService { get; set; } = default!;
        [Inject] public ISnackbar snackbar { get; set; } = default!;

        [CascadingParameter] public MudDialogInstance mudDialog { get; set; } = default!;
        public NoteDto currentNote { get; set; } = new NoteDto();
        public MudColor currentColor { get; set; } = new MudColor("#FFFFFF");

        [Parameter] public NotesBase notesBase { get; set; } = default!;
        [Parameter] public NoteDto note { get; set; } = new NoteDto();

        public FavoriteNotes favoriteNotes { get; set; } = new FavoriteNotes();
        public ImportantNotes importantNotes { get; set; } = new ImportantNotes();
        public SharedNotes sharedNotes { get; set; } = new SharedNotes();
        public StarredNotes starredNotes { get; set; } = new StarredNotes();

        #endregion Properties

        #region Public Methods

        public async Task CreateNoteAsync()
        {
            if (string.IsNullOrWhiteSpace(currentNote.Title) || string.IsNullOrWhiteSpace(currentNote.Content))
            {
                snackbar.Add("Title And Content Cannot Be Empty", Severity.Warning);
                return;
            }

            currentNote.Color = currentColor.ToString();

            if (note != null && note.NoteId != Guid.Empty)
            {
                await notesService.UpdateNoteAsync(currentNote, note.NoteId);
                snackbar.Add("Note Updated Successfully", Severity.Success);
            }
            else
            {
                await notesService.CreateNoteAsync(currentNote);
                snackbar.Add("Note Created Successfully", Severity.Success);
            }

            mudDialog.Close(DialogResult.Ok(true));

            if (notesBase != null)
            {
                await notesBase.LoadNotesAsync();
            }
            else
            {
                snackbar.Add("NotesBase is Null", Severity.Error);
            }
        }

        public async Task RemoveNoteAsync()
        {
            bool? confirm = await dialogService.ShowMessageBox("Remove Confirmation",
                "Are You Sure You Want To Remove This Note?", yesText: "Remove", cancelText: "Cancel");

            if (notesService == null)
            {
                snackbar.Add("NotesService Is Null", Severity.Error);
                return;
            }

            if (note != null && note.NoteId != Guid.Empty && confirm == true)
            {
                await notesService.RemoveNoteAsync(note.NoteId);
                snackbar.Add("Note Removed Successfully", Severity.Success);
            }

            mudDialog.Close(DialogResult.Ok(true));

            if (notesBase != null)
            {
                await notesBase.LoadNotesAsync();
            }
            else
            {
                snackbar.Add("NotesBase Is Null", Severity.Error);
            }
        }

        public async Task MarkAsFavorite()
        {
            try
            {
                await favoriteNotesService!.MarkAsFavoriteNoteAsync(favoriteNotes, note.NoteId);
                snackbar.Add("Note Marked As Favorite", Severity.Success);
            }
            catch (Exception error)
            {
                snackbar.Add($"Error marking note as Favorite: {error.Message}", Severity.Error);
            }
        }

        public async Task MarkAsImportant()
        {
            try
            {
                await importantNotesService.MarkNoteAsImportantAsync(new ImportantNotes(), note.NoteId);
                snackbar.Add("Note Marked As Important", Severity.Success);
            }
            catch (Exception error)
            {
                snackbar.Add($"Error marking note as Important: {error.Message}", Severity.Error);
            }
        }

        public async Task MarkAsStarred()
        {
            try
            {
                await starredNotesService.MarkNoteAsStarredAsync(new StarredNotes(), note.NoteId);
                snackbar.Add("Note Marked As Starred", Severity.Success);
            }
            catch (Exception error)
            {
                snackbar.Add($"Error marking note as Starred: {error.Message}", Severity.Error);
            }
        }

        public async Task MarkAsShared()
        {
            try
            {
                await sharedNotesService.MarkNoteAsSharedAsync(new SharedNotes(), note.NoteId);
                snackbar.Add("Note marked as Shared", Severity.Success);
            }
            catch (Exception error)
            {
                snackbar.Add($"Error marking note as Shared: {error.Message}", Severity.Error);
            }
        }

        public void Cancel()
        {
            mudDialog.Cancel();
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnParametersSet()
        {
            if (note != null && note.NoteId != Guid.Empty)
            {
                currentNote = note;

                if (!string.IsNullOrEmpty(note.Color))
                {
                    currentColor = new MudColor(note.Color);
                }
            }
        }

        #endregion Protected Methods
    }
}