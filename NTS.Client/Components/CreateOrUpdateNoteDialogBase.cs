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

            try
            {
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
            catch (Exception error)
            {
                snackbar.Add($"An Error Occurred: {error.Message}", Severity.Error);
            }
        }

        public async Task RemoveNoteAsync()
        {
            try
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
            catch (Exception error)
            {
                snackbar.Add($"Error Removing Note: {error.Message}", Severity.Error);
            }
        }

        public async Task MarkAsFavorite()
        {
            try
            {
                if (favoriteNotesService == null)
                {
                    Console.WriteLine("FavoriteNotesService Is Null");
                }

                await favoriteNotesService!.MarkAsFavoriteNoteAsync(favoriteNotes, note.NoteId);
            }
            catch (Exception error)
            {
                Console.WriteLine($"Error Marking Note as Favorite: {error.Message}");
            }
        }

        public async Task MarkAsImportant()
        {
            try
            {
                await importantNotesService.MarkNoteAsImportantAsync(new ImportantNotes(), note.NoteId);
            }
            catch (Exception error)
            {
                Console.WriteLine($"Error Marking Note As Important: {error.Message}");
            }
        }

        public async Task MarkAsStarred()
        {
            try
            {
                await starredNotesService.MarkNoteAsStarredAsync(new StarredNotes(), note.NoteId);
            }
            catch (Exception error)
            {
                Console.WriteLine($"Error Marking Note As Starred: {error.Message}");
            }
        }

        public async Task MarkAsShared()
        {
            try
            {
                await sharedNotesService.MarkNoteAsSharedAsync(new SharedNotes(), note.NoteId);
            }
            catch (Exception error)
            {
                Console.WriteLine($"Error Marking Note As Shared: {error.Message}");
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