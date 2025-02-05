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
        [CascadingParameter] public MudDialogInstance mudDialog { get; set; } = default!;
        [Parameter] public NotesBase notesBase { get; set; } = default!;
        [Parameter] public NoteDto note { get; set; } = new NoteDto();
        [Inject] public INotesService notesService { get; set; } = default!;
        [Inject] public IFavoriteNotesService favoriteNotesService { get; set; } = default!;
        [Inject] public IImportantNotesService importantNotesService { get; set; } = default!;
        [Inject] public ISharedNotesService sharedNotesService { get; set; } = default!;
        [Inject] public IStarredNotesService starredNotesService { get; set; } = default!;
        [Inject] public IDialogService dialogService { get; set; } = default!;
        [Inject] public ISnackbar snackbar { get; set; } = default!;

        public MudTextField<string>? multilineReference;
        public NoteDto currentNote { get; set; } = new NoteDto();
        public FavoriteNotes favoriteNotes { get; set; } = new FavoriteNotes();
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
                if (notesService is null)
                {
                    Console.WriteLine("NotesService Is Null");
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

                if (notesBase != null)
                {
                    await notesBase.LoadNotesAsync();
                }
                else
                {
                    Console.WriteLine("NotesBase Is Null");
                }
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
                await sharedNotesService.MarkNoteAsSharedAsync(note.NoteId);
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
    }
}