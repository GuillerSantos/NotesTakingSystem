using Microsoft.AspNetCore.Components;
using MudBlazor;
using NTS.Client.Models.DTOs;
using NTS.Client.Services.Contracts;

namespace NTS.Client.Components
{
    public class CreateNoteDialogBase : ComponentBase
    {
        [CascadingParameter] public MudDialogInstance mudDialog { get; set; }
        [Parameter] public NoteDto note { get; set; } = new NoteDto();
        [Inject] INotesService notesService { get; set; }
        [Inject] ISnackbar snackbar { get; set; }

        public MudTextField<string>? multilineReference;

        public NoteDto currentNote { get; set; } = new NoteDto
        {
            Priority = "Normal"
        };


        protected override void OnParametersSet()
        {
            if (note != null && note.NoteId != Guid.Empty)
            {
                currentNote = note;
            }
        }

        public async Task HandleCreateNoteAsync()
        {
            if (string.IsNullOrWhiteSpace(currentNote.Title) || string.IsNullOrWhiteSpace(currentNote.Content))
            {
                snackbar.Add("Title And Content Cannot Be Empty", Severity.Warning);
                return;
            }

            try
            {
                if (note != null)
                {
                    if (note.NoteId == Guid.Empty)
                    {
                        await notesService.CreateNoteAsync(currentNote);
                    }
                    else
                    {
                        await notesService.UpdateNoteAsync(currentNote, note.NoteId);
                    }

                    mudDialog.Close(DialogResult.Ok(true));
                }
            }
            catch (Exception error)
            {
                Console.WriteLine($"An Error Occurred: {error.Message}");
            }
        }
    }
}
