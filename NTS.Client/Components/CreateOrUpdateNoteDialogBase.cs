using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Utilities;
using NTS.Client.Models;
using NTS.Client.Models.DTOs;
using NTS.Client.Pages.DefaultUserPages;
using NTS.Client.Services.Contracts;

namespace NTS.Client.Components
{
    public class CreateOrUpdateNoteDialogBase : ComponentBase
    {
        [CascadingParameter] public MudDialogInstance mudDialog { get; set; }
        [Parameter] public NoteDto note { get; set; } = new NoteDto();
        [Inject] INotesService notesService { get; set; }
        [Inject] IFavoriteNotesService favoriteNotes { get; set; }
        [Inject] IImportantNotesService importantNotes { get; set; }
        [Inject] ISharedNotesService sharedNotes { get; set; }
        [Inject] IStarredNotesService starredNotes { get; set; }
        [Inject] IDialogService dialogService { get; set; }
        [Inject] ISnackbar snackbar { get; set; }
        private NotesBase notesBase { get; set; } = new NotesBase();

        public MudTextField<string>? multilineReference;
        public NoteDto currentNote { get; set; } = new NoteDto();

        public MudColor currentColor { get; set; } = new MudColor("#FFFFFF");

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

        
        public async Task CreateNoteAsync()
        {
            if (string.IsNullOrWhiteSpace(currentNote.Title) || string.IsNullOrWhiteSpace(currentNote.Content))
            {
                snackbar.Add("Title And Content Cannot Be Empty", Severity.Warning);
                return;
            }

            try
            {
                if (notesService == null)
                {
                    Console.WriteLine("notesService Is Null");
                    return;
                }

                currentNote.Color = currentColor.ToString();

                if (note != null && note.NoteId != Guid.Empty)
                {
                    await notesService.UpdateNoteAsync(currentNote, note.NoteId);
                }
                else
                {
                    await notesService.CreateNoteAsync(currentNote);
                }

                mudDialog.Close(DialogResult.Ok(true));
            }
            catch (Exception error)
            {
                Console.WriteLine($"An Error Occurred: {error.Message}");
            }
        }


        public async Task RemoveNoteAsync()
        {
            try
            {
                bool? confirm = await dialogService.ShowMessageBox("Remove Confirmmation", 
                    "Are You Sure You Want To Remove This Note?", yesText: "Remove", cancelText: "Cancel");

                if (notesService == null)
                {
                    Console.WriteLine("notesService Is Null");
                    return;
                }

                if (note != null && note.NoteId != Guid.Empty && confirm == true)
                {
                    await notesService.RemoveNoteAsync(note.NoteId);
                }

                mudDialog.Close(DialogResult.Ok(true));

                if (notesBase != null)
                {
                    await notesBase.LoadNotesAsync();
                }
                else
                {
                    Console.WriteLine("noteBase Is Null");
                }
            }
            catch (Exception error)
            {
                Console.WriteLine($"Error Removing Note: {error.Message}");
            }
        }


        public async Task MarkAsFavorite()
        {
            try
            {
                await favoriteNotes.MarkAsFavoriteNoteAsync(new FavoriteNotes(), note.NoteId);
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
                await importantNotes.MarkNoteAsImportantAsync(new ImportantNotes(), note.NoteId);
            }
            catch (Exception error)
            {
                Console.WriteLine($"Error Marking Note As Important: {error.Message}");
            }
        }


        public void Cancel()
        {
            mudDialog.Cancel();
        }
    }
}