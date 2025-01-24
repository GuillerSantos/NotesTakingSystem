using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Utilities;
using NTS.Client.Models.DTOs;
using NTS.Client.Services.Contracts;

namespace NTS.Client.Components
{
    public class CreateOrUpdateNoteDialogBase : ComponentBase
    {
        [CascadingParameter] public MudDialogInstance mudDialog { get; set; }
        [Parameter] public NoteDto note { get; set; } = new NoteDto();
        [Inject] INotesService notesService { get; set; }
        [Inject] ISnackbar snackbar { get; set; }

        public MudTextField<string>? multilineReference;
        public NoteDto currentNote { get; set; } = new NoteDto();

        // Initialize With A Default Color (Use A HEX Value)
        public MudColor currentColor { get; set; } = new MudColor("#FFFFFF");

        protected override void OnParametersSet()
        {
            if (note != null && note.NoteId != Guid.Empty)
            {
                currentNote = note;


                // Convert The String Color (HEX Value) When Loading The Note
                if (!string.IsNullOrEmpty(note.Color))
                {
                    currentColor = new MudColor(note.Color); // This Works As Long As Its a Valid HEX String Like Our Default Value Na White #FFFFFF
                }
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
                // Save The Color As A HES String, Including The "#"
                currentNote.Color = currentColor.ToString(); // This Will Give Your The HEX Color String Like "#FFFFFF"

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
            }
            catch (Exception error)
            {
                Console.WriteLine($"An Error Occurred: {error.Message}");
            }
        }
    }
}
